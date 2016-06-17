using UnityEngine;
using System.Collections;

public class Build : MonoBehaviour {

	public HexGrid hexGrid;
	public LoadMap loadMap;
	public BuildingStorage buildingStorage;
	public Resources resources;

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
		} else if (buildingname == "Necropolis") {
			GameObject playerbuilding = (GameObject)Instantiate (Necropolis, buildindex, Quaternion.Euler(90,0,0));
			playerbuilding.name = availableName;
		}
		//stores info of new summon to playerprefs for saving
		string ppName = AvailablePlayerPrefsName ();

		PlayerPrefs.SetString ("HexBuilding" + ppName, availableName);
		PlayerPrefs.SetString (availableName, "HexBuilding" + ppName);
		PlayerPrefs.SetInt ("HexBuildingHealth" + ppName, health);
		PlayerPrefs.SetInt ("HexBuildingIndex" + ppName, cellindex);
		hexGrid.BuildingCellIndex (cellindex, availableName);
	}

	//Check for next available entity number
	string AvailableName (string buildingname, string faction) {
		for (int i = 1; i <= 999; i++) {
			string num = i.ToString ();
			if (faction == "undead") {
				if (!buildingStorage.activePlayerBuildings.Contains (buildingname + num)) {
					string availableName = buildingname + num;
					buildingStorage.AddActivePlayerBuilding (availableName);
					return num;
				}
			}
			if (faction == "human") {
				if (!buildingStorage.activeEnemyBuildings.Contains (buildingname + num)) {
					string availableName = buildingname + num;
					buildingStorage.AddActiveEnemyBuilding (availableName);
					return num;
				}
			}
		} 
		return null;
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
		GameObject buildingObj = GameObject.Find (building);

		//------Grab Info Attacker------
		if (building == "Village") {
			return buildingObj.GetComponent<VillageMechanics> ().health;
		} else if (building == "Necropolis") {
			return buildingObj.GetComponent<NecropolisMechanics> ().health;
		}
		return 0;
	}

	public bool ValidBuilding(string building) {
		int souls = PlayerPrefs.GetInt ("Souls");
		int cost = buildingStorage.buildingSoulCost (building);
		if (souls >= cost) {
			resources.ChangeSouls (-cost);
			return true;
		}
		return false;
	}
}
