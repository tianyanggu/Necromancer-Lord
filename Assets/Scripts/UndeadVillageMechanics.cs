using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UndeadVillageMechanics : MonoBehaviour {

    public int health = 200;
    public int lasthealth = 200;
    public List<string> upgrades;

    public string currConstruction = "Empty";
    public int currConstructionTimer;

    public string currRecruitment = "Empty";
    public int currRecruitmentTimer;
    public bool IsRecruitmentQueued;

    //building times
    public int timeExcavationSite = 3;
    public int timeGraveyard = 2;
    //recruitment times
    public int timeZombie = 2;
    public int timeSkeleton = 3;
    public int timeSkeletonArcher = 3;

    public void TickProductionTimer()
    {
        if (currConstruction != "Empty")
        {
            currConstructionTimer--;
        }
    }

    public void TickRecruitmentTimer()
    {
        if (currRecruitment != "Empty")
        {
            currRecruitmentTimer--;
        }
    }

    public void UpdateProduction(string buildingName)
    {
        if (buildingName == "Graveyard")
        {
            currConstruction = "Graveyard";
            currConstructionTimer = timeGraveyard;
        }
        else if (buildingName == "Excavation Site")
        {
            currConstruction = "Excavation Site";
            currConstructionTimer = timeExcavationSite;
        }
        else if (buildingName == "Dark Fletchery")
        {
            currConstruction = "Dark Fletchery";
            currConstructionTimer = timeGraveyard;
        }
    }

    public void UpdateRecruitment(string recruitName)
    {
        if (recruitName == "Zombie")
        {
            currRecruitment = "Zombie";
            currRecruitmentTimer = timeZombie;
        }
        else if (recruitName == "Skeleton")
        {
            currRecruitment = "Skeleton";
            currRecruitmentTimer = timeSkeleton;
        }
        else if (recruitName == "Skeleton Archer")
        {
            currRecruitment = "Skeleton Archer";
            currRecruitmentTimer = timeSkeletonArcher;
        }
    }

    public void CompleteConstruction()
    {
        if (currConstruction != "Empty")
        {
            upgrades.Add(currConstruction);
            currConstruction = "Empty";
        }
    }

    public void CompleteRecruitment()
    {
        if (currRecruitment != "Empty")
        {
            Vector3 currPos = gameObject.transform.position;
            GameObject hexGrid = GameObject.Find("Hex Grid");
            int currIndex = hexGrid.GetComponent<HexGrid>().GetCellIndex(currPos);
            if (hexGrid.GetComponent<HexGrid>().GetEntityName(currIndex) == "Empty")
            {
                GameObject summon = GameObject.Find("Summon");
                summon.GetComponent<Summon>().SummonEntity(currIndex, currRecruitment);
                currRecruitment = "Empty";
            }
            else
            {
                IsRecruitmentQueued = true;
            }
        }
    }
}
