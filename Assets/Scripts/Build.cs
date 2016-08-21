using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class Build : MonoBehaviour {

	public HexGrid hexGrid;
	public LoadMap loadMap;
	public BuildingStorage buildingStorage;
	public Currency currency;
	public EntityStorage entityStorage;

	public GameObject Village;
	public GameObject Necropolis;


	//given an index and the type of summon, summons that entity with the next available name
	public void BuildBuilding (int cellindex, string buildingname) {
		Vector3 buildindex = hexGrid.GetCellPos(cellindex);
		buildindex.y = 0.2f;
		string faction = buildingStorage.whichFactionBuilding (buildingname);
		string availableNum = AvailableName (buildingname, faction);
		string availableName = buildingname + availableNum;
        int health = GetHealthInfo (buildingname);

		if (buildingname == "Village") {
			GameObject enemybuilding = (GameObject)Instantiate (Village, buildindex, Quaternion.Euler(90,0,0));
			enemybuilding.name = availableName;
            buildingStorage.AddActivePlayerBuilding(enemybuilding);
            hexGrid.SetBuildingObject(cellindex, enemybuilding);
        } else if (buildingname == "Necropolis") {
			GameObject playerbuilding = (GameObject)Instantiate (Necropolis, buildindex, Quaternion.Euler(90,0,0));
			playerbuilding.name = availableName;
            buildingStorage.AddActivePlayerBuilding(playerbuilding);
            hexGrid.SetBuildingObject(cellindex, playerbuilding);
        }
		//stores info of new summon to playerprefs for saving
		string ppName = AvailablePlayerPrefsName ();

		PlayerPrefs.SetString ("HexBuilding" + ppName, availableName);
		PlayerPrefs.SetString (availableName, "HexBuilding" + ppName);
		PlayerPrefs.SetInt ("HexBuildingHealth" + ppName, health);
		PlayerPrefs.SetInt ("HexBuildingIndex" + ppName, cellindex);
		hexGrid.SetBuildingName (cellindex, availableName);
    }

	//Check for next available entity number
	string AvailableName (string buildingname, string faction) {
		for (int i = 1; i <= 999; i++) {
			string num = i.ToString ();
			if (faction == "undead") {
                bool nameExists = false;
				foreach (GameObject playerBuildings in buildingStorage.activePlayerBuildings) {
					if (playerBuildings.name == buildingname + num)
                    {
                        nameExists = true;
                    }
				}
                if (!nameExists)
                {
                    return num;
                }
            }
			if (faction == "human") {
                bool nameExists = false;
                foreach (GameObject playerBuildings in buildingStorage.activePlayerBuildings)
                {
                    if (playerBuildings.name == buildingname + num)
                    {
                        nameExists = true;
                    }
                }
                if (!nameExists)
                {
                    return num;
                }
            }
		} 
		return null;
	}

	public void DestroyBuilding (int cellindex) {
		string buildingName = hexGrid.GetBuildingName (cellindex);
        GameObject building = hexGrid.GetBuildingObject (cellindex);
        buildingStorage.RemoveActivePlayerBuilding(building);
        Destroy (building);

		//delete from playerprefs
		string playerprefsName = PlayerPrefs.GetString (buildingName);
		string playerprefsNum = Regex.Replace(playerprefsName, "[^0-9 -]", string.Empty);
		PlayerPrefs.DeleteKey ("HexBuilding" + playerprefsNum);
		PlayerPrefs.DeleteKey (buildingName);
		PlayerPrefs.DeleteKey ("HexBuildingHealth" + playerprefsNum);
		PlayerPrefs.DeleteKey ("HexBuildingIndex" + playerprefsNum);
		hexGrid.SetBuildingName (cellindex, "Empty");
        hexGrid.SetBuildingObject (cellindex, null);
    }

	//Check for next available setstring number
	string AvailablePlayerPrefsName () {
		for (int i = 0; i < hexGrid.size; i++) {
			string num = i.ToString ();
			string allEntity = PlayerPrefs.GetString ("HexEntity" + i);
			if (allEntity == "") {
				return num;
			}
		}
		return null; //TODO error message if no available spaces, should not be possible to give null
	}

	//grabs health info
	int GetHealthInfo(string building) {
		if (building == "Village") {
			return Village.GetComponent<VillageMechanics> ().health;
		} else if (building == "Necropolis") {
			return Necropolis.GetComponent<NecropolisMechanics> ().health;
		}
		return 0;
	}

	//valid if have soul cost and entity/corpse cost
	public bool ValidBuilding(string building, int index) {
		int souls = PlayerPrefs.GetInt ("Souls");
		int cost = buildingStorage.buildingSoulCost (building);

		List<string> corpses = hexGrid.GetCorpses (index);
		string entityName = hexGrid.GetEntityName (index);
		string cleanEntity = Regex.Replace(entityName, @"[\d-]", string.Empty);
        string ppNum = PlayerPrefs.GetString(entityName);
		string numEntity = Regex.Replace(ppNum, "[^0-9 -]", string.Empty);

		//checks if fulfilled cost and removes paid cost from game
		if (souls >= cost) {
			if (corpses.Contains("Militia")) {
                currency.ChangeSouls (-cost);
				hexGrid.RemoveCorpses (index, "Militia");
				return true;
			} else if (cleanEntity == "Skeleton" || cleanEntity == "Zombie" || cleanEntity == "SkeletonArcher") {
                currency.ChangeSouls (-cost);
				GameObject entityGameObj = hexGrid.GetEntityObject(index);
                entityStorage.RemoveActivePlayerEntity(entityGameObj);
                Destroy (entityGameObj);
				hexGrid.SetEntityName (index, "Empty");
                hexGrid.SetEntityObject (index, null);
                GameObject healthText = GameObject.Find ("Health " + entityName);
				Destroy (healthText);
				PlayerPrefs.DeleteKey ("HexEntity" + numEntity);
				PlayerPrefs.DeleteKey ("HexEntityHealth" + numEntity);
				PlayerPrefs.DeleteKey ("HexEntityIndex" + numEntity);
				PlayerPrefs.DeleteKey (entityName);
				return true;
			}
		}
		return false;
	}
}
