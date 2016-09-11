using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;
using System.Collections.Generic;

public class EntityStats : MonoBehaviour {

    public List<string> undeadEntities = new List<string>();
    public List<string> humanEntities = new List<string>();

    void Awake()
    {
        //undead entities
        undeadEntities.Add("Necromancer");
        undeadEntities.Add("Skeleton");
        undeadEntities.Add("Zombie");
        undeadEntities.Add("SkeletonArcher");
        undeadEntities.Add("ArmoredSkeleton");
        undeadEntities.Add("DeathKnight");
        //human entities
        humanEntities.Add("Militia");
        humanEntities.Add("Archer");
        humanEntities.Add("Longbowman");
        humanEntities.Add("Crossbowman");
        humanEntities.Add("Footman");
        humanEntities.Add("MountedKnight");
        humanEntities.Add("HeroKing");
    }

    public string CleanName(GameObject entity)
    {
        string cleanEntity = Regex.Replace(entity.name.Substring(2), @"[\d-]", string.Empty);
        return cleanEntity;
    }

    #region health
    public int GetMaxHealth(string entity)
    {
        switch (entity)
        {
            case "Zombie":
                return 250;
            case "Skeleton":
                return 150;
            case "Necromancer":
                return 1000;
            case "SkeletonArcher":
                return 150;
            case "ArmoredSkeleton":
                return 250;
            case "DeathKnight":
                return 500;

            case "Militia":
                return 200;
            case "Archer":
                return 200;
            case "Longbowman":
                return 200;
            case "Crossbowman":
                return 250;
            case "Footman":
                return 300;
            case "MountedKnight":
                return 350;
            case "HeroKing":
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
            case "undead":
                health = entity.GetComponent<UndeadBehaviour>().maxhealth;
                break;
            case "human":
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
            case "undead":
                health = entity.GetComponent<UndeadBehaviour>().currhealth;
                break;
            case "human":
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
            case "undead":
                entity.GetComponent<UndeadBehaviour>().maxhealth = health;
                break;
            case "human":
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
            case "undead":
                entity.GetComponent<UndeadBehaviour>().currhealth = health;
                break;
            case "human":
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
            case "Zombie":
                return 0;
            case "Skeleton":
                return 0;
            case "Necromancer":
                return 100;
            case "SkeletonArcher":
                return 0;
            case "ArmoredSkeleton":
                return 0;
            case "DeathKnight":
                return 25;

            case "Militia":
                return 0;
            case "Archer":
                return 0;
            case "Longbowman":
                return 0;
            case "Crossbowman":
                return 0;
            case "Footman":
                return 0;
            case "MountedKnight":
                return 10;
            case "HeroKing":
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
            case "undead":
                mana = entity.GetComponent<UndeadBehaviour>().maxmana;
                break;
            case "human":
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
            case "undead":
                mana = entity.GetComponent<UndeadBehaviour>().currmana;
                break;
            case "human":
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
            case "undead":
                entity.GetComponent<UndeadBehaviour>().maxmana = mana;
                break;
            case "human":
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
            case "undead":
                entity.GetComponent<UndeadBehaviour>().currmana = mana;
                break;
            case "human":
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
            case "Zombie":
                return 10;
            case "Skeleton":
                return 20;
            case "Necromancer":
                return 50;
            case "SkeletonArcher":
                return 5;
            case "ArmoredSkeleton":
                return 30;
            case "DeathKnight":
                return 45;

            case "Militia":
                return 15;
            case "Archer":
                return 5;
            case "Longbowman":
                return 5;
            case "Crossbowman":
                return 15;
            case "Footman":
                return 30;
            case "MountedKnight":
                return 35;
            case "HeroKing":
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
            case "undead":
                attdmg = entity.GetComponent<UndeadBehaviour>().attackdmg;
                break;
            case "human":
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
            case "undead":
                entity.GetComponent<UndeadBehaviour>().attackdmg = dmg;
                break;
            case "human":
                entity.GetComponent<HumanBehaviour>().attackdmg = dmg;
                break;
        }
    }
    #endregion

