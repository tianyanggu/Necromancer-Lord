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

    public List<string> Buildings = new List<string>();
    public List<string> humanBuildings = new List<string>();
    //TODO make all max stats able to be changed by making them return from variables
    //TODO then save all max stats

    void Awake()
    {
        //undead buildings
        Buildings.Add(BuildingNames.UndeadVillage);
        Buildings.Add(BuildingNames.Necropolis);
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
        string player = building.GetComponent<Building>().playerID;
        return player;
    }

    public void SetPlayerID(GameObject building, string playerID)
    {
        building.GetComponent<Building>().playerID = playerID;
    }
    #endregion

    #region type
    public string GetType(GameObject building)
    {
        string type = building.GetComponent<Building>().type;
        return type;
    }

    public void SetType(GameObject building, string type)
    {
        building.GetComponent<Building>().type = type;
    }
    #endregion

    #region uniqueID
    public Guid GetUniqueID(GameObject building)
    {
        Guid player = building.GetComponent<Building>().uniqueID;
        return player;
    }

    public void SetUniqueID(GameObject building, Guid uniqueID)
    {
        building.GetComponent<Building>().uniqueID = uniqueID;
    }
    #endregion

    #region cellIndex
    public int GetCellIndex(GameObject building)
    {
        int player = building.GetComponent<Building>().cellIndex;
        return player;
    }

    public void SetCellIndex(GameObject building, int cellIndex)
    {
        building.GetComponent<Building>().cellIndex = cellIndex;
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
        int health = building.GetComponent<Building>().maxhealth;
        return health;
    }

    public int GetCurrHealth(GameObject building)
    {
        int health = building.GetComponent<Building>().currhealth;
        return health;
    }

    public void SetMaxHealth(GameObject building, int health)
    {
        building.GetComponent<Building>().maxhealth = health;
    }

    public void SetCurrHealth(GameObject building, int health)
    {
        building.GetComponent<Building>().currhealth = health;
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
        int range = building.GetComponent<Building>().range;
        return range;
    }

    public void SetRange(GameObject building, int range)
    {
        building.GetComponent<Building>().range = range;
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
        int rangeattdmg = building.GetComponent<Building>().rangedattackdmg;
        return rangeattdmg;
    }

    public void SetRangedAttackDmg(GameObject building, int rangeattdmg)
    {
        building.GetComponent<Building>().rangedattackdmg = rangeattdmg;
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
        int defense = building.GetComponent<Building>().defense;
        return defense;
    }

    public void SetDefense(GameObject building, int defense)
    {
        building.GetComponent<Building>().defense = defense;
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
        int vision = building.GetComponent<Building>().vision;
        return vision;
    }

    public void SetVision(GameObject building, int vision)
    {
        building.GetComponent<Building>().vision = vision;
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
        List<string> upgrades = building.GetComponent<Building>().upgrades;
        return upgrades;
    }

    public void AddCompletedUpgrades(GameObject building, string upgrade)
    {
        building.GetComponent<Building>().upgrades.Add(upgrade);
    }

    public void RemoveCompletedUpgrades(GameObject building, string upgrade)
    {
        building.GetComponent<Building>().upgrades.Remove(upgrade);
    }

    public string GetCurrConstruction(GameObject building)
    {
        string currConstruction = building.GetComponent<Building>().currConstruction;
        return currConstruction;
    }

    public void SetCurrConstruction(GameObject building, string upgrade)
    {
        building.GetComponent<Building>().currConstruction = upgrade;
    }

    public int GetCurrConstructionTimer(GameObject building)
    {
        int currConstructionTimer = building.GetComponent<Building>().currConstructionTimer;
        return currConstructionTimer;
    }

    public void SetCurrConstructionTimer(GameObject building, int time)
    {
        building.GetComponent<Building>().currConstructionTimer = time;
    }
    #endregion

    #region permaEffects
    public List<string> GetPermaEffects(GameObject building)
    {
        List<string> permaEffects = building.GetComponent<Building>().permaEffects;
        return permaEffects;
    }

    public void SetPermaEffects(GameObject building, List<string> permaEffects)
    {
        building.GetComponent<Building>().permaEffects = permaEffects;
    }

    public void AddPermaEffects(GameObject building, string permaEffects)
    {
        building.GetComponent<Building>().permaEffects.Add(permaEffects);
    }

    public void RemovePermaEffects(GameObject building, string permaEffects)
    {
        building.GetComponent<Building>().permaEffects.Remove(permaEffects);
    }
    #endregion

    #region tempEffects
    public List<KeyValuePair<string, int>> GetTempEffects(GameObject building)
    {
        List<KeyValuePair<string, int>> tempEffects = building.GetComponent<Building>().tempEffects;
        return tempEffects;
    }

    public void SetTempEffects(GameObject building, List<KeyValuePair<string, int>> tempEffects)
    {
        building.GetComponent<Building>().tempEffects = tempEffects;
    }

    public void AddTempEffects(GameObject building, string tempEffects, int duration)
    {
        building.GetComponent<Building>().tempEffects.Add(new KeyValuePair<string, int>(tempEffects, duration));
    }

    public void RemoveTempEffects(GameObject building, string tempEffects, int duration)
    {
        building.GetComponent<Building>().tempEffects.Remove(new KeyValuePair<string, int>(tempEffects, duration));
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
            case EntityNames.SkeletonArcher:
                return 4;

            case EntityNames.Militia:
                return 2;
        }
        return 0;
    }

    public string GetCurrRecruitment(GameObject building)
    {
        string currRecruitment = building.GetComponent<Building>().currRecruitment;
        return currRecruitment;
    }

    public void SetCurrRecruitment(GameObject building, string recruit)
    {
        building.GetComponent<Building>().currRecruitment = recruit;
    }

    public int GetCurrRecruitmentTimer(GameObject building)
    {
        int currRecruitmentTimer = building.GetComponent<Building>().currRecruitmentTimer;
        return currRecruitmentTimer;
    }

    public void SetCurrRecruitmentTimer(GameObject building, string time)
    {
        building.GetComponent<Building>().currRecruitment = time;
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
