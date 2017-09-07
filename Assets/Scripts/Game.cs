using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

public class Game : MonoBehaviour {

    public string gameID;
    public HexGrid hexGrid;
    public EntityStats entityStats;
    public BuildingStats buildingStats;
    public PlayerManager playerManager;
    public Currency currency;

    //Every Memento have to create new since current game is static
    public void SetMemento ()
    {
        gameID = "ttt";
        GameMemento.current = new GameMemento();
        GameMemento.current.gameID = gameID;

        GameMemento.current.hexGridMemento.width = hexGrid.width;
        GameMemento.current.hexGridMemento.height = hexGrid.height;
        GameMemento.current.hexGridMemento.size = hexGrid.size;

        foreach (var item in playerManager.activePlayersName)
        {
            GameMemento.current.activePlayersName.Add(item.Key, item.Value);
        }
        foreach (var item in playerManager.activePlayersFaction)
        {
            GameMemento.current.activePlayersFaction.Add(item.Key, item.Value);
        }
        foreach (var item in playerManager.activePlayersOrder)
        {
            GameMemento.current.activePlayersOrder.Add(item.Key, item.Value);
        }
        GameMemento.current.currPlayerOrder = playerManager.currPlayerOrder;
        GameMemento.current.currPlayer = playerManager.currPlayer;

        GameMemento.current.souls = currency.souls;
        GameMemento.current.gold = currency.gold;

        for (int i = 0; i < hexGrid.cells.Length; i++)
        {
            GameMemento.current.hexGridTerrainList.Add(hexGrid.cells[i].terrain);
            GameMemento.current.hexGridCorpsesList.Add(hexGrid.cells[i].corpses);
            GameMemento.current.hexGridFogList.Add(hexGrid.cells[i].fog);
            if (hexGrid.cells[i].entityObj != null)
            {
                EntityMemento entityMemento = new EntityMemento();
                SetEntitiesMemento(entityMemento, i);
                GameMemento.current.entityMementoList.Add(entityMemento);
            }
            if (hexGrid.cells[i].buildingObj != null)
            {
                BuildingMemento buildingMemento = new BuildingMemento();
                SetBuildingsMemento(buildingMemento, i);
                GameMemento.current.buildingMementoList.Add(buildingMemento);
            }
        }
    }

    //TODO check if need to create new list for  and readd everything to current
    private void SetEntitiesMemento (EntityMemento EntityMemento, int i)
    {
        EntityMemento.playerID = hexGrid.cells[i].entityObj.GetComponent<Entity>().playerID;
        EntityMemento.type = hexGrid.cells[i].entityObj.GetComponent<Entity>().type;
        EntityMemento.cellIndex = hexGrid.cells[i].entityObj.GetComponent<Entity>().cellIndex;
        EntityMemento.uniqueID = hexGrid.cells[i].entityObj.GetComponent<Entity>().uniqueID;

        EntityMemento.maxhealth = hexGrid.cells[i].entityObj.GetComponent<Entity>().maxhealth;
        EntityMemento.maxmana = hexGrid.cells[i].entityObj.GetComponent<Entity>().maxmana;
        EntityMemento.attackdmg = hexGrid.cells[i].entityObj.GetComponent<Entity>().attackdmg;
        EntityMemento.maxattackpoint = hexGrid.cells[i].entityObj.GetComponent<Entity>().maxattackpoint;
        EntityMemento.maxmovementpoint = hexGrid.cells[i].entityObj.GetComponent<Entity>().maxmovementpoint;
        EntityMemento.range = hexGrid.cells[i].entityObj.GetComponent<Entity>().range;
        EntityMemento.rangedattackdmg = hexGrid.cells[i].entityObj.GetComponent<Entity>().rangedattackdmg;
        EntityMemento.armor = hexGrid.cells[i].entityObj.GetComponent<Entity>().armor;
        EntityMemento.armorpiercing = hexGrid.cells[i].entityObj.GetComponent<Entity>().armorpiercing;
        EntityMemento.rangedarmorpiercing = hexGrid.cells[i].entityObj.GetComponent<Entity>().rangedarmorpiercing;
        EntityMemento.vision = hexGrid.cells[i].entityObj.GetComponent<Entity>().vision;
        EntityMemento.permaEffects = hexGrid.cells[i].entityObj.GetComponent<Entity>().permaEffects;
        EntityMemento.tempEffects = hexGrid.cells[i].entityObj.GetComponent<Entity>().tempEffects;

        EntityMemento.currhealth = hexGrid.cells[i].entityObj.GetComponent<Entity>().currhealth;
        EntityMemento.currmana = hexGrid.cells[i].entityObj.GetComponent<Entity>().currmana;
        EntityMemento.currattackpoint = hexGrid.cells[i].entityObj.GetComponent<Entity>().currattackpoint;
        EntityMemento.currmovementpoint = hexGrid.cells[i].entityObj.GetComponent<Entity>().currmovementpoint;

        EntityMemento.idle = hexGrid.cells[i].entityObj.GetComponent<Entity>().idle;
    }

    private void SetBuildingsMemento(BuildingMemento BuildingMemento, int i)
    {
        BuildingMemento.playerID = hexGrid.cells[i].buildingObj.GetComponent<Building>().playerID;
        BuildingMemento.type = hexGrid.cells[i].buildingObj.GetComponent<Building>().type;
        BuildingMemento.cellIndex = hexGrid.cells[i].buildingObj.GetComponent<Building>().cellIndex;
        BuildingMemento.uniqueID = hexGrid.cells[i].buildingObj.GetComponent<Building>().uniqueID;

        BuildingMemento.currhealth = hexGrid.cells[i].buildingObj.GetComponent<Building>().currhealth;
        BuildingMemento.maxhealth = hexGrid.cells[i].buildingObj.GetComponent<Building>().maxhealth;
        BuildingMemento.range = hexGrid.cells[i].buildingObj.GetComponent<Building>().range;
        BuildingMemento.rangedattackdmg = hexGrid.cells[i].buildingObj.GetComponent<Building>().rangedattackdmg;
        BuildingMemento.defense = hexGrid.cells[i].buildingObj.GetComponent<Building>().defense;
        BuildingMemento.vision = hexGrid.cells[i].buildingObj.GetComponent<Building>().vision;
        BuildingMemento.upgrades = hexGrid.cells[i].buildingObj.GetComponent<Building>().upgrades;
        BuildingMemento.permaEffects = hexGrid.cells[i].buildingObj.GetComponent<Building>().permaEffects;
        BuildingMemento.tempEffects = hexGrid.cells[i].buildingObj.GetComponent<Building>().tempEffects;

        BuildingMemento.currConstruction = hexGrid.cells[i].buildingObj.GetComponent<Building>().currConstruction;
        BuildingMemento.currConstructionTimer = hexGrid.cells[i].buildingObj.GetComponent<Building>().currConstructionTimer;
        BuildingMemento.currRecruitment = hexGrid.cells[i].buildingObj.GetComponent<Building>().currRecruitment;
        BuildingMemento.currRecruitmentTimer = hexGrid.cells[i].buildingObj.GetComponent<Building>().currRecruitmentTimer;
        BuildingMemento.isRecruitmentQueued = hexGrid.cells[i].buildingObj.GetComponent<Building>().isRecruitmentQueued;
    }
}
