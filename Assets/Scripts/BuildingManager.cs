using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

public class BuildingManager : MonoBehaviour {

    public BuildingStorage buildingStorage;

    private string cleanBuildingName;

    private bool necropolisClicked;
    private bool necropolisBuild;
    private bool necropolisRecruitment;

    public void DisplayBuilding (string building) {
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
        GameObject currbuilding = GameObject.Find(building);
        if (action == "Build") {
            currbuilding.GetComponent<NecropolisMechanics> ().currConstruction = production;
        }
        else if (action == "Recruit")
        {
            currbuilding.GetComponent<NecropolisMechanics> ().currRecruitment = production;
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
            if(GUI.Button(new Rect(800,240,120,20), "Soul Harvester")) {
                ProductionQueue(cleanBuildingName, "Build", "Soul Harvester");
		    }
            if(GUI.Button(new Rect(800,260,120,20), "Graveyard")) {
                ProductionQueue(cleanBuildingName, "Build", "Graveyard");
		    }
        }
        if (necropolisRecruitment)
        {
            if(GUI.Button(new Rect(800,240,120,20), "Zombie")) {
                ProductionQueue(cleanBuildingName, "Recruit", "Zombie");
		    }
            if(GUI.Button(new Rect(800,260,120,20), "Skeleton")) {
                ProductionQueue(cleanBuildingName, "Recruit", "Skeleton");
		    }
        }
    }
}
