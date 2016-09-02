using UnityEngine;
using System.Collections;

public class ArmoredSkeletonBehaviour : MonoBehaviour {

	public int health = 300;
	public int mana = 0;
	public int attack = 45;
	public int attackpoint = 1;
	public int movementpoint = 1;
	public int range = 1;
	public int armor = 30;
	public int armorpiercing = 20;
    public int vision = 2;

    public int lasthealth = 300;
	public int currattackpoint;
	public int currmovementpoint;

	public bool idle;
}
