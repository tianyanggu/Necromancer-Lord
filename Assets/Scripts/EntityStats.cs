using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;
using System.Collections.Generic;

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
                player = entity.GetComponent<UndeadBehaviour>().player;
                break;
            case FactionNames.Human:
                player = entity.GetComponent<HumanBehaviour>().player;
                break;
        }
        return player;
    }

    public void SetPlayerID(GameObject entity, string player)
    {
        string entityName = CleanName(entity);
        string faction = WhichFactionEntity(entityName);
        switch (faction)
        {
            case FactionNames.Undead:
                entity.GetComponent<UndeadBehaviour>().player = player;
                break;
            case FactionNames.Human:
                entity.GetComponent<HumanBehaviour>().player = player;
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
                type = entity.GetComponent<UndeadBehaviour>().type;
                break;
            case FactionNames.Human:
                type = entity.GetComponent<HumanBehaviour>().type;
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
                entity.GetComponent<UndeadBehaviour>().type = type;
                break;
            case FactionNames.Human:
                entity.GetComponent<HumanBehaviour>().type = type;
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
                health = entity.GetComponent<UndeadBehaviour>().maxhealth;
                break;
            case FactionNames.Human:
                health = entity.GetComponent<HumanBehaviour>().maxhealth;
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
                health = entity.GetComponent<UndeadBehaviour>().currhealth;
                break;
            case FactionNames.Human:
                health = entity.GetComponent<HumanBehaviour>().currhealth;
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
                entity.GetComponent<UndeadBehaviour>().maxhealth = health;
                break;
            case FactionNames.Human:
                entity.GetComponent<HumanBehaviour>().maxhealth = health;
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
                entity.GetComponent<UndeadBehaviour>().currhealth = health;
                break;
            case FactionNames.Human:
                entity.GetComponent<HumanBehaviour>().currhealth = health;
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
                mana = entity.GetComponent<UndeadBehaviour>().maxmana;
                break;
            case FactionNames.Human:
                mana = entity.GetComponent<HumanBehaviour>().maxmana;
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
                mana = entity.GetComponent<UndeadBehaviour>().currmana;
                break;
            case FactionNames.Human:
                mana = entity.GetComponent<HumanBehaviour>().currmana;
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
                entity.GetComponent<UndeadBehaviour>().maxmana = mana;
                break;
            case FactionNames.Human:
                entity.GetComponent<HumanBehaviour>().maxmana = mana;
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
                entity.GetComponent<UndeadBehaviour>().currmana = mana;
                break;
            case FactionNames.Human:
                entity.GetComponent<HumanBehaviour>().currmana = mana;
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
                attdmg = entity.GetComponent<UndeadBehaviour>().attackdmg;
                break;
            case FactionNames.Human:
                attdmg = entity.GetComponent<HumanBehaviour>().attackdmg;
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
                entity.GetComponent<UndeadBehaviour>().attackdmg = dmg;
                break;
            case FactionNames.Human:
                entity.GetComponent<HumanBehaviour>().attackdmg = dmg;
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
                attpt = entity.GetComponent<UndeadBehaviour>().maxattackpoint;
                break;
            case FactionNames.Human:
                attpt = entity.GetComponent<HumanBehaviour>().maxattackpoint;
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
                attpt = entity.GetComponent<UndeadBehaviour>().currattackpoint;
                break;
            case FactionNames.Human:
                attpt = entity.GetComponent<HumanBehaviour>().currattackpoint;
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
                entity.GetComponent<UndeadBehaviour>().maxattackpoint = attpt;
                break;
            case FactionNames.Human:
                entity.GetComponent<HumanBehaviour>().maxattackpoint = attpt;
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
                entity.GetComponent<UndeadBehaviour>().currattackpoint = attpt;
                break;
            case FactionNames.Human:
                entity.GetComponent<HumanBehaviour>().currattackpoint = attpt;
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
                movept = entity.GetComponent<UndeadBehaviour>().maxmovementpoint;
                break;
            case FactionNames.Human:
                movept = entity.GetComponent<HumanBehaviour>().maxmovementpoint;
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
                movept = entity.GetComponent<UndeadBehaviour>().currmovementpoint;
                break;
            case FactionNames.Human:
                movept = entity.GetComponent<HumanBehaviour>().currmovementpoint;
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
                entity.GetComponent<UndeadBehaviour>().maxmovementpoint = movept;
                break;
            case FactionNames.Human:
                entity.GetComponent<HumanBehaviour>().maxmovementpoint = movept;
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
                entity.GetComponent<UndeadBehaviour>().currmovementpoint = movept;
                break;
            case FactionNames.Human:
                entity.GetComponent<HumanBehaviour>().currmovementpoint = movept;
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
                range = entity.GetComponent<UndeadBehaviour>().range;
                break;
            case FactionNames.Human:
                range = entity.GetComponent<HumanBehaviour>().range;
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
                entity.GetComponent<UndeadBehaviour>().range = range;
                break;
            case FactionNames.Human:
                entity.GetComponent<HumanBehaviour>().range = range;
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
                rangeattdmg = entity.GetComponent<UndeadBehaviour>().rangedattackdmg;
                break;
            case FactionNames.Human:
                rangeattdmg = entity.GetComponent<HumanBehaviour>().rangedattackdmg;
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
                entity.GetComponent<UndeadBehaviour>().rangedattackdmg = rangeattdmg;
                break;
            case FactionNames.Human:
                entity.GetComponent<HumanBehaviour>().rangedattackdmg = rangeattdmg;
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
                armor = entity.GetComponent<UndeadBehaviour>().armor;
                break;
            case FactionNames.Human:
                armor = entity.GetComponent<HumanBehaviour>().armor;
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
                entity.GetComponent<UndeadBehaviour>().armor = armor;
                break;
            case FactionNames.Human:
                entity.GetComponent<HumanBehaviour>().armor = armor;
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
                armorpiercing = entity.GetComponent<UndeadBehaviour>().armorpiercing;
                break;
            case FactionNames.Human:
                armorpiercing = entity.GetComponent<HumanBehaviour>().armorpiercing;
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
                entity.GetComponent<UndeadBehaviour>().armorpiercing = armorpiercing;
                break;
            case FactionNames.Human:
                entity.GetComponent<HumanBehaviour>().armorpiercing = armorpiercing;
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
                rangedarmorpiercing = entity.GetComponent<UndeadBehaviour>().rangedarmorpiercing;
                break;
            case FactionNames.Human:
                rangedarmorpiercing = entity.GetComponent<HumanBehaviour>().rangedarmorpiercing;
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
                entity.GetComponent<UndeadBehaviour>().rangedarmorpiercing = rangedarmorpiercing;
                break;
            case FactionNames.Human:
                entity.GetComponent<HumanBehaviour>().rangedarmorpiercing = rangedarmorpiercing;
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
                vision = entity.GetComponent<UndeadBehaviour>().vision;
                break;
            case FactionNames.Human:
                vision = entity.GetComponent<HumanBehaviour>().vision;
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
                entity.GetComponent<UndeadBehaviour>().vision = vision;
                break;
            case FactionNames.Human:
                entity.GetComponent<HumanBehaviour>().vision = vision;
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
                idle = entity.GetComponent<UndeadBehaviour>().idle;
                break;
            case FactionNames.Human:
                idle = entity.GetComponent<HumanBehaviour>().idle;
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
                entity.GetComponent<UndeadBehaviour>().idle = idle;
                break;
            case FactionNames.Human:
                entity.GetComponent<HumanBehaviour>().idle = idle;
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
