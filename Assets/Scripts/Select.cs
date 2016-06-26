using UnityEngine;
using System.Collections;

public class Select : MonoBehaviour {
	public HexGrid hexGrid;
	private int currindex;

	public int ChangeTerrain (Color[] colors, Color activeColor) {

		RaycastHit hit;
		Ray inputRay = Camera.main.ScreenPointToRay (Input.mousePosition);

		if (Physics.Raycast (inputRay, out hit, Mathf.Infinity)) {
			if (hit.collider.tag == "HexMesh") {
				currindex = hexGrid.GetCellIndex (hit.point);

					//else if (activeColor == Color.green) {
				if (activeColor == colors [0]) {
					PlayerPrefs.SetString ("Hex" + currindex, "Grass");
					hexGrid.SetTerrain(currindex, "Grass");
					hexGrid.ColorCell (hit.point, activeColor);
				}
					//else if (activeColor == Color.blue) {
					else if (activeColor == colors [1]) {
					PlayerPrefs.SetString ("Hex" + currindex, "Water");
					hexGrid.SetTerrain(currindex, "Water");
					hexGrid.ColorCell (hit.point, activeColor);
				}
					//else if (activeColor == Color.white) {
					else if (activeColor == colors [2]) {
					PlayerPrefs.SetString ("Hex" + currindex, "Mountain");
					hexGrid.SetTerrain(currindex, "Mountain");
					hexGrid.ColorCell (hit.point, activeColor);
				}
			}
			//hexGrid.ColorCell(hit.point, activeColor);
		}
		return currindex;
	}

	public int GetCurrIndex () {

		RaycastHit hit;
		Ray inputRay = Camera.main.ScreenPointToRay (Input.mousePosition);

		if (Physics.Raycast (inputRay, out hit, Mathf.Infinity)) {
			if (hit.collider.tag == "HexMesh") {
				currindex = hexGrid.GetCellIndex (hit.point);
			}
		}
		return currindex;
	}

}
