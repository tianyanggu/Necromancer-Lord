using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour {

    public string gameID;
    public HexGrid hexGrid;

    //Every Memento have to create new since current game is static
    public void SetMemento ()
    {
        gameID = "ttt";
        GameMemento.current = new GameMemento();
        GameMemento.current.gameID = gameID;
        GameMemento.current.hexGridMemento.width = hexGrid.width;
        GameMemento.current.hexGridMemento.height = hexGrid.height;
        GameMemento.current.hexGridMemento.size = hexGrid.size;

        
        for (int i = 0; i < hexGrid.cells.Length; i++)
        {
            Debug.Log(hexGrid.cells[i].coordinates);

            GameMemento.current.hexGridTerrainList.Add(hexGrid.cells[i].terrain);
            GameMemento.current.hexGridBuildingNameList.Add(hexGrid.cells[i].buildingName);
            GameMemento.current.hexGridEntityNameList.Add(hexGrid.cells[i].entityName);
            GameMemento.current.hexGridCorpsesList.Add(hexGrid.cells[i].corpses);
            GameMemento.current.hexGridFogList.Add(hexGrid.cells[i].fog);
        }

        Debug.Log(GameMemento.current.hexGridMemento);
    }
}
