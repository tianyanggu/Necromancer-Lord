using UnityEngine;
using UnityEngine.EventSystems;

public class HexMapEditor : MonoBehaviour {

	public Color[] colors;

	public HexGrid hexGrid;

	private Color activeColor;

	public int selected;
	public string selectedentity;

	public GameObject Necromancer;
	public GameObject Militia;

	void Awake () {
		SelectColor(0);

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

		Vector3 start = hexGrid.GetCellPos(11);
		GameObject playerNecromancer = (GameObject)Instantiate (Necromancer, start, Quaternion.identity);
		playerNecromancer.name = "Necromancer";
		hexGrid.EntityCellIndex (11, "Necromancer");

		Vector3 militiastart = hexGrid.GetCellPos(12);
		GameObject militia1 = (GameObject)Instantiate (Militia, militiastart, Quaternion.identity);
		militia1.name = "Militia1";
		hexGrid.EntityCellIndex (12, "Militia1");

		Vector3 militiastart2 = hexGrid.GetCellPos(15);
		GameObject militia2 = (GameObject)Instantiate (Militia, militiastart2, Quaternion.identity);
		militia2.name = "Militia2";
		hexGrid.EntityCellIndex (15, "Militia2");
	}

	void Update () {
		if (
			Input.GetMouseButton(0) &&
			!EventSystem.current.IsPointerOverGameObject()
		) {
			//Vector3 pos = new Vector3(50.3f, 0.0f, 25.0f);
			//hexGrid.ColorCell(pos, activeColor);

			//coordy equals -2 -3 which equals -5
			//use coord x and z for coordinates
			//hexGrid.ColorCellCoordinates(2, 3, activeColor);
			//hexGrid.ColorCellCoordinates(3, 3, activeColor);

			//Vector3 pos2 = poslist[1];
			//hexGrid.ColorCell(pos2, activeColor);
			HandleInput();
		}
	}

	void HandleInput () {
		Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(inputRay, out hit)) {
			Color currcolor = hexGrid.GetCellColor(hit.point);
			int currindex = hexGrid.GetCellIndex(hit.point);

			if (activeColor == colors[0]) {
				PlayerPrefs.SetString ("Hex" + currindex, "Village");
				hexGrid.ColorCell(hit.point, activeColor);
			}
			//else if (activeColor == Color.green) {
			else if (activeColor == colors[1]) {
				PlayerPrefs.SetString ("Hex" + currindex, "Grass");
				hexGrid.ColorCell(hit.point, activeColor);
			}
			//else if (activeColor == Color.blue) {
			else if (activeColor == colors[2]) {
				PlayerPrefs.SetString ("Hex" + currindex, "Water");
				hexGrid.ColorCell(hit.point, activeColor);
			}
			//else if (activeColor == Color.white) {
			else if (activeColor == colors[3]) {
				PlayerPrefs.SetString ("Hex" + currindex, "Mountain");
				hexGrid.ColorCell(hit.point, activeColor);
			}

			//hexGrid.ColorCell(hit.point, activeColor);


			//-----Selector--------------
			Vector3 cellcoord = hexGrid.GetCellPos(currindex);
			//Instantiate (Necromancer, cellcoord, Quaternion.identity);

			string currEntity = hexGrid.GetEntity(currindex);
			if (currEntity == "Necromancer") {
				selected = currindex;
				selectedentity = "Necromancer";
			}
			if (currEntity != "Necromancer") {
				GameObject playerNecromancer = GameObject.Find("Necromancer");
				playerNecromancer.transform.position = cellcoord;

				GameObject defender = GameObject.Find(currEntity);
				Destroy (defender);
				hexGrid.EntityCellIndex (currindex, "Empty");
			}


 			//playerNecromancer.GetComponent<NecromancerBehaviour> ().health = playerNecromancer.GetComponent<NecromancerBehaviour> ().health - 2;

		}
	}

	public void SelectColor (int index) {
		activeColor = colors[index];
	}
}