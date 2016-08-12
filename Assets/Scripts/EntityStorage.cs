using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EntityStorage : MonoBehaviour {

	public List<string> playerEntities = new List<string> ();
	public List<string> enemyEntities = new List<string> ();

	public List<GameObject> activePlayerEntities = new List<GameObject> ();
	public List<GameObject> activeEnemyEntities = new List<GameObject> ();

	void Start () {
		//player controlled entities
		playerEntities.Add ("Necromancer");
		playerEntities.Add ("Skeleton");
		playerEntities.Add ("Zombie");
        playerEntities.Add ("SkeletonArcher");
        playerEntities.Add ("ArmoredSkeleton");
        playerEntities.Add ("DeathKnight");
		//enemy entities
		enemyEntities.Add ("Militia");
        enemyEntities.Add ("Archer");
        enemyEntities.Add ("Longbowman");
        enemyEntities.Add ("Crossbowman");
        enemyEntities.Add ("Footman");
        enemyEntities.Add ("MountedKnight");
        enemyEntities.Add ("HeroKing");

		ListActivePlayerEntities ();
		ListActiveEnemyEntities ();
	}

	public void ListActivePlayerEntities () {
		foreach (string entity in playerEntities) {
			for (int i = 1; i <= 99; i++) {
				string num = i.ToString ();
				GameObject gameEntity = GameObject.Find (entity + num);
				if (gameEntity != null) {
					activePlayerEntities.Add (gameEntity);
				}
			}
		}
	}

	public void ListActiveEnemyEntities () {
		foreach (string entity in enemyEntities) {
			for (int i = 1; i <= 99; i++) {
				string num = i.ToString ();
				GameObject gameEntity = GameObject.Find (entity + num);
				if (gameEntity != null) {
					activeEnemyEntities.Add (gameEntity);
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
		} else if (entity == "ArmoredSkeleton") {
			return "undead";
		} else if (entity == "DeathKnight") {
			return "undead";
		}

        else if (entity == "Militia") {
			return "human";
		} else if (entity == "Archer") {
			return "human";
		} else if (entity == "Longbowman") {
			return "human";
		} else if (entity == "Crossbowman") {
			return "human";
		} else if (entity == "Footman") {
			return "human";
		} else if (entity == "MountedKnight") {
			return "human";
		} else if (entity == "HeroKing") {
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
		} else if (entity == "ArmoredSkeleton") {
			return 200;
		} else if (entity == "DeathKnight") {
			return 250;
		}
		return 0;
	}

	public void AddActivePlayerEntity (GameObject entityObject) {
		activePlayerEntities.Add (entityObject);
	} 

	public void RemoveActivePlayerEntity (GameObject entityObject) {
		activePlayerEntities.Remove (entityObject);
	} 

	public void AddActiveEnemyEntity (GameObject entityObject) {
		activeEnemyEntities.Add (entityObject);
	} 

	public void RemoveActiveEnemyEntity (GameObject entityObject) {
		activeEnemyEntities.Remove (entityObject);
	} 
}
