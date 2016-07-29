using UnityEngine;
using System.Collections;

public class NecropolisMechanics : MonoBehaviour {

	public int health = 200;

	public int lasthealth = 200;

	public string currConstruction = "Empty";
    public int currConstructionTimer;

    public string currRecruitment = "Empty";
    public int currRecruitmentTimer;

    //building times
    public int timeSoulHarvester = 3;
    public int timeGraveyard = 2;
    //recruitment times
    public int timeZombie = 2;
    public int timeSkeleton = 3;


    public void TickProductionTimer ()
    {
        if (currConstruction != "Empty" )
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
        if (buildingName == "Soul Harvester")
        {
            currConstruction = "Soul Harvester";
            currConstructionTimer = timeSoulHarvester;
        }
        else if (buildingName == "Graveyard")
        {
            currConstruction = "Graveyard";
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
    }
}
