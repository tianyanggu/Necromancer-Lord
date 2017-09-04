using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System;

public static class BuildingNames
{
    public const string UndeadVillage = "UndeadVillage";
    public const string Necropolis = "Necropolis";

    public const string Village = "Village";
}

public static class UpgradeNames
{
    public const string Graveyard = "Graveyard";
    public const string ExcavationSite = "ExcavationSite";

    public const string SinewFletchery = "SinewFletchery";
}

public class BuildingStats : MonoBehaviour {

    public List<string> undeadBuildings = new List<string>();
    public List<string> humanBuildings = new List<string>();
    //TODO make all max stats able to be changed by making them return from variables
    //TODO then save all max stats

    void Awake()
    {
        //undead buildings
        undeadBuildings.Add(BuildingNames.UndeadVillage);
        undeadBuildings.Add(BuildingNames.Necropolis);
        //human buildings
        humanBuildings.Add(BuildingNames.Village);
    }

    public string CleanName(GameObject building)
    {
        string cleanbuilding = Regex.Replace(building.name.Substring(2), @"[\d-]", string.Empty);
        return cleanbuilding;
    }

    //TODO recruitment time of building, might fit in entity instead

    #region playerID
    public string GetPlayerID(GameObject building)
    {
        string buildingName = CleanName(building);
        string faction = WhichFactionBuilding(buildingName);
        string player = string.Empty;
        switch (faction)
        {
            case FactionNames.Undead:
                player = building.GetComponent<UndeadBuilding>().playerID;
                break;
            case FactionNames.Human:
                player = building.GetComponent<HumanBuilding>().playerID;
                break;
        }
        return player;
    }

    public void SetPlayerID(GameObject building, string playerID)
    {
        string buildingName = CleanName(building);
        string faction = WhichFactionBuilding(buildingName);
        switch (faction)
        {
            case FactionNames.Undead:
                building.GetComponent<UndeadBuilding>().playerID = playerID;
                break;
            case FactionNames.Human:
                building.GetComponent<HumanBuilding>().playerID = playerID;
                break;
        }
    }
    #endregion

    #region type
    public string GetType(GameObject building)
    {
        string buildingName = CleanName(building);
        string faction = WhichFactionBuilding(buildingName);
        string type = string.Empty;
        switch (faction)
        {
            case FactionNames.Undead:
                type = building.GetComponent<UndeadBuilding>().type;
                break;
            case FactionNames.Human:
                type = building.GetComponent<HumanBuilding>().type;
                break;
        }
        return type;
    }

    public void SetType(GameObject building, string type)
    {
        string buildingName = CleanName(building);
        string faction = WhichFactionBuilding(buildingName);
        switch (faction)
        {
            case FactionNames.Undead:
                building.GetComponent<UndeadBuilding>().type = type;
                break;
            case FactionNames.Human:
                building.GetComponent<HumanBuilding>().type = type;
                break;
        }
    }
    #endregion

    #region uniqueID
    public Guid GetUniqueID(GameObject building)
    {
        string buildingName = CleanName(building);
        string faction = WhichFactionBuilding(buildingName);
        Guid player = Guid.Empty;
        switch (faction)
        {
            case FactionNames.Undead:
                player = building.GetComponent<UndeadBuilding>().uniqueID;
                break;
            case FactionNames.Human:
                player = building.GetComponent<HumanBuilding>().uniqueID;
                break;
        }
        return player;
    }

    public void SetUniqueID(GameObject building, Guid uniqueID)
    {
        string buildingName = CleanName(building);
        string faction = WhichFactionBuilding(buildingName);
        switch (faction)
        {
            case FactionNames.Undead:
                building.GetComponent<UndeadBuilding>().uniqueID = uniqueID;
                break;
            case FactionNames.Human:
                building.GetComponent<HumanBuilding>().uniqueID = uniqueID;
                break;
        }
    }
    #endregion

