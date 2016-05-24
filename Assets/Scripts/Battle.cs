using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class Battle : MonoBehaviour {

	public HexGrid hexGrid;
	public LoadMap loadMap;
	public Movement movement;

	private int attackerdmg = 0;
	private int attackerdmgtotal = 0;
	private int attackerhealth = 0;
	private int attackerhealthtotal = 0;
	private int attackerlasthealth = 0;
	private int attackersize = 0;
	private int attackerrange = 0;
	private int attackerrangedmg = 0;
	private int defenderdmg = 0;
	private int defenderdmgtotal = 0;
	private int defenderhealth = 0;
	private int defenderhealthtotal = 0;
	private int defenderlasthealth = 0;
	private int defendersize = 0;
	//private int defenderrange = 0;

	private string cleanSelectedEntity;
	private string cleanCurrEntity;

	private List<string> playerEntities = new List<string> ();
	private List<string> enemyEntities = new List<string> ();

	public bool Attack (int selectedindex, int currindex, string selectedentity) {
		//player controlled entities
		playerEntities.Add ("Necromancer");
		playerEntities.Add ("Skeleton");
		playerEntities.Add ("Zombie");
		//enemy entities
		enemyEntities.Add ("Militia");

		//------Parses Entities------
		string currEntity = hexGrid.GetEntity(currindex);
		//get first part of currEntity as clean and number as num
		cleanCurrEntity = Regex.Replace(currEntity, @"[\d-]", string.Empty);
		string numCurrEntity = Regex.Replace(currEntity, "[^0-9 -]", string.Empty);
		cleanSelectedEntity = Regex.Replace(selectedentity, @"[\d-]", string.Empty);
		//Debug.Log ("Size " + cleanCurrEntity + numCurrEntity);
		Vector3 cellcoord = hexGrid.GetCellPos(currindex);

		//------Movement Empty Cell------
		if (playerEntities.Contains (cleanSelectedEntity) && currEntity == "Empty") {
			GameObject playerEntity = GameObject.Find (selectedentity);
			//find movement
			int playermovement = 0;
			int playercurrmovepoint = 0;
			if (cleanSelectedEntity == "Zombie") {
				playermovement = playerEntity.GetComponent<ZombieBehaviour> ().movementpoint;
				playercurrmovepoint = playerEntity.GetComponent<ZombieBehaviour> ().currmovementpoint;
			} else if (cleanSelectedEntity == "Skeleton") {
				playermovement = playerEntity.GetComponent<SkeletonBehaviour> ().movementpoint;
				playercurrmovepoint = playerEntity.GetComponent<SkeletonBehaviour> ().currmovementpoint;
			} else if (cleanSelectedEntity == "Necromancer") {
				playermovement = playerEntity.GetComponent<NecromancerBehaviour> ().movementpoint;
				playercurrmovepoint = playerEntity.GetComponent<NecromancerBehaviour> ().currmovementpoint;
			}
			//determine movement tiles
			List<int> possmovement = null;
			if (playercurrmovepoint == 0) {
				return false;
			}
			else if (playermovement == 1 && playercurrmovepoint == 1) {
				possmovement = movement.GetCellIndexesOneHexAway (selectedindex);
			} else if (playermovement != 1 && playercurrmovepoint == 1) {
				//TODO create one hex away blockers
				possmovement = movement.GetCellIndexesOneHexAway (selectedindex);
			} else if (playermovement == 2) {
//				possmovement = movement.GetCellIndexesTwoHexAway (selectedindex);
				possmovement = movement.GetCellIndexesTwoHexAwayBlockers (selectedindex);
			}

			if (possmovement.Contains (currindex)) {
				GameObject playerSize = GameObject.Find ("Size " + selectedentity);
				playerEntity.transform.position = cellcoord;
				playerSize.transform.position = new Vector3 (cellcoord.x, cellcoord.y + 0.1f, cellcoord.z);
				hexGrid.EntityCellIndex (selectedindex, "Empty");
				hexGrid.EntityCellIndex (currindex, selectedentity);
				return true;



				//if used one movementpoint
//				if (cleanSelectedEntity == "Zombie") {
//					playerEntity.GetComponent<ZombieBehaviour> ().currmovementpoint = playerEntity.GetComponent<ZombieBehaviour> ().currmovementpoint - 1;
//				} else if (cleanSelectedEntity == "Skeleton") {
//					playerEntity.GetComponent<SkeletonBehaviour> ().currmovementpoint = playerEntity.GetComponent<SkeletonBehaviour> ().currmovementpoint - 1;
//				} else if (cleanSelectedEntity == "Necromancer") {
//					playerEntity.GetComponent<NecromancerBehaviour> ().currmovementpoint = playerEntity.GetComponent<NecromancerBehaviour> ().currmovementpoint - 1;
//				}
//				// if used two movementpoints
//				if (cleanSelectedEntity == "Zombie") {
//					playerEntity.GetComponent<ZombieBehaviour> ().currmovementpoint = playerEntity.GetComponent<ZombieBehaviour> ().currmovementpoint - 2;
//				} else if (cleanSelectedEntity == "Skeleton") {
//					playerEntity.GetComponent<SkeletonBehaviour> ().currmovementpoint = playerEntity.GetComponent<SkeletonBehaviour> ().currmovementpoint - 2;
//				} else if (cleanSelectedEntity == "Necromancer") {
//					playerEntity.GetComponent<NecromancerBehaviour> ().currmovementpoint = playerEntity.GetComponent<NecromancerBehaviour> ().currmovementpoint - 2;
//				}
			}
			//------Encounter Enemy------
		} else if (playerEntities.Contains (cleanSelectedEntity) && enemyEntities.Contains (cleanCurrEntity)) {
			GameObject attacker = GameObject.Find (selectedentity);
			GameObject defender = GameObject.Find (currEntity);

			//check if you can attack
			int playercurrattpoint = 0;
			if (cleanSelectedEntity == "Zombie") {
				playercurrattpoint = attacker.GetComponent<ZombieBehaviour> ().currattackpoint;
			} else if (cleanSelectedEntity == "Skeleton") {
				playercurrattpoint = attacker.GetComponent<SkeletonBehaviour> ().currattackpoint;;
			} else if (cleanSelectedEntity == "Necromancer") {
				playercurrattpoint = attacker.GetComponent<NecromancerBehaviour> ().currattackpoint;
			}
			if (playercurrattpoint == 0) {
				return false;
			}
				
			List<int> possattacktiles = null;

			GetAttackerInfo (selectedentity);
			GetDefenderInfo (currEntity);

			int unitsdied = hexGrid.GetCorpses(currindex);
			int olddefendersize = defendersize;

			//------Determine Attack Range------
			if (attackerrange == 1) {
				possattacktiles = movement.GetCellIndexesOneHexAway (selectedindex);
			} else if (attackerrange == 2) {
				possattacktiles = movement.GetCellIndexesTwoHexAway (selectedindex);
			}

			//------Calc Defender New Health-------
			if (possattacktiles.Contains (currindex)) {
				if (defendersize > 0) {
					//if melee attack or range attack
					if (attackerrange == 1) {
						attackerdmgtotal = attackerdmg * attackersize;
						attackerhealthtotal = attackerhealth * (attackersize - 1) + attackerlasthealth;
						defenderdmgtotal = defenderdmg * defendersize;
						defenderhealthtotal = defenderhealth * (defendersize - 1) + defenderlasthealth;

						//calc dmg to defender health and size
						defenderhealthtotal = defenderhealthtotal - attackerdmgtotal;
						defendersize = (defenderhealthtotal + defenderhealth - 1) / defenderhealth;
						if (defendersize < 0) {
							defendersize = 0;
						}
						//set new corpses created on tile
						int addcorpses = olddefendersize - defendersize;
						unitsdied = unitsdied + addcorpses;
						hexGrid.CorpsesCellIndex(currindex, unitsdied);

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
					} else if (attackerrange >= 2) {
						attackerdmgtotal = attackerrangedmg * attackersize;
						defenderhealthtotal = defenderhealth * (defendersize - 1) + defenderlasthealth;

						//calc dmg to defender health and size
						defenderhealthtotal = defenderhealthtotal - attackerdmgtotal;
						defendersize = (defenderhealthtotal + defenderhealth - 1) / defenderhealth;
						if (defendersize < 0) {
							defendersize = 0;
						}
						//set new corpses created on tile
						int addcorpses = olddefendersize - defendersize;
						unitsdied = unitsdied + addcorpses;
						hexGrid.CorpsesCellIndex(currindex, unitsdied);
						int defenderhealthmod = defenderhealthtotal % defenderhealth;
						if (defenderhealthmod == 0) {
							defenderlasthealth = defenderhealth;
						} else {
							defenderlasthealth = defenderhealthmod;
						}
					}

					if (defendersize <= 0) {
						Destroy (defender);
						hexGrid.EntityCellIndex (currindex, "Empty");
						GameObject defenderSizeText = GameObject.Find ("Size " + currEntity);
						Destroy (defenderSizeText);
					}
					if (attackersize <= 0) {
						Destroy (attacker);
						hexGrid.EntityCellIndex (selectedindex, "Empty");
						GameObject attackerSizeText = GameObject.Find ("Size " + selectedentity);
						Destroy (attackerSizeText);
					} else if (attackersize > 0 && defendersize <= 0) {
						attacker.transform.position = cellcoord;
						hexGrid.EntityCellIndex (currindex, selectedentity);
						hexGrid.EntityCellIndex (selectedindex, "Empty");
						GameObject defenderSizeText = GameObject.Find ("Size " + currEntity);
						Destroy (defenderSizeText);
						GameObject attackerSizeText = GameObject.Find ("Size " + selectedentity);
						attackerSizeText.transform.position = new Vector3 (cellcoord.x, cellcoord.y + 0.1f, cellcoord.z);
					}

					//------Set New Info Attacker------
					if (cleanSelectedEntity == "Necromancer") {
						attacker.GetComponent<NecromancerBehaviour> ().size = attackersize;
						attacker.GetComponent<NecromancerBehaviour> ().lasthealth = attackerlasthealth;
						Text attsizetext = GameObject.Find ("Size " + selectedentity).GetComponent<Text> ();
						attsizetext.text = attackersize.ToString ();
						attacker.GetComponent<NecromancerBehaviour> ().currattackpoint = attacker.GetComponent<NecromancerBehaviour> ().currattackpoint - 1;
					} else if (cleanSelectedEntity == "Skeleton") {
						attacker.GetComponent<SkeletonBehaviour> ().size = attackersize;
						attacker.GetComponent<SkeletonBehaviour> ().lasthealth = attackerlasthealth;
						Text attsizetext = GameObject.Find ("Size " + selectedentity).GetComponent<Text> ();
						attsizetext.text = attackersize.ToString ();
						attacker.GetComponent<SkeletonBehaviour> ().currattackpoint = attacker.GetComponent<SkeletonBehaviour> ().currattackpoint - 1;
					} else if (cleanSelectedEntity == "Zombie") {
						attacker.GetComponent<ZombieBehaviour> ().size = attackersize;
						attacker.GetComponent<ZombieBehaviour> ().lasthealth = attackerlasthealth;
						Text attsizetext = GameObject.Find ("Size " + selectedentity).GetComponent<Text> ();
						attsizetext.text = attackersize.ToString ();
						attacker.GetComponent<ZombieBehaviour> ().currattackpoint = attacker.GetComponent<ZombieBehaviour> ().currattackpoint - 1;
					}

					//------Set New Info Defender------
					if (cleanCurrEntity == "Militia") {
						defender.GetComponent<MilitiaBehaviour> ().size = defendersize;
						defender.GetComponent<MilitiaBehaviour> ().lasthealth = defenderlasthealth;
						Text defsizetext = GameObject.Find ("Size " + cleanCurrEntity + numCurrEntity).GetComponent<Text> ();
						defsizetext.text = defendersize.ToString ();
					}

					return true;
				}


			} else {
				//Debug.Log ("problem");
			}
		}

		//playerNecromancer.GetComponent<NecromancerBehaviour> ().health = playerNecromancer.GetComponent<NecromancerBehaviour> ().health - 2;
		return false;
	}

	void GetAttackerInfo(string selectedentity) {
		GameObject attacker = GameObject.Find (selectedentity);

		//------Grab Info Attacker------
		if (cleanSelectedEntity == "Necromancer") {
			attackersize = attacker.GetComponent<NecromancerBehaviour> ().size;
			attackerdmg = attacker.GetComponent<NecromancerBehaviour> ().attack;
			attackerhealth = attacker.GetComponent<NecromancerBehaviour> ().health;
			attackerlasthealth = attacker.GetComponent<NecromancerBehaviour> ().lasthealth;
			attackerrange = attacker.GetComponent<NecromancerBehaviour> ().range;
			attackerrangedmg = attacker.GetComponent<NecromancerBehaviour> ().rangeattack;

		} else if (cleanSelectedEntity == "Skeleton") {
			attackersize = attacker.GetComponent<SkeletonBehaviour> ().size;
			attackerdmg = attacker.GetComponent<SkeletonBehaviour> ().attack;
			attackerhealth = attacker.GetComponent<SkeletonBehaviour> ().health;
			attackerlasthealth = attacker.GetComponent<SkeletonBehaviour> ().lasthealth;
			attackerrange = attacker.GetComponent<SkeletonBehaviour> ().range;
		} else if (cleanSelectedEntity == "Zombie") {
			attackersize = attacker.GetComponent<ZombieBehaviour> ().size;
			attackerdmg = attacker.GetComponent<ZombieBehaviour> ().attack;
			attackerhealth = attacker.GetComponent<ZombieBehaviour> ().health;
			attackerlasthealth = attacker.GetComponent<ZombieBehaviour> ().lasthealth;
			attackerrange = attacker.GetComponent<ZombieBehaviour> ().range;
		}
	}

	void GetDefenderInfo(string currEntity) {
		GameObject defender = GameObject.Find (currEntity);

		//------Grab Info Defender------
		if (cleanCurrEntity == "Militia") {
			defendersize = defender.GetComponent<MilitiaBehaviour> ().size;
			defenderdmg = defender.GetComponent<MilitiaBehaviour> ().attack;
			defenderhealth = defender.GetComponent<MilitiaBehaviour> ().health;
			defenderlasthealth = defender.GetComponent<MilitiaBehaviour> ().lasthealth;
			//defenderrange = defender.GetComponent<MilitiaBehaviour> ().range;
		}
	}
}
