using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class GameMemento {

    public static GameMemento current;
    //TODO just add the 3 hexgrid details to this. keeping for now to remember how to do objects
    public HexGridMemento hexGridMemento;

    public List<string> hexGridTerrainList;
    public List<string> hexGridBuildingNameList;
    public List<string> hexGridEntityNameList;
    public List<List<string>> hexGridCorpsesList;
    public List<bool> hexGridFogList;

    public string gameID;

    public GameMemento () {
        hexGridMemento = new HexGridMemento();
        hexGridTerrainList = new List<string>();
        hexGridBuildingNameList = new List<string>();
        hexGridEntityNameList = new List<string>();
        hexGridCorpsesList = new List<List<string>>();
        hexGridFogList = new List<bool>();
    }
}
