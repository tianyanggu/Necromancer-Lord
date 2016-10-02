﻿using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class Battle : MonoBehaviour {

	public HexGrid hexGrid;
	public Movement movement;
	public EntityStorage entityStorage;
	public Currency currency;
    public Summon summon;
    public Build build;
    public EntityStats entityStats;

    //entity stats
    private int attackerdmg = 0;
	private int attackercurrhealth = 0;
	private int attackerrange = 0;
	private int attackerrangedmg = 0;
	private int attackerarmor = 0;
	private int attackerarmorpiercing = 0;
	private int defenderdmg = 0;
	private int defendercurrhealth = 0;
	private int defenderarmor = 0;
	private int defenderarmorpiercing = 0;

	//entity action points
	private int attackermovepoint = 0;
	private int attackercurrmovepoint = 0;
    private int attackercurrattpoint = 0;

	public bool Attack (int selectedindex, int currindex) {
        //------Parses Entities------
        string selectedEntityName = hexGrid.GetEntityName(selectedindex);
        GameObject selectedEntity = hexGrid.GetEntityObject(selectedindex);
        if (selectedEntityName == "Empty") {
			return false;
		}
		string currEntityName = hexGrid.GetEntityName(currindex);
        GameObject currEntity = hexGrid.GetEntityObject(currindex);

        //get coordinates of currEntity
        Vector3 cellcoord = hexGrid.GetCellPos(currindex);

		string storedNameSel = PlayerPrefs.GetString (selectedEntityName);
		string numSelEntity = Regex.Replace(storedNameSel, "[^0-9 -]", string.Empty);
		string storedNameCurr = PlayerPrefs.GetString (currEntityName);
		string numCurrEntity = Regex.Replace(storedNameCurr, "[^0-9 -]", string.Empty);

		//string selFaction = entityStorage.WhichFaction(cleanSelEntity);
		//string currFaction = entityStorage.WhichFaction(cleanCurrEntity);

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
			if (attackercurrmovepoint == 0) {
				return false;
			}
			else if (attackermovepoint == 1 && attackercurrmovepoint == 1) {
				possmovement = movement.GetCellIndexesOneHexAway (selectedindex);
			} else if (attackermovepoint != 1) {
				possmovement = movement.GetCellIndexesBlockers (selectedindex, attackercurrmovepoint);
			}

			if (possmovement.Contains (currindex)) {
				//get min movement points used
				if (attackermovepoint == 1 && attackercurrmovepoint == 1) {
					SetMovementPoints (selectedEntity, 1);
				} else if (attackermovepoint != 1) {
					int minmove = movement.GetMovementPointsUsed (selectedindex, currindex, attackercurrmovepoint);
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
			if (attackercurrattpoint == 0) {
                return false;
            }
			List<int> possattacktiles = null;

			//------Determine Attack Range------
            //TODO attacker range 3+ tiles away
			if (attackerrange == 1) {
				possattacktiles = movement.GetCellIndexesOneHexAway (selectedindex);
			} else if (attackerrange >= 2) {
				possattacktiles = movement.GetCellIndexesTwoHexAway (selectedindex);
            }

            //------Calc Defender New Health-------
            if (possattacktiles.Contains (currindex)) {
				if (defendercurrhealth > 0) {
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
						defendercurrhealth = defendercurrhealth - totalattackerdmg;
                        attackercurrhealth = attackercurrhealth - totaldefenderdmg;

						PlayerPrefs.SetInt ("HexEntityHealth" + numCurrEntity, defendercurrhealth);
						PlayerPrefs.SetInt ("HexEntityHealth" + numSelEntity, attackercurrhealth);
					//range attack
					} else if (attackerrange >= 2) {
                        //armor piercing damage is minimum of armor or piercing damage
                        int attackerpierceddmg = Mathf.Min(defenderarmor, attackerarmorpiercing);
						//calc dmg to defender health
						int totalattackerrangedmg = attackerrangedmg - defenderarmor + attackerpierceddmg;
						if (totalattackerrangedmg < 1) {
							totalattackerrangedmg = 1;
						}
                        defendercurrhealth = defendercurrhealth - totalattackerrangedmg;

						PlayerPrefs.SetInt ("HexEntityHealth" + numCurrEntity, defendercurrhealth);
					}

					//check new status
					if (defendercurrhealth <= 0) {
                        summon.KillEntity(currindex);
					}
					if (attackercurrhealth <= 0) {
                        summon.KillEntity(selectedindex);
                    } 
					if (attackercurrhealth > 0 && defendercurrhealth <= 0) {
                        //move to defender's position if have enough movement points and is not ranged unit
                        int minmove = movement.GetMovementPointsUsed (selectedindex, currindex, attackercurrmovepoint);
						if (attackercurrmovepoint >= minmove && attackerrange == 1) {
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
        attackercurrattpoint = entityStats.GetCurrAttackPoint(attacker);
        attackerdmg = entityStats.GetCurrAttackDmg(attacker);
        attackerrangedmg = entityStats.GetCurrRangedAttackDmg(attacker);
        attackercurrhealth = entityStats.GetCurrHealth(attacker);
        attackerrange = entityStats.GetCurrRange(attacker);
        attackerarmor = entityStats.GetCurrArmor(attacker);
        attackerarmorpiercing = entityStats.GetCurrArmorPiercing(attacker);
    }

	void GetDefenderInfo(GameObject defender) {
        defenderdmg = entityStats.GetCurrAttackDmg(defender);
        defendercurrhealth = entityStats.GetCurrHealth(defender);
        defenderarmor = entityStats.GetCurrArmor(defender);
        defenderarmorpiercing = entityStats.GetCurrArmorPiercing(defender);
    }

	void SetAttackerInfo(GameObject attacker, string selectedentity) {
        entityStats.SetCurrHealth(attacker, attackercurrhealth);
        Text atthealthtext = GameObject.Find("Health " + selectedentity).GetComponent<Text>();
        atthealthtext.text = attackercurrhealth.ToString();
        entityStats.SetCurrAttackPoint(attacker, entityStats.GetCurrAttackPoint(attacker) - 1);
	}

	void SetDefenderInfo(GameObject defender, string currEntity) {
        entityStats.SetCurrHealth(defender, defendercurrhealth);
        Text defhealthtext = GameObject.Find("Health " + currEntity).GetComponent<Text>();
        defhealthtext.text = defendercurrhealth.ToString();
	}


    //find movement points
    void GetMovementInfo(GameObject attacker) {
        attackermovepoint = entityStats.GetCurrMaxMovementPoint(attacker);
        attackercurrmovepoint = entityStats.GetCurrMovementPoint(attacker);
	}

    //set new movement points
    void SetMovementPoints(GameObject attacker, int change) {
        entityStats.SetCurrMovementPoint(attacker, attackercurrmovepoint - change);
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
