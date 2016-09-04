using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildingStorage : MonoBehaviour {

    public PlayerManager playerManager;

    public List<string> undeadBuildings = new List<string>();
    public List<string> humanBuildings = new List<string>();

    public List<GameObject> activePlayerABuildings = new List<GameObject>();
    public List<GameObject> activePlayerBBuildings = new List<GameObject>();
    public List<GameObject> activePlayerCBuildings = new List<GameObject>();

    public List<List<string>> factionBuildingList = new List<List<string>>();
    public List<List<GameObject>> activePlayersBuildingList = new List<List<GameObject>>();

    void Start () {
        //player controlled buildings
        undeadBuildings.Add ("Necropolis");
        //enemy entities
        humanBuildings.Add ("Village");

        ListActivePlayerBuildings ();
	}

	public void ListActivePlayerBuildings () {
		foreach (string playerID in playerManager.activePlayers) {
            //get which faction buildings needs to be checked for
            foreach (string building in BuildingFactionLists(PlayerPrefs.GetString(playerID)))
            {
                for (int i = 1; i <= 99; i++)
                {
                    string num = i.ToString();
                    GameObject gameBuilding = GameObject.Find(playerID + building + num);
                    if (gameBuilding != null)
                    {
                        char playerFirstLetter = playerID[0];
                        PlayerBuildingList(playerFirstLetter).Add(gameBuilding);
                    }
                }
            }
		}
	}

	//returns faction for building
	public string WhichFactionBuilding(string entity) {
		//------Determine Faction------
		if (entity == "Necropolis") {
			return "undead";
		} else if (entity == "Village") {
			return "human";
		}
		return "unknown";
	}

    public List<string> BuildingFactionLists(string factionName)
    {
        //------Determine Faction Entity List------
        switch (factionName)
        {
            case "undead":
                return undeadBuildings;
            case "human":
                return humanBuildings;
        }
        return new List<string>();
    }

    public List<GameObject> PlayerBuildingList(char playerID)
    {
        //------Determine Faction Entity List------
        switch (playerID)
        {
            case 'A':
                return activePlayerABuildings;
            case 'B':
                return activePlayerBBuildings;
            case 'C':
                return activePlayerCBuildings;
        }
        return new List<GameObject>();
    }

    //returns building soul cost
    public int buildingSoulCost(string building) {
        //------Determine Cost------
        switch (building)
        {
            case "Necropolis":
                return 200;
        }
		return 0;
	}
}
