using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EntityStorage : MonoBehaviour {

	public List<string> playerEntities = new List<string> ();
	public List<string> enemyEntities = new List<string> ();

	public List<string> activeEntities = new List<string> ();

	void Start () {
		//player controlled entities
		playerEntities.Add ("Necromancer");
		playerEntities.Add ("Skeleton");
		playerEntities.Add ("Zombie");
		//enemy entities
		enemyEntities.Add ("Militia");

		ListActiveEntities ();
	}

	public void ListActiveEntities () {
		foreach (string entity in playerEntities) {
			for (int i = 1; i <= 99; i++) {
				string num = i.ToString ();
				string entityName = entity + num;
				GameObject gameEntity = GameObject.Find (entity + num);
				if (gameEntity != null) {
					activeEntities.Add (entityName);
				}
			}
		}
	}

	public void AddActiveEntity (string entityName) {
		activeEntities.Add (entityName);
	} 

	public void RemoveActiveEntity (string entityName) {
		activeEntities.Remove (entityName);
	} 
}
