﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildingStorage : MonoBehaviour {

	public List<string> playerBuildings = new List<string> ();
	public List<string> enemyBuildings = new List<string> ();

	public List<GameObject> activePlayerBuildings = new List<GameObject> ();
	public List<GameObject> activeEnemyBuildings = new List<GameObject> ();

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
                string num = i.ToString();
                string buildingName = building + num;
                GameObject gameEntity = GameObject.Find(building + num);
                if (gameEntity != null)
                {
                    activePlayerBuildings.Add(gameEntity);
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
					activeEnemyBuildings.Add (gameEntity);
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

	//returns summon soul cost
	public int buildingSoulCost(string entity) {
		//------Determine Cost------
		if (entity == "Necropolis") {
			return 200;
		}
		return 0;
	}

	public void AddActivePlayerBuilding (GameObject entityObject) {
		activePlayerBuildings.Add (entityObject);
	} 

	public void RemoveActivePlayerBuilding (GameObject entityObject) {
		activePlayerBuildings.Remove (entityObject);
	} 

	public void AddActiveEnemyBuilding (GameObject entityObject) {
		activeEnemyBuildings.Add (entityObject);
	} 

	public void RemoveActiveEnemyBuilding (GameObject entityObject) {
		activeEnemyBuildings.Remove (entityObject);
	} 
}
