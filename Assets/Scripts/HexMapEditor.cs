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
        
        SaveLoad.Load();
        GameMemento.current = SaveLoad.savedGame;
        if (GameMemento.current.hexGridMemento.size != 0) //load from most recent if player presses continue
        {
            Debug.Log("Loading");
            playerManager.SetActivePlayers();
            loadMap.LoadHexTiles();
            loadMap.LoadTerrain();
            loadMap.LoadBuildings();
            loadMap.LoadEntities();
            loadMap.LoadResources();
            loadMap.LoadCorpses();
        }
        else //create new game when no game, set from player settings in menu
        {
            Debug.Log("New");
            playerManager.SetActivePlayers(); //TODO modify set active players to set new players when new game
            loadMap.LoadNewHexTiles(12, 12);
            loadMap.LoadRandom(12); //sets the seed of the terrain spawn
        }

        //		List<int> test = hexGrid.GetCellIndexesOneHexAway (28);
        //		int test0 = test [0];
        //		int test1 = test [1];
        //		Debug.Log (test0);
        //		Debug.Log (test1);


        //TODO Overlay to add players
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

        //TODO lock ability for user to save while attack in progress because battle object is not saved

		//-----Selector--------------
		Debug.Log(currindex);
		//string currEntityName = hexGrid.GetEntityName (currindex);
        GameObject currEntityObj = hexGrid.GetEntityObject(currindex);
        GameObject currBuildingObj = hexGrid.GetBuildingObject(currindex);
        //string cleanCurrEntity = Regex.Replace(currEntityName.Substring(2), @"[\d-]", string.Empty);
        //string cleanCurrBuilding = Regex.Replace(currBuildingName.Substring(2), @"[\d-]", string.Empty);

        char playerChar = playerManager.currPlayer[0];
        if (entityStorage.PlayerEntityList(playerChar).Contains (currEntityObj)) {
			selectedindex = currindex;
            //TODO list info for curr entity, display it
			lockbattle = false;
		}
        if (buildingStorage.PlayerBuildingList(playerChar).Contains(currBuildingObj)) {
            buildingManager.DisplayBuilding(currindex);
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
		GameObject currEntityObject = hexGrid.GetEntityObject(currindex);
		if (currEntityObject == null) {
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
            string playerFaction = playerManager.activePlayersFaction[playerManager.currPlayer];
            foreach (string entity in entityStorage.EntityFactionLists(playerFaction)) {
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
		if (currEntityObject == null) {
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
		GameObject currBuildingObject = hexGrid.GetBuildingObject(currindex);
		if (currBuildingObject == null) {
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
            string playerFaction = playerManager.activePlayersFaction[playerManager.currPlayer];
            foreach (string building in buildingStorage.BuildingFactionLists(playerFaction)) {
				int spacing = i * 20;
				if (GUI.Button (new Rect (150, 150 + spacing, 120, 20), "Building " + building)) {
					bool validbuilding = build.ValidBuilding (building, currindex);
					if (validbuilding) {
						build.BuildBuilding (currindex, building, playerManager.currPlayer);
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

//TODO refactor lists of entities, battles, and load map