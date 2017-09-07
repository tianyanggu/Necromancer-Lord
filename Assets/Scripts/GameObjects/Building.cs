using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Building : MonoBehaviour {
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
    public List<string> upgrades = new List<string>();
    public List<string> permaEffects = new List<string>();
    public List<KeyValuePair<string, int>> tempEffects = new List<KeyValuePair<string, int>>();

    public string currConstruction = "Empty";
    public int currConstructionTimer;

    public string currRecruitment = "Empty";
    public int currRecruitmentTimer;
    public bool isRecruitmentQueued;

    public void TickProductionTimer()
    {
        if (currConstruction != "Empty")
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

    public void CompleteConstruction()
    {
        if (currConstruction != "Empty")
        {
            upgrades.Add(currConstruction);
            currConstruction = "Empty";
        }
    }

    public void CompleteRecruitment()
    {
        if (currRecruitment != "Empty")
        {
            Vector3 currPos = gameObject.transform.position;
            GameObject hexGrid = GameObject.Find("Hex Grid");
            int currIndex = hexGrid.GetComponent<HexGrid>().GetCellIndex(currPos);
            if (hexGrid.GetComponent<HexGrid>().GetEntityObject(currIndex) == null)
            {
                GameObject summon = GameObject.Find("Summon");
                string buildingOwner = name.Substring(0, 1);
                summon.GetComponent<Summon>().SummonEntity(currIndex, currRecruitment, buildingOwner);
                currRecruitment = "Empty";
            }
            else
            {
                isRecruitmentQueued = true;
            }
        }
    }
}
