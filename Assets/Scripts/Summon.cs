using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Summon : MonoBehaviour {
	public HexGrid hexGrid;
	public LoadMap loadMap;
	public EntityStorage entityStorage;
	public Resources resources;

	public GameObject Necromancer;
	public GameObject Skeleton;
	public GameObject Zombie;
    public GameObject SkeletonArcher;
    public GameObject ArmoredSkeleton;
    public GameObject DeathKnight;

    public GameObject Militia;
    public GameObject Archer;
    public GameObject Longbowman;
    public GameObject Crossbowman;
    public GameObject Footman;
    public GameObject MountedKnight;
    public GameObject HeroKing;

	//given an index and the type of summon, summons that entity with the next available name
	public void SummonEntity (int cellindex, string summonname) {
		Vector3 summonindex = hexGrid.GetCellPos(cellindex);
		summonindex.y = 0.2f;
		string faction = entityStorage.whichFaction (summonname);
		string availableNum = AvailableName (summonname, faction);
		string availableName = summonname + availableNum;
		int health = GetHealthInfo (summonname);

		if (summonname == "Skeleton") {
			GameObject playerentity = (GameObject)Instantiate (Skeleton, summonindex, Quaternion.identity);
			playerentity.name = availableName;
		} else if (summonname == "Necromancer") {
			GameObject playerentity = (GameObject)Instantiate (Necromancer, summonindex, Quaternion.identity);
			playerentity.name = availableName;
		} else if (summonname == "Zombie") {
			GameObject playerentity = (GameObject)Instantiate (Zombie, summonindex, Quaternion.identity);
			playerentity.name = availableName;
		} else if (summonname == "SkeletonArcher") {
			GameObject enemyentity = (GameObject)Instantiate (SkeletonArcher, summonindex, Quaternion.identity);
			enemyentity.name = availableName;
		} else if (summonname == "ArmoredSkeleton") {
			GameObject enemyentity = (GameObject)Instantiate (ArmoredSkeleton, summonindex, Quaternion.identity);
			enemyentity.name = availableName;
		} else if (summonname == "DeathKnight") {
			GameObject enemyentity = (GameObject)Instantiate (DeathKnight, summonindex, Quaternion.identity);
			enemyentity.name = availableName;
		}

        else if (summonname == "Militia") {
			GameObject enemyentity = (GameObject)Instantiate (Militia, summonindex, Quaternion.identity);
			enemyentity.name = availableName;
		} else if (summonname == "Archer") {
			GameObject enemyentity = (GameObject)Instantiate (Archer, summonindex, Quaternion.identity);
			enemyentity.name = availableName;
		} else if (summonname == "Longbowman") {
			GameObject enemyentity = (GameObject)Instantiate (Longbowman, summonindex, Quaternion.identity);
			enemyentity.name = availableName;
		} else if (summonname == "Crossbowman") {
			GameObject enemyentity = (GameObject)Instantiate (Crossbowman, summonindex, Quaternion.identity);
			enemyentity.name = availableName;
		} else if (summonname == "Footman") {
			GameObject enemyentity = (GameObject)Instantiate (Footman, summonindex, Quaternion.identity);
			enemyentity.name = availableName;
		} else if (summonname == "MountedKnight") {
			GameObject enemyentity = (GameObject)Instantiate (MountedKnight, summonindex, Quaternion.identity);
			enemyentity.name = availableName;
		} else if (summonname == "HeroKing") {
			GameObject enemyentity = (GameObject)Instantiate (HeroKing, summonindex, Quaternion.identity);
			enemyentity.name = availableName;
		}
		//stores info of new summon to playerprefs for saving
		string ppName = AvailablePlayerPrefsName ();

		PlayerPrefs.SetString ("HexEntity" + ppName, availableName);
		PlayerPrefs.SetString (availableName, "HexEntity" + ppName);
		PlayerPrefs.SetInt ("HexEntityHealth" + ppName, health);
		PlayerPrefs.SetInt ("HexEntityIndex" + ppName, cellindex);
		hexGrid.SetEntity (cellindex, availableName);
		loadMap.CreateHealthLabel (cellindex, health, availableName);
	}

	//Check for next available entity number
	string AvailableName (string summonname, string faction) {
		for (int i = 1; i <= 999; i++) {
			string num = i.ToString ();
			if (faction == "undead") {
				if (!entityStorage.activePlayerEntities.Contains (summonname + num)) {
					string availableName = summonname + num;
					entityStorage.AddActivePlayerEntity (availableName);
					return num;
				}
			}
			if (faction == "human") {
				if (!entityStorage.activeEnemyEntities.Contains (summonname + num)) {
					string availableName = summonname + num;
					entityStorage.AddActiveEnemyEntity (availableName);
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
	int GetHealthInfo(string entity) {
		GameObject sumentity = GameObject.Find (entity);

		//------Grab Info Attacker------
		if (entity == "Zombie") {
			return sumentity.GetComponent<ZombieBehaviour> ().health;
		} else if (entity == "Skeleton") {
			return sumentity.GetComponent<SkeletonBehaviour> ().health;
		} else if (entity == "Necromancer") {
			return sumentity.GetComponent<NecromancerBehaviour> ().health;
		} else if (entity == "SkeletonArcher") {
			return sumentity.GetComponent<SkeletonArcherBehaviour> ().health;
		} else if (entity == "ArmoredSkeleton") {
			return sumentity.GetComponent<ArmoredSkeletonBehaviour> ().health;
		} else if (entity == "DeathKnight") {
			return sumentity.GetComponent<DeathKnightBehaviour> ().health;
		}

        else if (entity == "Militia") {
			return sumentity.GetComponent<MilitiaBehaviour> ().health;
		}  else if (entity == "Archer") {
			return sumentity.GetComponent<ArcherBehaviour> ().health;
		}  else if (entity == "Longbowman") {
			return sumentity.GetComponent<LongbowmanBehaviour> ().health;
		}  else if (entity == "Crossbowman") {
			return sumentity.GetComponent<CrossbowmanBehaviour> ().health;
		}  else if (entity == "Footman") {
			return sumentity.GetComponent<FootmanBehaviour> ().health;
		}  else if (entity == "MountedKnight") {
			return sumentity.GetComponent<MountedKnightBehaviour> ().health;
		}  else if (entity == "HeroKing") {
			return sumentity.GetComponent<HeroKingBehaviour> ().health;
		} 
		return 0;
	}

	public bool ValidSummon(string entity) {
		int souls = PlayerPrefs.GetInt ("Souls");
		int cost = entityStorage.summonSoulCost (entity);
		if (souls >= cost) {
			resources.ChangeSouls (-cost);
			return true;
		}
		return false;
	}
}