    #region cellIndex
    public int GetCellIndex(GameObject building)
    {
        string buildingName = CleanName(building);
        string faction = WhichFactionBuilding(buildingName);
        int player = 0;
        switch (faction)
        {
            case FactionNames.Undead:
                player = building.GetComponent<UndeadBuilding>().cellIndex;
                break;
            case FactionNames.Human:
                player = building.GetComponent<HumanBuilding>().cellIndex;
                break;
        }
        return player;
    }

    public void SetCellIndex(GameObject building, int cellIndex)
    {
        string buildingName = CleanName(building);
        string faction = WhichFactionBuilding(buildingName);
        switch (faction)
        {
            case FactionNames.Undead:
                building.GetComponent<UndeadBuilding>().cellIndex = cellIndex;
                break;
            case FactionNames.Human:
                building.GetComponent<HumanBuilding>().cellIndex = cellIndex;
                break;
        }
    }
    #endregion

    #region health
    public int GetMaxHealth(string building)
    {
        switch (building)
        {
            case BuildingNames.UndeadVillage:
                return 150;
            case BuildingNames.Necropolis:
                return 300;

            case BuildingNames.Village:
                return 200;
        }
        return 0;
    }

    public int GetCurrMaxHealth(GameObject building)
    {
        string buildingName = CleanName(building);
        string faction = WhichFactionBuilding(buildingName);
        int health = 0;
        switch (faction)
        {
            case FactionNames.Undead:
                health = building.GetComponent<UndeadBuilding>().maxhealth;
                break;
            case FactionNames.Human:
                health = building.GetComponent<HumanBuilding>().maxhealth;
                break;
        }
        return health;
    }

    public int GetCurrHealth(GameObject building)
    {
        string buildingName = CleanName(building);
        string faction = WhichFactionBuilding(buildingName);
        int health = 0;
        switch (faction)
        {
            case FactionNames.Undead:
                health = building.GetComponent<UndeadBuilding>().currhealth;
                break;
            case FactionNames.Human:
                health = building.GetComponent<HumanBuilding>().currhealth;
                break;
        }
        return health;
    }

    public void SetMaxHealth(GameObject building, int health)
    {
        string buildingName = CleanName(building);
        string faction = WhichFactionBuilding(buildingName);
        switch (faction)
        {
            case FactionNames.Undead:
                building.GetComponent<UndeadBuilding>().maxhealth = health;
                break;
            case FactionNames.Human:
                building.GetComponent<HumanBuilding>().maxhealth = health;
                break;
        }
    }

    public void SetCurrHealth(GameObject building, int health)
    {
        string buildingName = CleanName(building);
        string faction = WhichFactionBuilding(buildingName);
        switch (faction)
        {
            case FactionNames.Undead:
                building.GetComponent<UndeadBuilding>().currhealth = health;
                break;
            case FactionNames.Human:
                building.GetComponent<HumanBuilding>().currhealth = health;
                break;
        }
    }
    #endregion

    #region range
    public int GetRange(string building)
    {
        switch (building)
        {
            case BuildingNames.UndeadVillage:
                return 0;
            case BuildingNames.Necropolis:
                return 3;

            case BuildingNames.Village:
                return 0;
        }
        return 0;
    }

    public int GetCurrRange(GameObject building)
    {
        string buildingName = CleanName(building);
        string faction = WhichFactionBuilding(buildingName);
        int range = 0;
        switch (faction)
        {
            case FactionNames.Undead:
                range = building.GetComponent<UndeadBuilding>().range;
                break;
            case FactionNames.Human:
                range = building.GetComponent<HumanBuilding>().range;
                break;
        }
        return range;
    }

    public void SetRange(GameObject building, int range)
    {
        string buildingName = CleanName(building);
        string faction = WhichFactionBuilding(buildingName);
        switch (faction)
        {
            case FactionNames.Undead:
                building.GetComponent<UndeadBuilding>().range = range;
                break;
            case FactionNames.Human:
                building.GetComponent<HumanBuilding>().range = range;
                break;
        }
    }
    #endregion

