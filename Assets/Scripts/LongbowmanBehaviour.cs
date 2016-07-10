using UnityEngine;
using System.Collections;

public class LongbowmanBehaviour : MonoBehaviour {

	public int health = 175;
	public int mana = 0;
	public int attack = 10;
	public int rangeattack = 20;
	public int attackpoint = 1;
	public int movementpoint = 2;
	public int range = 3;
	public int armor = 20;
	public int armorpiercing = 20;
    public int vision = 2;

	public int lasthealth = 175;
	public int currattackpoint;
	public int currmovementpoint;

	public bool idle;
}
