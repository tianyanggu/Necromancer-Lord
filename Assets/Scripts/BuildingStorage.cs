using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildingStorage : MonoBehaviour {

	public List<string> playerBuildings = new List<string> ();
	public List<string> enemyBuildings = new List<string> ();

	public List<string> activePlayerBuildings = new List<string> ();
	public List<string> activeEnemyBuildings = new List<string> ();

	void Start () {
		//player controlled buildings
		playerBuildings.Add ("Necropolis");
		//enemy entities
		enemyBuildings.Add ("Village");

		ListActivePlayerBuildings ();
		ListActiveEnemyBuildings ();
	}

	public void ListActivePlayerBuildings () {
		foreach (string building in playerBuildings) {
			for (int i = 1; i <= 99; i++) {
				string num = i.ToString ();
				string buildingName = building + num;
				GameObject gameEntity = GameObject.Find (building + num);
				if (gameEntity != null) {
					activePlayerBuildings.Add (buildingName);
				}
			}
		}
	}

	public void ListActiveEnemyBuildings () {
		foreach (string building in enemyBuildings) {
			for (int i = 1; i <= 99; i++) {
				string num = i.ToString ();
				string buildingName = building + num;
				GameObject gameEntity = GameObject.Find (building + num);
				if (gameEntity != null) {
					activeEnemyBuildings.Add (buildingName);
				}
			}
		}
	}

	//returns faction for building
	public string whichFactionBuilding(string entity) {
		//------Determine Faction------
		if (entity == "Necropolis") {
			return "undead";
		} else if (entity == "Village") {
			return "human";
		}
		return "unknown";
	}

	//returns summon corpse cost
	public int buildingCorpseCost(string entity) {
		//------Determine Cost------
		if (entity == "Necropolis") {
			return 500;
		}
		return 0;
	}

	public void AddActivePlayerEntity (string entityName) {
		activePlayerBuildings.Add (entityName);
	} 

	public void RemoveActivePlayerEntity (string entityName) {
		activePlayerBuildings.Remove (entityName);
	} 

	public void AddActiveEnemyEntity (string entityName) {
		activeEnemyBuildings.Add (entityName);
	} 

	public void RemoveActiveEnemyEntity (string entityName) {
		activeEnemyBuildings.Remove (entityName);
	} 
}