    #region rangedattackdmg
    public int GetRangedAttackDmg(string building)
    {
        switch (building)
        {
            case BuildingNames.UndeadVillage:
                return 0;
            case BuildingNames.Necropolis:
                return 30;

            case BuildingNames.Village:
                return 0;
        }
        return 0;
    }

    public int GetCurrRangedAttackDmg(GameObject building)
    {
        string buildingName = CleanName(building);
        string faction = WhichFactionBuilding(buildingName);
        int rangeattdmg = 0;
        switch (faction)
        {
            case FactionNames.Undead:
                rangeattdmg = building.GetComponent<UndeadBuilding>().rangedattackdmg;
                break;
            case FactionNames.Human:
                rangeattdmg = building.GetComponent<HumanBuilding>().rangedattackdmg;
                break;
        }
        return rangeattdmg;
    }

    public void SetRangedAttackDmg(GameObject building, int rangeattdmg)
    {
        string buildingName = CleanName(building);
        string faction = WhichFactionBuilding(buildingName);
        switch (faction)
        {
            case FactionNames.Undead:
                building.GetComponent<UndeadBuilding>().rangedattackdmg = rangeattdmg;
                break;
            case FactionNames.Human:
                building.GetComponent<HumanBuilding>().rangedattackdmg = rangeattdmg;
                break;
        }
    }
    #endregion

    #region defense
    public int GetDefense (string building)
    {
        switch (building)
        {
            case BuildingNames.UndeadVillage:
                return 5;
            case BuildingNames.Necropolis:
                return 20;

            case BuildingNames.Village:
                return 5;
        }
        return 0;
    }

    public int GetCurrDefense(GameObject building)
    {
        string buildingName = CleanName(building);
        string faction = WhichFactionBuilding(buildingName);
        int defense = 0;
        switch (faction)
        {
            case FactionNames.Undead:
                defense = building.GetComponent<UndeadBuilding>().defense;
                break;
            case FactionNames.Human:
                defense = building.GetComponent<HumanBuilding>().defense;
                break;
        }
        return defense;
    }

    public void SetDefense(GameObject building, int defense)
    {
        string buildingName = CleanName(building);
        string faction = WhichFactionBuilding(buildingName);
        switch (faction)
        {
            case FactionNames.Undead:
                building.GetComponent<UndeadBuilding>().defense = defense;
                break;
            case FactionNames.Human:
                building.GetComponent<HumanBuilding>().defense = defense;
                break;
        }
    }
    #endregion

    #region vision
    public int GetVision(string building)
    {
        switch (building)
        {
            case BuildingNames.UndeadVillage:
                return 2;
            case BuildingNames.Necropolis:
                return 4;

            case BuildingNames.Village:
                return 2;
        }
        return 0;
    }

    public int GetCurrVision(GameObject building)
    {
        string buildingName = CleanName(building);
        string faction = WhichFactionBuilding(buildingName);
        int vision = 0;
        switch (faction)
        {
            case FactionNames.Undead:
                vision = building.GetComponent<UndeadBuilding>().vision;
                break;
            case FactionNames.Human:
                vision = building.GetComponent<HumanBuilding>().vision;
                break;
        }
        return vision;
    }

    public void SetVision(GameObject building, int vision)
    {
        string buildingName = CleanName(building);
        string faction = WhichFactionBuilding(buildingName);
        switch (faction)
        {
            case FactionNames.Undead:
                building.GetComponent<UndeadBuilding>().vision = vision;
                break;
            case FactionNames.Human:
                building.GetComponent<HumanBuilding>().vision = vision;
                break;
        }
    }
    #endregion

    //Upgrades - does not follow switch case of buildings since checks upgrade's time
    #region Upgrades
    public List<string> GetPossibleUpgrades(string building)
    {
        switch (building)
        {
            case BuildingNames.UndeadVillage:
                return new List<string>()
                {
                    UpgradeNames.Graveyard
                };
            case BuildingNames.Necropolis:
                return new List<string>()
                {
                    UpgradeNames.Graveyard,
                    UpgradeNames.ExcavationSite,
                    UpgradeNames.SinewFletchery
                };

            case BuildingNames.Village:
                return new List<string>()
                {
                    "Chapel"
                };
        }
        return new List<string>();
    }

