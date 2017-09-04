using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System;

public static class EntityNames
{
    public const string Necromancer = "Necromancer";
    public const string Skeleton = "Skeleton";
    public const string Zombie = "Zombie";
    public const string SkeletonArcher = "SkeletonArcher";
    public const string ArmoredSkeleton = "ArmoredSkeleton";
    public const string DeathKnight = "DeathKnight";

    public const string Militia = "Militia";
    public const string Archer = "Archer";
    public const string Longbowman = "Longbowman";
    public const string Crossbowman = "Crossbowman";
    public const string Footman = "Footman";
    public const string MountedKnight = "MountedKnight";
    public const string LightsChosen = "Light's Chosen";
}

public class EntityStats : MonoBehaviour {

    public List<string> undeadEntities = new List<string>();
    public List<string> humanEntities = new List<string>();
    //TODO make all max stats able to be changed by making them return from variables
    //TODO then save all max stats
    
    void Awake()
    {
        //undead entities
        undeadEntities.Add(EntityNames.Necromancer);
        undeadEntities.Add(EntityNames.Skeleton);
        undeadEntities.Add(EntityNames.Zombie);
        undeadEntities.Add(EntityNames.SkeletonArcher);
        undeadEntities.Add(EntityNames.ArmoredSkeleton);
        undeadEntities.Add(EntityNames.DeathKnight);
        //human entities
        humanEntities.Add(EntityNames.Militia);
        humanEntities.Add(EntityNames.Archer);
        humanEntities.Add(EntityNames.Longbowman);
        humanEntities.Add(EntityNames.Crossbowman);
        humanEntities.Add(EntityNames.Footman);
        humanEntities.Add(EntityNames.MountedKnight);
        humanEntities.Add(EntityNames.LightsChosen);
    }

    public string CleanName(GameObject entity)
    {
        string cleanEntity = Regex.Replace(entity.name.Substring(2), @"[\d-]", string.Empty);
        return cleanEntity;
    }

    #region playerID
    public string GetPlayerID(GameObject entity)
    {
        string entityName = CleanName(entity);
        string faction = WhichFactionEntity(entityName);
        string player = string.Empty;
        switch (faction)
        {
            case FactionNames.Undead:
                player = entity.GetComponent<UndeadEntity>().playerID;
                break;
            case FactionNames.Human:
                player = entity.GetComponent<HumanEntity>().playerID;
                break;
        }
        return player;
    }

    public void SetPlayerID(GameObject entity, string playerID)
    {
        string entityName = CleanName(entity);
        string faction = WhichFactionEntity(entityName);
        switch (faction)
        {
            case FactionNames.Undead:
                entity.GetComponent<UndeadEntity>().playerID = playerID;
                break;
            case FactionNames.Human:
                entity.GetComponent<HumanEntity>().playerID = playerID;
                break;
        }
    }
    #endregion

    #region type
    public string GetType(GameObject entity)
    {
        string entityName = CleanName(entity);
        string faction = WhichFactionEntity(entityName);
        string type = string.Empty;
        switch (faction)
        {
            case FactionNames.Undead:
                type = entity.GetComponent<UndeadEntity>().type;
                break;
            case FactionNames.Human:
                type = entity.GetComponent<HumanEntity>().type;
                break;
        }
        return type;
    }

    public void SetType(GameObject entity, string type)
    {
        string entityName = CleanName(entity);
        string faction = WhichFactionEntity(entityName);
        switch (faction)
        {
            case FactionNames.Undead:
                entity.GetComponent<UndeadEntity>().type = type;
                break;
            case FactionNames.Human:
                entity.GetComponent<HumanEntity>().type = type;
                break;
        }
    }
    #endregion

    #region uniqueID
    public Guid GetUniqueID(GameObject entity)
    {
        string entityName = CleanName(entity);
        string faction = WhichFactionEntity(entityName);
        Guid player = Guid.Empty;
        switch (faction)
        {
            case FactionNames.Undead:
                player = entity.GetComponent<UndeadEntity>().uniqueID;
                break;
            case FactionNames.Human:
                player = entity.GetComponent<HumanEntity>().uniqueID;
                break;
        }
        return player;
    }

