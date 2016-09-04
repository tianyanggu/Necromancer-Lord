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
	public Currency currency;
    public Summon summon;
    public Build build;

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
        string selectedEntityName = hexGrid.GetEntityName(selectedindex);
        GameObject selectedEntity = hexGrid.GetEntityObject(selectedindex);
        if (selectedEntityName == "Empty") {
			return false;
		}
		string currEntityName = hexGrid.GetEntityName(currindex);
        GameObject currEntity = hexGrid.GetEntityObject(currindex);

        //get first part of currEntity as clean and number as num
        cleanSelEntity = Regex.Replace(selectedEntityName.Substring(2), @"[\d-]", string.Empty);
        cleanCurrEntity = Regex.Replace(currEntityName.Substring(2), @"[\d-]", string.Empty);
        //get coordinates of currEntity
        Vector3 cellcoord = hexGrid.GetCellPos(currindex);

		string storedNameSel = PlayerPrefs.GetString (selectedEntityName);
		string numSelEntity = Regex.Replace(storedNameSel, "[^0-9 -]", string.Empty);
		string storedNameCurr = PlayerPrefs.GetString (currEntityName);
		string numCurrEntity = Regex.Replace(storedNameCurr, "[^0-9 -]", string.Empty);

		string selFaction = entityStorage.WhichFaction(cleanSelEntity);
		string currFaction = entityStorage.WhichFaction(cleanCurrEntity);

        //check if on same team
        string selTeam = selectedEntityName.Substring(1, 1);
        string currTeam = currEntityName.Substring(1, 1);
        if (selTeam == currTeam)
        {
            return false;
        }

        GetMovementInfo (selectedEntity);

		//------Movement Empty Cell------
		if (currEntityName == "Empty") {
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
					SetMovementPoints (selectedEntity, 1);
				} else if (playermovepoint != 1) {
					int minmove = movement.GetMovementPointsUsed (selectedindex, currindex, playercurrmovepoint);
					//set new movement points remaining
					SetMovementPoints (selectedEntity, minmove);
				}

				GameObject playerHealth = GameObject.Find ("Health " + selectedEntityName);
                selectedEntity.transform.position = cellcoord;
				playerHealth.transform.position = new Vector3 (cellcoord.x, cellcoord.y + 0.1f, cellcoord.z);
				hexGrid.SetEntityName (selectedindex, "Empty");
                hexGrid.SetEntityObject(selectedindex, null);
				hexGrid.SetEntityName (currindex, selectedEntityName);
                hexGrid.SetEntityObject(currindex, selectedEntity);

                PlayerPrefs.SetInt ("HexEntityIndex" + numSelEntity, currindex);

				return true;
			}
			//------Encounter Enemy------
		} else if (selTeam != currTeam && currEntityName != "Empty") {
            GameObject attacker = selectedEntity;
            GameObject defender = currEntity;

			GetAttackerInfo (attacker);
			GetDefenderInfo (defender);

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
                        summon.KillEntity(currindex);
					}
					if (attackerlasthealth <= 0) {
                        summon.KillEntity(selectedindex);
                    } 
					if (attackerlasthealth > 0 && defenderlasthealth <= 0) {
                        //move to defender's position if have enough movement points and is not ranged unit
                        int minmove = movement.GetMovementPointsUsed (selectedindex, currindex, playercurrmovepoint);
						if (playercurrmovepoint >= minmove && attackerrange == 1) {
							attacker.transform.position = cellcoord;
							hexGrid.SetEntityName (currindex, selectedEntityName);
                            hexGrid.SetEntityObject (currindex, attacker);
                            GameObject attackerHealthText = GameObject.Find ("Health " + selectedEntityName);
							attackerHealthText.transform.position = new Vector3 (cellcoord.x, cellcoord.y + 0.1f, cellcoord.z);
							SetMovementPoints (attacker, minmove);
							PlayerPrefs.SetInt ("HexEntityIndex" + numSelEntity, currindex);
						} else if (attackerrange == 2) {
                            //do nothing
                            //TODO remove if nothing needed in future for attack range 2
                        }
					}

                    //Set New Info
                    SetAttackerInfo (attacker, selectedEntityName);
					SetDefenderInfo (defender, currEntityName);

					return true;
				}
			}
		}

		return false;
	}

	void GetAttackerInfo(GameObject attacker) {
        switch (cleanSelEntity)
        {
            //UNDEAD
            case "Zombie":
                playercurrattpoint = attacker.GetComponent<ZombieBehaviour>().currattackpoint;

                attackerdmg = attacker.GetComponent<ZombieBehaviour>().attack;
                //attackerhealth = attacker.GetComponent<ZombieBehaviour> ().health;
                attackerlasthealth = attacker.GetComponent<ZombieBehaviour>().lasthealth;
                attackerrange = attacker.GetComponent<ZombieBehaviour>().range;
                attackerarmor = attacker.GetComponent<ZombieBehaviour>().armor;
                attackerarmorpiercing = attacker.GetComponent<ZombieBehaviour>().armorpiercing;
                break;
            case "Skeleton":
                playercurrattpoint = attacker.GetComponent<SkeletonBehaviour>().currattackpoint;

                attackerdmg = attacker.GetComponent<SkeletonBehaviour>().attack;
                //attackerhealth = attacker.GetComponent<SkeletonBehaviour> ().health;
                attackerlasthealth = attacker.GetComponent<SkeletonBehaviour>().lasthealth;
                attackerrange = attacker.GetComponent<SkeletonBehaviour>().range;
                attackerarmor = attacker.GetComponent<SkeletonBehaviour>().armor;
                attackerarmorpiercing = attacker.GetComponent<SkeletonBehaviour>().armorpiercing;
                break;
            case "Necromancer":
                playercurrattpoint = attacker.GetComponent<NecromancerBehaviour>().currattackpoint;

                attackerdmg = attacker.GetComponent<NecromancerBehaviour>().attack;
                //attackerhealth = attacker.GetComponent<NecromancerBehaviour> ().health;
                attackerlasthealth = attacker.GetComponent<NecromancerBehaviour>().lasthealth;
                attackerrange = attacker.GetComponent<NecromancerBehaviour>().range;
                attackerrangedmg = attacker.GetComponent<NecromancerBehaviour>().rangeattack;
                attackerarmor = attacker.GetComponent<NecromancerBehaviour>().armor;
                attackerarmorpiercing = attacker.GetComponent<NecromancerBehaviour>().armorpiercing;
                break;
            case "SkeletonArcher":
                playercurrattpoint = attacker.GetComponent<SkeletonArcherBehaviour>().currattackpoint;

                attackerdmg = attacker.GetComponent<SkeletonArcherBehaviour>().attack;
                //attackerhealth = attacker.GetComponent<SkeletonArcherBehaviour> ().health;
                attackerlasthealth = attacker.GetComponent<SkeletonArcherBehaviour>().lasthealth;
                attackerrange = attacker.GetComponent<SkeletonArcherBehaviour>().range;
                attackerrangedmg = attacker.GetComponent<SkeletonArcherBehaviour>().rangeattack;
                attackerarmor = attacker.GetComponent<SkeletonArcherBehaviour>().armor;
                attackerarmorpiercing = attacker.GetComponent<SkeletonArcherBehaviour>().armorpiercing;
                break;
            case "ArmoredSkeleton":
                playercurrattpoint = attacker.GetComponent<ArmoredSkeletonBehaviour>().currattackpoint;

                attackerdmg = attacker.GetComponent<ArmoredSkeletonBehaviour>().attack;
                //attackerhealth = attacker.GetComponent<ArmoredSkeletonBehaviour> ().health;
                attackerlasthealth = attacker.GetComponent<ArmoredSkeletonBehaviour>().lasthealth;
                attackerrange = attacker.GetComponent<ArmoredSkeletonBehaviour>().range;
                attackerarmor = attacker.GetComponent<ArmoredSkeletonBehaviour>().armor;
                attackerarmorpiercing = attacker.GetComponent<ArmoredSkeletonBehaviour>().armorpiercing;
                break;
            case "DeathKnight":
                playercurrattpoint = attacker.GetComponent<DeathKnightBehaviour>().currattackpoint;

                attackerdmg = attacker.GetComponent<DeathKnightBehaviour>().attack;
                //attackerhealth = attacker.GetComponent<DeathKnightBehaviour> ().health;
                attackerlasthealth = attacker.GetComponent<DeathKnightBehaviour>().lasthealth;
                attackerrange = attacker.GetComponent<DeathKnightBehaviour>().range;
                attackerarmor = attacker.GetComponent<DeathKnightBehaviour>().armor;
                attackerarmorpiercing = attacker.GetComponent<DeathKnightBehaviour>().armorpiercing;
                break;

            
            //HUMANS
            case "Militia":
                playercurrattpoint = attacker.GetComponent<MilitiaBehaviour>().currattackpoint;

                attackerdmg = attacker.GetComponent<MilitiaBehaviour>().attack;
                //attackerhealth = attacker.GetComponent<MilitiaBehaviour> ().health;
                attackerlasthealth = attacker.GetComponent<MilitiaBehaviour>().lasthealth;
                attackerrange = attacker.GetComponent<MilitiaBehaviour>().range;
                attackerarmor = attacker.GetComponent<MilitiaBehaviour>().armor;
                attackerarmorpiercing = attacker.GetComponent<MilitiaBehaviour>().armorpiercing;
                break;
            case "Archer":
                playercurrattpoint = attacker.GetComponent<ArcherBehaviour>().currattackpoint;

                attackerdmg = attacker.GetComponent<ArcherBehaviour>().attack;
                //attackerhealth = attacker.GetComponent<ArcherBehaviour> ().health;
                attackerlasthealth = attacker.GetComponent<ArcherBehaviour>().lasthealth;
                attackerrange = attacker.GetComponent<ArcherBehaviour>().range;
                attackerrangedmg = attacker.GetComponent<ArcherBehaviour>().rangeattack;
                attackerarmor = attacker.GetComponent<ArcherBehaviour>().armor;
                attackerarmorpiercing = attacker.GetComponent<ArcherBehaviour>().armorpiercing;
                break;
            case "Longbowman":
                playercurrattpoint = attacker.GetComponent<LongbowmanBehaviour>().currattackpoint;

                attackerdmg = attacker.GetComponent<LongbowmanBehaviour>().attack;
                //attackerhealth = attacker.GetComponent<LongbowmanBehaviour> ().health;
                attackerlasthealth = attacker.GetComponent<LongbowmanBehaviour>().lasthealth;
                attackerrange = attacker.GetComponent<LongbowmanBehaviour>().range;
                attackerrangedmg = attacker.GetComponent<LongbowmanBehaviour>().rangeattack;
                attackerarmor = attacker.GetComponent<LongbowmanBehaviour>().armor;
                attackerarmorpiercing = attacker.GetComponent<LongbowmanBehaviour>().armorpiercing;
                break;
            case "Crossbowman":
                playercurrattpoint = attacker.GetComponent<CrossbowmanBehaviour>().currattackpoint;

                attackerdmg = attacker.GetComponent<CrossbowmanBehaviour>().attack;
                //attackerhealth = attacker.GetComponent<CrossbowmanBehaviour> ().health;
                attackerlasthealth = attacker.GetComponent<CrossbowmanBehaviour>().lasthealth;
                attackerrange = attacker.GetComponent<CrossbowmanBehaviour>().range;
                attackerrangedmg = attacker.GetComponent<CrossbowmanBehaviour>().rangeattack;
                attackerarmor = attacker.GetComponent<CrossbowmanBehaviour>().armor;
                attackerarmorpiercing = attacker.GetComponent<CrossbowmanBehaviour>().armorpiercing;
                break;
            case "Footman":
                playercurrattpoint = attacker.GetComponent<FootmanBehaviour>().currattackpoint;

                attackerdmg = attacker.GetComponent<FootmanBehaviour>().attack;
                //attackerhealth = attacker.GetComponent<FootmanBehaviour> ().health;
                attackerlasthealth = attacker.GetComponent<FootmanBehaviour>().lasthealth;
                attackerrange = attacker.GetComponent<FootmanBehaviour>().range;
                attackerarmor = attacker.GetComponent<FootmanBehaviour>().armor;
                attackerarmorpiercing = attacker.GetComponent<FootmanBehaviour>().armorpiercing;
                break;
            case "MountedKnight":
                playercurrattpoint = attacker.GetComponent<MountedKnightBehaviour>().currattackpoint;

                attackerdmg = attacker.GetComponent<MountedKnightBehaviour>().attack;
                //attackerhealth = attacker.GetComponent<MountedKnightBehaviour> ().health;
                attackerlasthealth = attacker.GetComponent<MountedKnightBehaviour>().lasthealth;
                attackerrange = attacker.GetComponent<MountedKnightBehaviour>().range;
                attackerarmor = attacker.GetComponent<MountedKnightBehaviour>().armor;
                attackerarmorpiercing = attacker.GetComponent<MountedKnightBehaviour>().armorpiercing;
                break;
            case "HeroKing":
                playercurrattpoint = attacker.GetComponent<HeroKingBehaviour>().currattackpoint;

                attackerdmg = attacker.GetComponent<HeroKingBehaviour>().attack;
                //attackerhealth = attacker.GetComponent<HeroKingBehaviour> ().health;
                attackerlasthealth = attacker.GetComponent<HeroKingBehaviour>().lasthealth;
                attackerrange = attacker.GetComponent<HeroKingBehaviour>().range;
                attackerarmor = attacker.GetComponent<HeroKingBehaviour>().armor;
                attackerarmorpiercing = attacker.GetComponent<HeroKingBehaviour>().armorpiercing;
                break;
        }
    }

	void GetDefenderInfo(GameObject defender) {
        switch (cleanCurrEntity)
        {
            //UNDEAD
            case "Zombie":
                defenderdmg = defender.GetComponent<ZombieBehaviour>().attack;
                //defenderhealth = defender.GetComponent<ZombieBehaviour> ().health;
                defenderlasthealth = defender.GetComponent<ZombieBehaviour>().lasthealth;
                //defenderrange = defender.GetComponent<ZombieBehaviour> ().range;
                defenderarmor = defender.GetComponent<ZombieBehaviour>().armor;
                defenderarmorpiercing = defender.GetComponent<ZombieBehaviour>().armorpiercing;
                break;
            case "Skeleton":
                defenderdmg = defender.GetComponent<SkeletonBehaviour>().attack;
                //defenderhealth = defender.GetComponent<SkeletonBehaviour> ().health;
                defenderlasthealth = defender.GetComponent<SkeletonBehaviour>().lasthealth;
                //defenderrange = defender.GetComponent<SkeletonBehaviour> ().range;
                defenderarmor = defender.GetComponent<SkeletonBehaviour>().armor;
                defenderarmorpiercing = defender.GetComponent<SkeletonBehaviour>().armorpiercing;
                break;
            case "Necromancer":
                defenderdmg = defender.GetComponent<NecromancerBehaviour>().attack;
                //defenderhealth = defender.GetComponent<NecromancerBehaviour> ().health;
                defenderlasthealth = defender.GetComponent<NecromancerBehaviour>().lasthealth;
                //defenderrange = defender.GetComponent<NecromancerBehaviour> ().range;
                defenderarmor = defender.GetComponent<NecromancerBehaviour>().armor;
                defenderarmorpiercing = defender.GetComponent<NecromancerBehaviour>().armorpiercing;
                break;
            case "SkeletonArcher":
                defenderdmg = defender.GetComponent<SkeletonArcherBehaviour>().attack;
                //defenderhealth = defender.GetComponent<SkeletonArcherBehaviour> ().health;
                defenderlasthealth = defender.GetComponent<SkeletonArcherBehaviour>().lasthealth;
                //defenderrange = defender.GetComponent<SkeletonArcherBehaviour> ().range;
                defenderarmor = defender.GetComponent<SkeletonArcherBehaviour>().armor;
                defenderarmorpiercing = defender.GetComponent<SkeletonArcherBehaviour>().armorpiercing;
                break;
            case "ArmoredSkeleton":
                defenderdmg = defender.GetComponent<ArmoredSkeletonBehaviour>().attack;
                //defenderhealth = defender.GetComponent<ArmoredSkeletonBehaviour> ().health;
                defenderlasthealth = defender.GetComponent<ArmoredSkeletonBehaviour>().lasthealth;
                //defenderrange = defender.GetComponent<ArmoredSkeletonBehaviour> ().range;
                defenderarmor = defender.GetComponent<ArmoredSkeletonBehaviour>().armor;
                defenderarmorpiercing = defender.GetComponent<ArmoredSkeletonBehaviour>().armorpiercing;
                break;
            case "DeathKnight":
                defenderdmg = defender.GetComponent<DeathKnightBehaviour>().attack;
                //defenderhealth = defender.GetComponent<DeathKnightBehaviour> ().health;
                defenderlasthealth = defender.GetComponent<DeathKnightBehaviour>().lasthealth;
                //defenderrange = defender.GetComponent<DeathKnightBehaviour> ().range;
                defenderarmor = defender.GetComponent<DeathKnightBehaviour>().armor;
                defenderarmorpiercing = defender.GetComponent<DeathKnightBehaviour>().armorpiercing;
                break;

            //HUMANS
            case "Militia":
                defenderdmg = defender.GetComponent<MilitiaBehaviour>().attack;
                //defenderhealth = defender.GetComponent<MilitiaBehaviour> ().health;
                defenderlasthealth = defender.GetComponent<MilitiaBehaviour>().lasthealth;
                //defenderrange = defender.GetComponent<MilitiaBehaviour> ().range;
                defenderarmor = defender.GetComponent<MilitiaBehaviour>().armor;
                defenderarmorpiercing = defender.GetComponent<MilitiaBehaviour>().armorpiercing;
                break;
            case "Archer":
                defenderdmg = defender.GetComponent<ArcherBehaviour>().attack;
                //defenderhealth = defender.GetComponent<ArcherBehaviour> ().health;
                defenderlasthealth = defender.GetComponent<ArcherBehaviour>().lasthealth;
                //defenderrange = defender.GetComponent<ArcherBehaviour> ().range;
                defenderarmor = defender.GetComponent<ArcherBehaviour>().armor;
                defenderarmorpiercing = defender.GetComponent<ArcherBehaviour>().armorpiercing;
                break;
            case "Longbowman":
                defenderdmg = defender.GetComponent<LongbowmanBehaviour>().attack;
                //defenderhealth = defender.GetComponent<LongbowmanBehaviour> ().health;
                defenderlasthealth = defender.GetComponent<LongbowmanBehaviour>().lasthealth;
                //defenderrange = defender.GetComponent<LongbowmanBehaviour> ().range;
                defenderarmor = defender.GetComponent<LongbowmanBehaviour>().armor;
                defenderarmorpiercing = defender.GetComponent<LongbowmanBehaviour>().armorpiercing;
                break;
            case "Crossbowman":
                defenderdmg = defender.GetComponent<CrossbowmanBehaviour>().attack;
                //defenderhealth = defender.GetComponent<CrossbowmanBehaviour> ().health;
                defenderlasthealth = defender.GetComponent<CrossbowmanBehaviour>().lasthealth;
                //defenderrange = defender.GetComponent<CrossbowmanBehaviour> ().range;
                defenderarmor = defender.GetComponent<CrossbowmanBehaviour>().armor;
                defenderarmorpiercing = defender.GetComponent<CrossbowmanBehaviour>().armorpiercing;
                break;
            case "Footman":
                defenderdmg = defender.GetComponent<FootmanBehaviour>().attack;
                //defenderhealth = defender.GetComponent<FootmanBehaviour> ().health;
                defenderlasthealth = defender.GetComponent<FootmanBehaviour>().lasthealth;
                //defenderrange = defender.GetComponent<FootmanBehaviour> ().range;
                defenderarmor = defender.GetComponent<FootmanBehaviour>().armor;
                defenderarmorpiercing = defender.GetComponent<FootmanBehaviour>().armorpiercing;
                break;
            case "MountedKnight":
                defenderdmg = defender.GetComponent<MountedKnightBehaviour>().attack;
                //defenderhealth = defender.GetComponent<MountedKnightBehaviour> ().health;
                defenderlasthealth = defender.GetComponent<MountedKnightBehaviour>().lasthealth;
                //defenderrange = defender.GetComponent<MountedKnightBehaviour> ().range;
                defenderarmor = defender.GetComponent<MountedKnightBehaviour>().armor;
                defenderarmorpiercing = defender.GetComponent<MountedKnightBehaviour>().armorpiercing;
                break;
            case "HeroKing":
                defenderdmg = defender.GetComponent<HeroKingBehaviour>().attack;
                //defenderhealth = defender.GetComponent<HeroKingBehaviour> ().health;
                defenderlasthealth = defender.GetComponent<HeroKingBehaviour>().lasthealth;
                //defenderrange = defender.GetComponent<HeroKingBehaviour> ().range;
                defenderarmor = defender.GetComponent<HeroKingBehaviour>().armor;
                defenderarmorpiercing = defender.GetComponent<HeroKingBehaviour>().armorpiercing;
                break;
        }
	}

	void SetAttackerInfo(GameObject attacker, string selectedentity) {
        switch (cleanSelEntity)
        {
            //UNDEAD
            case "Zombie":
                {
                    attacker.GetComponent<ZombieBehaviour>().lasthealth = attackerlasthealth;
                    Text atthealthtext = GameObject.Find("Health " + selectedentity).GetComponent<Text>();
                    atthealthtext.text = attackerlasthealth.ToString();
                    attacker.GetComponent<ZombieBehaviour>().currattackpoint = attacker.GetComponent<ZombieBehaviour>().currattackpoint - 1;
                }
                break;
            case "Skeleton":
                {
                    attacker.GetComponent<SkeletonBehaviour>().lasthealth = attackerlasthealth;
                    Text atthealthtext = GameObject.Find("Health " + selectedentity).GetComponent<Text>();
                    atthealthtext.text = attackerlasthealth.ToString();
                    attacker.GetComponent<SkeletonBehaviour>().currattackpoint = attacker.GetComponent<SkeletonBehaviour>().currattackpoint - 1;
                }
                break;
            case "Necromancer":
                {
                    attacker.GetComponent<NecromancerBehaviour>().lasthealth = attackerlasthealth;
                    Text atthealthtext = GameObject.Find("Health " + selectedentity).GetComponent<Text>();
                    atthealthtext.text = attackerlasthealth.ToString();
                    attacker.GetComponent<NecromancerBehaviour>().currattackpoint = attacker.GetComponent<NecromancerBehaviour>().currattackpoint - 1;
                }
                break;
            case "SkeletonArcher":
                {
                    attacker.GetComponent<SkeletonArcherBehaviour>().lasthealth = attackerlasthealth;
                    Text atthealthtext = GameObject.Find("Health " + selectedentity).GetComponent<Text>();
                    atthealthtext.text = attackerlasthealth.ToString();
                    attacker.GetComponent<SkeletonArcherBehaviour>().currattackpoint = attacker.GetComponent<SkeletonArcherBehaviour>().currattackpoint - 1;
                }
                break;
            case "ArmoredSkeleton":
                {
                    attacker.GetComponent<ArmoredSkeletonBehaviour>().lasthealth = attackerlasthealth;
                    Text atthealthtext = GameObject.Find("Health " + selectedentity).GetComponent<Text>();
                    atthealthtext.text = attackerlasthealth.ToString();
                    attacker.GetComponent<ArmoredSkeletonBehaviour>().currattackpoint = attacker.GetComponent<ArmoredSkeletonBehaviour>().currattackpoint - 1;
                }
                break;
            case "DeathKnight":
                {
                    attacker.GetComponent<DeathKnightBehaviour>().lasthealth = attackerlasthealth;
                    Text atthealthtext = GameObject.Find("Health " + selectedentity).GetComponent<Text>();
                    atthealthtext.text = attackerlasthealth.ToString();
                    attacker.GetComponent<DeathKnightBehaviour>().currattackpoint = attacker.GetComponent<DeathKnightBehaviour>().currattackpoint - 1;
                }
                break;


            //HUMANS
            case "Militia":
                {
                    attacker.GetComponent<MilitiaBehaviour>().lasthealth = attackerlasthealth;
                    Text atthealthtext = GameObject.Find("Health " + selectedentity).GetComponent<Text>();
                    atthealthtext.text = attackerlasthealth.ToString();
                    attacker.GetComponent<MilitiaBehaviour>().currattackpoint = attacker.GetComponent<MilitiaBehaviour>().currattackpoint - 1;
                }
                break;
            case "Archer":
                {
                    attacker.GetComponent<ArcherBehaviour>().lasthealth = attackerlasthealth;
                    Text atthealthtext = GameObject.Find("Health " + selectedentity).GetComponent<Text>();
                    atthealthtext.text = attackerlasthealth.ToString();
                    attacker.GetComponent<ArcherBehaviour>().currattackpoint = attacker.GetComponent<ArcherBehaviour>().currattackpoint - 1;
                }
                break;
            case "Longbowman":
                {
                    attacker.GetComponent<LongbowmanBehaviour>().lasthealth = attackerlasthealth;
                    Text atthealthtext = GameObject.Find("Health " + selectedentity).GetComponent<Text>();
                    atthealthtext.text = attackerlasthealth.ToString();
                    attacker.GetComponent<LongbowmanBehaviour>().currattackpoint = attacker.GetComponent<LongbowmanBehaviour>().currattackpoint - 1;
                }
                break;
            case "Crossbowman":
                {
                    attacker.GetComponent<CrossbowmanBehaviour>().lasthealth = attackerlasthealth;
                    Text atthealthtext = GameObject.Find("Health " + selectedentity).GetComponent<Text>();
                    atthealthtext.text = attackerlasthealth.ToString();
                    attacker.GetComponent<CrossbowmanBehaviour>().currattackpoint = attacker.GetComponent<CrossbowmanBehaviour>().currattackpoint - 1;
                }
                break;
            case "Footman":
                {
                    attacker.GetComponent<FootmanBehaviour>().lasthealth = attackerlasthealth;
                    Text atthealthtext = GameObject.Find("Health " + selectedentity).GetComponent<Text>();
                    atthealthtext.text = attackerlasthealth.ToString();
                    attacker.GetComponent<FootmanBehaviour>().currattackpoint = attacker.GetComponent<FootmanBehaviour>().currattackpoint - 1;
                }
                break;
            case "MountedKnight":
                {
                    attacker.GetComponent<MountedKnightBehaviour>().lasthealth = attackerlasthealth;
                    Text atthealthtext = GameObject.Find("Health " + selectedentity).GetComponent<Text>();
                    atthealthtext.text = attackerlasthealth.ToString();
                    attacker.GetComponent<MountedKnightBehaviour>().currattackpoint = attacker.GetComponent<MountedKnightBehaviour>().currattackpoint - 1;
                }
                break;
            case "HeroKing":
                {
                    attacker.GetComponent<HeroKingBehaviour>().lasthealth = attackerlasthealth;
                    Text atthealthtext = GameObject.Find("Health " + selectedentity).GetComponent<Text>();
                    atthealthtext.text = attackerlasthealth.ToString();
                    attacker.GetComponent<HeroKingBehaviour>().currattackpoint = attacker.GetComponent<HeroKingBehaviour>().currattackpoint - 1;
                }
                break;
        }
	}

	void SetDefenderInfo(GameObject defender, string currEntity) {
        switch (cleanCurrEntity)
        {
            //UNDEAD
            case "Zombie":
                {
                    defender.GetComponent<ZombieBehaviour>().lasthealth = defenderlasthealth;
                    Text defhealthtext = GameObject.Find("Health " + currEntity).GetComponent<Text>();
                    defhealthtext.text = defenderlasthealth.ToString();
                }
                break;
            case "Skeleton":
                {
                    defender.GetComponent<SkeletonBehaviour>().lasthealth = defenderlasthealth;
                    Text defhealthtext = GameObject.Find("Health " + currEntity).GetComponent<Text>();
                    defhealthtext.text = defenderlasthealth.ToString();
                }
                break;
            case "Necromancer":
                {
                    defender.GetComponent<NecromancerBehaviour>().lasthealth = defenderlasthealth;
                    Text defhealthtext = GameObject.Find("Health " + currEntity).GetComponent<Text>();
                    defhealthtext.text = defenderlasthealth.ToString();
                }
                break;
            case "SkeletonArcher":
                {
                    defender.GetComponent<SkeletonArcherBehaviour>().lasthealth = defenderlasthealth;
                    Text defhealthtext = GameObject.Find("Health " + currEntity).GetComponent<Text>();
                    defhealthtext.text = defenderlasthealth.ToString();
                }
                break;
            case "ArmoredSkeleton":
                {
                    defender.GetComponent<ArmoredSkeletonBehaviour>().lasthealth = defenderlasthealth;
                    Text defhealthtext = GameObject.Find("Health " + currEntity).GetComponent<Text>();
                    defhealthtext.text = defenderlasthealth.ToString();
                }
                break;
            case "DeathKnight":
                {
                    defender.GetComponent<DeathKnightBehaviour>().lasthealth = defenderlasthealth;
                    Text defhealthtext = GameObject.Find("Health " + currEntity).GetComponent<Text>();
                    defhealthtext.text = defenderlasthealth.ToString();
                }
                break;


            //HUMANS
            case "Militia":
                {
                    defender.GetComponent<MilitiaBehaviour>().lasthealth = defenderlasthealth;
                    Text defhealthtext = GameObject.Find("Health " + currEntity).GetComponent<Text>();
                    defhealthtext.text = defenderlasthealth.ToString();
                }
                break;
            case "Archer":
                {
                    defender.GetComponent<ArcherBehaviour>().lasthealth = defenderlasthealth;
                    Text defhealthtext = GameObject.Find("Health " + currEntity).GetComponent<Text>();
                    defhealthtext.text = defenderlasthealth.ToString();
                }
                break;
            case "Longbowman":
                {
                    defender.GetComponent<LongbowmanBehaviour>().lasthealth = defenderlasthealth;
                    Text defhealthtext = GameObject.Find("Health " + currEntity).GetComponent<Text>();
                    defhealthtext.text = defenderlasthealth.ToString();
                }
                break;
            case "Crossbowman":
                {
                    defender.GetComponent<CrossbowmanBehaviour>().lasthealth = defenderlasthealth;
                    Text defhealthtext = GameObject.Find("Health " + currEntity).GetComponent<Text>();
                    defhealthtext.text = defenderlasthealth.ToString();
                }
                break;
            case "Footman":
                {
                    defender.GetComponent<FootmanBehaviour>().lasthealth = defenderlasthealth;
                    Text defhealthtext = GameObject.Find("Health " + currEntity).GetComponent<Text>();
                    defhealthtext.text = defenderlasthealth.ToString();
                }
                break;
            case "MountedKnight":
                {
                    defender.GetComponent<MountedKnightBehaviour>().lasthealth = defenderlasthealth;
                    Text defhealthtext = GameObject.Find("Health " + currEntity).GetComponent<Text>();
                    defhealthtext.text = defenderlasthealth.ToString();
                }
                break;
            case "HeroKing":
                {
                    defender.GetComponent<HeroKingBehaviour>().lasthealth = defenderlasthealth;
                    Text defhealthtext = GameObject.Find("Health " + currEntity).GetComponent<Text>();
                    defhealthtext.text = defenderlasthealth.ToString();
                }
                break;
        }
	}


    //find movement points
    void GetMovementInfo(GameObject playerEntity) {
        switch (cleanSelEntity)
        {
            //UNDEAD
            case "Zombie":
                playermovepoint = playerEntity.GetComponent<ZombieBehaviour>().movementpoint;
                playercurrmovepoint = playerEntity.GetComponent<ZombieBehaviour>().currmovementpoint;
                break;
            case "Skeleton":
                playermovepoint = playerEntity.GetComponent<SkeletonBehaviour>().movementpoint;
                playercurrmovepoint = playerEntity.GetComponent<SkeletonBehaviour>().currmovementpoint;
                break;
            case "Necromancer":
                playermovepoint = playerEntity.GetComponent<NecromancerBehaviour>().movementpoint;
                playercurrmovepoint = playerEntity.GetComponent<NecromancerBehaviour>().currmovementpoint;
                break;
            case "SkeletonArcher":
                playermovepoint = playerEntity.GetComponent<SkeletonArcherBehaviour>().movementpoint;
                playercurrmovepoint = playerEntity.GetComponent<SkeletonArcherBehaviour>().currmovementpoint;
                break;
            case "ArmoredSkeleton":
                playermovepoint = playerEntity.GetComponent<ArmoredSkeletonBehaviour>().movementpoint;
                playercurrmovepoint = playerEntity.GetComponent<ArmoredSkeletonBehaviour>().currmovementpoint;
                break;
            case "DeathKnight":
                playermovepoint = playerEntity.GetComponent<DeathKnightBehaviour>().movementpoint;
                playercurrmovepoint = playerEntity.GetComponent<DeathKnightBehaviour>().currmovementpoint;
                break;


            //HUMANS
            case "Militia":
                playermovepoint = playerEntity.GetComponent<MilitiaBehaviour>().movementpoint;
                playercurrmovepoint = playerEntity.GetComponent<MilitiaBehaviour>().currmovementpoint;
                break;
            case "Archer":
                playermovepoint = playerEntity.GetComponent<ArcherBehaviour>().movementpoint;
                playercurrmovepoint = playerEntity.GetComponent<ArcherBehaviour>().currmovementpoint;
                break;
            case "Longbowman":
                playermovepoint = playerEntity.GetComponent<LongbowmanBehaviour>().movementpoint;
                playercurrmovepoint = playerEntity.GetComponent<LongbowmanBehaviour>().currmovementpoint;
                break;
            case "Crossbowman":
                playermovepoint = playerEntity.GetComponent<CrossbowmanBehaviour>().movementpoint;
                playercurrmovepoint = playerEntity.GetComponent<CrossbowmanBehaviour>().currmovementpoint;
                break;
            case "Footman":
                playermovepoint = playerEntity.GetComponent<FootmanBehaviour>().movementpoint;
                playercurrmovepoint = playerEntity.GetComponent<FootmanBehaviour>().currmovementpoint;
                break;
            case "MountedKnight":
                playermovepoint = playerEntity.GetComponent<MountedKnightBehaviour>().movementpoint;
                playercurrmovepoint = playerEntity.GetComponent<MountedKnightBehaviour>().currmovementpoint;
                break;
            case "HeroKing":
                playermovepoint = playerEntity.GetComponent<HeroKingBehaviour>().movementpoint;
                playercurrmovepoint = playerEntity.GetComponent<HeroKingBehaviour>().currmovementpoint;
                break;
        }
	}

    //set new movement points
    void SetMovementPoints(GameObject playerEntity, int change) {
        switch (cleanSelEntity)
        {
            //UNDEAD
            case "Zombie":
                playerEntity.GetComponent<ZombieBehaviour>().currmovementpoint = playerEntity.GetComponent<ZombieBehaviour>().currmovementpoint - change;
                break;
            case "Skeleton":
                playerEntity.GetComponent<SkeletonBehaviour>().currmovementpoint = playerEntity.GetComponent<SkeletonBehaviour>().currmovementpoint - change;
                break;
            case "Necromancer":
                playerEntity.GetComponent<NecromancerBehaviour>().currmovementpoint = playerEntity.GetComponent<NecromancerBehaviour>().currmovementpoint - change;
                break;
            case "SkeletonArcher":
                playerEntity.GetComponent<SkeletonArcherBehaviour>().currmovementpoint = playerEntity.GetComponent<SkeletonArcherBehaviour>().currmovementpoint - change;
                break;
            case "ArmoredSkeleton":
                playerEntity.GetComponent<ArmoredSkeletonBehaviour>().currmovementpoint = playerEntity.GetComponent<ArmoredSkeletonBehaviour>().currmovementpoint - change;
                break;
            case "DeathKnight":
                playerEntity.GetComponent<DeathKnightBehaviour>().currmovementpoint = playerEntity.GetComponent<DeathKnightBehaviour>().currmovementpoint - change;
                break;


            //HUMANS
            case "Militia":
                playerEntity.GetComponent<MilitiaBehaviour>().currmovementpoint = playerEntity.GetComponent<MilitiaBehaviour>().currmovementpoint - change;
                break;
            case "Archer":
                playerEntity.GetComponent<ArcherBehaviour>().currmovementpoint = playerEntity.GetComponent<ArcherBehaviour>().currmovementpoint - change;
                break;
            case "Longbowman":
                playerEntity.GetComponent<LongbowmanBehaviour>().currmovementpoint = playerEntity.GetComponent<LongbowmanBehaviour>().currmovementpoint - change;
                break;
            case "Crossbowman":
                playerEntity.GetComponent<CrossbowmanBehaviour>().currmovementpoint = playerEntity.GetComponent<CrossbowmanBehaviour>().currmovementpoint - change;
                break;
            case "Footman":
                playerEntity.GetComponent<FootmanBehaviour>().currmovementpoint = playerEntity.GetComponent<FootmanBehaviour>().currmovementpoint - change;
                break;
            case "MountedKnight":
                playerEntity.GetComponent<MountedKnightBehaviour>().currmovementpoint = playerEntity.GetComponent<MountedKnightBehaviour>().currmovementpoint - change;
                break;
            case "HeroKing":
                playerEntity.GetComponent<HeroKingBehaviour>().currmovementpoint = playerEntity.GetComponent<HeroKingBehaviour>().currmovementpoint - change;
                break;
        }
	}

	void CalcSouls(string faction, string diedentity) {
		if (faction == "undead") {
            switch (diedentity)
            {
                //HUMANS
                case "Militia":
                    currency.ChangeSouls(100);
                    break;
                case "Archer":
                    currency.ChangeSouls(150);
                    break;
                case "Longbowman":
                    currency.ChangeSouls(200);
                    break;
                case "Crossbowman":
                    currency.ChangeSouls(150);
                    break;
                case "Footman":
                    currency.ChangeSouls(150);
                    break;
                case "MountedKnight":
                    currency.ChangeSouls(200);
                    break;
                case "HeroKing":
                    currency.ChangeSouls(1000);
                    break;
            }
		}
	}
}
