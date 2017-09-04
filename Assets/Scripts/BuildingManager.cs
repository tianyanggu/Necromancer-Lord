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

    GameObject currBuilding;
    private string cleanBuildingName;

    private bool necropolisClicked;
    private bool necropolisBuild;
    private bool necropolisRecruitment;

    private bool RecruitmentQueued;

    public void DisplayBuilding (string building, int index) {
        cleanBuildingName = Regex.Replace(building, @"[\d-]", string.Empty);
        currBuilding = hexGrid.GetBuildingObject(index);

        string faction = buildingStorage.WhichFactionBuilding(cleanBuildingName);
        if (faction == FactionNames.Undead) {
            if (cleanBuildingName == BuildingNames.Necropolis) {
                necropolisClicked = true;
                if (currBuilding.GetComponent<UndeadBuilding>().isRecruitmentQueued == true)
                {
                    RecruitmentQueued = true;
                }
            }
        } else if (faction == FactionNames.Human) {

        }
    }

    public void ProductionQueue (string action, string production) {
        if (action == ActionNames.Build) {
            if (cleanBuildingName == BuildingNames.Necropolis)
            {
                currBuilding.GetComponent<UndeadBuilding>().currConstruction = production;
            }
        }
        else if (action == ActionNames.Recruit)
        {
            if (cleanBuildingName == BuildingNames.Necropolis)
            {
                currBuilding.GetComponent<UndeadBuilding>().currRecruitment = production;
            }
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
            string cleanStorageBuildingName = Regex.Replace(currBuildings.name.Substring(2), @"[\d-]", string.Empty);
            if (cleanStorageBuildingName == BuildingNames.Necropolis)
            {
                currBuildings.GetComponent<UndeadBuilding>().TickProductionTimer();
                currBuildings.GetComponent<UndeadBuilding>().TickRecruitmentTimer();
                if (currBuildings.GetComponent<UndeadBuilding>().currConstructionTimer <= 0)
                {
                    currBuildings.GetComponent<UndeadBuilding>().CompleteConstruction();
                }
                if (currBuildings.GetComponent<UndeadBuilding>().currRecruitmentTimer <= 0)
                {
                    currBuildings.GetComponent<UndeadBuilding>().CompleteRecruitment();
                }
            }
        }
    }

    void OnGUI () {
        //x position, y position, width, length
        if (necropolisClicked)
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
            if(GUI.Button(new Rect(700,260,120,20), "Recruitment")) {
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
                    currBuilding.GetComponent<UndeadBuilding>().CompleteRecruitment();
                    if (currBuilding.GetComponent<UndeadBuilding>().isRecruitmentQueued == false)
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
