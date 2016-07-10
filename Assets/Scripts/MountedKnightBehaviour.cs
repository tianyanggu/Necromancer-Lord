using UnityEngine;
using System.Collections;

public class MountedKnightBehaviour : MonoBehaviour {

	public int health = 400;
	public int mana = 0;
	public int attack = 45;
	public int attackpoint = 1;
	public int movementpoint = 4;
	public int range = 1;
	public int armor = 35;
	public int armorpiercing = 25;
    public int vision = 3;

	public int lasthealth = 400;
	public int currattackpoint;
	public int currmovementpoint;

	public bool idle;
}
