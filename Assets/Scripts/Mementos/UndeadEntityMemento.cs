using UnityEngine;
using System.Collections;

[System.Serializable]
public class UndeadEntityMemento {
    public string name;
    public int cellIndex;

    public int maxhealth;
    public int maxmana;
    public int attackdmg;
    public int maxattackpoint;
    public int maxmovementpoint;
    public int range;
    public int rangedattackdmg;
    public int armor;
    public int armorpiercing;
    public int rangedarmorpiercing;
    public int vision;

    public int currhealth;
    public int currmana;
    public int currattackpoint;
    public int currmovementpoint;

    public bool idle;
}
