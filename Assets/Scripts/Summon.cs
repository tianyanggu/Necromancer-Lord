using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;
using System;

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

        //Instantiate the prefab from the resources folder, add to player entity list and set it to the hex tile
        GameObject entity = (GameObject)Instantiate(Resources.Load(summonname), summonindex, Quaternion.identity);
        Guid entityID = Guid.NewGuid();
        entity.name = entityID.ToString();
        char playerChar = playerid[0];
        entityStorage.PlayerEntityList(playerChar).Add(entity);
        hexGrid.SetEntityObject(cellindex, entity);

        //sets stats for entity
        entityStats.SetPlayerID(entity, playerid);
        entityStats.SetType(entity, summonname);
        entityStats.SetUniqueID(entity, entityID);
        entityStats.SetCellIndex(entity, cellindex);

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

        loadMap.CreateHealthLabel(cellindex, health, entity.name);
	}

    public void SummonEntityMemento(EntityMemento entityMemento)
    {
        string playerId = entityMemento.playerID;
        string entityType = entityMemento.type;
        int cellIndex = entityMemento.cellIndex;

        Vector3 summonindex = hexGrid.GetCellPos(cellIndex);
        summonindex.y = 0.2f;

        //Instantiate the prefab from the resources folder
        GameObject entity = (GameObject)Instantiate(Resources.Load(entityType), summonindex, Quaternion.identity);
        entity.name = entityMemento.uniqueID.ToString();
        char playerChar = playerId[0];
        entityStorage.PlayerEntityList(playerChar).Add(entity);
        hexGrid.SetEntityObject(entityMemento.cellIndex, entity);

        entityStats.SetPlayerID(entity, entityMemento.playerID);
        entityStats.SetType(entity, entityMemento.type);
        entityStats.SetUniqueID(entity, entityMemento.uniqueID);
        entityStats.SetCellIndex(entity, entityMemento.cellIndex);

        entityStats.SetCurrHealth(entity, entityMemento.currhealth);
        entityStats.SetMaxHealth(entity, entityMemento.maxhealth);
        entityStats.SetCurrMana(entity, entityMemento.currmana);
        entityStats.SetMaxMana(entity, entityMemento.maxmana);
        entityStats.SetAttackDmg(entity, entityMemento.attackdmg);
        entityStats.SetCurrAttackPoint(entity, entityMemento.currattackpoint);
        entityStats.SetMaxAttackPoint(entity, entityMemento.maxattackpoint);
        entityStats.SetCurrMovementPoint(entity, entityMemento.currmovementpoint);
        entityStats.SetMaxMovementPoint(entity, entityMemento.maxmovementpoint);
        entityStats.SetRange(entity, entityMemento.range);
        entityStats.SetRangedAttackDmg(entity, entityMemento.rangedattackdmg);
        entityStats.SetArmor(entity, entityMemento.armor);
        entityStats.SetArmorPiercing(entity, entityMemento.armorpiercing);
        entityStats.SetRangedArmorPiercing(entity, entityMemento.rangedarmorpiercing);
        entityStats.SetVision(entity, entityMemento.vision);
        entityStats.SetPermaEffects(entity, entityMemento.permaEffects);
        entityStats.SetTempEffects(entity, entityMemento.tempEffects);

        loadMap.CreateHealthLabel(entityMemento.cellIndex, entityMemento.currhealth, entity.name);
    }

    //TODO for human entities
	public bool ValidSummon(string entity) {
        string faction = entityStats.WhichFactionEntity(entity);
        switch (faction)
        {
            case FactionNames.Undead:
                int souls = currency.souls;
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
        GameObject entityObj = hexGrid.GetEntityObject(cellindex);
        char playerFirstLetter = entityStats.GetPlayerID(entityObj)[0];
        entityStorage.PlayerEntityList(playerFirstLetter).Remove(entityObj);
        Destroy(entityObj);

        hexGrid.SetEntityObject(cellindex, null);
        GameObject attackerHealthText = GameObject.Find("Health " + entityStats.GetUniqueID(entityObj).ToString());
        Destroy(attackerHealthText);

        //add to corpses to hex tile
        hexGrid.AddCorpse(cellindex, entityStats.GetType(entityObj)); //TODO remove once corpse list complete
    }
}
