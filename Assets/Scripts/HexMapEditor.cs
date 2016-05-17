using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class HexMapEditor : MonoBehaviour {

	public Color[] colors;
	private Color activeColor;

	public HexGrid hexGrid;
	public Select select;
	public LoadMap loadMap;
	public Battle battle;
	public Summon summon;

	public int currindex;

	public int selectedindex;
	public string selectedentity;
	public string actionpoint;

	public bool lockbattle;

	void Awake () {
		SelectColor(0);
		lockbattle = false;
		loadMap.LoadHexTiles ();

		loadMap.LoadTerrain ();

		loadMap.LoadEntities ();

//		List<int> test = hexGrid.GetCellIndexesOneHexAway (28);
//		int test0 = test [0];
//		int test1 = test [1];
//		int test2 = test [2];
//		int test3 = test [3];
//		int test4 = test [4];
//		int test5 = test [5];
//		//int test6 = test [6];
//		Debug.Log (test0);
//		Debug.Log (test1);
//		Debug.Log (test2);
//		Debug.Log (test3);
//		Debug.Log (test4);
//		Debug.Log (test5);
//		//Debug.Log (test6);
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
		currindex = select.GetCellIndex (colors, activeColor);
		Debug.Log (currindex);
		//----PlayerEntities----------
		List<string> playerEntities = new List<string> ();
		playerEntities.Add ("Necromancer");
		playerEntities.Add ("Skeleton");

		//-----Selector--------------
		string currEntity = hexGrid.GetEntity (currindex);
		string cleanCurrEntity = Regex.Replace(currEntity, @"[\d-]", string.Empty);

		if (playerEntities.Contains (cleanCurrEntity)) {
			selectedindex = currindex;
			selectedentity = currEntity;
			lockbattle = false;
		} 
		if (lockbattle == false) {
			bool checkAttHappen = battle.Attack (selectedindex, currindex, selectedentity);
			if (checkAttHappen == true) {
				lockbattle = true;
			}
		}
		//Debug.Log (lockbattle);
	}

	public void SelectColor (int index) {
		activeColor = colors[index];
	}

	void OnGUI () {
		// Make a background box
		//x position, y position, width, length
		GUI.Box(new Rect(10,120,140,90), "Loader Menu");

		if(GUI.Button(new Rect(20,150,120,20), "Summon Skeleton")) {
			summon.SummonEntity(1,currindex,"Skeleton");
		}

	}
}

//TODO refactor lists of entities,  battles, and load map