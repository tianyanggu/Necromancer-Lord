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
    public EntityStats entityStats;
    Canvas gridCanvas;

    //remove after testing
    public Summon summon;
    public GameObject Village;
	public GameObject Necropolis;
    //end of remove

    public void LoadHexTiles () {
		hexGrid.SetSize (12,12);
	}

	public void LoadResources () {
		int soulAmount = PlayerPrefs.GetInt ("Souls");
        currency.SetSouls (soulAmount);
	}

	public void LoadEntities () {
		gridCanvas = GetComponentInChildren<Canvas>();

        //summon.SummonEntity(14, "Necromancer", "AA");
        //summon.SummonEntity(12, "Militia", "BB");
        //summon.SummonEntity(15, "Militia", "CA");
        //summon.SummonEntity(3, "Skeleton", "AA");
        //summon.SummonEntity(18, "Zombie", "AA");

        //TODO change diff stats, will incl. all stats once new save system implemented
		for (int i = 0; i < hexGrid.size; i++) {
			string allEntities = PlayerPrefs.GetString ("HexEntity" + i);
			int allHealth = PlayerPrefs.GetInt ("HexEntityHealth" + i);
			int allIndex = PlayerPrefs.GetInt ("HexEntityIndex" + i);

            if (allEntities != string.Empty)
            {
                string cleanEntity = Regex.Replace(allEntities.Substring(2), @"[\d-]", string.Empty);
                Vector3 spawn = hexGrid.GetCellPos(allIndex);
                spawn.y = 0.2f;
                GameObject entity = (GameObject)Instantiate(Resources.Load(cleanEntity), spawn, Quaternion.identity);
                entity.name = allEntities;
                hexGrid.SetEntityName(allIndex, allEntities);
                hexGrid.SetEntityObject(allIndex, entity);

                //TODO remove once, new save for sets stats for entity
                int health = entityStats.GetMaxHealth(cleanEntity);
                entityStats.SetMaxHealth(entity, health);
                entityStats.SetCurrHealth(entity, health);
                int mana = entityStats.GetMaxMana(cleanEntity);
                entityStats.SetMaxMana(entity, mana);
                entityStats.SetCurrMana(entity, mana);
                int dmg = entityStats.GetAttackDmg(cleanEntity);
                entityStats.SetAttackDmg(entity, dmg);
                int attpt = entityStats.GetMaxAttackPoint(cleanEntity);
                entityStats.SetMaxAttackPoint(entity, attpt);
                int movept = entityStats.GetMaxMovementPoint(cleanEntity);
                entityStats.SetMaxMovementPoint(entity, movept);
                int range = entityStats.GetRange(cleanEntity);
                entityStats.SetRange(entity, range);
                int rangedattdmg = entityStats.GetRangedAttackDmg(cleanEntity);
                entityStats.SetRangedAttackDmg(entity, rangedattdmg);
                int armor = entityStats.GetArmor(cleanEntity);
                entityStats.SetArmor(entity, armor);
                int armorpiercing = entityStats.GetArmorPiercing(cleanEntity);
                entityStats.SetArmorPiercing(entity, armorpiercing);
                int rangedarmorpiercing = entityStats.GetRangedArmorPiercing(cleanEntity);
                entityStats.SetRangedArmorPiercing(entity, rangedarmorpiercing);
                int vision = entityStats.GetVision(cleanEntity);
                entityStats.SetVision(entity, vision);
                //end of remove

                CreateHealthLabel(allIndex, allHealth, allEntities);
            }
        }
	}

    void SetBuildingHealth(GameObject building, string buildingName, int health)
    {
        switch (buildingName)
        {
            case "Necropolis":
                building.GetComponent<NecropolisMechanics>().lasthealth = health;
                break;

            case "Village":
                building.GetComponent<VillageMechanics>().lasthealth = health;
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
		eVillagem.name = "CAVillage1";
		hexGrid.SetBuildingName (15, "CAVillage1");
        hexGrid.SetBuildingObject(15, eVillagem);

        Vector3 build2 = hexGrid.GetCellPos(14);
		build2.y = 0.1f;
		GameObject pNecropolisn = (GameObject)Instantiate (Necropolis, build2, Quaternion.Euler(90,0,0));
		pNecropolisn.name = "AANecropolis1";
		hexGrid.SetBuildingName(14, "AANecropolis1");
        hexGrid.SetBuildingObject(14, pNecropolisn);

        for (int i = 0; i < hexGrid.size; i++) {
			string allBuildings = PlayerPrefs.GetString ("HexBuilding" + i);
			int allHealth = PlayerPrefs.GetInt ("HexBuildingHealth" + i);
			int allIndex = PlayerPrefs.GetInt ("HexBuildingIndex" + i);

            if (allBuildings != string.Empty)
            {
                string cleanBuilding = Regex.Replace(allBuildings.Substring(2), @"[\d-]", string.Empty);
                Vector3 spawn = hexGrid.GetCellPos(allIndex);
                spawn.y = 0.1f;
                GameObject building = (GameObject)Instantiate(Resources.Load(cleanBuilding), spawn, Quaternion.Euler(90, 0, 0));
                building.name = allBuildings;
                hexGrid.SetBuildingName(allIndex, allBuildings);
                hexGrid.SetBuildingObject(allIndex, building);
                SetBuildingHealth(building, cleanBuilding, allHealth);
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
			//float buildingSeedVal = Random.value;
			//if (buildingSeedVal >= 0.15) {
				//TODO remove when finished implementing storing several maps
				//build.DestroyBuilding(i);
			//} else if (terrainSeedVal < 0.15 && terrainSeedVal >= 0.10) {
			//	build.BuildBuilding (i, "Village");
			//}
		}
	}
}
