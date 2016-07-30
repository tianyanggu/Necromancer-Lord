using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

public class BuildingManager : MonoBehaviour {

    public BuildingStorage buildingStorage;

    private string cleanBuildingName;
    private string selBuilding;

    private bool necropolisClicked;
    private bool necropolisBuild;
    private bool necropolisRecruitment;

    public void DisplayBuilding (string building) {
        selBuilding = building;
        cleanBuildingName = Regex.Replace(building, @"[\d-]", string.Empty);
        
        string faction = buildingStorage.whichFactionBuilding(cleanBuildingName);
        if (faction == "undead") {
            if (cleanBuildingName == "Necropolis") {
                necropolisClicked = true;
                //determine for production what to queue for a building
            }
        } else if (faction == "human") {

        }
    }

    public void ProductionQueue (string building, string action, string production) {
        GameObject currBuilding = GameObject.Find(building);
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
        foreach (string building in buildingStorage.activePlayerBuildings)
        {
            GameObject currBuilding = GameObject.Find(building);
            string cleanStorageBuildingName = Regex.Replace(building, @"[\d-]", string.Empty);
            if (cleanStorageBuildingName == "Necropolis")
            {
                currBuilding.GetComponent<NecropolisMechanics>().TickProductionTimer();
                currBuilding.GetComponent<NecropolisMechanics>().TickRecruitmentTimer();
                if (currBuilding.GetComponent<NecropolisMechanics>().currConstructionTimer <= 0)
                {
                    currBuilding.GetComponent<NecropolisMechanics>().CompleteConstruction();
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
