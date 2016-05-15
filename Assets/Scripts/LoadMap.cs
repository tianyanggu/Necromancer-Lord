using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI;

public class LoadMap : MonoBehaviour {
	public HexGrid hexGrid;
	public Text sizeLabel;
	Canvas gridCanvas;

	public GameObject Necromancer;
	public GameObject Skeleton;
	public GameObject Militia;

	public void LoadEntities () {
		gridCanvas = GetComponentInChildren<Canvas>();

		Vector3 start = hexGrid.GetCellPos(11);
		GameObject playerNecromancer = (GameObject)Instantiate (Necromancer, start, Quaternion.identity);
		playerNecromancer.name = "Necromancer";
		hexGrid.EntityCellIndex (11, "Necromancer");
		playerNecromancer.GetComponent<NecromancerBehaviour> ().size = 1;
		CreateSizeLabel (11, 1, "Necromancer");

		Vector3 militiastart = hexGrid.GetCellPos(12);
		GameObject militia1 = (GameObject)Instantiate (Militia, militiastart, Quaternion.identity);
		militia1.name = "Militia1";
		hexGrid.EntityCellIndex (12, "Militia1");
		militia1.GetComponent<MilitiaBehaviour> ().size = 10;
		CreateSizeLabel (12, 10, "Militia1");

		Vector3 militiastart2 = hexGrid.GetCellPos(15);
		GameObject militia2 = (GameObject)Instantiate (Militia, militiastart2, Quaternion.identity);
		militia2.name = "Militia2";
		hexGrid.EntityCellIndex (15, "Militia2");
		militia2.GetComponent<MilitiaBehaviour> ().size = 15;
		CreateSizeLabel (15, 15, "Militia2");

		Vector3 start2 = hexGrid.GetCellPos(3);
		GameObject playerSkeleton = (GameObject)Instantiate (Skeleton, start2, Quaternion.identity);
		playerSkeleton.name = "Skeleton";
		hexGrid.EntityCellIndex (3, "Skeleton");
		playerSkeleton.GetComponent<SkeletonBehaviour> ().size = 10;
		CreateSizeLabel (3, 10, "Skeleton");


	}

	public void CreateSizeLabel (int index, int size, string entity) {
		Text label = Instantiate<Text>(sizeLabel);
		label.rectTransform.SetParent(gridCanvas.transform, false);
		Vector3 sizepos = hexGrid.GetCellPos (index);
		label.rectTransform.anchoredPosition = new Vector2(sizepos.x, sizepos.z);
		//int? sizestring = playerSkeleton.GetComponent<SkeletonBehaviour> ().size;
		label.text = size.ToString();
		label.name = "Size " + entity;
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
