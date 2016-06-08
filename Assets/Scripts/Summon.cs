using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Summon : MonoBehaviour {
	public HexGrid hexGrid;
	public LoadMap loadMap;
	public EntityStorage entityStorage;

	public GameObject Necromancer;
	public GameObject Skeleton;
	public GameObject Zombie;
	public GameObject Militia;

	//given an index and the type of summon, summons that entity with the next available name
	public void SummonEntity (int cellindex, string summonname) {
		Vector3 summonindex = hexGrid.GetCellPos(cellindex);
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
		} else if (summonname == "Militia") {
			GameObject enemyentity = (GameObject)Instantiate (Militia, summonindex, Quaternion.identity);
			enemyentity.name = availableName;
		}
		//stores info of new summon to playerprefs for saving
		string ppName = AvailablePlayerPrefsName ();

		PlayerPrefs.SetString ("HexEntity" + ppName, availableName);
		PlayerPrefs.SetString (availableName, "HexEntity" + ppName);
		PlayerPrefs.SetInt ("HexEntityHealth" + ppName, health);
		PlayerPrefs.SetInt ("HexEntityIndex" + ppName, cellindex);
		hexGrid.EntityCellIndex (cellindex, availableName);
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
		} else if (entity == "Militia") {
			return sumentity.GetComponent<MilitiaBehaviour> ().health;
		}
		return 0;
	}
}
