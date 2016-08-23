using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class LoadMap : MonoBehaviour {
	public HexGrid hexGrid;
	public Text healthLabel;
	public Currency currency;
	public Build build;
	Canvas gridCanvas;

	public GameObject Necromancer;
	public GameObject Skeleton;
	public GameObject Zombie;
    public GameObject SkeletonArcher;
    public GameObject ArmoredSkeleton;
    public GameObject DeathKnight;

    public GameObject Militia;
    public GameObject Archer;
    public GameObject Longbowman;
    public GameObject Crossbowman;
    public GameObject Footman;
    public GameObject MountedKnight;
    public GameObject HeroKing;

    public GameObject Village;
	public GameObject Necropolis;


	public void LoadHexTiles () {
		hexGrid.SetSize (12,12);
	}

	public void LoadResources () {
		int soulAmount = PlayerPrefs.GetInt ("Souls");
        currency.SetSouls (soulAmount);
	}

	public void LoadEntities () {
		gridCanvas = GetComponentInChildren<Canvas>();

		Vector3 start = hexGrid.GetCellPos(14);
		start.y = 0.2f;
		GameObject playerNecromancer = (GameObject)Instantiate (Necromancer, start, Quaternion.identity);
		playerNecromancer.name = "Necromancer1";
		hexGrid.SetEntityName (14, "Necromancer1");
        hexGrid.SetEntityObject (14, playerNecromancer);
        int ncurrhealth = playerNecromancer.GetComponent<NecromancerBehaviour> ().health;
		CreateHealthLabel (14, ncurrhealth, "Necromancer1");

		Vector3 militiastart = hexGrid.GetCellPos(12);
		militiastart.y = 0.2f;
		GameObject militia1 = (GameObject)Instantiate (Militia, militiastart, Quaternion.identity);
		militia1.name = "Militia1";
		hexGrid.SetEntityName (12, "Militia1");
        hexGrid.SetEntityObject(12, militia1);
        int mcurrhealth = militia1.GetComponent<MilitiaBehaviour> ().health;
		CreateHealthLabel (12, mcurrhealth, "Militia1");

		Vector3 militiastart2 = hexGrid.GetCellPos(15);
		militiastart2.y = 0.2f;
		GameObject militia2 = (GameObject)Instantiate (Militia, militiastart2, Quaternion.identity);
		militia2.name = "Militia2";
		hexGrid.SetEntityName (15, "Militia2");
        hexGrid.SetEntityObject (15, militia2);
        int m2currhealth = militia2.GetComponent<MilitiaBehaviour> ().health;
		CreateHealthLabel (15, m2currhealth, "Militia2");

		Vector3 start2 = hexGrid.GetCellPos(3);
		start2.y = 0.2f;
		GameObject playerSkeleton = (GameObject)Instantiate (Skeleton, start2, Quaternion.identity);
		playerSkeleton.name = "Skeleton1";
		hexGrid.SetEntityName (3, "Skeleton1");
        hexGrid.SetEntityObject (3, playerSkeleton);
        int scurrhealth = playerSkeleton.GetComponent<SkeletonBehaviour> ().health;
		CreateHealthLabel (3, scurrhealth, "Skeleton1");

		Vector3 start3 = hexGrid.GetCellPos(18);
		start3.y = 0.2f;
		GameObject playerZombie = (GameObject)Instantiate (Zombie, start3, Quaternion.identity);
		playerZombie.name = "Zombie1";
		hexGrid.SetEntityName (18, "Zombie1");
        hexGrid.SetEntityObject (18, playerZombie);
        int zcurrhealth = playerZombie.GetComponent<ZombieBehaviour> ().health;
		CreateHealthLabel (18, zcurrhealth, "Zombie1");

		for (int i = 0; i < hexGrid.size; i++) {
			string allEntities = PlayerPrefs.GetString ("HexEntity" + i);
			int allHealth = PlayerPrefs.GetInt ("HexEntityHealth" + i);
			string cleanEntity = Regex.Replace(allEntities, @"[\d-]", string.Empty);
			int allIndex = PlayerPrefs.GetInt ("HexEntityIndex" + i);

            if (allEntities != "Empty" && cleanEntity != string.Empty)
            {
                Vector3 spawn = hexGrid.GetCellPos(allIndex);
                spawn.y = 0.2f;
                GameObject entity = (GameObject)Instantiate(Resources.Load(cleanEntity), spawn, Quaternion.identity);
                entity.name = allEntities;
                hexGrid.SetEntityName(allIndex, allEntities);
                hexGrid.SetEntityObject(allIndex, entity);
                SetHealth(entity, cleanEntity, allHealth);
                CreateHealthLabel(allIndex, allHealth, allEntities);
            }
        }
	}

    void SetHealth (GameObject entity, string entityName, int health)
    {
        switch (entityName)
        {
            case "Necromancer":
                entity.GetComponent<NecromancerBehaviour>().lasthealth = health;
                break;
            case "Zombie":
                entity.GetComponent<ZombieBehaviour>().lasthealth = health;
                break;
            case "Skeleton":
                entity.GetComponent<SkeletonBehaviour>().lasthealth = health;
                break;
            case "SkeletonArcher":
                entity.GetComponent<SkeletonArcherBehaviour>().lasthealth = health;
                break;
            case "ArmoredSkeleton":
                entity.GetComponent<ArmoredSkeletonBehaviour>().lasthealth = health;
                break;
            case "DeathKnight":
                entity.GetComponent<DeathKnightBehaviour>().lasthealth = health;
                break;

            case "Militia":
                entity.GetComponent<MilitiaBehaviour>().lasthealth = health;
                break;
            case "Archer":
                entity.GetComponent<ArcherBehaviour>().lasthealth = health;
                break;
            case "Longbowman":
                entity.GetComponent<LongbowmanBehaviour>().lasthealth = health;
                break;
            case "Crossbowman":
                entity.GetComponent<CrossbowmanBehaviour>().lasthealth = health;
                break;
            case "Footman":
                entity.GetComponent<FootmanBehaviour>().lasthealth = health;
                break;
            case "MountedKnight":
                entity.GetComponent<MountedKnightBehaviour>().lasthealth = health;
                break;
            case "HeroKing":
                entity.GetComponent<HeroKingBehaviour>().lasthealth = health;
                break;
        }
    }


    public void CreateHealthLabel (int index, int health, string entity) {
		Text label = Instantiate<Text>(healthLabel);
		label.rectTransform.SetParent(gridCanvas.transform, false);
		Vector3 healthpos = hexGrid.GetCellPos (index);
		label.rectTransform.anchoredPosition = new Vector2(healthpos.x, healthpos.z);
		label.text = health.ToString();
		label.name = "Health " + entity;
	}

	public void LoadTerrain () {
		
		for (int i = 0; i < hexGrid.size; i++) {
			string allTerrain = PlayerPrefs.GetString ("Hex" + i);

			if (allTerrain == "Grass") {
				hexGrid.ColorCellIndex (i, Color.green);
				hexGrid.SetTerrain (i, "Grass");
			} else if (allTerrain == "Water") {
				hexGrid.ColorCellIndex (i, Color.blue);
				hexGrid.SetTerrain (i, "Water");
			} else if (allTerrain == "Mountain") {
				hexGrid.ColorCellIndex (i, Color.red);
				hexGrid.SetTerrain (i, "Mountain");
			}
		}
	}

	public void LoadBuildings () {
		gridCanvas = GetComponentInChildren<Canvas>();

		Vector3 build1 = hexGrid.GetCellPos(15);
		build1.y = 0.1f;
		GameObject eVillagem = (GameObject)Instantiate (Village, build1, Quaternion.Euler(90,0,0));
		eVillagem.name = "Village1";
		hexGrid.SetBuildingName (15, "Village1");
        hexGrid.SetBuildingObject(15, eVillagem);

        Vector3 build2 = hexGrid.GetCellPos(14);
		build2.y = 0.1f;
		GameObject pNecropolisn = (GameObject)Instantiate (Necropolis, build2, Quaternion.Euler(90,0,0));
		pNecropolisn.name = "Necropolis1";
		hexGrid.SetBuildingName(14, "Necropolis1");
        hexGrid.SetBuildingObject(14, pNecropolisn);

        for (int i = 0; i < hexGrid.size; i++) {
			string allBuildings = PlayerPrefs.GetString ("HexBuilding" + i);
			int allHealth = PlayerPrefs.GetInt ("HexBuildingHealth" + i);
			string cleanBuilding = Regex.Replace(allBuildings, @"[\d-]", string.Empty);
			int allIndex = PlayerPrefs.GetInt ("HexBuildingIndex" + i);

			if (cleanBuilding == "Village") {
				Vector3 spawn = hexGrid.GetCellPos(allIndex);
				spawn.y = 0.1f;
				GameObject eVillage = (GameObject)Instantiate (Village, spawn, Quaternion.Euler(90,0,0));
				eVillage.name = allBuildings;
				hexGrid.SetBuildingName (allIndex, allBuildings);
                hexGrid.SetBuildingObject(allIndex, eVillage);
                eVillage.GetComponent<VillageMechanics> ().lasthealth = allHealth;
			} else if (cleanBuilding == "Necropolis") {
				Vector3 spawn = hexGrid.GetCellPos(allIndex);
				spawn.y = 0.1f;
				GameObject pNecropolis = (GameObject)Instantiate (Necropolis, spawn, Quaternion.Euler(90,0,0));
				pNecropolis.name = allBuildings;
				hexGrid.SetBuildingName(allIndex, allBuildings);
                hexGrid.SetBuildingObject(allIndex, pNecropolis);
                pNecropolis.GetComponent<NecropolisMechanics> ().lasthealth = allHealth;
			}
		}
	}

	public void LoadCorpses () {
		for (int i = 0; i < hexGrid.size; i++) {
			for (int j = 0; j < 4; j++) {
				string allCorpses = PlayerPrefs.GetString ("HexCorpses" + i + "corpse" + j);
				string cleanCorpse = Regex.Replace(allCorpses, @"[\d-]", string.Empty);

				if (cleanCorpse == "Militia") {
					hexGrid.SetCorpses (i, allCorpses);
				} else if (cleanCorpse == "Archer") {
					hexGrid.SetCorpses (i, allCorpses);
				} else if (cleanCorpse == "Longbowman") {
					hexGrid.SetCorpses (i, allCorpses);
				} else if (cleanCorpse == "Crossbowman") {
					hexGrid.SetCorpses (i, allCorpses);
				} else if (cleanCorpse == "Footman") {
					hexGrid.SetCorpses (i, allCorpses);
				} else if (cleanCorpse == "MountedKnight") {
					hexGrid.SetCorpses (i, allCorpses);
				} else if (cleanCorpse == "HeroKing") {
					hexGrid.SetCorpses (i, allCorpses);
				}
			}
		}
	}

	public void LoadRandom (int seed) {
        Random.InitState(seed);
		for (int i = 0; i < hexGrid.size; i++) {
			//terrain generated via seed
			float terrainSeedVal = Random.value;
			if (terrainSeedVal >= 0.25) {
				hexGrid.SetTerrain (i, "Grass");
				PlayerPrefs.SetString ("Hex" + i, "Grass");
			} else if (terrainSeedVal < 0.25 && terrainSeedVal >= 0.10) {
				hexGrid.SetTerrain (i, "Water");
				PlayerPrefs.SetString ("Hex" + i, "Water");
			} else if (terrainSeedVal < 0.10) {
				hexGrid.SetTerrain (i, "Mountain");
				PlayerPrefs.SetString ("Hex" + i, "Mountain");
			}

			//buildings generated via seed
			float buildingSeedVal = Random.value;
			if (buildingSeedVal >= 0.15) {
				//TODO remove when finished implementing storing several maps
				build.DestroyBuilding(i);
			} else if (terrainSeedVal < 0.15 && terrainSeedVal >= 0.10) {
				build.BuildBuilding (i, "Village");
			}
		}
	}
}
