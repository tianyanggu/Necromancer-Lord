using UnityEngine;
using System.Collections;

public class Summon : MonoBehaviour {
	public HexGrid hexGrid;

	public GameObject Necromancer;
	public GameObject Skeleton;
	public GameObject Militia;

	public void SummonEntity (int size, int cellindex, string summonname) {

		Vector3 summonindex = hexGrid.GetCellPos(cellindex);
		GameObject playerentity = (GameObject)Instantiate (Skeleton, summonindex, Quaternion.identity);
		playerentity.name = summonname;
		hexGrid.EntityCellIndex (cellindex, summonname);


	}

}