    public int GetConstructionCompletionTime(string upgrade)
    {
        switch (upgrade)
        {
            case UpgradeNames.Graveyard:
                return 2;
            case UpgradeNames.ExcavationSite:
                return 2;
            case UpgradeNames.SinewFletchery:
                return 3;

            case "Chapel":
                return 2;
        }
        return 0;
    }

    public List<string> GetCompletedUpgrades(GameObject building)
    {
        string buildingName = CleanName(building);
        string faction = WhichFactionBuilding(buildingName);
        List<string> upgrades = new List<string>();
        switch (faction)
        {
            case FactionNames.Undead:
                upgrades = building.GetComponent<UndeadBuilding>().upgrades;
                break;
            case FactionNames.Human:
                upgrades = building.GetComponent<HumanBuilding>().upgrades;
                break;
        }
        return upgrades;
    }

    public void AddCompletedUpgrades(GameObject building, string upgrade)
    {
        string buildingName = CleanName(building);
        string faction = WhichFactionBuilding(buildingName);
        switch (faction)
        {
            case FactionNames.Undead:
                building.GetComponent<UndeadBuilding>().upgrades.Add(upgrade);
                break;
            case FactionNames.Human:
                building.GetComponent<HumanBuilding>().upgrades.Add(upgrade);
                break;
        }
    }

    public void RemoveCompletedUpgrades(GameObject building, string upgrade)
    {
        string buildingName = CleanName(building);
        string faction = WhichFactionBuilding(buildingName);
        switch (faction)
        {
            case FactionNames.Undead:
                building.GetComponent<UndeadBuilding>().upgrades.Remove(upgrade);
                break;
            case FactionNames.Human:
                building.GetComponent<HumanBuilding>().upgrades.Remove(upgrade);
                break;
        }
    }

    public string GetCurrConstruction(GameObject building)
    {
        string buildingName = CleanName(building);
        string faction = WhichFactionBuilding(buildingName);
        string currConstruction = string.Empty;
        switch (faction)
        {
            case FactionNames.Undead:
                currConstruction = building.GetComponent<UndeadBuilding>().currConstruction;
                break;
            case FactionNames.Human:
                currConstruction = building.GetComponent<HumanBuilding>().currConstruction;
                break;
        }
        return currConstruction;
    }

    public void SetCurrConstruction(GameObject building, string upgrade)
    {
        string buildingName = CleanName(building);
        string faction = WhichFactionBuilding(buildingName);
        switch (faction)
        {
            case FactionNames.Undead:
                building.GetComponent<UndeadBuilding>().currConstruction = upgrade;
                break;
            case FactionNames.Human:
                building.GetComponent<HumanBuilding>().currConstruction = upgrade;
                break;
        }
    }

    public int GetCurrConstructionTimer(GameObject building)
    {
        string buildingName = CleanName(building);
        string faction = WhichFactionBuilding(buildingName);
        int currConstructionTimer = 0;
        switch (faction)
        {
            case FactionNames.Undead:
                currConstructionTimer = building.GetComponent<UndeadBuilding>().currConstructionTimer;
                break;
            case FactionNames.Human:
                currConstructionTimer = building.GetComponent<HumanBuilding>().currConstructionTimer;
                break;
        }
        return currConstructionTimer;
    }

    public void SetCurrConstructionTimer(GameObject building, int time)
    {
        string buildingName = CleanName(building);
        string faction = WhichFactionBuilding(buildingName);
        switch (faction)
        {
            case FactionNames.Undead:
                building.GetComponent<UndeadBuilding>().currConstructionTimer = time;
                break;
            case FactionNames.Human:
                building.GetComponent<HumanBuilding>().currConstructionTimer = time;
                break;
        }
    }
    #endregion

