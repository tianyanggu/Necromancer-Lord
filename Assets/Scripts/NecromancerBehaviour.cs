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

	public int lasthealth;
	public int currattackpoint;
	public int currmovementpoint;

	public bool idle;
}
