using UnityEngine;
using System.Collections;

public class CrossbowmanBehaviour : MonoBehaviour {

	public int health = 150;
	public int mana = 0;
	public int attack = 15;
	public int rangeattack = 30;
	public int attackpoint = 1;
	public int movementpoint = 2;
	public int range = 2;
	public int armor = 25;
	public int armorpiercing = 20;
    public int vision = 2;

	public int lasthealth = 150;
	public int currattackpoint;
	public int currmovementpoint;

	public bool idle;
}
