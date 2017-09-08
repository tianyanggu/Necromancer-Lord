using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class GameMemento {

    public static GameMemento current;
    public string gameID;

    //TODO just add the 3 hexgrid details to this. keeping for now to remember how to do objects
    public HexGridMemento hexGridMemento;

    public Dictionary<string, string> activePlayersName;
    public Dictionary<string, string> activePlayersFaction;
    public Dictionary<int, string> activePlayersOrder;
    public int currPlayerOrder;
    public string currPlayer;

    public int souls;
    public int gold;

    public List<string> hexGridTerrainList;
    public List<string> hexGridBuildingNameList;
    public List<string> hexGridEntityNameList;
    public List<List<string>> hexGridCorpsesList;
    public List<List<string>> hexGridGroundEffectsList;
    public List<List<string>> hexGridHasVisionList;
    public List<bool> hexGridFogList;

    public List<EntityMemento> entityMementoList;
    public List<BuildingMemento> buildingMementoList;

    public GameMemento () {
        hexGridMemento = new HexGridMemento();

        activePlayersName = new Dictionary<string, string>();
        activePlayersFaction = new Dictionary<string, string>();
        activePlayersOrder = new Dictionary<int, string>();

        hexGridTerrainList = new List<string>();
        hexGridBuildingNameList = new List<string>();
        hexGridEntityNameList = new List<string>();
        hexGridCorpsesList = new List<List<string>>();
        hexGridGroundEffectsList = new List<List<string>>();
        hexGridHasVisionList = new List<List<string>>();
        hexGridCorpsesList = new List<List<string>>();
        hexGridFogList = new List<bool>();

        entityMementoList = new List<EntityMemento>();
        buildingMementoList = new List<BuildingMemento>();
    }
}
