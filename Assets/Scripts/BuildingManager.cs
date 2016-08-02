using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;
using System.Linq;

public class BuildingManager : MonoBehaviour {

    public BuildingStorage buildingStorage;
    GameObject currBuilding;

    private string cleanBuildingName;
    private string selBuilding;

    private bool necropolisClicked;
    private bool necropolisBuild;
    private bool necropolisRecruitment;

    private bool RecruitmentQueued;

    public void DisplayBuilding (string building) {
        selBuilding = building;
        cleanBuildingName = Regex.Replace(building, @"[\d-]", string.Empty);
        currBuilding = GameObject.Find(building);

        string faction = buildingStorage.whichFactionBuilding(cleanBuildingName);
        if (faction == "undead") {
            if (cleanBuildingName == "Necropolis") {
                necropolisClicked = true;
                if (currBuilding.GetComponent<NecropolisMechanics>().IsRecruitmentQueued == true)
                {
                    RecruitmentQueued = true;
                }
            }
        } else if (faction == "human") {

        }
    }

    public void ProductionQueue (string building, string action, string production) {
        if (action == "Build") {
            if (cleanBuildingName == "Necropolis")
            {
                currBuilding.GetComponent<NecropolisMechanics>().UpdateProduction(production);
            }
        }
        else if (action == "Recruit")
        {
            if (cleanBuildingName == "Necropolis")
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
        foreach (string building in buildingStorage.activePlayerBuildings)
        {
            GameObject currBuildings = GameObject.Find(building);
            string cleanStorageBuildingName = Regex.Replace(building, @"[\d-]", string.Empty);
            if (cleanStorageBuildingName == "Necropolis")
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
            if (GUI.Button(new Rect(800, 240, 120, 20), "Graveyard")) //Allows Recruiting Zombies
            {
                ProductionQueue(selBuilding, "Build", "Graveyard"); 
            }
            if (GUI.Button(new Rect(800,260,120,20), "Excavation Site")) //Allows Recruiting Skeletons
            {
                ProductionQueue(selBuilding, "Build", "Excavation Site"); 
            }
            if (GUI.Button(new Rect(800, 280, 120, 20), "Dark Fletchery")) //Allows Recruiting Skeleton Archers. Requires Excavation Site.
            {
                ProductionQueue(selBuilding, "Build", "Dark Fletchery");
            }
            //if (GUI.Button(new Rect(800, 260, 120, 20), "Dark Magic Forge")) //Allows Recruiting Skeletons Mages. Requires Excavation Site.
            //{
            //    ProductionQueue(selBuilding, "Build", "Skeleton Mage");
            //}
        }
        if (necropolisRecruitment)
        {
            if(GUI.Button(new Rect(800,240,120,20), "Zombie")) //Weak Early Tier Melee Unit. Resistent to other weak unit's attacks.
            {
                ProductionQueue(selBuilding, "Recruit", "Zombie");
		    }
            if(GUI.Button(new Rect(800,260,120,20), "Skeleton")) //Early Tier Melee Unit.
            {
                ProductionQueue(selBuilding, "Recruit", "Skeleton");
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
