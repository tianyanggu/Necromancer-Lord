using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class BuildingStats : MonoBehaviour {

    public List<string> undeadBuildings = new List<string>();
    public List<string> humanBuildings = new List<string>();
    //TODO make all max stats able to be changed by making them return from variables
    //TODO then save all max stats

    void Awake()
    {
        //undead buildings
        undeadBuildings.Add("UndeadVillage");
        undeadBuildings.Add("Necropolis");
        //human buildings
        humanBuildings.Add("Village");
    }

    public string CleanName(GameObject building)
    {
        string cleanEntity = Regex.Replace(building.name.Substring(2), @"[\d-]", string.Empty);
        return cleanEntity;
    }

    //TODO construction time of building, might fit in entity instead

    #region health
    public int GetMaxHealth(string entity)
    {
        switch (entity)
        {
            case "UndeadVillage":
                return 150;
            case "Necropolis":
                return 300;

            case "Village":
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
            case "undead":
                health = building.GetComponent<UndeadBuilding>().maxhealth;
                break;
            case "human":
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
            case "undead":
                health = building.GetComponent<UndeadBuilding>().currhealth;
                break;
            case "human":
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
            case "undead":
                building.GetComponent<UndeadBuilding>().maxhealth = health;
                break;
            case "human":
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
            case "undead":
                building.GetComponent<UndeadBuilding>().currhealth = health;
                break;
            case "human":
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
            case "UndeadVillage":
                return 0;
            case "Necropolis":
                return 3;

            case "Village":
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
            case "undead":
                range = building.GetComponent<UndeadBuilding>().range;
                break;
            case "human":
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
            case "undead":
                building.GetComponent<UndeadBuilding>().range = range;
                break;
            case "human":
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
            case "UndeadVillage":
                return 0;
            case "Necropolis":
                return 30;

            case "Village":
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
            case "undead":
                rangeattdmg = building.GetComponent<UndeadBuilding>().rangedattackdmg;
                break;
            case "human":
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
            case "undead":
                building.GetComponent<UndeadBuilding>().rangedattackdmg = rangeattdmg;
                break;
            case "human":
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
            case "UndeadVillage":
                return 5;
            case "Necropolis":
                return 20;

            case "Village":
                return 5;
        }
        return 0;
    }

    public int GetCurrDefense(GameObject building)
    {
        string entityName = CleanName(building);
        string faction = WhichFactionBuilding(entityName);
        int defense = 0;
        switch (faction)
        {
            case "undead":
                defense = building.GetComponent<UndeadBuilding>().defense;
                break;
            case "human":
                defense = building.GetComponent<HumanBuilding>().defense;
                break;
        }
        return defense;
    }

    public void SetDefense(GameObject building, int defense)
    {
        string entityName = CleanName(building);
        string faction = WhichFactionBuilding(entityName);
        switch (faction)
        {
            case "undead":
                building.GetComponent<UndeadBuilding>().defense = defense;
                break;
            case "human":
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
            case "UndeadVillage":
                return 2;
            case "Necropolis":
                return 4;

            case "Village":
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
            case "undead":
                vision = building.GetComponent<UndeadBuilding>().vision;
                break;
            case "human":
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
            case "undead":
                building.GetComponent<UndeadBuilding>().vision = vision;
                break;
            case "human":
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
            case "UndeadVillage":
                return new List<string>()
                {
                    "Graveyard"
                };
            case "Necropolis":
                return new List<string>()
                {
                    "Graveyard",
                    "Excavation Site",
                    "Dark Fletchery"
                };

            case "Village":
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
            case "Graveyard":
                return 2;
            case "Excavation Site":
                return 2;
            case "Dark Fletchery":
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
            case "undead":
                upgrades = building.GetComponent<UndeadBuilding>().upgrades;
                break;
            case "human":
                upgrades = building.GetComponent<HumanBuilding>().upgrades;
                break;
        }
        return upgrades;
    }

    public string GetCurrConstruction(GameObject building)
    {
        string buildingName = CleanName(building);
        string faction = WhichFactionBuilding(buildingName);
        string currConstruction = string.Empty;
        switch (faction)
        {
            case "undead":
                currConstruction = building.GetComponent<UndeadBuilding>().currConstruction;
                break;
            case "human":
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
            case "undead":
                building.GetComponent<UndeadBuilding>().currConstruction = upgrade;
                break;
            case "human":
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
            case "undead":
                currConstructionTimer = building.GetComponent<UndeadBuilding>().currConstructionTimer;
                break;
            case "human":
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
            case "undead":
                building.GetComponent<UndeadBuilding>().currConstructionTimer = time;
                break;
            case "human":
                building.GetComponent<HumanBuilding>().currConstructionTimer = time;
                break;
        }
    }
    #endregion

    //Recruitment - does not follow building name rule, follows upgrade on building and player faction
    //also does not follow switch case of buildings since checks entity
    #region 
    public HashSet<string> GetPossibleRecruitment(List<string> upgrades)
    {
        HashSet<string> possibleRecruitment = new HashSet<string>();
        for (int i=0; i<upgrades.Count; i++)
        {
            switch (upgrades[i])
            {
                case "Graveyard":
                    if (!possibleRecruitment.Contains("Zombie"))
                    {
                        possibleRecruitment.Add("Zombie");
                    }
                    if (!possibleRecruitment.Contains("Skeleton"))
                    {
                        possibleRecruitment.Add("Skeleton");
                    }
                    break;
                case "Excavation Site":
                    break;
                case "Dark Fletchery":
                    if (!possibleRecruitment.Contains("Skeleton Archer"))
                    {
                        possibleRecruitment.Add("Skeleton Archer");
                    }
                    break;

                case "Village":
                    break;
                default:
                    break;
            }
        }
        return possibleRecruitment;
    }

    public int GetRecruitmentTime(string entity)
    {
        switch (entity)
        {
            case "Zombie":
                return 2;
            case "Skeleton":
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
            case "undead":
                currRecruitment = building.GetComponent<UndeadBuilding>().currRecruitment;
                break;
            case "human":
                currRecruitment = building.GetComponent<HumanBuilding>().currRecruitment;
                break;
        }
        return currRecruitment;
    }

    public void SetCurrRecruitment(GameObject building, string entity)
    {
        string buildingName = CleanName(building);
        string faction = WhichFactionBuilding(buildingName);
        switch (faction)
        {
            case "undead":
                building.GetComponent<UndeadBuilding>().currRecruitment = entity;
                break;
            case "human":
                building.GetComponent<HumanBuilding>().currRecruitment = entity;
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
            case "undead":
                currRecruitmentTimer = building.GetComponent<UndeadBuilding>().currRecruitmentTimer;
                break;
            case "human":
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
            case "undead":
                building.GetComponent<UndeadBuilding>().currRecruitment = time;
                break;
            case "human":
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
            case "UndeadVillage":
                return "undead";
            case "Necropolis":
                return "undead";

            case "Village":
                return "human";
        }
        return "unknown";
    }

    //returns summon soul cost
    public int buildSoulCost(string entity)
    {
        //------Determine Cost------
        switch (entity)
        {
            case "UndeadVillage":
                return 100;
            case "Necropolis":
                return 200;
        }
        return 0;
    }
}
