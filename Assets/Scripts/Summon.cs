using UnityEngine;
using System.Collections;

public class Summon : MonoBehaviour {
	public HexGrid hexGrid;
	public LoadMap loadMap;

	public GameObject Necromancer;
	public GameObject Skeleton;
	public GameObject Militia;

	private string avaliablename;

	public void SummonEntity (int size, int cellindex, string summonname) {
		Vector3 summonindex = hexGrid.GetCellPos(cellindex);
		avaliablename = AvailableName (summonname);
		GameObject playerentity = (GameObject)Instantiate (Skeleton, summonindex, Quaternion.identity);
		if (summonname == "Skeleton") {
			playerentity.GetComponent<SkeletonBehaviour> ().size = size;
		}
		playerentity.name = avaliablename;
		hexGrid.EntityCellIndex (cellindex, avaliablename);
		loadMap.CreateSizeLabel (cellindex, size, avaliablename);


	}

	//CHECK FOR NEXT AVALIABLE NUMBER
	string AvailableName (string summonname) {
		for (int i = 1; i <= 99; i++) {
			string num = i.ToString ();
			if (GameObject.Find (summonname) == null) {
				avaliablename = summonname;
				return avaliablename;
			} else if (GameObject.Find (summonname + num) == null) {
				avaliablename = summonname + num;
				Debug.Log (avaliablename);
				return avaliablename;
			} else {
				Debug.Log ("Inform Max is 99");
			}
		} 
		//split for optimization if over 9
//		for (int i = 10; i <= 999; i++) {
//			string num = i.ToString ();
//			if (GameObject.Find (summonname) == null) {
//				avaliablename = summonname;
//				return avaliablename;
//			} else if (GameObject.Find (summonname + num) == null) {
//				avaliablename = summonname + num;
//				Debug.Log (avaliablename);
//				return avaliablename;
//			}
//		}

		return null;
	}

}