    #region maxattackpoint
    public int GetMaxAttackPoint(string entity)
    {
        switch (entity)
        {
            case "Zombie":
                return 1;
            case "Skeleton":
                return 1;
            case "Necromancer":
                return 2;
            case "SkeletonArcher":
                return 1;
            case "ArmoredSkeleton":
                return 1;
            case "DeathKnight":
                return 1;

            case "Militia":
                return 1;
            case "Archer":
                return 1;
            case "Longbowman":
                return 1;
            case "Crossbowman":
                return 1;
            case "Footman":
                return 1;
            case "MountedKnight":
                return 1;
            case "HeroKing":
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
            case "undead":
                attpt = entity.GetComponent<UndeadBehaviour>().maxattackpoint;
                break;
            case "human":
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
            case "undead":
                attpt = entity.GetComponent<UndeadBehaviour>().currattackpoint;
                break;
            case "human":
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
            case "undead":
                entity.GetComponent<UndeadBehaviour>().maxattackpoint = attpt;
                break;
            case "human":
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
            case "undead":
                entity.GetComponent<UndeadBehaviour>().currattackpoint = attpt;
                break;
            case "human":
                entity.GetComponent<HumanBehaviour>().currattackpoint = attpt;
                break;
        }
    }
    #endregion

    #region maxmovementpoint
    public int GetMaxMovementPoint(string entity)
    {
        switch (entity)
        {
            case "Zombie":
                return 1;
            case "Skeleton":
                return 1;
            case "Necromancer":
                return 2;
            case "SkeletonArcher":
                return 1;
            case "ArmoredSkeleton":
                return 1;
            case "DeathKnight":
                return 1;

            case "Militia":
                return 1;
            case "Archer":
                return 1;
            case "Longbowman":
                return 1;
            case "Crossbowman":
                return 1;
            case "Footman":
                return 1;
            case "MountedKnight":
                return 1;
            case "HeroKing":
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
            case "undead":
                movept = entity.GetComponent<UndeadBehaviour>().maxmovementpoint;
                break;
            case "human":
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
            case "undead":
                movept = entity.GetComponent<UndeadBehaviour>().maxmovementpoint;
                break;
            case "human":
                movept = entity.GetComponent<HumanBehaviour>().maxmovementpoint;
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
            case "undead":
                entity.GetComponent<UndeadBehaviour>().maxmovementpoint = movept;
                break;
            case "human":
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
            case "undead":
                entity.GetComponent<UndeadBehaviour>().maxmovementpoint = movept;
                break;
            case "human":
                entity.GetComponent<HumanBehaviour>().maxmovementpoint = movept;
                break;
        }
    }
    #endregion

