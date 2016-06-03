using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Summon : MonoBehaviour {
	public HexGrid hexGrid;
	public LoadMap loadMap;
	public EntityStorage entityStorage;

	public GameObject Necromancer;
	public GameObject Skeleton;
	public GameObject Militia;

	private string availableName;

	public void SummonEntity (int size, int cellindex, string summonname) {
		Vector3 summonindex = hexGrid.GetCellPos(cellindex);
		availableName = AvailableName (summonname);
		GameObject playerentity = (GameObject)Instantiate (Skeleton, summonindex, Quaternion.identity);
		if (summonname == "Skeleton") {
			playerentity.GetComponent<SkeletonBehaviour> ().size = size;
		}
		playerentity.name = availableName;
		hexGrid.EntityCellIndex (cellindex, availableName);
		loadMap.CreateSizeLabel (cellindex, size, availableName);
	}

	//CHECK FOR NEXT AVALIABLE NUMBER
	string AvailableName (string summonname) {
		for (int i = 1; i <= 999; i++) {
			string num = i.ToString ();

			if (!entityStorage.activeEntities.Contains(summonname + num)) {
				availableName = summonname + num;
				entityStorage.AddActiveEntity (availableName);
				return availableName;
			} else if (num == "999") {
				//TODO error message of 999 max reached
			}
		} 
		return null;
	}

}