    public void SetUniqueID(GameObject entity, Guid uniqueID)
    {
        string entityName = CleanName(entity);
        string faction = WhichFactionEntity(entityName);
        switch (faction)
        {
            case FactionNames.Undead:
                entity.GetComponent<UndeadEntity>().uniqueID = uniqueID;
                break;
            case FactionNames.Human:
                entity.GetComponent<HumanEntity>().uniqueID = uniqueID;
                break;
        }
    }
    #endregion

    #region cellIndex
    public int GetCellIndex(GameObject entity)
    {
        string entityName = CleanName(entity);
        string faction = WhichFactionEntity(entityName);
        int player = 0;
        switch (faction)
        {
            case FactionNames.Undead:
                player = entity.GetComponent<UndeadEntity>().cellIndex;
                break;
            case FactionNames.Human:
                player = entity.GetComponent<HumanEntity>().cellIndex;
                break;
        }
        return player;
    }

    public void SetCellIndex(GameObject entity, int cellIndex)
    {
        string entityName = CleanName(entity);
        string faction = WhichFactionEntity(entityName);
        switch (faction)
        {
            case FactionNames.Undead:
                entity.GetComponent<UndeadEntity>().cellIndex = cellIndex;
                break;
            case FactionNames.Human:
                entity.GetComponent<HumanEntity>().cellIndex = cellIndex;
                break;
        }
    }
    #endregion

    #region health
    public int GetMaxHealth(string entity)
    {
        switch (entity)
        {
            case EntityNames.Zombie:
                return 250;
            case EntityNames.Skeleton:
                return 150;
            case EntityNames.Necromancer:
                return 1000;
            case EntityNames.SkeletonArcher:
                return 150;
            case EntityNames.ArmoredSkeleton:
                return 250;
            case EntityNames.DeathKnight:
                return 500;

            case EntityNames.Militia:
                return 200;
            case EntityNames.Archer:
                return 200;
            case EntityNames.Longbowman:
                return 200;
            case EntityNames.Crossbowman:
                return 250;
            case EntityNames.Footman:
                return 300;
            case EntityNames.MountedKnight:
                return 350;
            case EntityNames.LightsChosen:
                return 800;
        }
        return 0;
    }

    public int GetCurrMaxHealth(GameObject entity)
    {
        string entityName = CleanName(entity);
        string faction = WhichFactionEntity(entityName);
        int health = 0;
        switch (faction)
        {
            case FactionNames.Undead:
                health = entity.GetComponent<UndeadEntity>().maxhealth;
                break;
            case FactionNames.Human:
                health = entity.GetComponent<HumanEntity>().maxhealth;
                break;
        }
        return health;
    }

    public int GetCurrHealth(GameObject entity)
    {
        string entityName = CleanName(entity);
        string faction = WhichFactionEntity(entityName);
        int health = 0;
        switch (faction)
        {
            case FactionNames.Undead:
                health = entity.GetComponent<UndeadEntity>().currhealth;
                break;
            case FactionNames.Human:
                health = entity.GetComponent<HumanEntity>().currhealth;
                break;
        }
        return health;
    }

    public void SetMaxHealth(GameObject entity, int health)
    {
        string entityName = CleanName(entity);
        string faction = WhichFactionEntity(entityName);
        switch (faction)
        {
            case FactionNames.Undead:
                entity.GetComponent<UndeadEntity>().maxhealth = health;
                break;
            case FactionNames.Human:
                entity.GetComponent<HumanEntity>().maxhealth = health;
                break;
        }
    }

    public void SetCurrHealth(GameObject entity, int health)
    {
        string entityName = CleanName(entity);
        string faction = WhichFactionEntity(entityName);
        switch (faction)
        {
            case FactionNames.Undead:
                entity.GetComponent<UndeadEntity>().currhealth = health;
                break;
            case FactionNames.Human:
                entity.GetComponent<HumanEntity>().currhealth = health;
                break;
        }
    }
    #endregion

    #region mana
    public int GetMaxMana(string entity)
    {
        switch (entity)
        {
            case EntityNames.Zombie:
                return 0;
            case EntityNames.Skeleton:
                return 0;
            case EntityNames.Necromancer:
                return 100;
            case EntityNames.SkeletonArcher:
                return 0;
            case EntityNames.ArmoredSkeleton:
                return 0;
            case EntityNames.DeathKnight:
                return 25;

            case EntityNames.Militia:
                return 0;
            case EntityNames.Archer:
                return 0;
            case EntityNames.Longbowman:
                return 0;
            case EntityNames.Crossbowman:
                return 0;
            case EntityNames.Footman:
                return 0;
            case EntityNames.MountedKnight:
                return 10;
            case EntityNames.LightsChosen:
                return 80;
        }
        return 0;
    }

