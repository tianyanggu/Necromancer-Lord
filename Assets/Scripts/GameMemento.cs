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
    public List<bool> hexGridFogList;

    public List<UndeadEntityMemento> undeadEntityMementoList;
    public List<HumanEntityMemento> humanEntityMementoList;
    public List<UndeadBuildingMemento> undeadBuildingMementoList;
    public List<HumanBuildingMemento> humanBuildingMementoList;

    public GameMemento () {
        hexGridMemento = new HexGridMemento();

        activePlayersName = new Dictionary<string, string>();
        activePlayersFaction = new Dictionary<string, string>();
        activePlayersOrder = new Dictionary<int, string>();

        hexGridTerrainList = new List<string>();
        hexGridBuildingNameList = new List<string>();
        hexGridEntityNameList = new List<string>();
        hexGridCorpsesList = new List<List<string>>();
        hexGridFogList = new List<bool>();

        undeadEntityMementoList = new List<UndeadEntityMemento>();
        humanEntityMementoList = new List<HumanEntityMemento>();
        undeadBuildingMementoList = new List<UndeadBuildingMemento>();
        humanBuildingMementoList = new List<HumanBuildingMemento>();
    }
}
