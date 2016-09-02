using UnityEngine;
using System.Collections;

public class HeroKingBehaviour : MonoBehaviour {

	public int health = 800;
	public int mana = 0;
	public int attack = 50;
	public int attackpoint = 1;
	public int movementpoint = 2;
	public int range = 1;
	public int armor = 50;
	public int armorpiercing = 35;
    public int vision = 3;

	public int lasthealth = 800;
	public int currattackpoint;
	public int currmovementpoint;

	public bool idle;
}
