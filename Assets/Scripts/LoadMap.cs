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
        int height = GameMemento.current.hexGridMemento.height;
        int width = GameMemento.current.hexGridMemento.width;
        hexGrid.SetSize (height, width);
	}

    public void LoadNewHexTiles(int height, int width)
    {
        hexGrid.SetSize(height, width);
    }

    public void LoadResources () {
		int souls = GameMemento.current.souls;
        currency.SetSouls (souls);
	}

	public void LoadEntities () {
		gridCanvas = GetComponentInChildren<Canvas>();

        //summon.SummonEntity(14, EntityNames.Necromancer, "AA");
        //summon.SummonEntity(12, EntityNames.Militia, "BB");
        //summon.SummonEntity(15, EntityNames.Militia, "CA");
        //summon.SummonEntity(3, EntityNames.Skeleton, "AA");
        //summon.SummonEntity(18, EntityNames.Zombie, "AA");

        List<UndeadEntityMemento> undeadEntityList = GameMemento.current.undeadEntityMementoList;
        int undeadEntityListLength = undeadEntityList.Count;
        List<HumanEntityMemento> humanEntityList = GameMemento.current.humanEntityMementoList;
        int humanEntityListLength = humanEntityList.Count;

        //TODO maybe pass memento to method instead of super long
        for (int i = 0; i < undeadEntityListLength; i++)
        {
            summon.SummonUndeadEntity(GameMemento.current.undeadEntityMementoList[i]);
        }
        for (int i = 0; i < humanEntityListLength; i++)
        {
            summon.SummonHumanEntity(GameMemento.current.humanEntityMementoList[i]);
        }
    }

    void SetBuildingHealth(GameObject building, string buildingName, int health)
    {
        switch (buildingName)
        {
            case BuildingNames.Necropolis:
                building.GetComponent<UndeadBuilding>().currhealth = health;
                break;

            case BuildingNames.Village:
                building.GetComponent<HumanBuilding>().currhealth = health;
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
            string allTerrain = GameMemento.current.hexGridTerrainList[i];

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

				if (cleanCorpse == EntityNames.Militia) {
					hexGrid.SetCorpses (i, allCorpses);
				} else if (cleanCorpse == EntityNames.Archer) {
					hexGrid.SetCorpses (i, allCorpses);
				} else if (cleanCorpse == EntityNames.Longbowman) {
					hexGrid.SetCorpses (i, allCorpses);
				} else if (cleanCorpse == EntityNames.Crossbowman) {
					hexGrid.SetCorpses (i, allCorpses);
				} else if (cleanCorpse == EntityNames.Footman) {
					hexGrid.SetCorpses (i, allCorpses);
				} else if (cleanCorpse == EntityNames.MountedKnight) {
					hexGrid.SetCorpses (i, allCorpses);
				} else if (cleanCorpse == EntityNames.LightsChosen) {
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
                hexGrid.ColorCellIndex(i, Color.green);
            } else if (terrainSeedVal < 0.25 && terrainSeedVal >= 0.10) {
				hexGrid.SetTerrain (i, "Water");
                hexGrid.ColorCellIndex(i, Color.blue);
            } else if (terrainSeedVal < 0.10) {
				hexGrid.SetTerrain (i, "Mountain");
                hexGrid.ColorCellIndex(i, Color.red);
            }

			//buildings generated via seed
			//float buildingSeedVal = Random.value;
			//if (buildingSeedVal >= 0.15) {
				//TODO remove when finished implementing storing several maps
				//build.DestroyBuilding(i);
			//} else if (terrainSeedVal < 0.15 && terrainSeedVal >= 0.10) {
			//	build.BuildBuilding (i, BuildingNames.Village);
			//}
		}
	}
}
