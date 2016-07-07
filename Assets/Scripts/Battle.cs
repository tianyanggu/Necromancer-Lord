﻿using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class Battle : MonoBehaviour {

	public HexGrid hexGrid;
	public LoadMap loadMap;
	public Movement movement;
	public EntityStorage entityStorage;
	public Resources resources;

	//entity stats
	private int attackerdmg = 0;
	//private int attackerhealth = 0;
	private int attackerlasthealth = 0;
	private int attackerrange = 0;
	private int attackerrangedmg = 0;
	private int attackerarmor = 0;
	private int attackerarmorpiercing = 0;
	private int defenderdmg = 0;
	//private int defenderhealth = 0;
	private int defenderlasthealth = 0;
	private int defenderarmor = 0;
	private int defenderarmorpiercing = 0;

	//entity action points
	private int playermovepoint = 0;
	private int playercurrmovepoint = 0;
	private int playercurrattpoint = 0;

	private string cleanSelEntity;
	private string cleanCurrEntity;

	public bool Attack (int selectedindex, int currindex) {
		//------Parses Entities------
		string selectedEntity = hexGrid.GetEntity(selectedindex);
		if (selectedEntity == "Empty") {
			return false;
		}
		string currEntity = hexGrid.GetEntity(currindex);
		//get first part of currEntity as clean and number as num
		cleanCurrEntity = Regex.Replace(currEntity, @"[\d-]", string.Empty);
		cleanSelEntity = Regex.Replace(selectedEntity, @"[\d-]", string.Empty);
		Vector3 cellcoord = hexGrid.GetCellPos(currindex);

		string storedNameSel = PlayerPrefs.GetString (selectedEntity);
		string numSelEntity = Regex.Replace(storedNameSel, "[^0-9 -]", string.Empty);
		string storedNameCurr = PlayerPrefs.GetString (currEntity);
		string numCurrEntity = Regex.Replace(storedNameCurr, "[^0-9 -]", string.Empty);

		string selFaction = entityStorage.whichFaction(cleanSelEntity);
		string currFaction = entityStorage.whichFaction(cleanCurrEntity);

		GetMovementInfo (selectedEntity);

		//------Movement Empty Cell------
		if (currEntity == "Empty") {
			//determine movement tiles
			List<int> possmovement = null;
			if (playercurrmovepoint == 0) {
				return false;
			}
			else if (playermovepoint == 1 && playercurrmovepoint == 1) {
				possmovement = movement.GetCellIndexesOneHexAway (selectedindex);
			} else if (playermovepoint != 1) {
				possmovement = movement.GetCellIndexesBlockers (selectedindex, playercurrmovepoint);
			}

			if (possmovement.Contains (currindex)) {
				//get min movement points used
				if (playermovepoint == 1 && playercurrmovepoint == 1) {
					NewMovementPoints (selectedEntity, 1);
				} else if (playermovepoint != 1) {
					int minmove = movement.GetMovementPointsUsed (selectedindex, currindex, playercurrmovepoint);
					//set new movement points remaining
					NewMovementPoints (selectedEntity, minmove);
				}

				GameObject selEntity = GameObject.Find (selectedEntity);
				GameObject playerHealth = GameObject.Find ("Health " + selectedEntity);
				selEntity.transform.position = cellcoord;
				playerHealth.transform.position = new Vector3 (cellcoord.x, cellcoord.y + 0.1f, cellcoord.z);
				hexGrid.SetEntity (selectedindex, "Empty");
				hexGrid.SetEntity (currindex, selectedEntity);

				PlayerPrefs.SetInt ("HexEntityIndex" + numSelEntity, currindex);

				return true;
			}
			//------Encounter Enemy------
		} else if (selFaction != currFaction) {
			GameObject attacker = GameObject.Find (selectedEntity);
			GameObject defender = GameObject.Find (currEntity);

			GetAttackerInfo (selectedEntity);
			GetDefenderInfo (currEntity);

			//check if you can attack
			if (playercurrattpoint == 0) {
				return false;
			}
			List<int> possattacktiles = null;

			//------Determine Attack Range------
			if (attackerrange == 1) {
				possattacktiles = movement.GetCellIndexesOneHexAway (selectedindex);
			} else if (attackerrange == 2) {
				possattacktiles = movement.GetCellIndexesTwoHexAway (selectedindex);
			}

			//------Calc Defender New Health-------
			if (possattacktiles.Contains (currindex)) {
				if (defenderlasthealth > 0) {
					//if melee attack 
					if (attackerrange == 1) {
						//armor piercing damage is minimum of armor or piercing damage
						int attackerpierceddmg = Mathf.Min(defenderarmor, attackerarmorpiercing);
						int defenderpierceddmg = Mathf.Min(attackerarmor, defenderarmorpiercing);
						//calc dmg to attacker and defender health, damage cannot be lower than 1
						int totalattackerdmg = attackerdmg - defenderarmor + attackerpierceddmg;
						if (totalattackerdmg < 1) {
							totalattackerdmg = 1;
						}
						int totaldefenderdmg = defenderdmg - attackerarmor + defenderpierceddmg;
						if (totaldefenderdmg < 1) {
							totaldefenderdmg = 1;
						}
						defenderlasthealth = defenderlasthealth - totalattackerdmg;
						attackerlasthealth = attackerlasthealth - totaldefenderdmg;

						PlayerPrefs.SetInt ("HexEntityHealth" + numCurrEntity, defenderlasthealth);
						PlayerPrefs.SetInt ("HexEntityHealth" + numSelEntity, attackerlasthealth);
					//range attack
					} else if (attackerrange >= 2) {
						//armor piercing damage is minimum of armor or piercing damage
						int attackerpierceddmg = Mathf.Min(defenderarmor, attackerarmorpiercing);
						//calc dmg to defender health
						int totalattackerrangedmg = attackerrangedmg - defenderarmor + attackerpierceddmg;
						if (totalattackerrangedmg < 1) {
							totalattackerrangedmg = 1;
						}
						defenderlasthealth = defenderlasthealth - totalattackerrangedmg;

						PlayerPrefs.SetInt ("HexEntityHealth" + numCurrEntity, defenderlasthealth);
					}

					//check new status
					if (defenderlasthealth <= 0) {
						Destroy (defender);
						hexGrid.SetEntity (currindex, "Empty");
						GameObject defenderHealthText = GameObject.Find ("Health " + currEntity);
						Destroy (defenderHealthText);
						entityStorage.RemoveActiveEnemyEntity (currEntity);
						//store new info
						PlayerPrefs.DeleteKey ("HexEntity" + numCurrEntity);
						PlayerPrefs.DeleteKey ("HexEntityHealth" + numCurrEntity);
						PlayerPrefs.DeleteKey ("HexEntityIndex" + numCurrEntity);
						PlayerPrefs.DeleteKey (currEntity);
						CalcSouls (selFaction, cleanCurrEntity);
						if (currFaction != "undead") {
							hexGrid.SetCorpses (currindex, currEntity);
						}
					}
					if (attackerlasthealth <= 0) {
						Destroy (attacker);
						hexGrid.SetEntity (selectedindex, "Empty");
						GameObject attackerHealthText = GameObject.Find ("Health " + selectedEntity);
						Destroy (attackerHealthText);
						entityStorage.RemoveActivePlayerEntity (selectedEntity);
						//store new info
						PlayerPrefs.DeleteKey ("HexEntity" + numSelEntity);
						PlayerPrefs.DeleteKey ("HexEntityHealth" + numSelEntity);
						PlayerPrefs.DeleteKey ("HexEntityIndex" + numSelEntity);
						PlayerPrefs.DeleteKey (selectedEntity);
						if (selFaction != "undead") {
							hexGrid.SetCorpses (selectedindex, selectedEntity);
						}
					} 
					if (attackerlasthealth > 0 && defenderlasthealth <= 0) {
						//move to defender's position if have enough movement points and is not ranged unit
						int minmove = movement.GetMovementPointsUsed (selectedindex, currindex, playercurrmovepoint);
						if (playercurrmovepoint >= minmove && attackerrange == 1) {
							attacker.transform.position = cellcoord;
							hexGrid.SetEntity (currindex, selectedEntity);
							GameObject attackerHealthText = GameObject.Find ("Health " + selectedEntity);
							attackerHealthText.transform.position = new Vector3 (cellcoord.x, cellcoord.y + 0.1f, cellcoord.z);
							NewMovementPoints (selectedEntity, minmove);
							PlayerPrefs.SetInt ("HexEntityIndex" + numSelEntity, currindex);
						} else if (attackerrange == 2) {
							//do nothing
						}
					}

					//Set New Info
					SetAttackerInfo (selectedEntity);
					SetDefenderInfo (currEntity);

					return true;
				}
			}
		}

		return false;
	}

	void GetAttackerInfo(string selectedentity) {
		GameObject attacker = GameObject.Find (selectedentity);

		//------Grab Info Attacker------
		if (cleanSelEntity == "Zombie") {
			playercurrattpoint = attacker.GetComponent<ZombieBehaviour> ().currattackpoint;
		} else if (cleanSelEntity == "Skeleton") {
			playercurrattpoint = attacker.GetComponent<SkeletonBehaviour> ().currattackpoint;;
		} else if (cleanSelEntity == "Necromancer") {
			playercurrattpoint = attacker.GetComponent<NecromancerBehaviour> ().currattackpoint;
		} else if (cleanSelEntity == "Militia") {
			playercurrattpoint = attacker.GetComponent<MilitiaBehaviour> ().currattackpoint;
		} else if (cleanSelEntity == "SkeletonArcher") {
            playercurrattpoint = attacker.GetComponent<SkeletonArcherBehaviour>().currattackpoint;
        }

        if (cleanSelEntity == "Necromancer") {
			attackerdmg = attacker.GetComponent<NecromancerBehaviour> ().attack;
			//attackerhealth = attacker.GetComponent<NecromancerBehaviour> ().health;
			attackerlasthealth = attacker.GetComponent<NecromancerBehaviour> ().lasthealth;
			attackerrange = attacker.GetComponent<NecromancerBehaviour> ().range;
			attackerrangedmg = attacker.GetComponent<NecromancerBehaviour> ().rangeattack;
			attackerarmor = attacker.GetComponent<NecromancerBehaviour> ().armor;
			attackerarmorpiercing = attacker.GetComponent<NecromancerBehaviour> ().armorpiercing;
		} else if (cleanSelEntity == "Skeleton") {
			attackerdmg = attacker.GetComponent<SkeletonBehaviour> ().attack;
			//attackerhealth = attacker.GetComponent<SkeletonBehaviour> ().health;
			attackerlasthealth = attacker.GetComponent<SkeletonBehaviour> ().lasthealth;
			attackerrange = attacker.GetComponent<SkeletonBehaviour> ().range;
			attackerarmor = attacker.GetComponent<SkeletonBehaviour> ().armor;
			attackerarmorpiercing = attacker.GetComponent<SkeletonBehaviour> ().armorpiercing;
		} else if (cleanSelEntity == "Zombie") {
			attackerdmg = attacker.GetComponent<ZombieBehaviour> ().attack;
			//attackerhealth = attacker.GetComponent<ZombieBehaviour> ().health;
			attackerlasthealth = attacker.GetComponent<ZombieBehaviour> ().lasthealth;
			attackerrange = attacker.GetComponent<ZombieBehaviour> ().range;
			attackerarmor = attacker.GetComponent<ZombieBehaviour> ().armor;
			attackerarmorpiercing = attacker.GetComponent<ZombieBehaviour> ().armorpiercing;
		} else if (cleanSelEntity == "Militia") {
			attackerdmg = attacker.GetComponent<MilitiaBehaviour> ().attack;
			//attackerhealth = attacker.GetComponent<MilitiaBehaviour> ().health;
			attackerlasthealth = attacker.GetComponent<MilitiaBehaviour> ().lasthealth;
			attackerrange = attacker.GetComponent<MilitiaBehaviour> ().range;
			attackerarmor = attacker.GetComponent<MilitiaBehaviour> ().armor;
			attackerarmorpiercing = attacker.GetComponent<MilitiaBehaviour> ().armorpiercing;
		} else if (cleanSelEntity == "SkeletonArcher") {
            attackerdmg = attacker.GetComponent<SkeletonArcherBehaviour>().attack;
            //attackerhealth = attacker.GetComponent<SkeletonArcherBehaviour> ().health;
            attackerlasthealth = attacker.GetComponent<SkeletonArcherBehaviour>().lasthealth;
            attackerrange = attacker.GetComponent<SkeletonArcherBehaviour>().range;
            attackerrangedmg = attacker.GetComponent<SkeletonArcherBehaviour> ().rangeattack;
            attackerarmor = attacker.GetComponent<SkeletonArcherBehaviour>().armor;
            attackerarmorpiercing = attacker.GetComponent<SkeletonArcherBehaviour>().armorpiercing;
        }
    }

	void GetDefenderInfo(string currEntity) {
		GameObject defender = GameObject.Find (currEntity);

		//------Grab Info Defender------
		if (cleanCurrEntity == "Militia") {
			defenderdmg = defender.GetComponent<MilitiaBehaviour> ().attack;
			//defenderhealth = defender.GetComponent<MilitiaBehaviour> ().health;
			defenderlasthealth = defender.GetComponent<MilitiaBehaviour> ().lasthealth;
			//defenderrange = defender.GetComponent<MilitiaBehaviour> ().range;
			defenderarmor = defender.GetComponent<MilitiaBehaviour> ().armor;
			defenderarmorpiercing = defender.GetComponent<MilitiaBehaviour> ().armorpiercing;
		} else if (cleanCurrEntity == "Zombie") {
			defenderdmg = defender.GetComponent<ZombieBehaviour> ().attack;
			//defenderhealth = defender.GetComponent<ZombieBehaviour> ().health;
			defenderlasthealth = defender.GetComponent<ZombieBehaviour> ().lasthealth;
			//defenderrange = defender.GetComponent<ZombieBehaviour> ().range;
			defenderarmor = defender.GetComponent<ZombieBehaviour> ().armor;
			defenderarmorpiercing = defender.GetComponent<ZombieBehaviour> ().armorpiercing;
		} else if (cleanCurrEntity == "Necromancer") {
			defenderdmg = defender.GetComponent<NecromancerBehaviour> ().attack;
			//defenderhealth = defender.GetComponent<NecromancerBehaviour> ().health;
			defenderlasthealth = defender.GetComponent<NecromancerBehaviour> ().lasthealth;
			//defenderrange = defender.GetComponent<NecromancerBehaviour> ().range;
			defenderarmor = defender.GetComponent<NecromancerBehaviour> ().armor;
			defenderarmorpiercing = defender.GetComponent<NecromancerBehaviour> ().armorpiercing;
		} else if (cleanCurrEntity == "Skeleton") {
			defenderdmg = defender.GetComponent<SkeletonBehaviour> ().attack;
			//defenderhealth = defender.GetComponent<SkeletonBehaviour> ().health;
			defenderlasthealth = defender.GetComponent<SkeletonBehaviour> ().lasthealth;
			//defenderrange = defender.GetComponent<SkeletonBehaviour> ().range;
			defenderarmor = defender.GetComponent<SkeletonBehaviour> ().armor;
			defenderarmorpiercing = defender.GetComponent<SkeletonBehaviour> ().armorpiercing;
		} else if (cleanCurrEntity == "SkeletonArcher") {
			defenderdmg = defender.GetComponent<SkeletonArcherBehaviour> ().attack;
			//defenderhealth = defender.GetComponent<SkeletonArcherBehaviour> ().health;
			defenderlasthealth = defender.GetComponent<SkeletonArcherBehaviour> ().lasthealth;
			//defenderrange = defender.GetComponent<SkeletonBehaviour> ().range;
			defenderarmor = defender.GetComponent<SkeletonArcherBehaviour> ().armor;
			defenderarmorpiercing = defender.GetComponent<SkeletonArcherBehaviour> ().armorpiercing;
		}
	}

	void SetAttackerInfo(string selectedentity) {
		GameObject attacker = GameObject.Find (selectedentity);

		//------Set New Info Attacker------
		if (cleanSelEntity == "Necromancer") {
			attacker.GetComponent<NecromancerBehaviour> ().lasthealth = attackerlasthealth;
			Text atthealthtext = GameObject.Find ("Health " + selectedentity).GetComponent<Text> ();
			atthealthtext.text = attackerlasthealth.ToString ();
			attacker.GetComponent<NecromancerBehaviour> ().currattackpoint = attacker.GetComponent<NecromancerBehaviour> ().currattackpoint - 1;
		} else if (cleanSelEntity == "Skeleton") {
			attacker.GetComponent<SkeletonBehaviour> ().lasthealth = attackerlasthealth;
			Text atthealthtext = GameObject.Find ("Health " + selectedentity).GetComponent<Text> ();
			atthealthtext.text = attackerlasthealth.ToString ();
			attacker.GetComponent<SkeletonBehaviour> ().currattackpoint = attacker.GetComponent<SkeletonBehaviour> ().currattackpoint - 1;
		} else if (cleanSelEntity == "Zombie") {
			attacker.GetComponent<ZombieBehaviour> ().lasthealth = attackerlasthealth;
			Text atthealthtext = GameObject.Find ("Health " + selectedentity).GetComponent<Text> ();
			atthealthtext.text = attackerlasthealth.ToString ();
			attacker.GetComponent<ZombieBehaviour> ().currattackpoint = attacker.GetComponent<ZombieBehaviour> ().currattackpoint - 1;
		} else if (cleanSelEntity == "Militia") {
			attacker.GetComponent<MilitiaBehaviour> ().lasthealth = attackerlasthealth;
			Text atthealthtext = GameObject.Find ("Health " + selectedentity).GetComponent<Text> ();
			atthealthtext.text = attackerlasthealth.ToString ();
			attacker.GetComponent<MilitiaBehaviour> ().currattackpoint = attacker.GetComponent<MilitiaBehaviour> ().currattackpoint - 1;
		} else if (cleanSelEntity == "SkeletonArcher") {
			attacker.GetComponent<SkeletonArcherBehaviour> ().lasthealth = attackerlasthealth;
			Text atthealthtext = GameObject.Find ("Health " + selectedentity).GetComponent<Text> ();
			atthealthtext.text = attackerlasthealth.ToString ();
			attacker.GetComponent<SkeletonArcherBehaviour> ().currattackpoint = attacker.GetComponent<SkeletonArcherBehaviour> ().currattackpoint - 1;
		}
	}

	void SetDefenderInfo(string currEntity) {
		GameObject defender = GameObject.Find (currEntity);

		//------Set New Info Defender------
		if (cleanCurrEntity == "Militia") {
			defender.GetComponent<MilitiaBehaviour> ().lasthealth = defenderlasthealth;
			Text defhealthtext = GameObject.Find ("Health " + currEntity).GetComponent<Text> ();
			defhealthtext.text = defenderlasthealth.ToString ();
		} else if (cleanCurrEntity == "Zombie") {
			defender.GetComponent<ZombieBehaviour> ().lasthealth = defenderlasthealth;
			Text defhealthtext = GameObject.Find ("Health " + currEntity).GetComponent<Text> ();
			defhealthtext.text = defenderlasthealth.ToString ();
		} else if (cleanCurrEntity == "Skeleton") {
			defender.GetComponent<SkeletonBehaviour> ().lasthealth = defenderlasthealth;
			Text defhealthtext = GameObject.Find ("Health " + currEntity).GetComponent<Text> ();
			defhealthtext.text = defenderlasthealth.ToString ();
		} else if (cleanCurrEntity == "Necromancer") {
			defender.GetComponent<NecromancerBehaviour> ().lasthealth = defenderlasthealth;
			Text defhealthtext = GameObject.Find ("Health " + currEntity).GetComponent<Text> ();
			defhealthtext.text = defenderlasthealth.ToString ();
		} else if (cleanCurrEntity == "SkeletonArcher") {
			defender.GetComponent<SkeletonArcherBehaviour> ().lasthealth = defenderlasthealth;
			Text defhealthtext = GameObject.Find ("Health " + currEntity).GetComponent<Text> ();
			defhealthtext.text = defenderlasthealth.ToString ();
		}
	}

	void GetMovementInfo(string selectedentity) {
		GameObject playerEntity = GameObject.Find (selectedentity);
		//find movement points
		if (cleanSelEntity == "Zombie") {
			playermovepoint = playerEntity.GetComponent<ZombieBehaviour> ().movementpoint;
			playercurrmovepoint = playerEntity.GetComponent<ZombieBehaviour> ().currmovementpoint;
		} else if (cleanSelEntity == "Skeleton") {
			playermovepoint = playerEntity.GetComponent<SkeletonBehaviour> ().movementpoint;
			playercurrmovepoint = playerEntity.GetComponent<SkeletonBehaviour> ().currmovementpoint;
		} else if (cleanSelEntity == "Necromancer") {
			playermovepoint = playerEntity.GetComponent<NecromancerBehaviour> ().movementpoint;
			playercurrmovepoint = playerEntity.GetComponent<NecromancerBehaviour> ().currmovementpoint;
		} else if (cleanSelEntity == "Militia") {
			playermovepoint = playerEntity.GetComponent<MilitiaBehaviour> ().movementpoint;
			playercurrmovepoint = playerEntity.GetComponent<MilitiaBehaviour> ().currmovementpoint;
		} else if (cleanSelEntity == "SkeletonArcher") {
			playermovepoint = playerEntity.GetComponent<SkeletonArcherBehaviour> ().movementpoint;
			playercurrmovepoint = playerEntity.GetComponent<SkeletonArcherBehaviour> ().currmovementpoint;
		}
	}

	void NewMovementPoints(string selectedentity, int change) {
		GameObject playerEntity = GameObject.Find (selectedentity);
		//set movement points
		if (cleanSelEntity == "Zombie") {
			playerEntity.GetComponent<ZombieBehaviour> ().currmovementpoint = playerEntity.GetComponent<ZombieBehaviour> ().currmovementpoint - change;
		} else if (cleanSelEntity == "Skeleton") {
			playerEntity.GetComponent<SkeletonBehaviour> ().currmovementpoint = playerEntity.GetComponent<SkeletonBehaviour> ().currmovementpoint - change;
		} else if (cleanSelEntity == "Necromancer") {
			playerEntity.GetComponent<NecromancerBehaviour> ().currmovementpoint = playerEntity.GetComponent<NecromancerBehaviour> ().currmovementpoint - change;
		} else if (cleanSelEntity == "Militia") {
			playerEntity.GetComponent<MilitiaBehaviour> ().currmovementpoint = playerEntity.GetComponent<MilitiaBehaviour> ().currmovementpoint - change;
		} else if (cleanSelEntity == "SkeletonArcher") {
			playerEntity.GetComponent<SkeletonArcherBehaviour> ().currmovementpoint = playerEntity.GetComponent<SkeletonArcherBehaviour> ().currmovementpoint - change;
		}
	}

	void CalcSouls(string faction, string diedentity) {
		if (faction == "undead") {
			if (diedentity == "Militia") {
				resources.ChangeSouls (100);
			}
		}
	}
}