    public int GetCurrMaxMana(GameObject entity)
    {
        string entityName = CleanName(entity);
        string faction = WhichFactionEntity(entityName);
        int mana = 0;
        switch (faction)
        {
            case FactionNames.Undead:
                mana = entity.GetComponent<UndeadEntity>().maxmana;
                break;
            case FactionNames.Human:
                mana = entity.GetComponent<HumanEntity>().maxmana;
                break;
        }
        return mana;
    }

    public int GetCurrMana(GameObject entity)
    {
        string entityName = CleanName(entity);
        string faction = WhichFactionEntity(entityName);
        int mana = 0;
        switch (faction)
        {
            case FactionNames.Undead:
                mana = entity.GetComponent<UndeadEntity>().currmana;
                break;
            case FactionNames.Human:
                mana = entity.GetComponent<HumanEntity>().currmana;
                break;
        }
        return mana;
    }

    public void SetMaxMana(GameObject entity, int mana)
    {
        string entityName = CleanName(entity);
        string faction = WhichFactionEntity(entityName);
        switch (faction)
        {
            case FactionNames.Undead:
                entity.GetComponent<UndeadEntity>().maxmana = mana;
                break;
            case FactionNames.Human:
                entity.GetComponent<HumanEntity>().maxmana = mana;
                break;
        }
    }

    public void SetCurrMana(GameObject entity, int mana)
    {
        string entityName = CleanName(entity);
        string faction = WhichFactionEntity(entityName);
        switch (faction)
        {
            case FactionNames.Undead:
                entity.GetComponent<UndeadEntity>().currmana = mana;
                break;
            case FactionNames.Human:
                entity.GetComponent<HumanEntity>().currmana = mana;
                break;
        }
    }
    #endregion

    #region attackdmg
    public int GetAttackDmg(string entity)
    {
        switch (entity)
        {
            case EntityNames.Zombie:
                return 10;
            case EntityNames.Skeleton:
                return 20;
            case EntityNames.Necromancer:
                return 50;
            case EntityNames.SkeletonArcher:
                return 5;
            case EntityNames.ArmoredSkeleton:
                return 30;
            case EntityNames.DeathKnight:
                return 45;

            case EntityNames.Militia:
                return 15;
            case EntityNames.Archer:
                return 5;
            case EntityNames.Longbowman:
                return 5;
            case EntityNames.Crossbowman:
                return 15;
            case EntityNames.Footman:
                return 30;
            case EntityNames.MountedKnight:
                return 35;
            case EntityNames.LightsChosen:
                return 150;
        }
        return 0;
    }

    public int GetCurrAttackDmg(GameObject entity)
    {
        string entityName = CleanName(entity);
        string faction = WhichFactionEntity(entityName);
        int attdmg = 0;
        switch (faction)
        {
            case FactionNames.Undead:
                attdmg = entity.GetComponent<UndeadEntity>().attackdmg;
                break;
            case FactionNames.Human:
                attdmg = entity.GetComponent<HumanEntity>().attackdmg;
                break;
        }
        return attdmg;
    }

    public void SetAttackDmg(GameObject entity, int dmg)
    {
        string entityName = CleanName(entity);
        string faction = WhichFactionEntity(entityName);
        switch (faction)
        {
            case FactionNames.Undead:
                entity.GetComponent<UndeadEntity>().attackdmg = dmg;
                break;
            case FactionNames.Human:
                entity.GetComponent<HumanEntity>().attackdmg = dmg;
                break;
        }
    }
    #endregion

    #region attackpoint
    public int GetMaxAttackPoint(string entity)
    {
        switch (entity)
        {
            case EntityNames.Zombie:
                return 1;
            case EntityNames.Skeleton:
                return 1;
            case EntityNames.Necromancer:
                return 2;
            case EntityNames.SkeletonArcher:
                return 1;
            case EntityNames.ArmoredSkeleton:
                return 1;
            case EntityNames.DeathKnight:
                return 1;

            case EntityNames.Militia:
                return 1;
            case EntityNames.Archer:
                return 1;
            case EntityNames.Longbowman:
                return 1;
            case EntityNames.Crossbowman:
                return 1;
            case EntityNames.Footman:
                return 1;
            case EntityNames.MountedKnight:
                return 1;
            case EntityNames.LightsChosen:
                return 2;
        }
        return 0;
    }

