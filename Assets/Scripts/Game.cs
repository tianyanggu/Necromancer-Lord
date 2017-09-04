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
            GameMemento.current.hexGridBuildingNameList.Add(hexGrid.cells[i].buildingName);
            GameMemento.current.hexGridCorpsesList.Add(hexGrid.cells[i].corpses);
            GameMemento.current.hexGridFogList.Add(hexGrid.cells[i].fog);
            if (hexGrid.cells[i].entityObj != null)
            {
                string entityType = Regex.Replace(hexGrid.cells[i].entityName.Substring(2), @"[\d-]", string.Empty);
                if (entityStats.WhichFactionEntity(entityType) == FactionNames.Undead)
                {
                    UndeadEntityMemento undeadEntityMemento = new UndeadEntityMemento();
                    SetUndeadEntities(undeadEntityMemento, i);
                    GameMemento.current.undeadEntityMementoList.Add(undeadEntityMemento);
                } else if (entityStats.WhichFactionEntity(entityType) == FactionNames.Human)
                {
                    HumanEntityMemento humanEntityMemento = new HumanEntityMemento();
                    SetHumanEntities(humanEntityMemento, i);
                    GameMemento.current.humanEntityMementoList.Add(humanEntityMemento);
                }
            }
            if (hexGrid.cells[i].buildingObj != null)
            {
                string buildingType = Regex.Replace(hexGrid.cells[i].entityName.Substring(2), @"[\d-]", string.Empty);
                if (buildingStats.WhichFactionBuilding(buildingType) == FactionNames.Undead)
                {
                    UndeadBuildingMemento undeadBuildingMemento = new UndeadBuildingMemento();
                    SetUndeadBuildings(undeadBuildingMemento, i);
                    GameMemento.current.undeadBuildingMementoList.Add(undeadBuildingMemento);
                }
                else if (buildingStats.WhichFactionBuilding(buildingType) == FactionNames.Human)
                {
                    HumanBuildingMemento humanBuildingMemento = new HumanBuildingMemento();
                    SetHumanBuildings(humanBuildingMemento, i);
                    GameMemento.current.humanBuildingMementoList.Add(humanBuildingMemento);
                }
            }
        }
    }

    private void SetUndeadEntities (UndeadEntityMemento undeadEntityMemento, int i)
    {
        undeadEntityMemento.playerID = hexGrid.cells[i].entityObj.GetComponent<UndeadEntity>().playerID;
        undeadEntityMemento.type = hexGrid.cells[i].entityObj.GetComponent<UndeadEntity>().type;
        undeadEntityMemento.cellIndex = hexGrid.cells[i].entityObj.GetComponent<UndeadEntity>().cellIndex;

        undeadEntityMemento.maxhealth = hexGrid.cells[i].entityObj.GetComponent<UndeadEntity>().maxhealth;
        undeadEntityMemento.maxmana = hexGrid.cells[i].entityObj.GetComponent<UndeadEntity>().maxmana;
        undeadEntityMemento.attackdmg = hexGrid.cells[i].entityObj.GetComponent<UndeadEntity>().attackdmg;
        undeadEntityMemento.maxattackpoint = hexGrid.cells[i].entityObj.GetComponent<UndeadEntity>().maxattackpoint;
        undeadEntityMemento.maxmovementpoint = hexGrid.cells[i].entityObj.GetComponent<UndeadEntity>().maxmovementpoint;
        undeadEntityMemento.range = hexGrid.cells[i].entityObj.GetComponent<UndeadEntity>().range;
        undeadEntityMemento.rangedattackdmg = hexGrid.cells[i].entityObj.GetComponent<UndeadEntity>().rangedattackdmg;
        undeadEntityMemento.armor = hexGrid.cells[i].entityObj.GetComponent<UndeadEntity>().armor;
        undeadEntityMemento.armorpiercing = hexGrid.cells[i].entityObj.GetComponent<UndeadEntity>().armorpiercing;
        undeadEntityMemento.rangedarmorpiercing = hexGrid.cells[i].entityObj.GetComponent<UndeadEntity>().rangedarmorpiercing;
        undeadEntityMemento.vision = hexGrid.cells[i].entityObj.GetComponent<UndeadEntity>().vision;

        undeadEntityMemento.currhealth = hexGrid.cells[i].entityObj.GetComponent<UndeadEntity>().currhealth;
        undeadEntityMemento.currmana = hexGrid.cells[i].entityObj.GetComponent<UndeadEntity>().currmana;
        undeadEntityMemento.currattackpoint = hexGrid.cells[i].entityObj.GetComponent<UndeadEntity>().currattackpoint;
        undeadEntityMemento.currmovementpoint = hexGrid.cells[i].entityObj.GetComponent<UndeadEntity>().currmovementpoint;

        undeadEntityMemento.idle = hexGrid.cells[i].entityObj.GetComponent<UndeadEntity>().idle;
    }

    private void SetHumanEntities(HumanEntityMemento humanEntityMemento, int i)
    {
        humanEntityMemento.playerID = hexGrid.cells[i].entityObj.GetComponent<HumanEntity>().playerID;
        humanEntityMemento.type = hexGrid.cells[i].entityObj.GetComponent<HumanEntity>().type;
        humanEntityMemento.cellIndex = hexGrid.cells[i].entityObj.GetComponent<HumanEntity>().cellIndex;

        humanEntityMemento.maxhealth = hexGrid.cells[i].entityObj.GetComponent<HumanEntity>().maxhealth;
        humanEntityMemento.maxmana = hexGrid.cells[i].entityObj.GetComponent<HumanEntity>().maxmana;
        humanEntityMemento.attackdmg = hexGrid.cells[i].entityObj.GetComponent<HumanEntity>().attackdmg;
        humanEntityMemento.maxattackpoint = hexGrid.cells[i].entityObj.GetComponent<HumanEntity>().maxattackpoint;
        humanEntityMemento.maxmovementpoint = hexGrid.cells[i].entityObj.GetComponent<HumanEntity>().maxmovementpoint;
        humanEntityMemento.range = hexGrid.cells[i].entityObj.GetComponent<HumanEntity>().range;
        humanEntityMemento.rangedattackdmg = hexGrid.cells[i].entityObj.GetComponent<HumanEntity>().rangedattackdmg;
        humanEntityMemento.armor = hexGrid.cells[i].entityObj.GetComponent<HumanEntity>().armor;
        humanEntityMemento.armorpiercing = hexGrid.cells[i].entityObj.GetComponent<HumanEntity>().armorpiercing;
        humanEntityMemento.rangedarmorpiercing = hexGrid.cells[i].entityObj.GetComponent<HumanEntity>().rangedarmorpiercing;
        humanEntityMemento.vision = hexGrid.cells[i].entityObj.GetComponent<HumanEntity>().vision;

        humanEntityMemento.currhealth = hexGrid.cells[i].entityObj.GetComponent<HumanEntity>().currhealth;
        humanEntityMemento.currmana = hexGrid.cells[i].entityObj.GetComponent<HumanEntity>().currmana;
        humanEntityMemento.currattackpoint = hexGrid.cells[i].entityObj.GetComponent<HumanEntity>().currattackpoint;
        humanEntityMemento.currmovementpoint = hexGrid.cells[i].entityObj.GetComponent<HumanEntity>().currmovementpoint;

        humanEntityMemento.idle = hexGrid.cells[i].entityObj.GetComponent<HumanEntity>().idle;
    }

    private void SetUndeadBuildings(UndeadBuildingMemento undeadBuildingMemento, int i)
    {
        undeadBuildingMemento.playerID = hexGrid.cells[i].buildingObj.GetComponent<UndeadBuilding>().playerID;
        undeadBuildingMemento.type = hexGrid.cells[i].buildingObj.GetComponent<UndeadBuilding>().type;
        undeadBuildingMemento.cellIndex = hexGrid.cells[i].buildingObj.GetComponent<UndeadBuilding>().cellIndex;

        undeadBuildingMemento.currhealth = hexGrid.cells[i].buildingObj.GetComponent<UndeadBuilding>().currhealth;
        undeadBuildingMemento.maxhealth = hexGrid.cells[i].buildingObj.GetComponent<UndeadBuilding>().maxhealth;
        undeadBuildingMemento.range = hexGrid.cells[i].buildingObj.GetComponent<UndeadBuilding>().range;
        undeadBuildingMemento.rangedattackdmg = hexGrid.cells[i].buildingObj.GetComponent<UndeadBuilding>().rangedattackdmg;
        undeadBuildingMemento.defense = hexGrid.cells[i].buildingObj.GetComponent<UndeadBuilding>().defense;
        undeadBuildingMemento.vision = hexGrid.cells[i].buildingObj.GetComponent<UndeadBuilding>().vision;
        undeadBuildingMemento.upgrades = hexGrid.cells[i].buildingObj.GetComponent<UndeadBuilding>().upgrades;

        undeadBuildingMemento.currConstruction = hexGrid.cells[i].buildingObj.GetComponent<UndeadBuilding>().currConstruction;
        undeadBuildingMemento.currConstructionTimer = hexGrid.cells[i].buildingObj.GetComponent<UndeadBuilding>().currConstructionTimer;
        undeadBuildingMemento.currRecruitment = hexGrid.cells[i].buildingObj.GetComponent<UndeadBuilding>().currRecruitment;
        undeadBuildingMemento.currRecruitmentTimer = hexGrid.cells[i].buildingObj.GetComponent<UndeadBuilding>().currRecruitmentTimer;
        undeadBuildingMemento.isRecruitmentQueued = hexGrid.cells[i].buildingObj.GetComponent<UndeadBuilding>().isRecruitmentQueued;
    }

    private void SetHumanBuildings(HumanBuildingMemento humanBuildingMemento, int i)
    {
        humanBuildingMemento.playerID = hexGrid.cells[i].buildingObj.GetComponent<HumanBuilding>().playerID;
        humanBuildingMemento.type = hexGrid.cells[i].buildingObj.GetComponent<HumanBuilding>().type;
        humanBuildingMemento.cellIndex = hexGrid.cells[i].buildingObj.GetComponent<HumanBuilding>().cellIndex;

        humanBuildingMemento.currhealth = hexGrid.cells[i].buildingObj.GetComponent<HumanBuilding>().currhealth;
        humanBuildingMemento.maxhealth = hexGrid.cells[i].buildingObj.GetComponent<HumanBuilding>().maxhealth;
        humanBuildingMemento.range = hexGrid.cells[i].buildingObj.GetComponent<HumanBuilding>().range;
        humanBuildingMemento.rangedattackdmg = hexGrid.cells[i].buildingObj.GetComponent<HumanBuilding>().rangedattackdmg;
        humanBuildingMemento.defense = hexGrid.cells[i].buildingObj.GetComponent<HumanBuilding>().defense;
        humanBuildingMemento.vision = hexGrid.cells[i].buildingObj.GetComponent<HumanBuilding>().vision;
        humanBuildingMemento.upgrades = hexGrid.cells[i].buildingObj.GetComponent<HumanBuilding>().upgrades;

        humanBuildingMemento.currConstruction = hexGrid.cells[i].buildingObj.GetComponent<HumanBuilding>().currConstruction;
        humanBuildingMemento.currConstructionTimer = hexGrid.cells[i].buildingObj.GetComponent<HumanBuilding>().currConstructionTimer;
        humanBuildingMemento.currRecruitment = hexGrid.cells[i].buildingObj.GetComponent<HumanBuilding>().currRecruitment;
        humanBuildingMemento.currRecruitmentTimer = hexGrid.cells[i].buildingObj.GetComponent<HumanBuilding>().currRecruitmentTimer;
        humanBuildingMemento.isRecruitmentQueued = hexGrid.cells[i].buildingObj.GetComponent<HumanBuilding>().isRecruitmentQueued;
    }
}
