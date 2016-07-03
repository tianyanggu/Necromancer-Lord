using UnityEngine;
using System.Collections;

public class NecromancerBehaviour : MonoBehaviour {

	public int health = 300;
	public int mana = 10;
	public int attack = 50;
	public int rangeattack = 100;
	public int attackpoint = 2;
	public int movementpoint = 2;
	public int range = 2;
	public int armor = 30;
	public int armorpiercing = 30;
    public int vision = 4;

	public int lasthealth = 300;
	public int currattackpoint;
	public int currmovementpoint;

	public bool idle;
}
