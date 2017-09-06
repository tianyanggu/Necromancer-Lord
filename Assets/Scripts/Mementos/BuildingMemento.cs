using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[System.Serializable]
public class BuildingMemento {
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
    public List<string> permaEffects;
    public List<KeyValuePair<string, int>> tempEffects;

    public string currConstruction;
    public int currConstructionTimer;

    public string currRecruitment;
    public int currRecruitmentTimer;
    public bool isRecruitmentQueued;
}
