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
	public Build build;
	public Locate locate;
	public EntityStorage entityStorage;
	public BuildingStorage buildingStorage;
	public AIBehaviour aiBehaviour;

	public int currindex;

	public int selectedindex;
	public string selectedentity;

	public bool lockbattle;
	public bool editmode;

	public int turn;

	private bool summonclickededitor;
	private bool summonclicked;
	private bool buildingclicked;


	void Awake () {
		SelectColor(0);
		lockbattle = false;
		editmode = false;
		loadMap.LoadHexTiles ();
		loadMap.LoadTerrain ();
		loadMap.LoadBuildings ();
		loadMap.LoadEntities ();
		loadMap.LoadResources ();
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
			HandleInput ();
		}
	}

	void HandleInput () {
		if (editmode == true) {
			currindex = select.ChangeTerrain (colors, activeColor);
		} else {
			currindex = select.GetCurrIndex ();
		}

		//-----Selector--------------
		//Debug.Log(currindex);
		string currEntity = hexGrid.GetEntity (currindex);
		string cleanCurrEntity = Regex.Replace(currEntity, @"[\d-]", string.Empty);

		if (entityStorage.playerEntities.Contains (cleanCurrEntity)) {
			selectedindex = currindex;
			selectedentity = currEntity;
			lockbattle = false;
		} 
		if (lockbattle == false) {
			bool checkAttHappen = battle.Attack (selectedindex, currindex);
			if (checkAttHappen == true) {
				lockbattle = true;
			}
		}
	}

	public void SelectColor (int index) {
		activeColor = colors[index];
	}

	void OnGUI () {
		// Make a background box
		//x position, y position, width, length
		GUI.Box(new Rect(10,120,140,150), "Menu");

		//drop down menu after summon for various entities, non-editor with validation for souls
		string currEntity = hexGrid.GetEntity(currindex);
		if (currEntity == "Empty") {
			if (GUI.Button (new Rect (20, 150, 120, 20), "Summon")) {
				summonclicked = true;
			}
		}
		if (summonclicked) {
			int i = 0;
			foreach (string entity in entityStorage.playerEntities) {
				int spacing = i * 20;
				if (GUI.Button (new Rect (150, 150 + spacing, 120, 20), "Summon" + entity)) {
					bool validsummon = summon.ValidSummon (entity);
					if (validsummon) {
						summon.SummonEntity (currindex, entity);
					}
					summonclicked = false;
				}
				i++;
			}
		}
		//drop down menu after summon for various entities
		if (currEntity == "Empty") {
			if (GUI.Button (new Rect (20, 180, 120, 20), "Summon")) {
				summonclickededitor = true;
			}
		}
		if (summonclickededitor) {
			int i = 0;
			foreach (string entity in entityStorage.playerEntities) {
				int spacing = i * 20;
				if (GUI.Button (new Rect (150, 150 + spacing, 120, 20), "Summon" + entity)) {
					if (editmode == true) {
						summon.SummonEntity (currindex, entity);
						summonclicked = false;
					}
				}
				i++;
			}
			foreach (string entity in entityStorage.enemyEntities) {
				int spacing = i * 20;
				if (GUI.Button (new Rect (150, 150 + spacing, 120, 20), "Summon" + entity)) {
					if (editmode == true) {
						summon.SummonEntity (currindex, entity);
						summonclicked = false;
					}
				}
				i++;
			}
		}

		//drop down menu after summon for various buildings
		string currBuilding = hexGrid.GetBuilding(currindex);
		if (currBuilding == "Empty") {
			if (GUI.Button (new Rect (20, 210, 120, 20), "Building")) {
				buildingclicked = true;
			}
		}
		if (buildingclicked) {
			int i = 0;
			foreach (string building in buildingStorage.playerBuildings) {
				int spacing = i * 20;
				if (GUI.Button (new Rect (150, 150 + spacing, 120, 20), "Building " + building)) {
					bool validbuilding = build.ValidBuilding (building);
					if (validbuilding) {
						build.BuildBuilding (currindex, building);
					}
					buildingclicked = false;
				}
				i++;
			}
		}

		//toggles editor mode
		if(GUI.Button(new Rect(20,240,120,20), "Toggle Map Edit")) {
			if (editmode == false) {
				editmode = true;
			} else {
				editmode = false;
			}
		}

		//determine if all troops moved and turn can end
		string turnstring = turn.ToString ();
		if(GUI.Button(new Rect(30,330,60,60), turnstring)) {
			bool checkall = locate.CheckAll ();
			if (checkall == true) {
				turn++;
				//add points back to units
				locate.SetAllActive();
				locate.SetAllMovementPoints();
				locate.SetAllAttackPoints();

				//enemy units turn
				//aiBehaviour.Actions(15);
			}
		}

		//sets remaining units idle
		if(GUI.Button(new Rect(30,300,60,20), "Set All Idle")) {
			locate.SetAllIdle ();
		}
	}
}

//TODO refactor lists of entities,  battles, and load map