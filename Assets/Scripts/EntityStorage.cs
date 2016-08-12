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
        switch (entity)
        {
            case "Zombie":
                return "undead";
            case "Skeleton":
                return "undead";
            case "Necromancer":
                return "undead";
            case "SkeletonArcher":
                return "undead";
            case "ArmoredSkeleton":
                return "undead";
            case "DeathKnight":
                return "undead";

            case "Militia":
                return "human";
            case "Archer":
                return "human";
            case "Longbowman":
                return "human";
            case "Crossbowman":
                return "human";
            case "Footman":
                return "human";
            case "MountedKnight":
                return "human";
            case "HeroKing":
                return "human";
        }
		return "unknown";
	}

	//returns summon soul cost
	public int summonSoulCost(string entity) {
        //------Determine Cost------
        switch (entity)
        {
            case "Zombie":
                return 100;
            case "Skeleton":
                return 150;
            case "Necromancer":
                return 10000;
            case "SkeletonArcher":
                return 150;
            case "ArmoredSkeleton":
                return 200;
            case "DeathKnight":
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
