using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
	public bool editmode;

	private string summontextfieldeditor = "";
	private string summontextfield = "";
	private int summonquantityeditor;
	private int summonquantity;


	void Awake () {
		SelectColor(0);
		lockbattle = false;
		editmode = false;
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
		if (editmode == true) {
			currindex = select.ChangeTerrain (colors, activeColor);
		} else {
			currindex = select.GetCurrIndex ();
		}
		//----PlayerEntities----------
		List<string> playerEntities = new List<string> ();
		playerEntities.Add ("Necromancer");
		playerEntities.Add ("Skeleton");
		playerEntities.Add ("Zombie");

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
		GUI.Box(new Rect(10,120,140,150), "Menu");

		int availablecorpses = hexGrid.GetCorpses (currindex);

		if (editmode == true) {
			//		GUIStyle summoninputstyle = new GUIStyle();
			//		summoninputstyle.onActive.textColor = Color.yellow;
			summontextfieldeditor = GUI.TextField (new Rect (20, 170, 120, 20), summontextfieldeditor, 5);
			summontextfieldeditor = Regex.Replace (summontextfieldeditor, "[^0-9 -]", string.Empty);
			int summontextfieldnumeditor = 0;
			Int32.TryParse (summontextfieldeditor, out summontextfieldnumeditor);
			summonquantityeditor = summontextfieldnumeditor;
		}
		summontextfield = GUI.TextField (new Rect (20, 250, 120, 20), summontextfield, 5);
		summontextfield = Regex.Replace (summontextfield, "[^0-9 -]", string.Empty);
		int summontextfieldnum = 0;
		Int32.TryParse (summontextfield, out summontextfieldnum);
		if (summontextfieldnum > availablecorpses) {
			summonquantity = availablecorpses;
			string availablecorpsesstring = availablecorpses.ToString ();
			summontextfield = availablecorpsesstring;
		} else {
			summonquantity = summontextfieldnum;
		}

		if(GUI.Button(new Rect(20,150,120,20), "Summon Skeleton")) {
			if (editmode == true) {
				if (summonquantityeditor > 0) {
					summon.SummonEntity (summonquantityeditor, currindex, "Skeleton");
				}
			} else {
				if (summonquantity > 0) {
					summon.SummonEntity (summonquantity, currindex, "Skeleton");
				}
			}
		}
			
		if(GUI.Button(new Rect(20,220,120,20), "Toggle Map Edit")) {
			if (editmode == false) {
				editmode = true;
			} else {
				editmode = false;
			}
		}

		//editor summon buttons
		if(GUI.Button(new Rect(20,190,40,20), "+1")) {
			int addone = (summonquantityeditor + 1);
			string addonetext = addone.ToString ();
			summontextfieldeditor = addonetext;
		}
		if(GUI.Button(new Rect(50,190,40,20), "+10")) {
			int addten = (summonquantityeditor + 10);
			string addtentext = addten.ToString ();
			summontextfieldeditor = addtentext;
		}
		if(GUI.Button(new Rect(90,190,40,20), "+100")) {
			int addhundred = (summonquantityeditor + 100);
			string addhundredtext = addhundred.ToString ();
			summontextfieldeditor = addhundredtext;
		}
		//summon buttons
		if(GUI.Button(new Rect(20,270,40,20), "+1")) {
			int addone = (summonquantity + 1);
			string addonetext = addone.ToString ();
			if (availablecorpses < addone) {
				string availablecorpsesstring = availablecorpses.ToString ();
				summontextfield = availablecorpsesstring;
			} else {
				summontextfield = addonetext;
			}
		}
		if(GUI.Button(new Rect(50,270,40,20), "+10")) {
			int addten = (summonquantity + 10);
			string addtentext = addten.ToString ();
			if (availablecorpses < addten) {
				string availablecorpsesstring = availablecorpses.ToString ();
				summontextfield = availablecorpsesstring;
			} else {
				summontextfield = addtentext;
			}
		}
		if(GUI.Button(new Rect(90,270,40,20), "+100")) {
			int addhundred = (summonquantity + 100);
			string addhundredtext = addhundred.ToString ();
			if (availablecorpses < addhundred) {
				string availablecorpsesstring = availablecorpses.ToString ();
				summontextfield = availablecorpsesstring;
			} else {
				summontextfield = addhundredtext;
			}
		}
	}
}

//TODO refactor lists of entities,  battles, and load map