using UnityEngine;
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
		} else if (cleanSelEntity == "SkeletonArcher") {
            playercurrattpoint = attacker.GetComponent<SkeletonArcherBehaviour>().currattackpoint;
        } else if (cleanSelEntity == "ArmoredSkeleton") {
            playercurrattpoint = attacker.GetComponent<ArmoredSkeletonBehaviour>().currattackpoint;
        } else if (cleanSelEntity == "DeathKnight") {
            playercurrattpoint = attacker.GetComponent<DeathKnightBehaviour>().currattackpoint;
        }

        else if (cleanSelEntity == "Militia") {
			playercurrattpoint = attacker.GetComponent<MilitiaBehaviour> ().currattackpoint;
		} else if (cleanSelEntity == "Archer") {
			playercurrattpoint = attacker.GetComponent<ArcherBehaviour> ().currattackpoint;
		} else if (cleanSelEntity == "Longbowman") {
			playercurrattpoint = attacker.GetComponent<LongbowmanBehaviour> ().currattackpoint;
		} else if (cleanSelEntity == "Crossbowman") {
			playercurrattpoint = attacker.GetComponent<CrossbowmanBehaviour> ().currattackpoint;
		} else if (cleanSelEntity == "Footman") {
			playercurrattpoint = attacker.GetComponent<FootmanBehaviour> ().currattackpoint;
		} else if (cleanSelEntity == "MountedKnight") {
			playercurrattpoint = attacker.GetComponent<MountedKnightBehaviour> ().currattackpoint;
		} else if (cleanSelEntity == "HeroKing") {
			playercurrattpoint = attacker.GetComponent<HeroKingBehaviour> ().currattackpoint;
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
		} else if (cleanSelEntity == "SkeletonArcher") {
            attackerdmg = attacker.GetComponent<SkeletonArcherBehaviour>().attack;
            //attackerhealth = attacker.GetComponent<SkeletonArcherBehaviour> ().health;
            attackerlasthealth = attacker.GetComponent<SkeletonArcherBehaviour>().lasthealth;
            attackerrange = attacker.GetComponent<SkeletonArcherBehaviour>().range;
            attackerrangedmg = attacker.GetComponent<SkeletonArcherBehaviour> ().rangeattack;
            attackerarmor = attacker.GetComponent<SkeletonArcherBehaviour>().armor;
            attackerarmorpiercing = attacker.GetComponent<SkeletonArcherBehaviour>().armorpiercing;
        } else if (cleanSelEntity == "ArmoredSkeleton") {
            attackerdmg = attacker.GetComponent<ArmoredSkeletonBehaviour>().attack;
            //attackerhealth = attacker.GetComponent<ArmoredSkeletonBehaviour> ().health;
            attackerlasthealth = attacker.GetComponent<ArmoredSkeletonBehaviour>().lasthealth;
            attackerrange = attacker.GetComponent<ArmoredSkeletonBehaviour>().range;
            attackerarmor = attacker.GetComponent<ArmoredSkeletonBehaviour>().armor;
            attackerarmorpiercing = attacker.GetComponent<ArmoredSkeletonBehaviour>().armorpiercing;
        } else if (cleanSelEntity == "DeathKnight") {
            attackerdmg = attacker.GetComponent<DeathKnightBehaviour>().attack;
            //attackerhealth = attacker.GetComponent<DeathKnightBehaviour> ().health;
            attackerlasthealth = attacker.GetComponent<DeathKnightBehaviour>().lasthealth;
            attackerrange = attacker.GetComponent<DeathKnightBehaviour>().range;
            attackerarmor = attacker.GetComponent<DeathKnightBehaviour>().armor;
            attackerarmorpiercing = attacker.GetComponent<DeathKnightBehaviour>().armorpiercing;
        }

         else if (cleanSelEntity == "Militia") {
			attackerdmg = attacker.GetComponent<MilitiaBehaviour> ().attack;
			//attackerhealth = attacker.GetComponent<MilitiaBehaviour> ().health;
			attackerlasthealth = attacker.GetComponent<MilitiaBehaviour> ().lasthealth;
			attackerrange = attacker.GetComponent<MilitiaBehaviour> ().range;
			attackerarmor = attacker.GetComponent<MilitiaBehaviour> ().armor;
			attackerarmorpiercing = attacker.GetComponent<MilitiaBehaviour> ().armorpiercing;
		} else if (cleanSelEntity == "Archer") {
			attackerdmg = attacker.GetComponent<ArcherBehaviour> ().attack;
			//attackerhealth = attacker.GetComponent<ArcherBehaviour> ().health;
			attackerlasthealth = attacker.GetComponent<ArcherBehaviour> ().lasthealth;
			attackerrange = attacker.GetComponent<ArcherBehaviour> ().range;
            attackerrangedmg = attacker.GetComponent<ArcherBehaviour> ().rangeattack;
			attackerarmor = attacker.GetComponent<ArcherBehaviour> ().armor;
			attackerarmorpiercing = attacker.GetComponent<ArcherBehaviour> ().armorpiercing;
		} else if (cleanSelEntity == "Longbowman") {
			attackerdmg = attacker.GetComponent<LongbowmanBehaviour> ().attack;
			//attackerhealth = attacker.GetComponent<LongbowmanBehaviour> ().health;
			attackerlasthealth = attacker.GetComponent<LongbowmanBehaviour> ().lasthealth;
			attackerrange = attacker.GetComponent<LongbowmanBehaviour> ().range;
            attackerrangedmg = attacker.GetComponent<LongbowmanBehaviour> ().rangeattack;
			attackerarmor = attacker.GetComponent<LongbowmanBehaviour> ().armor;
			attackerarmorpiercing = attacker.GetComponent<LongbowmanBehaviour> ().armorpiercing;
		} else if (cleanSelEntity == "Crossbowman") {
			attackerdmg = attacker.GetComponent<CrossbowmanBehaviour> ().attack;
			//attackerhealth = attacker.GetComponent<CrossbowmanBehaviour> ().health;
			attackerlasthealth = attacker.GetComponent<CrossbowmanBehaviour> ().lasthealth;
			attackerrange = attacker.GetComponent<CrossbowmanBehaviour> ().range;
            attackerrangedmg = attacker.GetComponent<CrossbowmanBehaviour> ().rangeattack;
			attackerarmor = attacker.GetComponent<CrossbowmanBehaviour> ().armor;
			attackerarmorpiercing = attacker.GetComponent<CrossbowmanBehaviour> ().armorpiercing;
		} else if (cleanSelEntity == "Footman") {
			attackerdmg = attacker.GetComponent<FootmanBehaviour> ().attack;
			//attackerhealth = attacker.GetComponent<FootmanBehaviour> ().health;
			attackerlasthealth = attacker.GetComponent<FootmanBehaviour> ().lasthealth;
			attackerrange = attacker.GetComponent<FootmanBehaviour> ().range;
			attackerarmor = attacker.GetComponent<FootmanBehaviour> ().armor;
			attackerarmorpiercing = attacker.GetComponent<FootmanBehaviour> ().armorpiercing;
		} else if (cleanSelEntity == "MountedKnight") {
			attackerdmg = attacker.GetComponent<MountedKnightBehaviour> ().attack;
			//attackerhealth = attacker.GetComponent<MountedKnightBehaviour> ().health;
			attackerlasthealth = attacker.GetComponent<MountedKnightBehaviour> ().lasthealth;
			attackerrange = attacker.GetComponent<MountedKnightBehaviour> ().range;
			attackerarmor = attacker.GetComponent<MountedKnightBehaviour> ().armor;
			attackerarmorpiercing = attacker.GetComponent<MountedKnightBehaviour> ().armorpiercing;
		} else if (cleanSelEntity == "HeroKing") {
			attackerdmg = attacker.GetComponent<HeroKingBehaviour> ().attack;
			//attackerhealth = attacker.GetComponent<HeroKingBehaviour> ().health;
			attackerlasthealth = attacker.GetComponent<HeroKingBehaviour> ().lasthealth;
			attackerrange = attacker.GetComponent<HeroKingBehaviour> ().range;
			attackerarmor = attacker.GetComponent<HeroKingBehaviour> ().armor;
			attackerarmorpiercing = attacker.GetComponent<HeroKingBehaviour> ().armorpiercing;
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
		} else if (cleanCurrEntity == "Archer") {
			defenderdmg = defender.GetComponent<ArcherBehaviour> ().attack;
			//defenderhealth = defender.GetComponent<ArcherBehaviour> ().health;
			defenderlasthealth = defender.GetComponent<ArcherBehaviour> ().lasthealth;
			//defenderrange = defender.GetComponent<ArcherBehaviour> ().range;
			defenderarmor = defender.GetComponent<ArcherBehaviour> ().armor;
			defenderarmorpiercing = defender.GetComponent<ArcherBehaviour> ().armorpiercing;
		} else if (cleanCurrEntity == "Longbowman") {
			defenderdmg = defender.GetComponent<LongbowmanBehaviour> ().attack;
			//defenderhealth = defender.GetComponent<LongbowmanBehaviour> ().health;
			defenderlasthealth = defender.GetComponent<LongbowmanBehaviour> ().lasthealth;
			//defenderrange = defender.GetComponent<LongbowmanBehaviour> ().range;
			defenderarmor = defender.GetComponent<LongbowmanBehaviour> ().armor;
			defenderarmorpiercing = defender.GetComponent<LongbowmanBehaviour> ().armorpiercing;
		} else if (cleanCurrEntity == "Crossbowman") {
			defenderdmg = defender.GetComponent<CrossbowmanBehaviour> ().attack;
			//defenderhealth = defender.GetComponent<CrossbowmanBehaviour> ().health;
			defenderlasthealth = defender.GetComponent<CrossbowmanBehaviour> ().lasthealth;
			//defenderrange = defender.GetComponent<CrossbowmanBehaviour> ().range;
			defenderarmor = defender.GetComponent<CrossbowmanBehaviour> ().armor;
			defenderarmorpiercing = defender.GetComponent<CrossbowmanBehaviour> ().armorpiercing;
		} else if (cleanCurrEntity == "Footman") {
			defenderdmg = defender.GetComponent<FootmanBehaviour> ().attack;
			//defenderhealth = defender.GetComponent<FootmanBehaviour> ().health;
			defenderlasthealth = defender.GetComponent<FootmanBehaviour> ().lasthealth;
			//defenderrange = defender.GetComponent<FootmanBehaviour> ().range;
			defenderarmor = defender.GetComponent<FootmanBehaviour> ().armor;
			defenderarmorpiercing = defender.GetComponent<FootmanBehaviour> ().armorpiercing;
		} else if (cleanCurrEntity == "MountedKnight") {
			defenderdmg = defender.GetComponent<MountedKnightBehaviour> ().attack;
			//defenderhealth = defender.GetComponent<MountedKnightBehaviour> ().health;
			defenderlasthealth = defender.GetComponent<MountedKnightBehaviour> ().lasthealth;
			//defenderrange = defender.GetComponent<MountedKnightBehaviour> ().range;
			defenderarmor = defender.GetComponent<MountedKnightBehaviour> ().armor;
			defenderarmorpiercing = defender.GetComponent<MountedKnightBehaviour> ().armorpiercing;
		} else if (cleanCurrEntity == "HeroKing") {
			defenderdmg = defender.GetComponent<HeroKingBehaviour> ().attack;
			//defenderhealth = defender.GetComponent<HeroKingBehaviour> ().health;
			defenderlasthealth = defender.GetComponent<HeroKingBehaviour> ().lasthealth;
			//defenderrange = defender.GetComponent<HeroKingBehaviour> ().range;
			defenderarmor = defender.GetComponent<HeroKingBehaviour> ().armor;
			defenderarmorpiercing = defender.GetComponent<HeroKingBehaviour> ().armorpiercing;
		}

        else if (cleanCurrEntity == "Zombie") {
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
			//defenderrange = defender.GetComponent<SkeletonArcherBehaviour> ().range;
			defenderarmor = defender.GetComponent<SkeletonArcherBehaviour> ().armor;
			defenderarmorpiercing = defender.GetComponent<SkeletonArcherBehaviour> ().armorpiercing;
		} else if (cleanCurrEntity == "ArmoredSkeleton") {
			defenderdmg = defender.GetComponent<ArmoredSkeletonBehaviour> ().attack;
			//defenderhealth = defender.GetComponent<ArmoredSkeletonBehaviour> ().health;
			defenderlasthealth = defender.GetComponent<ArmoredSkeletonBehaviour> ().lasthealth;
			//defenderrange = defender.GetComponent<ArmoredSkeletonBehaviour> ().range;
			defenderarmor = defender.GetComponent<ArmoredSkeletonBehaviour> ().armor;
			defenderarmorpiercing = defender.GetComponent<ArmoredSkeletonBehaviour> ().armorpiercing;
		} else if (cleanCurrEntity == "DeathKnight") {
			defenderdmg = defender.GetComponent<DeathKnightBehaviour> ().attack;
			//defenderhealth = defender.GetComponent<DeathKnightBehaviour> ().health;
			defenderlasthealth = defender.GetComponent<DeathKnightBehaviour> ().lasthealth;
			//defenderrange = defender.GetComponent<DeathKnightBehaviour> ().range;
			defenderarmor = defender.GetComponent<DeathKnightBehaviour> ().armor;
			defenderarmorpiercing = defender.GetComponent<DeathKnightBehaviour> ().armorpiercing;
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
		} else if (cleanSelEntity == "SkeletonArcher") {
			attacker.GetComponent<SkeletonArcherBehaviour> ().lasthealth = attackerlasthealth;
			Text atthealthtext = GameObject.Find ("Health " + selectedentity).GetComponent<Text> ();
			atthealthtext.text = attackerlasthealth.ToString ();
			attacker.GetComponent<SkeletonArcherBehaviour> ().currattackpoint = attacker.GetComponent<SkeletonArcherBehaviour> ().currattackpoint - 1;
		} else if (cleanSelEntity == "ArmoredSkeleton") {
			attacker.GetComponent<ArmoredSkeletonBehaviour> ().lasthealth = attackerlasthealth;
			Text atthealthtext = GameObject.Find ("Health " + selectedentity).GetComponent<Text> ();
			atthealthtext.text = attackerlasthealth.ToString ();
			attacker.GetComponent<ArmoredSkeletonBehaviour> ().currattackpoint = attacker.GetComponent<ArmoredSkeletonBehaviour> ().currattackpoint - 1;
		} else if (cleanSelEntity == "DeathKnight") {
			attacker.GetComponent<DeathKnightBehaviour> ().lasthealth = attackerlasthealth;
			Text atthealthtext = GameObject.Find ("Health " + selectedentity).GetComponent<Text> ();
			atthealthtext.text = attackerlasthealth.ToString ();
			attacker.GetComponent<DeathKnightBehaviour> ().currattackpoint = attacker.GetComponent<DeathKnightBehaviour> ().currattackpoint - 1;
		}

        else if (cleanSelEntity == "Militia") {
			attacker.GetComponent<MilitiaBehaviour> ().lasthealth = attackerlasthealth;
			Text atthealthtext = GameObject.Find ("Health " + selectedentity).GetComponent<Text> ();
			atthealthtext.text = attackerlasthealth.ToString ();
			attacker.GetComponent<MilitiaBehaviour> ().currattackpoint = attacker.GetComponent<MilitiaBehaviour> ().currattackpoint - 1;
		} else if (cleanSelEntity == "Archer") {
			attacker.GetComponent<ArcherBehaviour> ().lasthealth = attackerlasthealth;
			Text atthealthtext = GameObject.Find ("Health " + selectedentity).GetComponent<Text> ();
			atthealthtext.text = attackerlasthealth.ToString ();
			attacker.GetComponent<ArcherBehaviour> ().currattackpoint = attacker.GetComponent<ArcherBehaviour> ().currattackpoint - 1;
		} else if (cleanSelEntity == "Longbowman") {
			attacker.GetComponent<LongbowmanBehaviour> ().lasthealth = attackerlasthealth;
			Text atthealthtext = GameObject.Find ("Health " + selectedentity).GetComponent<Text> ();
			atthealthtext.text = attackerlasthealth.ToString ();
			attacker.GetComponent<LongbowmanBehaviour> ().currattackpoint = attacker.GetComponent<LongbowmanBehaviour> ().currattackpoint - 1;
		} else if (cleanSelEntity == "Crossbowman") {
			attacker.GetComponent<CrossbowmanBehaviour> ().lasthealth = attackerlasthealth;
			Text atthealthtext = GameObject.Find ("Health " + selectedentity).GetComponent<Text> ();
			atthealthtext.text = attackerlasthealth.ToString ();
			attacker.GetComponent<CrossbowmanBehaviour> ().currattackpoint = attacker.GetComponent<CrossbowmanBehaviour> ().currattackpoint - 1;
		} else if (cleanSelEntity == "Footman") {
			attacker.GetComponent<FootmanBehaviour> ().lasthealth = attackerlasthealth;
			Text atthealthtext = GameObject.Find ("Health " + selectedentity).GetComponent<Text> ();
			atthealthtext.text = attackerlasthealth.ToString ();
			attacker.GetComponent<FootmanBehaviour> ().currattackpoint = attacker.GetComponent<FootmanBehaviour> ().currattackpoint - 1;
		} else if (cleanSelEntity == "MountedKnight") {
			attacker.GetComponent<MountedKnightBehaviour> ().lasthealth = attackerlasthealth;
			Text atthealthtext = GameObject.Find ("Health " + selectedentity).GetComponent<Text> ();
			atthealthtext.text = attackerlasthealth.ToString ();
			attacker.GetComponent<MountedKnightBehaviour> ().currattackpoint = attacker.GetComponent<MountedKnightBehaviour> ().currattackpoint - 1;
		} else if (cleanSelEntity == "HeroKing") {
			attacker.GetComponent<HeroKingBehaviour> ().lasthealth = attackerlasthealth;
			Text atthealthtext = GameObject.Find ("Health " + selectedentity).GetComponent<Text> ();
			atthealthtext.text = attackerlasthealth.ToString ();
			attacker.GetComponent<HeroKingBehaviour> ().currattackpoint = attacker.GetComponent<HeroKingBehaviour> ().currattackpoint - 1;
		} 
	}

	void SetDefenderInfo(string currEntity) {
		GameObject defender = GameObject.Find (currEntity);

		//------Set New Info Defender------
		if (cleanCurrEntity == "Militia") {
			defender.GetComponent<MilitiaBehaviour> ().lasthealth = defenderlasthealth;
			Text defhealthtext = GameObject.Find ("Health " + currEntity).GetComponent<Text> ();
			defhealthtext.text = defenderlasthealth.ToString ();
		} else if (cleanCurrEntity == "Archer") {
			defender.GetComponent<ArcherBehaviour> ().lasthealth = defenderlasthealth;
			Text defhealthtext = GameObject.Find ("Health " + currEntity).GetComponent<Text> ();
			defhealthtext.text = defenderlasthealth.ToString ();
		} else if (cleanCurrEntity == "Longbowman") {
			defender.GetComponent<LongbowmanBehaviour> ().lasthealth = defenderlasthealth;
			Text defhealthtext = GameObject.Find ("Health " + currEntity).GetComponent<Text> ();
			defhealthtext.text = defenderlasthealth.ToString ();
		} else if (cleanCurrEntity == "Crossbowman") {
			defender.GetComponent<CrossbowmanBehaviour> ().lasthealth = defenderlasthealth;
			Text defhealthtext = GameObject.Find ("Health " + currEntity).GetComponent<Text> ();
			defhealthtext.text = defenderlasthealth.ToString ();
		} else if (cleanCurrEntity == "Footman") {
			defender.GetComponent<FootmanBehaviour> ().lasthealth = defenderlasthealth;
			Text defhealthtext = GameObject.Find ("Health " + currEntity).GetComponent<Text> ();
			defhealthtext.text = defenderlasthealth.ToString ();
		} else if (cleanCurrEntity == "MountedKnight") {
			defender.GetComponent<MountedKnightBehaviour> ().lasthealth = defenderlasthealth;
			Text defhealthtext = GameObject.Find ("Health " + currEntity).GetComponent<Text> ();
			defhealthtext.text = defenderlasthealth.ToString ();
		} else if (cleanCurrEntity == "HeroKing") {
			defender.GetComponent<HeroKingBehaviour> ().lasthealth = defenderlasthealth;
			Text defhealthtext = GameObject.Find ("Health " + currEntity).GetComponent<Text> ();
			defhealthtext.text = defenderlasthealth.ToString ();
		}

        else if (cleanCurrEntity == "Zombie") {
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
		} else if (cleanCurrEntity == "ArmoredSkeleton") {
			defender.GetComponent<ArmoredSkeletonBehaviour> ().lasthealth = defenderlasthealth;
			Text defhealthtext = GameObject.Find ("Health " + currEntity).GetComponent<Text> ();
			defhealthtext.text = defenderlasthealth.ToString ();
		} else if (cleanCurrEntity == "DeathKnight") {
			defender.GetComponent<DeathKnightBehaviour> ().lasthealth = defenderlasthealth;
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
		} else if (cleanSelEntity == "SkeletonArcher") {
			playermovepoint = playerEntity.GetComponent<SkeletonArcherBehaviour> ().movementpoint;
			playercurrmovepoint = playerEntity.GetComponent<SkeletonArcherBehaviour> ().currmovementpoint;
		} else if (cleanSelEntity == "ArmoredSkeleton") {
			playermovepoint = playerEntity.GetComponent<ArmoredSkeletonBehaviour> ().movementpoint;
			playercurrmovepoint = playerEntity.GetComponent<ArmoredSkeletonBehaviour> ().currmovementpoint;
		} else if (cleanSelEntity == "DeathKnight") {
			playermovepoint = playerEntity.GetComponent<DeathKnightBehaviour> ().movementpoint;
			playercurrmovepoint = playerEntity.GetComponent<DeathKnightBehaviour> ().currmovementpoint;
		}

        else if (cleanSelEntity == "Militia") {
			playermovepoint = playerEntity.GetComponent<MilitiaBehaviour> ().movementpoint;
			playercurrmovepoint = playerEntity.GetComponent<MilitiaBehaviour> ().currmovementpoint;
		} else if (cleanSelEntity == "Archer") {
			playermovepoint = playerEntity.GetComponent<ArcherBehaviour> ().movementpoint;
			playercurrmovepoint = playerEntity.GetComponent<ArcherBehaviour> ().currmovementpoint;
		} else if (cleanSelEntity == "Longbowman") {
			playermovepoint = playerEntity.GetComponent<LongbowmanBehaviour> ().movementpoint;
			playercurrmovepoint = playerEntity.GetComponent<LongbowmanBehaviour> ().currmovementpoint;
		} else if (cleanSelEntity == "Crossbowman") {
			playermovepoint = playerEntity.GetComponent<CrossbowmanBehaviour> ().movementpoint;
			playercurrmovepoint = playerEntity.GetComponent<CrossbowmanBehaviour> ().currmovementpoint;
		} else if (cleanSelEntity == "Footman") {
			playermovepoint = playerEntity.GetComponent<FootmanBehaviour> ().movementpoint;
			playercurrmovepoint = playerEntity.GetComponent<FootmanBehaviour> ().currmovementpoint;
		} else if (cleanSelEntity == "MountedKnight") {
			playermovepoint = playerEntity.GetComponent<MountedKnightBehaviour> ().movementpoint;
			playercurrmovepoint = playerEntity.GetComponent<MountedKnightBehaviour> ().currmovementpoint;
		} else if (cleanSelEntity == "HeroKing") {
			playermovepoint = playerEntity.GetComponent<HeroKingBehaviour> ().movementpoint;
			playercurrmovepoint = playerEntity.GetComponent<HeroKingBehaviour> ().currmovementpoint;
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
		} else if (cleanSelEntity == "SkeletonArcher") {
			playerEntity.GetComponent<SkeletonArcherBehaviour> ().currmovementpoint = playerEntity.GetComponent<SkeletonArcherBehaviour> ().currmovementpoint - change;
		} else if (cleanSelEntity == "ArmoredSkeleton") {
			playerEntity.GetComponent<ArmoredSkeletonBehaviour> ().currmovementpoint = playerEntity.GetComponent<ArmoredSkeletonBehaviour> ().currmovementpoint - change;
		} else if (cleanSelEntity == "DeathKnight") {
			playerEntity.GetComponent<DeathKnightBehaviour> ().currmovementpoint = playerEntity.GetComponent<DeathKnightBehaviour> ().currmovementpoint - change;
		}

        else if (cleanSelEntity == "Militia") {
			playerEntity.GetComponent<MilitiaBehaviour> ().currmovementpoint = playerEntity.GetComponent<MilitiaBehaviour> ().currmovementpoint - change;
		} else if (cleanSelEntity == "Archer") {
			playerEntity.GetComponent<ArcherBehaviour> ().currmovementpoint = playerEntity.GetComponent<ArcherBehaviour> ().currmovementpoint - change;
		} else if (cleanSelEntity == "Longbowman") {
			playerEntity.GetComponent<LongbowmanBehaviour> ().currmovementpoint = playerEntity.GetComponent<LongbowmanBehaviour> ().currmovementpoint - change;
		} else if (cleanSelEntity == "Crossbowman") {
			playerEntity.GetComponent<CrossbowmanBehaviour> ().currmovementpoint = playerEntity.GetComponent<CrossbowmanBehaviour> ().currmovementpoint - change;
		} else if (cleanSelEntity == "Footman") {
			playerEntity.GetComponent<FootmanBehaviour> ().currmovementpoint = playerEntity.GetComponent<FootmanBehaviour> ().currmovementpoint - change;
		} else if (cleanSelEntity == "MountedKnight") {
			playerEntity.GetComponent<MountedKnightBehaviour> ().currmovementpoint = playerEntity.GetComponent<MountedKnightBehaviour> ().currmovementpoint - change;
		} else if (cleanSelEntity == "HeroKing") {
			playerEntity.GetComponent<HeroKingBehaviour> ().currmovementpoint = playerEntity.GetComponent<HeroKingBehaviour> ().currmovementpoint - change;
		} 
	}

	void CalcSouls(string faction, string diedentity) {
		if (faction == "undead") {
			if (diedentity == "Militia") {
				resources.ChangeSouls (100);
			} else if (diedentity == "Archer") {
				resources.ChangeSouls (150);
			} else if (diedentity == "Longbowman") {
				resources.ChangeSouls (200);
			} else if (diedentity == "Crossbowman") {
				resources.ChangeSouls (150);
			} else if (diedentity == "Footman") {
				resources.ChangeSouls (150);
			} else if (diedentity == "MountedKnight") {
				resources.ChangeSouls (200);
			} else if (diedentity == "HeroKing") {
				resources.ChangeSouls (1000);
			}
		}
	}
}
