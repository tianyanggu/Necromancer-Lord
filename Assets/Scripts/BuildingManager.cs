using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;
using System.Linq;

public static class ActionNames
{
    public const string Recruit = "Recruit";
    public const string Build = "Build";
}

public class BuildingManager : MonoBehaviour {

    public BuildingStorage buildingStorage;
    public HexGrid hexGrid;
    public PlayerManager playerManager;

    public GameObject currBuilding;
    public bool buildingSelected;

    private bool necropolisClicked;
    private bool necropolisBuild;
    private bool necropolisRecruitment;

    private bool RecruitmentQueued;

    public void DisplayBuilding (int index) {
        currBuilding = hexGrid.GetBuildingObject(index);
        buildingSelected = true;
        if (currBuilding != null) {
            if (currBuilding.GetComponent<Building>().isRecruitmentQueued == true)
            {
                RecruitmentQueued = true;
            }
        }
    }

    public void ProductionQueue (string action, string production) {
        if (action == ActionNames.Build) {
            //TODO check if valid given upgrades
            currBuilding.GetComponent<Building>().currConstruction = production;
        }
        else if (action == ActionNames.Recruit)
        {
            //TODO check if valid given upgrades
            currBuilding.GetComponent<Building>().currRecruitment = production;
        }
    }

    public void TickProduction()
    {
        //if (!buildingStorage.activePlayerBuildings.Any())
        //{
        //    return;
        //}
        string currPlayerid = playerManager.currPlayer;
        char playerChar = currPlayerid[0];
        foreach (GameObject currBuildings in buildingStorage.PlayerBuildingList(playerChar))
        {
            currBuildings.GetComponent<Building>().TickProductionTimer();
            currBuildings.GetComponent<Building>().TickRecruitmentTimer();
                if (currBuildings.GetComponent<Building>().currConstructionTimer <= 0)
                {
                    currBuildings.GetComponent<Building>().CompleteConstruction();
                }
                if (currBuildings.GetComponent<Building>().currRecruitmentTimer <= 0)
                {
                    currBuildings.GetComponent<Building>().CompleteRecruitment();
                }
        }
    }

    void OnGUI () {
        //TODO major functions and modifications needed
        //x position, y position, width, length
        if (buildingSelected)
        {
            if(GUI.Button(new Rect(700,240,120,20), ActionNames.Build)) {
                if (necropolisBuild == false) {
                    necropolisBuild = true;
                    necropolisRecruitment = false;
                } else {
                    necropolisBuild = false;
                    necropolisRecruitment = false;
                }
		    }
            if(GUI.Button(new Rect(700,260,120,20), ActionNames.Recruit)) {
                if (necropolisRecruitment == false) {
                    necropolisRecruitment = true;
                    necropolisBuild = false;
                } else {
                    necropolisRecruitment = false;
                    necropolisBuild = false;
                }
		    }
            if (RecruitmentQueued)
            {
                if (GUI.Button(new Rect(700, 280, 120, 20), "Complete Recruitment"))
                {
                    currBuilding.GetComponent<Building>().CompleteRecruitment();
                    if (currBuilding.GetComponent<Building>().isRecruitmentQueued == false)
                    {
                        RecruitmentQueued = false;
                    }
                }
            }
        }
        if (necropolisBuild)
        {
            //TODO Add hover window for text details
            if (GUI.Button(new Rect(800, 240, 120, 20), UpgradeNames.Graveyard)) //Allows Recruiting Zombies
            {
                ProductionQueue(ActionNames.Build, UpgradeNames.Graveyard); 
            }
            if (GUI.Button(new Rect(800,260,120,20), UpgradeNames.ExcavationSite)) //Allows Recruiting Skeletons
            {
                ProductionQueue(ActionNames.Build, UpgradeNames.ExcavationSite); 
            }
            if (GUI.Button(new Rect(800, 280, 120, 20), UpgradeNames.SinewFletchery)) //Allows Recruiting Skeleton Archers. Requires Excavation Site.
            {
                ProductionQueue(ActionNames.Build, UpgradeNames.SinewFletchery);
            }
            //if (GUI.Button(new Rect(800, 260, 120, 20), "Dark Magic Forge")) //Allows Recruiting Skeletons Mages. Requires Excavation Site.
            //{
            //    ProductionQueue(ActionNames.Build, "Skeleton Mage");
            //}
        }
        if (necropolisRecruitment)
        {
            if(GUI.Button(new Rect(800,240,120,20), EntityNames.Zombie)) //Weak Early Tier Melee Unit. Resistent to other weak unit's attacks.
            {
                ProductionQueue(ActionNames.Recruit, EntityNames.Zombie);
		    }
            if(GUI.Button(new Rect(800,260,120,20), EntityNames.Skeleton)) //Early Tier Melee Unit.
            {
                ProductionQueue(ActionNames.Recruit, EntityNames.Skeleton);
		    }
            if (GUI.Button(new Rect(800, 280, 120, 20), "Skeleton Archer")) //Early Tier Physical Ranged Unit.
            {
                ProductionQueue(ActionNames.Recruit, "Skeleton Archer");
            }
            //if (GUI.Button(new Rect(800, 260, 120, 20), "Skeleton Mage")) //Early Tier Magic Ranged Unit.
            //{
            //    ProductionQueue(ActionNames.Recruit, "Skeleton Mage");
            //}
        }
    }
}
