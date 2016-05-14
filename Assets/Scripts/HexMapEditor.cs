using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class HexMapEditor : MonoBehaviour {

	public Color[] colors;

	public Select select;
	public HexGrid hexGrid;
	public LoadMap loadMap;
	public Battle battle;

	private Color activeColor;

	public int currindex;

	public int selectedindex;
	public string selectedentity;
	public string actionpoint;

	void Awake () {
		SelectColor(0);

		loadMap.LoadTerrain ();

		loadMap.LoadEntities ();
	}

	void FixedUpdate () {
		if (Input.GetMouseButton (0) && !EventSystem.current.IsPointerOverGameObject ()) {
			//Vector3 pos = new Vector3(50.3f, 0.0f, 25.0f);
			//hexGrid.ColorCell(pos, activeColor);

			//coordy equals -2 -3 which equals -5
			//use coord x and z for coordinates
			//hexGrid.ColorCellCoordinates(2, 3, activeColor);
			//hexGrid.ColorCellCoordinates(3, 3, activeColor);

			//Vector3 pos2 = poslist[1];
			//hexGrid.ColorCell(pos2, activeColor);
			HandleInput ();
		}
	}

	void HandleInput () {
		currindex = select.GetCellIndex (colors);

		//----PlayerEntities----------
		List<string> playerEntities = new List<string> ();
		playerEntities.Add ("Necromancer");
		playerEntities.Add ("Skeleton");


		//-----Selector--------------
		string currEntity = hexGrid.GetEntity (currindex);
		if (playerEntities.Contains (currEntity)) {
			selectedindex = currindex;
			selectedentity = currEntity;
		} 
		battle.Attack (selectedindex, currindex, selectedentity);
	}

	public void SelectColor (int index) {
		activeColor = colors[index];
	}
}

//TODO refactor lists of entities,  battles, and load map