    public int GetCurrMaxAttackPoint(GameObject entity)
    {
        string entityName = CleanName(entity);
        string faction = WhichFactionEntity(entityName);
        int attpt = 0;
        switch (faction)
        {
            case FactionNames.Undead:
                attpt = entity.GetComponent<UndeadEntity>().maxattackpoint;
                break;
            case FactionNames.Human:
                attpt = entity.GetComponent<HumanEntity>().maxattackpoint;
                break;
        }
        return attpt;
    }

    public int GetCurrAttackPoint(GameObject entity)
    {
        string entityName = CleanName(entity);
        string faction = WhichFactionEntity(entityName);
        int attpt = 0;
        switch (faction)
        {
            case FactionNames.Undead:
                attpt = entity.GetComponent<UndeadEntity>().currattackpoint;
                break;
            case FactionNames.Human:
                attpt = entity.GetComponent<HumanEntity>().currattackpoint;
                break;
        }
        return attpt;
    }

    public void SetMaxAttackPoint(GameObject entity, int attpt)
    {
        string entityName = CleanName(entity);
        string faction = WhichFactionEntity(entityName);
        switch (faction)
        {
            case FactionNames.Undead:
                entity.GetComponent<UndeadEntity>().maxattackpoint = attpt;
                break;
            case FactionNames.Human:
                entity.GetComponent<HumanEntity>().maxattackpoint = attpt;
                break;
        }
    }

    public void SetCurrAttackPoint(GameObject entity, int attpt)
    {
        string entityName = CleanName(entity);
        string faction = WhichFactionEntity(entityName);
        switch (faction)
        {
            case FactionNames.Undead:
                entity.GetComponent<UndeadEntity>().currattackpoint = attpt;
                break;
            case FactionNames.Human:
                entity.GetComponent<HumanEntity>().currattackpoint = attpt;
                break;
        }
    }
    #endregion

    #region movementpoint
    public int GetMaxMovementPoint(string entity)
    {
        switch (entity)
        {
            case EntityNames.Zombie:
                return 1;
            case EntityNames.Skeleton:
                return 2;
            case EntityNames.Necromancer:
                return 3;
            case EntityNames.SkeletonArcher:
                return 2;
            case EntityNames.ArmoredSkeleton:
                return 2;
            case EntityNames.DeathKnight:
                return 3;

            case EntityNames.Militia:
                return 2;
            case EntityNames.Archer:
                return 2;
            case EntityNames.Longbowman:
                return 2;
            case EntityNames.Crossbowman:
                return 2;
            case EntityNames.Footman:
                return 2;
            case EntityNames.MountedKnight:
                return 3;
            case EntityNames.LightsChosen:
                return 2;
        }
        return 0;
    }

    public int GetCurrMaxMovementPoint(GameObject entity)
    {
        string entityName = CleanName(entity);
        string faction = WhichFactionEntity(entityName);
        int movept = 0;
        switch (faction)
        {
            case FactionNames.Undead:
                movept = entity.GetComponent<UndeadEntity>().maxmovementpoint;
                break;
            case FactionNames.Human:
                movept = entity.GetComponent<HumanEntity>().maxmovementpoint;
                break;
        }
        return movept;
    }

    public int GetCurrMovementPoint(GameObject entity)
    {
        string entityName = CleanName(entity);
        string faction = WhichFactionEntity(entityName);
        int movept = 0;
        switch (faction)
        {
            case FactionNames.Undead:
                movept = entity.GetComponent<UndeadEntity>().currmovementpoint;
                break;
            case FactionNames.Human:
                movept = entity.GetComponent<HumanEntity>().currmovementpoint;
                break;
        }
        return movept;
    }

    public void SetMaxMovementPoint(GameObject entity, int movept)
    {
        string entityName = CleanName(entity);
        string faction = WhichFactionEntity(entityName);
        switch (faction)
        {
            case FactionNames.Undead:
                entity.GetComponent<UndeadEntity>().maxmovementpoint = movept;
                break;
            case FactionNames.Human:
                entity.GetComponent<HumanEntity>().maxmovementpoint = movept;
                break;
        }
    }