    #region permaEffects
    public List<string> GetPermaEffects(GameObject building)
    {
        string buildingName = CleanName(building);
        string faction = WhichFactionBuilding(buildingName);
        List<string> permaEffects = new List<string>();
        switch (faction)
        {
            case FactionNames.Undead:
                permaEffects = building.GetComponent<UndeadBuilding>().permaEffects;
                break;
            case FactionNames.Human:
                permaEffects = building.GetComponent<HumanBuilding>().permaEffects;
                break;
        }
        return permaEffects;
    }

    public void SetPermaEffects(GameObject building, List<string> permaEffects)
    {
        string buildingName = CleanName(building);
        string faction = WhichFactionBuilding(buildingName);
        switch (faction)
        {
            case FactionNames.Undead:
                building.GetComponent<UndeadBuilding>().permaEffects = permaEffects;
                break;
            case FactionNames.Human:
                building.GetComponent<HumanBuilding>().permaEffects = permaEffects;
                break;
        }
    }

    public void AddPermaEffects(GameObject building, string permaEffects)
    {
        string buildingName = CleanName(building);
        string faction = WhichFactionBuilding(buildingName);
        switch (faction)
        {
            case FactionNames.Undead:
                building.GetComponent<UndeadBuilding>().permaEffects.Add(permaEffects);
                break;
            case FactionNames.Human:
                building.GetComponent<HumanBuilding>().permaEffects.Add(permaEffects);
                break;
        }
    }

    public void RemovePermaEffects(GameObject building, string permaEffects)
    {
        string buildingName = CleanName(building);
        string faction = WhichFactionBuilding(buildingName);
        switch (faction)
        {
            case FactionNames.Undead:
                building.GetComponent<UndeadBuilding>().permaEffects.Remove(permaEffects);
                break;
            case FactionNames.Human:
                building.GetComponent<HumanBuilding>().permaEffects.Remove(permaEffects);
                break;
        }
    }
    #endregion

    #region tempEffects
    public List<KeyValuePair<string, int>> GetTempEffects(GameObject building)
    {
        string buildingName = CleanName(building);
        string faction = WhichFactionBuilding(buildingName);
        List<KeyValuePair<string, int>> tempEffects = new List<KeyValuePair<string, int>>();
        switch (faction)
        {
            case FactionNames.Undead:
                tempEffects = building.GetComponent<UndeadBuilding>().tempEffects;
                break;
            case FactionNames.Human:
                tempEffects = building.GetComponent<HumanBuilding>().tempEffects;
                break;
        }
        return tempEffects;
    }

    public void SetTempEffects(GameObject building, List<KeyValuePair<string, int>> tempEffects)
    {
        string buildingName = CleanName(building);
        string faction = WhichFactionBuilding(buildingName);
        switch (faction)
        {
            case FactionNames.Undead:
                building.GetComponent<UndeadBuilding>().tempEffects = tempEffects;
                break;
            case FactionNames.Human:
                building.GetComponent<HumanBuilding>().tempEffects = tempEffects;
                break;
        }
    }

    public void AddTempEffects(GameObject building, string tempEffects, int duration)
    {
        string buildingName = CleanName(building);
        string faction = WhichFactionBuilding(buildingName);
        switch (faction)
        {
            case FactionNames.Undead:
                building.GetComponent<UndeadBuilding>().tempEffects.Add(new KeyValuePair<string, int>(tempEffects, duration));
                break;
            case FactionNames.Human:
                building.GetComponent<HumanBuilding>().tempEffects.Add(new KeyValuePair<string, int>(tempEffects, duration));
                break;
        }
    }

    public void RemoveTempEffects(GameObject building, string tempEffects, int duration)
    {
        string buildingName = CleanName(building);
        string faction = WhichFactionBuilding(buildingName);
        switch (faction)
        {
            case FactionNames.Undead:
                building.GetComponent<UndeadBuilding>().tempEffects.Remove(new KeyValuePair<string, int>(tempEffects, duration));
                break;
            case FactionNames.Human:
                building.GetComponent<HumanBuilding>().tempEffects.Remove(new KeyValuePair<string, int>(tempEffects, duration));
                break;
        }
    }
    #endregion

