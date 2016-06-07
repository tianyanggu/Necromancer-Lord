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

	public void SummonEntity (int cellindex, string summonname) {
		Vector3 summonindex = hexGrid.GetCellPos(cellindex);
		string availableNum = AvailableName (summonname);
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
		}

		PlayerPrefs.SetString (availableName, "HexEntity" + availableNum);
		PlayerPrefs.SetString ("HexEntity" + availableNum, availableName);
		PlayerPrefs.SetInt ("HexEntityHealth" + availableNum, health);
		PlayerPrefs.SetInt ("HexEntityIndex" + availableNum, cellindex);
		hexGrid.EntityCellIndex (cellindex, availableName);
		loadMap.CreateHealthLabel (cellindex, health, availableName);
	}

	//CHECK FOR NEXT AVALIABLE NUMBER
	string AvailableName (string summonname) {
		for (int i = 1; i <= 999; i++) {
			string num = i.ToString ();

			if (!entityStorage.activePlayerEntities.Contains(summonname + num)) {
				string availableName = summonname + num;
				entityStorage.AddActivePlayerEntity (availableName);
				return num;
			} else if (num == "999") {
				//TODO error message of 999 max reached
			}
		} 
		return null;
	}

	//grabs health info
	int GetHealthInfo(string entity) {
		GameObject sumentity = GameObject.Find (entity);

		//------Grab Info Attacker------
		if (entity == "Zombie") {
			return sumentity.GetComponent<ZombieBehaviour> ().health;
		} else if (entity == "Skeleton") {
			return sumentity.GetComponent<SkeletonBehaviour> ().health;;
		} else if (entity == "Necromancer") {
			return sumentity.GetComponent<NecromancerBehaviour> ().health;
		} else if (entity == "Militia") {
			return sumentity.GetComponent<MilitiaBehaviour> ().health;
		}
		return 0;
	}

}
