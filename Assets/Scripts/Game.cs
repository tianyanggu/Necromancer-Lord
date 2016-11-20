using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

public class Game : MonoBehaviour {

    public string gameID;
    public HexGrid hexGrid;
    public EntityStats entityStats;

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
            GameMemento.current.hexGridCorpsesList.Add(hexGrid.cells[i].corpses);
            GameMemento.current.hexGridFogList.Add(hexGrid.cells[i].fog);
            if (hexGrid.cells[i].entityObj != null)
            {
                string entityType = Regex.Replace(hexGrid.cells[i].entityName.Substring(2), @"[\d-]", string.Empty);
                if (entityStats.WhichFactionEntity(entityType) == "undead")
                {
                    UndeadEntityMemento undeadEntityMemento = new UndeadEntityMemento();
                    SetUndeadEntities(undeadEntityMemento, i);
                    GameMemento.current.undeadEntityMementoList.Add(undeadEntityMemento);
                } else if (entityStats.WhichFactionEntity(entityType) == "human")
                {
                    HumanEntityMemento humanEntityMemento = new HumanEntityMemento();
                    SetHumanEntities(humanEntityMemento, i);
                    GameMemento.current.humanEntityMementoList.Add(humanEntityMemento);
                }
            }
        }
    }

    private void SetUndeadEntities (UndeadEntityMemento undeadEntityMemento, int i)
    {
        undeadEntityMemento.name = hexGrid.cells[i].entityName;
        undeadEntityMemento.entityType = Regex.Replace(hexGrid.cells[i].entityName.Substring(2), @"[\d-]", string.Empty);
        undeadEntityMemento.playerId = Regex.Replace(hexGrid.cells[i].entityName.Substring(0, 2), @"[\d-]", string.Empty);
        undeadEntityMemento.cellIndex = i;

        undeadEntityMemento.maxhealth = hexGrid.cells[i].entityObj.GetComponent<UndeadBehaviour>().maxhealth;
        undeadEntityMemento.maxmana = hexGrid.cells[i].entityObj.GetComponent<UndeadBehaviour>().maxmana;
        undeadEntityMemento.attackdmg = hexGrid.cells[i].entityObj.GetComponent<UndeadBehaviour>().attackdmg;
        undeadEntityMemento.maxattackpoint = hexGrid.cells[i].entityObj.GetComponent<UndeadBehaviour>().maxattackpoint;
        undeadEntityMemento.maxmovementpoint = hexGrid.cells[i].entityObj.GetComponent<UndeadBehaviour>().maxmovementpoint;
        undeadEntityMemento.range = hexGrid.cells[i].entityObj.GetComponent<UndeadBehaviour>().range;
        undeadEntityMemento.rangedattackdmg = hexGrid.cells[i].entityObj.GetComponent<UndeadBehaviour>().rangedattackdmg;
        undeadEntityMemento.armor = hexGrid.cells[i].entityObj.GetComponent<UndeadBehaviour>().armor;
        undeadEntityMemento.armorpiercing = hexGrid.cells[i].entityObj.GetComponent<UndeadBehaviour>().armorpiercing;
        undeadEntityMemento.rangedarmorpiercing = hexGrid.cells[i].entityObj.GetComponent<UndeadBehaviour>().rangedarmorpiercing;
        undeadEntityMemento.vision = hexGrid.cells[i].entityObj.GetComponent<UndeadBehaviour>().vision;

        undeadEntityMemento.currhealth = hexGrid.cells[i].entityObj.GetComponent<UndeadBehaviour>().currhealth;
        undeadEntityMemento.currmana = hexGrid.cells[i].entityObj.GetComponent<UndeadBehaviour>().currmana;
        undeadEntityMemento.currattackpoint = hexGrid.cells[i].entityObj.GetComponent<UndeadBehaviour>().currattackpoint;
        undeadEntityMemento.currmovementpoint = hexGrid.cells[i].entityObj.GetComponent<UndeadBehaviour>().currmovementpoint;

        undeadEntityMemento.idle = hexGrid.cells[i].entityObj.GetComponent<UndeadBehaviour>().idle;
    }

    private void SetHumanEntities(HumanEntityMemento humanEntityMemento, int i)
    {
        humanEntityMemento.name = hexGrid.cells[i].entityName;
        humanEntityMemento.entityType = Regex.Replace(hexGrid.cells[i].entityName.Substring(2), @"[\d-]", string.Empty);
        humanEntityMemento.playerId = Regex.Replace(hexGrid.cells[i].entityName.Substring(0, 2), @"[\d-]", string.Empty);
        humanEntityMemento.cellIndex = i;

        humanEntityMemento.maxhealth = hexGrid.cells[i].entityObj.GetComponent<HumanBehaviour>().maxhealth;
        humanEntityMemento.maxmana = hexGrid.cells[i].entityObj.GetComponent<HumanBehaviour>().maxmana;
        humanEntityMemento.attackdmg = hexGrid.cells[i].entityObj.GetComponent<HumanBehaviour>().attackdmg;
        humanEntityMemento.maxattackpoint = hexGrid.cells[i].entityObj.GetComponent<HumanBehaviour>().maxattackpoint;
        humanEntityMemento.maxmovementpoint = hexGrid.cells[i].entityObj.GetComponent<HumanBehaviour>().maxmovementpoint;
        humanEntityMemento.range = hexGrid.cells[i].entityObj.GetComponent<HumanBehaviour>().range;
        humanEntityMemento.rangedattackdmg = hexGrid.cells[i].entityObj.GetComponent<HumanBehaviour>().rangedattackdmg;
        humanEntityMemento.armor = hexGrid.cells[i].entityObj.GetComponent<HumanBehaviour>().armor;
        humanEntityMemento.armorpiercing = hexGrid.cells[i].entityObj.GetComponent<HumanBehaviour>().armorpiercing;
        humanEntityMemento.rangedarmorpiercing = hexGrid.cells[i].entityObj.GetComponent<HumanBehaviour>().rangedarmorpiercing;
        humanEntityMemento.vision = hexGrid.cells[i].entityObj.GetComponent<HumanBehaviour>().vision;

        humanEntityMemento.currhealth = hexGrid.cells[i].entityObj.GetComponent<HumanBehaviour>().currhealth;
        humanEntityMemento.currmana = hexGrid.cells[i].entityObj.GetComponent<HumanBehaviour>().currmana;
        humanEntityMemento.currattackpoint = hexGrid.cells[i].entityObj.GetComponent<HumanBehaviour>().currattackpoint;
        humanEntityMemento.currmovementpoint = hexGrid.cells[i].entityObj.GetComponent<HumanBehaviour>().currmovementpoint;

        humanEntityMemento.idle = hexGrid.cells[i].entityObj.GetComponent<HumanBehaviour>().idle;
    }
}
