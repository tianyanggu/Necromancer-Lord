using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EntityStorage : MonoBehaviour {

	public List<string> playerEntities = new List<string> ();
	public List<string> enemyEntities = new List<string> ();

	public List<string> activePlayerEntities = new List<string> ();
	public List<string> activeEnemyEntities = new List<string> ();

	void Start () {
		//player controlled entities
		playerEntities.Add ("Necromancer");
		playerEntities.Add ("Skeleton");
		playerEntities.Add ("Zombie");
        playerEntities.Add ("SkeletonArcher");
		//enemy entities
		enemyEntities.Add ("Militia");

		ListActivePlayerEntities ();
		ListActiveEnemyEntities ();
	}

	public void ListActivePlayerEntities () {
		foreach (string entity in playerEntities) {
			for (int i = 1; i <= 99; i++) {
				string num = i.ToString ();
				string entityName = entity + num;
				GameObject gameEntity = GameObject.Find (entity + num);
				if (gameEntity != null) {
					activePlayerEntities.Add (entityName);
				}
			}
		}
	}

	public void ListActiveEnemyEntities () {
		foreach (string entity in enemyEntities) {
			for (int i = 1; i <= 99; i++) {
				string num = i.ToString ();
				string entityName = entity + num;
				GameObject gameEntity = GameObject.Find (entity + num);
				if (gameEntity != null) {
					activeEnemyEntities.Add (entityName);
				}
			}
		}
	}

	//returns faction
	public string whichFaction(string entity) {
		//------Determine Faction------
		if (entity == "Zombie") {
			return "undead";
		} else if (entity == "Skeleton") {
			return "undead";
		} else if (entity == "Necromancer") {
			return "undead";
		} else if (entity == "SkeletonArcher") {
			return "undead";
		}

        else if (entity == "Militia") {
			return "human";
		}
		return "unknown";
	}

	//returns summon soul cost
	public int summonSoulCost(string entity) {
		//------Determine Cost------
		if (entity == "Zombie") {
			return 100;
		} else if (entity == "Skeleton") {
			return 150;
		} else if (entity == "Necromancer") {
			return 10000;
		} else if (entity == "SkeletonArcher") {
			return 150;
		}
		return 0;
	}

	public void AddActivePlayerEntity (string entityName) {
		activePlayerEntities.Add (entityName);
	} 

	public void RemoveActivePlayerEntity (string entityName) {
		activePlayerEntities.Remove (entityName);
	} 

	public void AddActiveEnemyEntity (string entityName) {
		activeEnemyEntities.Add (entityName);
	} 

	public void RemoveActiveEnemyEntity (string entityName) {
		activeEnemyEntities.Remove (entityName);
	} 
}
