using UnityEngine;
using System.Collections;

public class DeathKnightBehaviour : MonoBehaviour {

	public int health = 500;
	public int mana = 0;
	public int attack = 50;
	public int attackpoint = 2;
	public int movementpoint = 1;
	public int range = 1;
	public int armor = 40;
	public int armorpiercing = 20;
    public int vision = 3;

    public int lasthealth = 500;
	public int currattackpoint;
	public int currmovementpoint;

	public bool idle;
}
