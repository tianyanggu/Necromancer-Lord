using UnityEngine;
using System.Collections;

public class Select : MonoBehaviour {
	public HexGrid hexGrid;
	private Color activeColor;
	private int currindex;

	public int GetCellIndex (Color[] colors) {
		
		Ray inputRay = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast (inputRay, out hit)) {
			currindex = hexGrid.GetCellIndex (hit.point);

			if (activeColor == colors [0]) {
				PlayerPrefs.SetString ("Hex" + currindex, "Village");
				hexGrid.ColorCell (hit.point, activeColor);
			}
			//else if (activeColor == Color.green) {
			else if (activeColor == colors [1]) {
				PlayerPrefs.SetString ("Hex" + currindex, "Grass");
				hexGrid.ColorCell (hit.point, activeColor);
			}
			//else if (activeColor == Color.blue) {
			else if (activeColor == colors [2]) {
				PlayerPrefs.SetString ("Hex" + currindex, "Water");
				hexGrid.ColorCell (hit.point, activeColor);
			}
			//else if (activeColor == Color.white) {
			else if (activeColor == colors [3]) {
				PlayerPrefs.SetString ("Hex" + currindex, "Mountain");
				hexGrid.ColorCell (hit.point, activeColor);
			}

			//hexGrid.ColorCell(hit.point, activeColor);
		}
		return currindex;
	}

}
