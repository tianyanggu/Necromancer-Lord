using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class Build : MonoBehaviour {

	public HexGrid hexGrid;
	public LoadMap loadMap;
	public BuildingStorage buildingStorage;
	public Resources resources;
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
		hexGrid.SetBuilding (cellindex, availableName);
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

	public void DestroyBuilding (int cellindex) {
		string buildingName = hexGrid.GetBuilding (cellindex);
		GameObject building = GameObject.Find (buildingName);
		Destroy (building);

		//delete from playerprefs
		string playerprefsName = PlayerPrefs.GetString (buildingName);
		string playerprefsNum = Regex.Replace(buildingName, "[^0-9 -]", string.Empty);
		PlayerPrefs.DeleteKey ("HexBuilding" + playerprefsNum);
		PlayerPrefs.DeleteKey (buildingName);
		PlayerPrefs.DeleteKey ("HexBuildingHealth" + playerprefsNum);
		PlayerPrefs.DeleteKey ("HexBuildingIndex" + playerprefsNum);
		hexGrid.SetBuilding (cellindex, "Empty");
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

	//valid if have soul cost and entity/corpse cost
	public bool ValidBuilding(string building, int index) {
		int souls = PlayerPrefs.GetInt ("Souls");
		int cost = buildingStorage.buildingSoulCost (building);

		List<string> corpses = hexGrid.GetCorpses (index);
		string entity = hexGrid.GetEntity (index);
		string cleanEntity = Regex.Replace(entity, @"[\d-]", string.Empty);
		string numEntity = Regex.Replace(entity, "[^0-9 -]", string.Empty);

		//checks if fulfilled cost and removes paid cost from game
		if (souls >= cost) {
			if (corpses.Contains("Militia")) {
				resources.ChangeSouls (-cost);
				hexGrid.RemoveCorpses (index, "Militia");
				return true;
			} else if (cleanEntity == "Skeleton" || cleanEntity == "Zombie") {
				resources.ChangeSouls (-cost);
				GameObject entityGameObj = GameObject.Find (entity);
				Destroy (entityGameObj);
				hexGrid.SetEntity (index, "Empty");
				GameObject healthText = GameObject.Find ("Health " + entity);
				Destroy (healthText);
				entityStorage.RemoveActiveEnemyEntity (entity);
				PlayerPrefs.DeleteKey ("HexEntity" + numEntity);
				PlayerPrefs.DeleteKey ("HexEntityHealth" + numEntity);
				PlayerPrefs.DeleteKey ("HexEntityIndex" + numEntity);
				PlayerPrefs.DeleteKey (entity);
				return true;
			}
		}
		return false;
	}
}