    //Recruitment - does not follow building name rule, follows upgrade on building and player faction
    //also does not follow switch case of buildings since checks building
    #region 
    public HashSet<string> GetPossibleRecruitment(List<string> upgrades)
    {
        HashSet<string> possibleRecruitment = new HashSet<string>();
        for (int i=0; i<upgrades.Count; i++)
        {
            switch (upgrades[i])
            {
                case UpgradeNames.Graveyard:
                    if (!possibleRecruitment.Contains(EntityNames.Zombie))
                    {
                        possibleRecruitment.Add(EntityNames.Zombie);
                    }
                    if (!possibleRecruitment.Contains(EntityNames.Skeleton))
                    {
                        possibleRecruitment.Add(EntityNames.Skeleton);
                    }
                    break;
                case UpgradeNames.ExcavationSite:
                    break;
                case UpgradeNames.SinewFletchery:
                    if (!possibleRecruitment.Contains("Skeleton Archer"))
                    {
                        possibleRecruitment.Add("Skeleton Archer");
                    }
                    break;

                case BuildingNames.Village:
                    break;
                default:
                    break;
            }
        }
        return possibleRecruitment;
    }

    public int GetRecruitmentTime(string building)
    {
        switch (building)
        {
            case EntityNames.Zombie:
                return 2;
            case EntityNames.Skeleton:
                return 4;
            case "Skeleton Archer":
                return 4;

            case "Villager":
                return 2;
        }
        return 0;
    }

    public string GetCurrRecruitment(GameObject building)
    {
        string buildingName = CleanName(building);
        string faction = WhichFactionBuilding(buildingName);
        string currRecruitment = string.Empty;
        switch (faction)
        {
            case FactionNames.Undead:
                currRecruitment = building.GetComponent<UndeadBuilding>().currRecruitment;
                break;
            case FactionNames.Human:
                currRecruitment = building.GetComponent<HumanBuilding>().currRecruitment;
                break;
        }
        return currRecruitment;
    }

    public void SetCurrRecruitment(GameObject building, string recruit)
    {
        string buildingName = CleanName(building);
        string faction = WhichFactionBuilding(buildingName);
        switch (faction)
        {
            case FactionNames.Undead:
                building.GetComponent<UndeadBuilding>().currRecruitment = recruit;
                break;
            case FactionNames.Human:
                building.GetComponent<HumanBuilding>().currRecruitment = recruit;
                break;
        }
    }

    public int GetCurrRecruitmentTimer(GameObject building)
    {
        string buildingName = CleanName(building);
        string faction = WhichFactionBuilding(buildingName);
        int currRecruitmentTimer = 0;
        switch (faction)
        {
            case FactionNames.Undead:
                currRecruitmentTimer = building.GetComponent<UndeadBuilding>().currRecruitmentTimer;
                break;
            case FactionNames.Human:
                currRecruitmentTimer = building.GetComponent<HumanBuilding>().currRecruitmentTimer;
                break;
        }
        return currRecruitmentTimer;
    }

    public void SetCurrRecruitmentTimer(GameObject building, string time)
    {
        string buildingName = CleanName(building);
        string faction = WhichFactionBuilding(buildingName);
        switch (faction)
        {
            case FactionNames.Undead:
                building.GetComponent<UndeadBuilding>().currRecruitment = time;
                break;
            case FactionNames.Human:
                building.GetComponent<HumanBuilding>().currRecruitment = time;
                break;
        }
    }
    #endregion

    //returns faction
    public string WhichFactionBuilding(string building)
    {
        //------Determine Faction------
        switch (building)
        {
            case BuildingNames.UndeadVillage:
                return FactionNames.Undead;
            case BuildingNames.Necropolis:
                return FactionNames.Undead;

            case BuildingNames.Village:
                return FactionNames.Human;
        }
        return "unknown";
    }

    //returns summon soul cost
    public int buildSoulCost(string building)
    {
        //------Determine Cost------
        switch (building)
        {
            case BuildingNames.UndeadVillage:
                return 100;
            case BuildingNames.Necropolis:
                return 200;
        }
        return 0;
    }
}
