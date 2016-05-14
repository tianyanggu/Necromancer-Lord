using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class Battle : MonoBehaviour {

	public HexGrid hexGrid;

	public void Attack (int selected, int currindex, string selectedentity) {
		string currEntity = hexGrid.GetEntity(currindex);
		Vector3 cellcoord = hexGrid.GetCellPos(currindex);

		List<string> playerEntities = new List<string> ();
		playerEntities.Add ("Necromancer");
		playerEntities.Add ("Skeleton");

		if (playerEntities.Contains (selectedentity) && currEntity == "Empty") {
			GameObject playerNecromancer = GameObject.Find (selectedentity);
			playerNecromancer.transform.position = cellcoord;
		} else if (playerEntities.Contains (selectedentity) && currEntity == "Militia1") {
			GameObject attacker = GameObject.Find (selectedentity);
			GameObject defender = GameObject.Find (currEntity);
			int attackerdmg = 0;
			int attackerhealth = 0;
			int defenderdmg = 0;
			int defenderhealth = 0;

			//------Grab Info Attacker------
			if (selectedentity == "Necromancer") {
				attackerdmg = attacker.GetComponent<NecromancerBehaviour> ().attack;
				attackerhealth = attacker.GetComponent<NecromancerBehaviour> ().health;
			} else if (selectedentity == "Skeleton") {
				attackerdmg = attacker.GetComponent<SkeletonBehaviour> ().attack;
				attackerhealth = attacker.GetComponent<SkeletonBehaviour> ().health;
			}

			//------Grab Info Defender------
			if (currEntity == "Militia1") {
				defenderdmg = defender.GetComponent<MilitiaBehaviour> ().attack;
				defenderhealth = defender.GetComponent<MilitiaBehaviour> ().health;
			}

			//------Calc Defender New Health-------
			if (defender.GetComponent<MilitiaBehaviour> ().health > 0) {

				defender.GetComponent<MilitiaBehaviour> ().health = defender.GetComponent<MilitiaBehaviour> ().health - attackerdmg;
				attacker.GetComponent<NecromancerBehaviour> ().health = attacker.GetComponent<NecromancerBehaviour> ().health - defenderdmg;
				int defhealth = defender.GetComponent<MilitiaBehaviour> ().health;
				int atthealth = attacker.GetComponent<NecromancerBehaviour> ().health;

				Debug.Log ("d " + defender.GetComponent<MilitiaBehaviour>().health);
				Debug.Log ("a " + attacker.GetComponent<NecromancerBehaviour> ().health);

				if (defhealth <= 0) {
					Destroy (defender);
					hexGrid.EntityCellIndex (currindex, "Empty");
				}
				if (atthealth <= 0) {
					Destroy (attacker);
					hexGrid.EntityCellIndex (selected, selectedentity);
				} else if (atthealth > 0 && defhealth <= 0){
					attacker.transform.position = cellcoord;
					hexGrid.EntityCellIndex (currindex, selectedentity);
				}

			}
		} else {
			Debug.Log ("problem");
		}

		//playerNecromancer.GetComponent<NecromancerBehaviour> ().health = playerNecromancer.GetComponent<NecromancerBehaviour> ().health - 2;
	}
}
