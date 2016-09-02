using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Text.RegularExpressions;
using System.Collections.Generic;

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
    public BuildingManager buildingManager;
    public PlayerManager playerManager;

	public int currindex;

	public int selectedindex;
    public string selectedBuilding;

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
		loadMap.LoadCorpses ();
        //sets the seed of the terrain spawn
        //loadMap.LoadRandom (12);

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

        //TODO Overlay to add players
        //PlayerPrefs.SetString("Player1", "AA");
        //PlayerPrefs.SetString("Player2", "BB");
        //PlayerPrefs.SetString("Player3", "CA");
        //PlayerPrefs.SetString("AA", "undead");
        //PlayerPrefs.SetString("BB", "human");
        //PlayerPrefs.SetString("CA", "human");
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
		//string currEntityName = hexGrid.GetEntityName (currindex);
        GameObject currEntityObj = hexGrid.GetEntityObject(currindex);
        string currBuilding = hexGrid.GetBuildingName (currindex);
		//string cleanCurrEntity = Regex.Replace(currEntityName.Substring(2), @"[\d-]", string.Empty);
        string cleanCurrBuilding = Regex.Replace(currBuilding.Substring(2), @"[\d-]", string.Empty);

        char playerChar = playerManager.currPlayer[0];
        if (entityStorage.PlayerEntityList(playerChar).Contains (currEntityObj)) {
			selectedindex = currindex;
            //TODO list info for curr entity, display it
			lockbattle = false;
		}
        if (buildingStorage.playerBuildings.Contains(cleanCurrBuilding)) {
            selectedBuilding = currBuilding;
            buildingManager.DisplayBuilding(selectedBuilding, currindex);
            //TODO GUI for buildings
        }
        //ensures attacks only happen once per update 
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
        //TODO show which player is currently active
		// Make a background box
		//x position, y position, width, length
		GUI.Box(new Rect(10,120,140,150), "Menu");

		//drop down menu after summon for various entities, non-editor with validation for souls
		string currEntityName = hexGrid.GetEntityName(currindex);
		if (currEntityName == "Empty") {
			if (GUI.Button (new Rect (20, 150, 120, 20), "Summon")) {
                if (summonclicked == false) {
                    summonclicked = true;
                } else {
                    summonclicked = false;
                }
			}
		}
		if (summonclicked) {
			int i = 0;
            string playerFaction = PlayerPrefs.GetString(playerManager.currPlayer);
            foreach (string entity in entityStorage.FactionLists(playerFaction)) {
				int spacing = i * 20;
				if (GUI.Button (new Rect (150, 150 + spacing, 120, 20), "Summon" + entity)) {
					bool validsummon = summon.ValidSummon (entity);
					if (validsummon) {
                        summon.SummonEntity (currindex, entity, playerManager.currPlayer);
					}
					summonclicked = false;
				}
				i++;
			}
		}
		//drop down menu after summon for various entities
		if (currEntityName == "Empty") {
			if (GUI.Button (new Rect (20, 180, 120, 20), "Summon")) {
                if (summonclickededitor == false) {
                    summonclickededitor = true;
                } else {
                    summonclickededitor = false;
                }
			}
		}
		if (summonclickededitor) {
			int i = 0;
            foreach (List<string> factionEntities in entityStorage.factionEntityList)
            {
                foreach (string entity in factionEntities)
                {
                    int spacing = i * 20;
                    if (GUI.Button(new Rect(150, 150 + spacing, 120, 20), "Summon" + entity))
                    {
                        if (editmode == true)
                        {
                            summon.SummonEntity(currindex, entity, playerManager.currPlayer);
                            summonclickededitor = false;
                        }
                    }
                    i++;
                }
			}
		}

		//drop down menu after summon for various buildings
		string currBuildingName = hexGrid.GetBuildingName(currindex);
		if (currBuildingName == "Empty") {
			if (GUI.Button (new Rect (20, 210, 120, 20), "Building")) {
				if (buildingclicked == false) {
                    buildingclicked = true;
                } else {
                    buildingclicked = false;
                }
			}
		}
		if (buildingclicked) {
			int i = 0;
			foreach (string building in buildingStorage.playerBuildings) {
				int spacing = i * 20;
				if (GUI.Button (new Rect (150, 150 + spacing, 120, 20), "Building " + building)) {
					bool validbuilding = build.ValidBuilding (building, currindex);
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
            char currPlayer = playerManager.currPlayer[0];
            bool checkall = locate.CheckAllPoints (currPlayer);
			if (checkall == true) {
				turn++;
                //ensures if player currently selects some entity, they can`t move it after clicking next turn
                selectedindex = 0;
                //add points back to units
                locate.SetAllIdleStatus(false, currPlayer);
				locate.SetAllMovementPoints();
				locate.SetAllAttackPoints();
                buildingManager.TickProduction();
                //next player's turn
                playerManager.NextActivePlayer();

                //ai actions
                //aiBehaviour.Actions(15);
            }
		}

		//sets remaining units idle
		if(GUI.Button(new Rect(30,300,60,20), "Set All Idle")) {
            char currPlayer = playerManager.currPlayer[0];
            locate.SetAllIdleStatus(true, currPlayer);
		}
	}
}

//TODO refactor lists of entities,  battles, and load map