    #region range
    public int GetRange(string entity)
    {
        switch (entity)
        {
            case "Zombie":
                return 1;
            case "Skeleton":
                return 1;
            case "Necromancer":
                return 3;
            case "SkeletonArcher":
                return 2;
            case "ArmoredSkeleton":
                return 1;
            case "DeathKnight":
                return 1;

            case "Militia":
                return 1;
            case "Archer":
                return 2;
            case "Longbowman":
                return 3;
            case "Crossbowman":
                return 2;
            case "Footman":
                return 1;
            case "MountedKnight":
                return 1;
            case "HeroKing":
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
            case "undead":
                range = entity.GetComponent<UndeadBehaviour>().range;
                break;
            case "human":
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
            case "undead":
                entity.GetComponent<UndeadBehaviour>().range = range;
                break;
            case "human":
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
            case "Zombie":
                return 0;
            case "Skeleton":
                return 0;
            case "Necromancer":
                return 100;
            case "SkeletonArcher":
                return 15;
            case "ArmoredSkeleton":
                return 0;
            case "DeathKnight":
                return 0;

            case "Militia":
                return 0;
            case "Archer":
                return 15;
            case "Longbowman":
                return 15;
            case "Crossbowman":
                return 25;
            case "Footman":
                return 0;
            case "MountedKnight":
                return 0;
            case "HeroKing":
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
            case "undead":
                rangeattdmg = entity.GetComponent<UndeadBehaviour>().rangedattackdmg;
                break;
            case "human":
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
            case "undead":
                entity.GetComponent<UndeadBehaviour>().rangedattackdmg = rangeattdmg;
                break;
            case "human":
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
            case "Zombie":
                return 0;
            case "Skeleton":
                return 10;
            case "Necromancer":
                return 35;
            case "SkeletonArcher":
                return 5;
            case "ArmoredSkeleton":
                return 0;
            case "DeathKnight":
                return 25;

            case "Militia":
                return 5;
            case "Archer":
                return 5;
            case "Longbowman":
                return 5;
            case "Crossbowman":
                return 15;
            case "Footman":
                return 15;
            case "MountedKnight":
                return 20;
            case "HeroKing":
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
            case "undead":
                armor = entity.GetComponent<UndeadBehaviour>().armor;
                break;
            case "human":
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
            case "undead":
                entity.GetComponent<UndeadBehaviour>().armor = armor;
                break;
            case "human":
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
            case "Zombie":
                return 0;
            case "Skeleton":
                return 5;
            case "Necromancer":
                return 15;
            case "SkeletonArcher":
                return 0;
            case "ArmoredSkeleton":
                return 10;
            case "DeathKnight":
                return 15;

            case "Militia":
                return 5;
            case "Archer":
                return 0;
            case "Longbowman":
                return 0;
            case "Crossbowman":
                return 5;
            case "Footman":
                return 10;
            case "MountedKnight":
                return 15;
            case "HeroKing":
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
            case "undead":
                armorpiercing = entity.GetComponent<UndeadBehaviour>().armorpiercing;
                break;
            case "human":
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
            case "undead":
                entity.GetComponent<UndeadBehaviour>().armorpiercing = armorpiercing;
                break;
            case "human":
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
            case "Zombie":
                return 0;
            case "Skeleton":
                return 0;
            case "Necromancer":
                return 50;
            case "SkeletonArcher":
                return 5;
            case "ArmoredSkeleton":
                return 0;
            case "DeathKnight":
                return 0;

            case "Militia":
                return 0;
            case "Archer":
                return 5;
            case "Longbowman":
                return 5;
            case "Crossbowman":
                return 15;
            case "Footman":
                return 0;
            case "MountedKnight":
                return 0;
            case "HeroKing":
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
            case "undead":
                rangedarmorpiercing = entity.GetComponent<UndeadBehaviour>().rangedarmorpiercing;
                break;
            case "human":
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
            case "undead":
                entity.GetComponent<UndeadBehaviour>().rangedarmorpiercing = rangedarmorpiercing;
                break;
            case "human":
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
            case "Zombie":
                return 1;
            case "Skeleton":
                return 2;
            case "Necromancer":
                return 3;
            case "SkeletonArcher":
                return 2;
            case "ArmoredSkeleton":
                return 2;
            case "DeathKnight":
                return 3;

            case "Militia":
                return 2;
            case "Archer":
                return 2;
            case "Longbowman":
                return 2;
            case "Crossbowman":
                return 2;
            case "Footman":
                return 2;
            case "MountedKnight":
                return 3;
            case "HeroKing":
                return 3;
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
            case "undead":
                vision = entity.GetComponent<UndeadBehaviour>().vision;
                break;
            case "human":
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
            case "undead":
                entity.GetComponent<UndeadBehaviour>().vision = vision;
                break;
            case "human":
                entity.GetComponent<HumanBehaviour>().vision = vision;
                break;
        }
    }
    #endregion

    public void SetIdle(GameObject entity, bool idle)
    {
        string entityName = CleanName(entity);
        string faction = WhichFactionEntity(entityName);
        switch (faction)
        {
            case "undead":
                entity.GetComponent<UndeadBehaviour>().idle = idle;
                break;
            case "human":
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
            case "Zombie":
                return "undead";
            case "Skeleton":
                return "undead";
            case "Necromancer":
                return "undead";
            case "SkeletonArcher":
                return "undead";
            case "ArmoredSkeleton":
                return "undead";
            case "DeathKnight":
                return "undead";

            case "Militia":
                return "human";
            case "Archer":
                return "human";
            case "Longbowman":
                return "human";
            case "Crossbowman":
                return "human";
            case "Footman":
                return "human";
            case "MountedKnight":
                return "human";
            case "HeroKing":
                return "human";
        }
        return "unknown";
    }

    //returns summon soul cost
    public int summonSoulCost(string entity)
    {
        //------Determine Cost------
        switch (entity)
        {
            case "Zombie":
                return 100;
            case "Skeleton":
                return 150;
            case "Necromancer":
                return 10000;
            case "SkeletonArcher":
                return 150;
            case "ArmoredSkeleton":
                return 200;
            case "DeathKnight":
                return 250;
        }
        return 0;
    }
}
