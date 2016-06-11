using UnityEngine;
using System.Collections;

public class MilitiaBehaviour : MonoBehaviour {

	public int health = 200;
	public int mana = 0;
	public int attack = 30;
	public int attackpoint = 1;
	public int movementpoint = 1;
	public int range = 1;
	public int armor = 10;
	public int armorpiercing = 5;

	public int lasthealth = 200;
	public int currattackpoint;
	public int currmovementpoint;

	public bool idle;
}
