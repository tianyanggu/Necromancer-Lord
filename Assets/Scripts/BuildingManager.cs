using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;
using System.Linq;

public class BuildingManager : MonoBehaviour {

    public BuildingStorage buildingStorage;
    public HexGrid hexGrid;
    public PlayerManager playerManager;

    GameObject currBuilding;

    private string cleanBuildingName;
    private string selBuilding;

    private bool necropolisClicked;
    private bool necropolisBuild;
    private bool necropolisRecruitment;

    private bool RecruitmentQueued;

    public void DisplayBuilding (string building, int index) {
        selBuilding = building;
        cleanBuildingName = Regex.Replace(building, @"[\d-]", string.Empty);
        currBuilding = hexGrid.GetBuildingObject(index);

        string faction = buildingStorage.WhichFactionBuilding(cleanBuildingName);
        if (faction == FactionNames.Undead) {
            if (cleanBuildingName == BuildingNames.Necropolis) {
                necropolisClicked = true;
                if (currBuilding.GetComponent<NecropolisMechanics>().IsRecruitmentQueued == true)
                {
                    RecruitmentQueued = true;
                }
            }
        } else if (faction == FactionNames.Human) {

        }
    }

    public void ProductionQueue (string building, string action, string production) {
        if (action == "Build") {
            if (cleanBuildingName == BuildingNames.Necropolis)
            {
                currBuilding.GetComponent<UndeadBuilding>().UpdateProduction(production);
            }
        }
        else if (action == "Recruit")
        {
            if (cleanBuildingName == BuildingNames.Necropolis)
            {
                currBuilding.GetComponent<NecropolisMechanics>().UpdateRecruitment(production);
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
                currBuildings.GetComponent<NecropolisMechanics>().TickProductionTimer();
                currBuildings.GetComponent<NecropolisMechanics>().TickRecruitmentTimer();
                if (currBuildings.GetComponent<NecropolisMechanics>().currConstructionTimer <= 0)
                {
                    currBuildings.GetComponent<NecropolisMechanics>().CompleteConstruction();
                }
                if (currBuildings.GetComponent<NecropolisMechanics>().currRecruitmentTimer <= 0)
                {
                    currBuildings.GetComponent<NecropolisMechanics>().CompleteRecruitment();
                }
            }
        }
    }

    void OnGUI () {
        //x position, y position, width, length
        if (necropolisClicked)
        {
            if(GUI.Button(new Rect(700,240,120,20), "Build")) {
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
                    currBuilding.GetComponent<NecropolisMechanics>().CompleteRecruitment();
                    if (currBuilding.GetComponent<NecropolisMechanics>().IsRecruitmentQueued == false)
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
                ProductionQueue(selBuilding, "Build", UpgradeNames.Graveyard); 
            }
            if (GUI.Button(new Rect(800,260,120,20), UpgradeNames.ExcavationSite)) //Allows Recruiting Skeletons
            {
                ProductionQueue(selBuilding, "Build", UpgradeNames.ExcavationSite); 
            }
            if (GUI.Button(new Rect(800, 280, 120, 20), UpgradeNames.SinewFletchery)) //Allows Recruiting Skeleton Archers. Requires Excavation Site.
            {
                ProductionQueue(selBuilding, "Build", UpgradeNames.SinewFletchery);
            }
            //if (GUI.Button(new Rect(800, 260, 120, 20), "Dark Magic Forge")) //Allows Recruiting Skeletons Mages. Requires Excavation Site.
            //{
            //    ProductionQueue(selBuilding, "Build", "Skeleton Mage");
            //}
        }
        if (necropolisRecruitment)
        {
            if(GUI.Button(new Rect(800,240,120,20), EntityNames.Zombie)) //Weak Early Tier Melee Unit. Resistent to other weak unit's attacks.
            {
                ProductionQueue(selBuilding, "Recruit", EntityNames.Zombie);
		    }
            if(GUI.Button(new Rect(800,260,120,20), EntityNames.Skeleton)) //Early Tier Melee Unit.
            {
                ProductionQueue(selBuilding, "Recruit", EntityNames.Skeleton);
		    }
            if (GUI.Button(new Rect(800, 280, 120, 20), "Skeleton Archer")) //Early Tier Physical Ranged Unit.
            {
                ProductionQueue(selBuilding, "Recruit", "Skeleton Archer");
            }
            //if (GUI.Button(new Rect(800, 260, 120, 20), "Skeleton Mage")) //Early Tier Magic Ranged Unit.
            //{
            //    ProductionQueue(selBuilding, "Recruit", "Skeleton Mage");
            //}
        }
    }
}