    public void SetCurrMovementPoint(GameObject entity, int movept)
    {
        string entityName = CleanName(entity);
        string faction = WhichFactionEntity(entityName);
        switch (faction)
        {
            case FactionNames.Undead:
                entity.GetComponent<UndeadEntity>().currmovementpoint = movept;
                break;
            case FactionNames.Human:
                entity.GetComponent<HumanEntity>().currmovementpoint = movept;
                break;
        }
    }
    #endregion

    #region range
    public int GetRange(string entity)
    {
        switch (entity)
        {
            case EntityNames.Zombie:
                return 1;
            case EntityNames.Skeleton:
                return 1;
            case EntityNames.Necromancer:
                return 3;
            case EntityNames.SkeletonArcher:
                return 2;
            case EntityNames.ArmoredSkeleton:
                return 1;
            case EntityNames.DeathKnight:
                return 1;

            case EntityNames.Militia:
                return 1;
            case EntityNames.Archer:
                return 2;
            case EntityNames.Longbowman:
                return 3;
            case EntityNames.Crossbowman:
                return 2;
            case EntityNames.Footman:
                return 1;
            case EntityNames.MountedKnight:
                return 1;
            case EntityNames.LightsChosen:
                return 3;
        }
        return 0;
    }

    public int GetCurrRange(GameObject entity)
    {
        string entityName = CleanName(entity);
        string faction = WhichFactionEntity(entityName);
        int range = 0;
        switch (faction)
        {
            case FactionNames.Undead:
                range = entity.GetComponent<UndeadEntity>().range;
                break;
            case FactionNames.Human:
                range = entity.GetComponent<HumanEntity>().range;
                break;
        }
        return range;
    }

    public void SetRange(GameObject entity, int range)
    {
        string entityName = CleanName(entity);
        string faction = WhichFactionEntity(entityName);
        switch (faction)
        {
            case FactionNames.Undead:
                entity.GetComponent<UndeadEntity>().range = range;
                break;
            case FactionNames.Human:
                entity.GetComponent<HumanEntity>().range = range;
                break;
        }
    }
    #endregion

    #region rangedattackdmg
    public int GetRangedAttackDmg(string entity)
    {
        switch (entity)
        {
            case EntityNames.Zombie:
                return 0;
            case EntityNames.Skeleton:
                return 0;
            case EntityNames.Necromancer:
                return 100;
            case EntityNames.SkeletonArcher:
                return 15;
            case EntityNames.ArmoredSkeleton:
                return 0;
            case EntityNames.DeathKnight:
                return 0;

            case EntityNames.Militia:
                return 0;
            case EntityNames.Archer:
                return 15;
            case EntityNames.Longbowman:
                return 15;
            case EntityNames.Crossbowman:
                return 25;
            case EntityNames.Footman:
                return 0;
            case EntityNames.MountedKnight:
                return 0;
            case EntityNames.LightsChosen:
                return 50;
        }
        return 0;
    }

    public int GetCurrRangedAttackDmg(GameObject entity)
    {
        string entityName = CleanName(entity);
        string faction = WhichFactionEntity(entityName);
        int rangeattdmg = 0;
        switch (faction)
        {
            case FactionNames.Undead:
                rangeattdmg = entity.GetComponent<UndeadEntity>().rangedattackdmg;
                break;
            case FactionNames.Human:
                rangeattdmg = entity.GetComponent<HumanEntity>().rangedattackdmg;
                break;
        }
        return rangeattdmg;
    }

    public void SetRangedAttackDmg(GameObject entity, int rangeattdmg)
    {
        string entityName = CleanName(entity);
        string faction = WhichFactionEntity(entityName);
        switch (faction)
        {
            case FactionNames.Undead:
                entity.GetComponent<UndeadEntity>().rangedattackdmg = rangeattdmg;
                break;
            case FactionNames.Human:
                entity.GetComponent<HumanEntity>().rangedattackdmg = rangeattdmg;
                break;
        }
    }
    #endregion

    #region armor
    public int GetArmor(string entity)
    {
        switch (entity)
        {
            case EntityNames.Zombie:
                return 0;
            case EntityNames.Skeleton:
                return 10;
            case EntityNames.Necromancer:
                return 35;
            case EntityNames.SkeletonArcher:
                return 5;
            case EntityNames.ArmoredSkeleton:
                return 0;
            case EntityNames.DeathKnight:
                return 25;

            case EntityNames.Militia:
                return 5;
            case EntityNames.Archer:
                return 5;
            case EntityNames.Longbowman:
                return 5;
            case EntityNames.Crossbowman:
                return 15;
            case EntityNames.Footman:
                return 15;
            case EntityNames.MountedKnight:
                return 20;
            case EntityNames.LightsChosen:
                return 45;
        }
        return 0;
    }

