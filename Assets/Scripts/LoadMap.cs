using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class LoadMap : MonoBehaviour {
	public HexGrid hexGrid;

	public GameObject Necromancer;
	public GameObject Skeleton;
	public GameObject Militia;

	public void LoadEntities () {
		
		Vector3 start = hexGrid.GetCellPos(11);
		GameObject playerNecromancer = (GameObject)Instantiate (Necromancer, start, Quaternion.identity);
		playerNecromancer.name = "Necromancer";
		hexGrid.EntityCellIndex (11, "Necromancer");

		Vector3 start2 = hexGrid.GetCellPos(3);
		GameObject playerSkeleton = (GameObject)Instantiate (Skeleton, start2, Quaternion.identity);
		playerSkeleton.name = "Skeleton";
		hexGrid.EntityCellIndex (3, "Skeleton");

		Vector3 militiastart = hexGrid.GetCellPos(12);
		GameObject militia1 = (GameObject)Instantiate (Militia, militiastart, Quaternion.identity);
		militia1.name = "Militia1";
		hexGrid.EntityCellIndex (12, "Militia1");

		Vector3 militiastart2 = hexGrid.GetCellPos(15);
		GameObject militia2 = (GameObject)Instantiate (Militia, militiastart2, Quaternion.identity);
		militia2.name = "Militia2";
		hexGrid.EntityCellIndex (15, "Militia2");

	}

	public void LoadTerrain () {
		
		for (int i = 0; i < 144; i++) {
			string allcolor = PlayerPrefs.GetString ("Hex" + i);

			if (allcolor == "Village") {
				hexGrid.ColorCellIndex (i, Color.yellow);
			}
			if (allcolor == "Grass") {
				hexGrid.ColorCellIndex (i, Color.green);
			}
			if (allcolor == "Water") {
				hexGrid.ColorCellIndex (i, Color.blue);
			}
			if (allcolor == "Mountain") {
				hexGrid.ColorCellIndex (i, Color.red);
			}
		}
	}
}
