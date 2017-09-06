using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;

public class Build : MonoBehaviour {

	public HexGrid hexGrid;
	public LoadMap loadMap;
	public BuildingStorage buildingStorage;
	public Currency currency;
	public EntityStorage entityStorage;
    public BuildingStats buildingStats;

	//given an index and the type of summon, summons that entity with the next available name
	public void BuildBuilding (int cellindex, string buildingname, string playerid) {
		Vector3 buildindex = hexGrid.GetCellPos(cellindex);
		buildindex.y = 0.2f;

        //Instantiate the prefab from the resources folder
        GameObject building = (GameObject)Instantiate(Resources.Load(buildingname), buildindex, Quaternion.identity);
        Guid buildingID = Guid.NewGuid();
        building.name = buildingID.ToString();
        char playerChar = playerid[0];
        buildingStorage.PlayerBuildingList(playerChar).Add(building);
        hexGrid.SetBuildingObject(cellindex, building);
        hexGrid.SetBuildingName(cellindex, building.name);

        //sets stats for building
        buildingStats.SetPlayerID(building, playerid);
        buildingStats.SetType(building, buildingname);
        buildingStats.SetUniqueID(building, buildingID);
        buildingStats.SetCellIndex(building, cellindex);

        int health = buildingStats.GetMaxHealth(buildingname);
        buildingStats.SetMaxHealth(building, health);
        buildingStats.SetCurrHealth(building, health);
        int range = buildingStats.GetRange(buildingname);
        buildingStats.SetRange(building, range);
        int rangedattdmg = buildingStats.GetRangedAttackDmg(buildingname);
        buildingStats.SetRangedAttackDmg(building, rangedattdmg);
        int defense = buildingStats.GetDefense(buildingname);
        buildingStats.SetDefense(building, defense);
        int vision = buildingStats.GetVision(buildingname);
        buildingStats.SetVision(building, vision);

        loadMap.CreateHealthLabel(cellindex, health, building.name);
    }

    public void BuildBuildingMemento(BuildingMemento buildingMemento)
    {
        string buildingId = buildingMemento.playerID;
        string buildingType = buildingMemento.type;
        int cellIndex = buildingMemento.cellIndex;

        Vector3 buildindex = hexGrid.GetCellPos(cellIndex);
        buildindex.y = 0.2f;

        //Instantiate the prefab from the resources folder
        GameObject building = (GameObject)Instantiate(Resources.Load(buildingType), buildindex, Quaternion.identity);
        building.name = buildingMemento.uniqueID.ToString();
        char buildingChar = buildingId[0];
        entityStorage.PlayerEntityList(buildingChar).Add(building);
        hexGrid.SetEntityObject(buildingMemento.cellIndex, building);
        hexGrid.SetEntityName(buildingMemento.cellIndex, building.name);

        buildingStats.SetPlayerID(building, buildingMemento.playerID);
        buildingStats.SetType(building, buildingMemento.type);
        buildingStats.SetUniqueID(building, buildingMemento.uniqueID);
        buildingStats.SetCellIndex(building, buildingMemento.cellIndex);

        buildingStats.SetCurrHealth(building, buildingMemento.currhealth);
        buildingStats.SetMaxHealth(building, buildingMemento.maxhealth);
        buildingStats.SetRange(building, buildingMemento.range);
        buildingStats.SetRangedAttackDmg(building, buildingMemento.rangedattackdmg);
        buildingStats.SetDefense(building, buildingMemento.defense);
        buildingStats.SetVision(building, buildingMemento.vision);
        buildingStats.SetUpgrades(building, buildingMemento.upgrades);
        buildingStats.SetPermaEffects(building, buildingMemento.permaEffects);
        buildingStats.SetTempEffects(building, buildingMemento.tempEffects);

        buildingStats.SetCurrConstruction(building, buildingMemento.currConstruction);
        buildingStats.SetCurrConstructionTimer(building, buildingMemento.currConstructionTimer);
        buildingStats.SetCurrRecruitment(building, buildingMemento.currRecruitment);
        buildingStats.SetCurrRecruitmentTimer(building, buildingMemento.currRecruitmentTimer);
        buildingStats.SetIsRecruitmentQueued(building, buildingMemento.isRecruitmentQueued);

        loadMap.CreateHealthLabel(buildingMemento.cellIndex, buildingMemento.currhealth, building.name);
    }

    public void DestroyBuilding (int cellindex) {
		string buildingName = hexGrid.GetBuildingName (cellindex);
        GameObject building = hexGrid.GetBuildingObject (cellindex);
        char playerChar = buildingName[0];
        buildingStorage.PlayerBuildingList(playerChar).Remove(building);
        Destroy (building);

        hexGrid.SetBuildingName(cellindex, "Empty");
        hexGrid.SetBuildingObject(cellindex, null);
        GameObject healthText = GameObject.Find("Health " + buildingName);
        Destroy(healthText);
    }

	//valid if have soul cost and entity/corpse cost
	public bool ValidBuilding(string building, int index) {
        string faction = buildingStorage.WhichFactionBuilding(building);
        switch (faction)
        {
            case FactionNames.Undead:
                int souls = PlayerPrefs.GetInt("Souls");
                int cost = buildingStorage.buildingSoulCost(building);

                List<string> corpses = hexGrid.GetCorpses(index);
                string entityName = hexGrid.GetEntityName(index);
                string cleanEntity = Regex.Replace(entityName.Substring(2), @"[\d-]", string.Empty);
                string ppNum = PlayerPrefs.GetString(entityName);
                string numEntity = Regex.Replace(ppNum, "[^0-9 -]", string.Empty);

                //checks if fulfilled cost and removes paid cost from game
                if (souls >= cost)
                {
                    if (corpses.Contains(EntityNames.Militia))
                    {
                        currency.ChangeSouls(-cost);
                        hexGrid.RemoveCorpses(index, EntityNames.Militia);
                        return true;
                    }
                    else if (cleanEntity == EntityNames.Skeleton || cleanEntity == EntityNames.Zombie || cleanEntity == EntityNames.SkeletonArcher)
                    {
                        currency.ChangeSouls(-cost);
                        GameObject entityGameObj = hexGrid.GetEntityObject(index);
                        char playerFirstLetter = entityName[0];
                        entityStorage.PlayerEntityList(playerFirstLetter).Remove(entityGameObj);
                        Destroy(entityGameObj);
                        hexGrid.SetEntityName(index, "Empty");
                        hexGrid.SetEntityObject(index, null);
                        GameObject healthText = GameObject.Find("Health " + entityName);
                        Destroy(healthText);
                        PlayerPrefs.DeleteKey("HexEntity" + numEntity);
                        PlayerPrefs.DeleteKey("HexEntityHealth" + numEntity);
                        PlayerPrefs.DeleteKey("HexEntityIndex" + numEntity);
                        PlayerPrefs.DeleteKey(entityName);
                        return true;
                    }
                }
                return false;

            case "humans":
                return true;
        }
        return false;
    }
}
