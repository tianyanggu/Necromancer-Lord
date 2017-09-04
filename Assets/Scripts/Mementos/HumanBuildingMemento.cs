using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HumanBuildingMemento {
    public string playerID;
    public string type;
    public Guid uniqueID;
    public int cellIndex;

    public int currhealth;
    public int maxhealth;
    public int range;
    public int rangedattackdmg;
    public int defense;
    public int vision;
    public List<string> upgrades;

    public string currConstruction;
    public int currConstructionTimer;

    public string currRecruitment;
    public int currRecruitmentTimer;
    public bool isRecruitmentQueued;
}
