using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

public class Summon : MonoBehaviour {
	public HexGrid hexGrid;
	public LoadMap loadMap;
	public EntityStorage entityStorage;
	public Currency currency;
    public EntityStats entityStats;

    //given an index and the type of summon, summons that entity with the next available name
    public void SummonEntity (int cellindex, string summonname, string playerid) {
		Vector3 summonindex = hexGrid.GetCellPos(cellindex);
		summonindex.y = 0.2f;
		string availableNum = AvailableName (summonname, playerid);
		string availableName = playerid + summonname + availableNum;

        //Instantiate the prefab from the resources folder
        GameObject entity = (GameObject)Instantiate(Resources.Load(summonname), summonindex, Quaternion.identity);
        entity.name = availableName;
        char playerChar = playerid[0];
        entityStorage.PlayerEntityList(playerChar).Add(entity);
        hexGrid.SetEntityObject(cellindex, entity);
        hexGrid.SetEntityName(cellindex, availableName);

        //sets stats for entity
        int health = entityStats.GetMaxHealth(summonname);
        entityStats.SetMaxHealth(entity, health);
        entityStats.SetCurrHealth(entity, health);
        int mana = entityStats.GetMaxMana(summonname);
        entityStats.SetMaxMana(entity, mana);
        entityStats.SetCurrMana(entity, mana);
        int dmg = entityStats.GetAttackDmg(summonname);
        entityStats.SetAttackDmg(entity, dmg);
        int attpt = entityStats.GetMaxAttackPoint(summonname);
        entityStats.SetMaxAttackPoint(entity, attpt);
        int movept = entityStats.GetMaxMovementPoint(summonname);
        entityStats.SetMaxMovementPoint(entity, movept);
        int range = entityStats.GetRange(summonname);
        entityStats.SetRange(entity, range);
        int rangedattdmg = entityStats.GetRangedAttackDmg(summonname);
        entityStats.SetRangedAttackDmg(entity, rangedattdmg);
        int armor = entityStats.GetArmor(summonname);
        entityStats.SetArmor(entity, armor);
        int armorpiercing = entityStats.GetArmorPiercing(summonname);
        entityStats.SetArmorPiercing(entity, armorpiercing);
        int rangedarmorpiercing = entityStats.GetRangedArmorPiercing(summonname);
        entityStats.SetRangedArmorPiercing(entity, rangedarmorpiercing);
        int vision = entityStats.GetVision(summonname);
        entityStats.SetVision(entity, vision);

        loadMap.CreateHealthLabel(cellindex, health, availableName);
	}

    public void SummonUndeadEntity(UndeadEntityMemento undeadEntityMemento)
    {
        string entityType = Regex.Replace(undeadEntityMemento.name.Substring(2), @"[\d-]", string.Empty);
        string playerId = Regex.Replace(undeadEntityMemento.name.Substring(0, 2), @"[\d-]", string.Empty);

        Vector3 summonindex = hexGrid.GetCellPos(undeadEntityMemento.cellIndex);
        summonindex.y = 0.2f;
        string availableNum = AvailableName(entityType, playerId);
        string availableName = playerId + entityType + availableNum;

        //Instantiate the prefab from the resources folder
        GameObject entity = (GameObject)Instantiate(Resources.Load(entityType), summonindex, Quaternion.identity);
        entity.name = availableName;
        char playerChar = playerId[0];
        entityStorage.PlayerEntityList(playerChar).Add(entity);
        hexGrid.SetEntityObject(undeadEntityMemento.cellIndex, entity);
        hexGrid.SetEntityName(undeadEntityMemento.cellIndex, availableName);

        entityStats.SetCurrHealth(entity, undeadEntityMemento.currhealth);
        entityStats.SetMaxHealth(entity, undeadEntityMemento.maxhealth);
        entityStats.SetCurrMana(entity, undeadEntityMemento.currmana);
        entityStats.SetMaxMana(entity, undeadEntityMemento.maxmana);
        entityStats.SetAttackDmg(entity, undeadEntityMemento.attackdmg);
        entityStats.SetCurrAttackPoint(entity, undeadEntityMemento.currattackpoint);
        entityStats.SetMaxAttackPoint(entity, undeadEntityMemento.maxattackpoint);
        entityStats.SetCurrMovementPoint(entity, undeadEntityMemento.currmovementpoint);
        entityStats.SetMaxMovementPoint(entity, undeadEntityMemento.maxmovementpoint);
        entityStats.SetRange(entity, undeadEntityMemento.range);
        entityStats.SetRangedAttackDmg(entity, undeadEntityMemento.rangedattackdmg);
        entityStats.SetArmor(entity, undeadEntityMemento.armor);
        entityStats.SetArmorPiercing(entity, undeadEntityMemento.armorpiercing);
        entityStats.SetRangedArmorPiercing(entity, undeadEntityMemento.rangedarmorpiercing);
        entityStats.SetVision(entity, undeadEntityMemento.vision);

        loadMap.CreateHealthLabel(undeadEntityMemento.cellIndex, undeadEntityMemento.currhealth, availableName);
    }