    public int GetCurrArmor(GameObject entity)
    {
        string entityName = CleanName(entity);
        string faction = WhichFactionEntity(entityName);
        int armor = 0;
        switch (faction)
        {
            case FactionNames.Undead:
                armor = entity.GetComponent<UndeadEntity>().armor;
                break;
            case FactionNames.Human:
                armor = entity.GetComponent<HumanEntity>().armor;
                break;
        }
        return armor;
    }

    public void SetArmor(GameObject entity, int armor)
    {
        string entityName = CleanName(entity);
        string faction = WhichFactionEntity(entityName);
        switch (faction)
        {
            case FactionNames.Undead:
                entity.GetComponent<UndeadEntity>().armor = armor;
                break;
            case FactionNames.Human:
                entity.GetComponent<HumanEntity>().armor = armor;
                break;
        }
    }
    #endregion

    #region armorpiercing
    public int GetArmorPiercing(string entity)
    {
        switch (entity)
        {
            case EntityNames.Zombie:
                return 0;
            case EntityNames.Skeleton:
                return 5;
            case EntityNames.Necromancer:
                return 15;
            case EntityNames.SkeletonArcher:
                return 0;
            case EntityNames.ArmoredSkeleton:
                return 10;
            case EntityNames.DeathKnight:
                return 15;

            case EntityNames.Militia:
                return 5;
            case EntityNames.Archer:
                return 0;
            case EntityNames.Longbowman:
                return 0;
            case EntityNames.Crossbowman:
                return 5;
            case EntityNames.Footman:
                return 10;
            case EntityNames.MountedKnight:
                return 15;
            case EntityNames.LightsChosen:
                return 35;
        }
        return 0;
    }

    public int GetCurrArmorPiercing(GameObject entity)
    {
        string entityName = CleanName(entity);
        string faction = WhichFactionEntity(entityName);
        int armorpiercing = 0;
        switch (faction)
        {
            case FactionNames.Undead:
                armorpiercing = entity.GetComponent<UndeadEntity>().armorpiercing;
                break;
            case FactionNames.Human:
                armorpiercing = entity.GetComponent<HumanEntity>().armorpiercing;
                break;
        }
        return armorpiercing;
    }

    public void SetArmorPiercing(GameObject entity, int armorpiercing)
    {
        string entityName = CleanName(entity);
        string faction = WhichFactionEntity(entityName);
        switch (faction)
        {
            case FactionNames.Undead:
                entity.GetComponent<UndeadEntity>().armorpiercing = armorpiercing;
                break;
            case FactionNames.Human:
                entity.GetComponent<HumanEntity>().armorpiercing = armorpiercing;
                break;
        }
    }
    #endregion

    #region rangedarmorpiercing
    public int GetRangedArmorPiercing(string entity)
    {
        switch (entity)
        {
            case EntityNames.Zombie:
                return 0;
            case EntityNames.Skeleton:
                return 0;
            case EntityNames.Necromancer:
                return 50;
            case EntityNames.SkeletonArcher:
                return 5;
            case EntityNames.ArmoredSkeleton:
                return 0;
            case EntityNames.DeathKnight:
                return 0;

            case EntityNames.Militia:
                return 0;
            case EntityNames.Archer:
                return 5;
            case EntityNames.Longbowman:
                return 5;
            case EntityNames.Crossbowman:
                return 15;
            case EntityNames.Footman:
                return 0;
            case EntityNames.MountedKnight:
                return 0;
            case EntityNames.LightsChosen:
                return 15;
        }
        return 0;
    }

    public int GetCurrRangedArmorPiercing(GameObject entity)
    {
        string entityName = CleanName(entity);
        string faction = WhichFactionEntity(entityName);
        int rangedarmorpiercing = 0;
        switch (faction)
        {
            case FactionNames.Undead:
                rangedarmorpiercing = entity.GetComponent<UndeadEntity>().rangedarmorpiercing;
                break;
            case FactionNames.Human:
                rangedarmorpiercing = entity.GetComponent<HumanEntity>().rangedarmorpiercing;
                break;
        }
        return rangedarmorpiercing;
    }

