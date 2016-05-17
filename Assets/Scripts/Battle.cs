using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class Battle : MonoBehaviour {

	public HexGrid hexGrid;
	public LoadMap loadMap;

	public bool Attack (int selectedindex, int currindex, string selectedentity) {
		//player controlled entities
		List<string> playerEntities = new List<string> ();
		playerEntities.Add ("Necromancer");
		playerEntities.Add ("Skeleton");

		List<string> enemyEntities = new List<string> ();
		enemyEntities.Add ("Militia");

		string currEntity = hexGrid.GetEntity(currindex);
		//get first part of currEntity as clean and number as num
		string cleanCurrEntity = Regex.Replace(currEntity, @"[\d-]", string.Empty);
		string numCurrEntity = Regex.Replace(currEntity, "[^0-9 -]", string.Empty);
		string cleanSelectedEntity = Regex.Replace(selectedentity, @"[\d-]", string.Empty);
		//Debug.Log ("Size " + cleanCurrEntity + numCurrEntity);
		Vector3 cellcoord = hexGrid.GetCellPos(currindex);


		//------Movement Empty Cell------
		if (playerEntities.Contains (cleanSelectedEntity) && currEntity == "Empty") {
			GameObject playerEntity = GameObject.Find (selectedentity);
			GameObject playerSize = GameObject.Find ("Size " + selectedentity);
			playerEntity.transform.position = cellcoord;
			playerSize.transform.position = new Vector3 (cellcoord.x, cellcoord.y + 0.1f, cellcoord.z);
			hexGrid.EntityCellIndex (selectedindex, "Empty");
			hexGrid.EntityCellIndex (currindex, selectedentity);
			return true;

		//------Encounter Enemy------
		} else if (playerEntities.Contains (cleanSelectedEntity) && enemyEntities.Contains (cleanCurrEntity)) {
			GameObject attacker = GameObject.Find (selectedentity);
			GameObject defender = GameObject.Find (currEntity);
			int attackerdmg = 0;
			int attackerdmgtotal = 0;
			int attackerhealth = 0;
			int attackerhealthtotal = 0;
			int attackerlasthealth = 0;
			int attackersize = 0;
			int defenderdmg = 0;
			int defenderdmgtotal = 0;
			int defenderhealth = 0;
			int defenderhealthtotal = 0;
			int defenderlasthealth = 0;
			int defendersize = 0;

			//------Grab Info Attacker------
			if (cleanSelectedEntity == "Necromancer") {
				attackersize = attacker.GetComponent<NecromancerBehaviour> ().size;
				attackerdmg = attacker.GetComponent<NecromancerBehaviour> ().attack;
				attackerhealth = attacker.GetComponent<NecromancerBehaviour> ().health;
				attackerlasthealth = attacker.GetComponent<NecromancerBehaviour> ().lasthealth;
			} else if (cleanSelectedEntity == "Skeleton") {
				attackersize = attacker.GetComponent<SkeletonBehaviour> ().size;
				attackerdmg = attacker.GetComponent<SkeletonBehaviour> ().attack;
				attackerhealth = attacker.GetComponent<SkeletonBehaviour> ().health;
				attackerlasthealth = attacker.GetComponent<SkeletonBehaviour> ().lasthealth;
			}

			//------Grab Info Defender------
			if (cleanCurrEntity == "Militia") {
				defendersize = defender.GetComponent<MilitiaBehaviour> ().size;
				defenderdmg = defender.GetComponent<MilitiaBehaviour> ().attack;
				defenderhealth = defender.GetComponent<MilitiaBehaviour> ().health;
				defenderlasthealth = defender.GetComponent<MilitiaBehaviour> ().lasthealth;
			}

			//------Calc Defender New Health-------
			if (defendersize > 0) {
				attackerdmgtotal = attackerdmg * attackersize;
				attackerhealthtotal = attackerhealth * (attackersize - 1) + attackerlasthealth;
				defenderdmgtotal = defenderdmg * defendersize;
				defenderhealthtotal = defenderhealth * (defendersize - 1) + defenderlasthealth;

				defenderhealthtotal = defenderhealthtotal - attackerdmgtotal;
				defendersize = (defenderhealthtotal + defenderhealth - 1) / defenderhealth;
				int defenderhealthmod = defenderhealthtotal % defenderhealth;
				if (defenderhealthmod == 0) {
					defenderlasthealth = defenderhealth;
				} else {
					defenderlasthealth = defenderhealthmod;
				}

				attackerhealthtotal = attackerhealthtotal - defenderdmgtotal;
				attackersize = (attackerhealthtotal + attackerhealth - 1) / attackerhealth;

				int attackerhealthmod = attackerhealthtotal % attackerhealth;
				if (attackerhealthmod == 0) {
					attackerlasthealth = attackerhealth;
				} else {
					attackerlasthealth = attackerhealthmod;
				}

				if (defendersize <= 0) {
					Destroy (defender);
					hexGrid.EntityCellIndex (currindex, "Empty");
				}
				if (attackersize <= 0) {
					Destroy (attacker);
					hexGrid.EntityCellIndex (selectedindex, "Empty");
				} else if (attackersize > 0 && defendersize <= 0){
					attacker.transform.position = cellcoord;
					hexGrid.EntityCellIndex (currindex, selectedentity);
				}

				//------Set New Info Attacker------
				if (cleanSelectedEntity == "Necromancer") {
					attacker.GetComponent<NecromancerBehaviour> ().size = attackersize;
					attacker.GetComponent<NecromancerBehaviour> ().lasthealth = attackerlasthealth;
					Text attsizetext = GameObject.Find ("Size " + selectedentity).GetComponent<Text>();
					attsizetext.text = attackersize.ToString();
				} else if (cleanSelectedEntity == "Skeleton") {
					attacker.GetComponent<SkeletonBehaviour> ().size = attackersize;
					attacker.GetComponent<SkeletonBehaviour> ().lasthealth = attackerlasthealth;
					Text attsizetext = GameObject.Find ("Size " + selectedentity).GetComponent<Text>();
					attsizetext.text = attackersize.ToString();
				}

				//------Set New Info Defender------
				if (cleanCurrEntity == "Militia") {
					defender.GetComponent<MilitiaBehaviour> ().size = defendersize;
					defender.GetComponent<MilitiaBehaviour> ().lasthealth = defenderlasthealth;
					Text defsizetext = GameObject.Find ("Size " + cleanCurrEntity + numCurrEntity).GetComponent<Text>();
					defsizetext.text = defendersize.ToString();
				}

				return true;
			}


		} else {
			//Debug.Log ("problem");
		}

		//playerNecromancer.GetComponent<NecromancerBehaviour> ().health = playerNecromancer.GetComponent<NecromancerBehaviour> ().health - 2;
		return false;
	}
}