    public void SummonHumanEntity(HumanEntityMemento humanEntityMemento)
    {
        string entityType = Regex.Replace(humanEntityMemento.name.Substring(2), @"[\d-]", string.Empty);
        string playerId = Regex.Replace(humanEntityMemento.name.Substring(0, 2), @"[\d-]", string.Empty);

        Vector3 summonindex = hexGrid.GetCellPos(humanEntityMemento.cellIndex);
        summonindex.y = 0.2f;
        string availableNum = AvailableName(entityType, playerId);
        string availableName = playerId + entityType + availableNum;

        //Instantiate the prefab from the resources folder
        GameObject entity = (GameObject)Instantiate(Resources.Load(entityType), summonindex, Quaternion.identity);
        entity.name = availableName;
        char playerChar = playerId[0];
        entityStorage.PlayerEntityList(playerChar).Add(entity);
        hexGrid.SetEntityObject(humanEntityMemento.cellIndex, entity);
        hexGrid.SetEntityName(humanEntityMemento.cellIndex, availableName);

        entityStats.SetCurrHealth(entity, humanEntityMemento.currhealth);
        entityStats.SetMaxHealth(entity, humanEntityMemento.maxhealth);
        entityStats.SetCurrMana(entity, humanEntityMemento.currmana);
        entityStats.SetMaxMana(entity, humanEntityMemento.maxmana);
        entityStats.SetAttackDmg(entity, humanEntityMemento.attackdmg);
        entityStats.SetCurrAttackPoint(entity, humanEntityMemento.currattackpoint);
        entityStats.SetMaxAttackPoint(entity, humanEntityMemento.maxattackpoint);
        entityStats.SetCurrMovementPoint(entity, humanEntityMemento.currmovementpoint);
        entityStats.SetMaxMovementPoint(entity, humanEntityMemento.maxmovementpoint);
        entityStats.SetRange(entity, humanEntityMemento.range);
        entityStats.SetRangedAttackDmg(entity, humanEntityMemento.rangedattackdmg);
        entityStats.SetArmor(entity, humanEntityMemento.armor);
        entityStats.SetArmorPiercing(entity, humanEntityMemento.armorpiercing);
        entityStats.SetRangedArmorPiercing(entity, humanEntityMemento.rangedarmorpiercing);
        entityStats.SetVision(entity, humanEntityMemento.vision);

        loadMap.CreateHealthLabel(humanEntityMemento.cellIndex, humanEntityMemento.currhealth, availableName);
    }

    //Check for next available entity number
    string AvailableName (string summonname, string playerid) {
		for (int i = 1; i <= 999; i++) {
			string num = i.ToString ();
            bool nameExists = false;
            char playerChar = playerid[0];
            foreach (GameObject playerEntity in entityStorage.PlayerEntityList(playerChar))
            {
                if (playerEntity.name == playerid + summonname + num)
                {
                    nameExists = true;
                }
            }
            if (!nameExists)
            {
                return num;
            }
        } 
		return null;
	}

    //TODO for human entities
	public bool ValidSummon(string entity) {
        string faction = entityStats.WhichFactionEntity(entity);
        switch (faction)
        {
            case FactionNames.Undead:
                int souls = PlayerPrefs.GetInt("Souls");
                int cost = entityStats.summonSoulCost(entity);
                if (souls >= cost)
                {
                    currency.ChangeSouls(-cost);
                    return true;
                }
                return false;
            case "humans":
                return true;
        }
        return false;
    }

    public void KillEntity(int cellindex)
    {
        string entityName = hexGrid.GetEntityName(cellindex);
        GameObject entityObj = hexGrid.GetEntityObject(cellindex);
        char playerFirstLetter = entityName[0];
        entityStorage.PlayerEntityList(playerFirstLetter).Remove(entityObj);
        Destroy(entityObj);

        hexGrid.SetEntityName(cellindex, "Empty");
        hexGrid.SetEntityObject(cellindex, null);
        GameObject attackerHealthText = GameObject.Find("Health " + entityName);
        Destroy(attackerHealthText);

        //add to corpses if not undead
        string cleanEntity = Regex.Replace(entityName.Substring(2), @"[\d-]", string.Empty);
        if (entityStats.WhichFactionEntity(cleanEntity) != FactionNames.Undead)
        {
            hexGrid.SetCorpses(cellindex, entityName); //TODO remove once corpse list complete
        }
    }
}