    public void SetRangedArmorPiercing(GameObject entity, int rangedarmorpiercing)
    {
        string entityName = CleanName(entity);
        string faction = WhichFactionEntity(entityName);
        switch (faction)
        {
            case FactionNames.Undead:
                entity.GetComponent<UndeadEntity>().rangedarmorpiercing = rangedarmorpiercing;
                break;
            case FactionNames.Human:
                entity.GetComponent<HumanEntity>().rangedarmorpiercing = rangedarmorpiercing;
                break;
        }
    }
    #endregion

    #region vision
    public int GetVision(string entity)
    {
        switch (entity)
        {
            case EntityNames.Zombie:
                return 2;
            case EntityNames.Skeleton:
                return 3;
            case EntityNames.Necromancer:
                return 4;
            case EntityNames.SkeletonArcher:
                return 3;
            case EntityNames.ArmoredSkeleton:
                return 3;
            case EntityNames.DeathKnight:
                return 4;

            case EntityNames.Militia:
                return 3;
            case EntityNames.Archer:
                return 3;
            case EntityNames.Longbowman:
                return 3;
            case EntityNames.Crossbowman:
                return 3;
            case EntityNames.Footman:
                return 3;
            case EntityNames.MountedKnight:
                return 4;
            case EntityNames.LightsChosen:
                return 4;
        }
        return 0;
    }

    public int GetCurrVision(GameObject entity)
    {
        string entityName = CleanName(entity);
        string faction = WhichFactionEntity(entityName);
        int vision = 0;
        switch (faction)
        {
            case FactionNames.Undead:
                vision = entity.GetComponent<UndeadEntity>().vision;
                break;
            case FactionNames.Human:
                vision = entity.GetComponent<HumanEntity>().vision;
                break;
        }
        return vision;
    }

    public void SetVision(GameObject entity, int vision)
    {
        string entityName = CleanName(entity);
        string faction = WhichFactionEntity(entityName);
        switch (faction)
        {
            case FactionNames.Undead:
                entity.GetComponent<UndeadEntity>().vision = vision;
                break;
            case FactionNames.Human:
                entity.GetComponent<HumanEntity>().vision = vision;
                break;
        }
    }
    #endregion

    #region permaEffects
    public List<string> GetPermaEffects(GameObject entity)
    {
        string entityName = CleanName(entity);
        string faction = WhichFactionEntity(entityName);
        List<string> permaEffects = new List<string>();
        switch (faction)
        {
            case FactionNames.Undead:
                permaEffects = entity.GetComponent<UndeadEntity>().permaEffects;
                break;
            case FactionNames.Human:
                permaEffects = entity.GetComponent<HumanEntity>().permaEffects;
                break;
        }
        return permaEffects;
    }

    public void SetPermaEffects(GameObject entity, List<string> permaEffects)
    {
        string entityName = CleanName(entity);
        string faction = WhichFactionEntity(entityName);
        switch (faction)
        {
            case FactionNames.Undead:
                entity.GetComponent<UndeadEntity>().permaEffects = permaEffects;
                break;
            case FactionNames.Human:
                entity.GetComponent<HumanEntity>().permaEffects = permaEffects;
                break;
        }
    }

    public void AddPermaEffects(GameObject entity, string permaEffects)
    {
        string entityName = CleanName(entity);
        string faction = WhichFactionEntity(entityName);
        switch (faction)
        {
            case FactionNames.Undead:
                entity.GetComponent<UndeadEntity>().permaEffects.Add(permaEffects);
                break;
            case FactionNames.Human:
                entity.GetComponent<HumanEntity>().permaEffects.Add(permaEffects);
                break;
        }
    }

    public void RemovePermaEffects(GameObject entity, string permaEffects)
    {
        string entityName = CleanName(entity);
        string faction = WhichFactionEntity(entityName);
        switch (faction)
        {
            case FactionNames.Undead:
                entity.GetComponent<UndeadEntity>().permaEffects.Remove(permaEffects);
                break;
            case FactionNames.Human:
                entity.GetComponent<HumanEntity>().permaEffects.Remove(permaEffects);
                break;
        }
    }
    #endregion

