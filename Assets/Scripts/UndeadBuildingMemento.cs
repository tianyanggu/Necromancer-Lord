using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class UndeadBuildingMemento {
    public string name; // TODO entityType and playerId stored in name of undeadentities, need to be changed in next update so it is field in undead entities
    public string buildingType;
    public string playerId;
    public int cellIndex;

    public int health;
    public int lasthealth;
    public List<string> upgrades;

    public string currConstruction;
    public int currConstructionTimer;

    public string currRecruitment;
    public int currRecruitmentTimer;
    public bool IsRecruitmentQueued;
}