    #region tempEffects
    public List<KeyValuePair<string,int>> GetTempEffects(GameObject entity)
    {
        string entityName = CleanName(entity);
        string faction = WhichFactionEntity(entityName);
        List<KeyValuePair<string, int>> tempEffects = new List<KeyValuePair<string, int>>();
        switch (faction)
        {
            case FactionNames.Undead:
                tempEffects = entity.GetComponent<UndeadEntity>().tempEffects;
                break;
            case FactionNames.Human:
                tempEffects = entity.GetComponent<HumanEntity>().tempEffects;
                break;
        }
        return tempEffects;
    }

    public void SetTempEffects(GameObject entity, List<KeyValuePair<string, int>> tempEffects)
    {
        string entityName = CleanName(entity);
        string faction = WhichFactionEntity(entityName);
        switch (faction)
        {
            case FactionNames.Undead:
                entity.GetComponent<UndeadEntity>().tempEffects = tempEffects;
                break;
            case FactionNames.Human:
                entity.GetComponent<HumanEntity>().tempEffects = tempEffects;
                break;
        }
    }

    public void AddTempEffects(GameObject entity, string tempEffects, int duration)
    {
        string entityName = CleanName(entity);
        string faction = WhichFactionEntity(entityName);
        switch (faction)
        {
            case FactionNames.Undead:
                entity.GetComponent<UndeadEntity>().tempEffects.Add(new KeyValuePair<string, int>(tempEffects, duration));
                break;
            case FactionNames.Human:
                entity.GetComponent<HumanEntity>().tempEffects.Add(new KeyValuePair<string, int>(tempEffects, duration));
                break;
        }
    }

    public void RemoveTempEffects(GameObject entity, string tempEffects, int duration)
    {
        string entityName = CleanName(entity);
        string faction = WhichFactionEntity(entityName);
        switch (faction)
        {
            case FactionNames.Undead:
                entity.GetComponent<UndeadEntity>().tempEffects.Remove(new KeyValuePair<string, int>(tempEffects, duration));
                break;
            case FactionNames.Human:
                entity.GetComponent<HumanEntity>().tempEffects.Remove(new KeyValuePair<string, int>(tempEffects, duration));
                break;
        }
    }
    #endregion

    public bool GetIdle(GameObject entity)
    {
        string entityName = CleanName(entity);
        string faction = WhichFactionEntity(entityName);
        bool idle = false;
        switch (faction)
        {
            case FactionNames.Undead:
                idle = entity.GetComponent<UndeadEntity>().idle;
                break;
            case FactionNames.Human:
                idle = entity.GetComponent<HumanEntity>().idle;
                break;
        }
        return idle;
    }

    public void SetIdle(GameObject entity, bool idle)
    {
        string entityName = CleanName(entity);
        string faction = WhichFactionEntity(entityName);
        switch (faction)
        {
            case FactionNames.Undead:
                entity.GetComponent<UndeadEntity>().idle = idle;
                break;
            case FactionNames.Human:
                entity.GetComponent<HumanEntity>().idle = idle;
                break;
        }
    }

    //returns faction
    public string WhichFactionEntity(string entity)
    {
        //------Determine Faction------
        switch (entity)
        {
            case EntityNames.Zombie:
                return FactionNames.Undead;
            case EntityNames.Skeleton:
                return FactionNames.Undead;
            case EntityNames.Necromancer:
                return FactionNames.Undead;
            case EntityNames.SkeletonArcher:
                return FactionNames.Undead;
            case EntityNames.ArmoredSkeleton:
                return FactionNames.Undead;
            case EntityNames.DeathKnight:
                return FactionNames.Undead;

            case EntityNames.Militia:
                return FactionNames.Human;
            case EntityNames.Archer:
                return FactionNames.Human;
            case EntityNames.Longbowman:
                return FactionNames.Human;
            case EntityNames.Crossbowman:
                return FactionNames.Human;
            case EntityNames.Footman:
                return FactionNames.Human;
            case EntityNames.MountedKnight:
                return FactionNames.Human;
            case EntityNames.LightsChosen:
                return FactionNames.Human;
        }
        return "unknown";
    }

    //returns summon soul cost
    public int summonSoulCost(string entity)
    {
        //------Determine Cost------
        switch (entity)
        {
            case EntityNames.Zombie:
                return 100;
            case EntityNames.Skeleton:
                return 150;
            case EntityNames.Necromancer:
                return 10000;
            case EntityNames.SkeletonArcher:
                return 150;
            case EntityNames.ArmoredSkeleton:
                return 200;
            case EntityNames.DeathKnight:
                return 250;
        }
        return 0;
    }
}
