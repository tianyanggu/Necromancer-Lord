using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class AIBehaviour : MonoBehaviour {

	public Movement movement;
	public HexGrid hexGrid;

	private List<string> playerEntities = new List<string> ();
	private List<string> enemyEntities = new List<string> ();

	private List<string> nearbyPlayerEntities = new List<string> ();
	private List<int> nearbyPlayerEntitiesDistance = new List<int> ();
	private List<int> nearbyPlayerEntitiesSize = new List<int> ();

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

	private int attackercurrattpoint = 0;

	// Use this for initialization
	public List<string> ScanEntities (int index) {
		playerEntities.Add ("Necromancer");
		playerEntities.Add ("Skeleton");
		playerEntities.Add ("Zombie");

		ScanEntitiesHelper (index, 3, 0);
		List<string> scan = nearbyPlayerEntities;

		return scan;
	}

	public void ScanEntitiesHelper (int index, int maxDistance, int usedDistance) {
		if (maxDistance > 0) {
			HexCoordinates coord = hexGrid.GetCellCoord (index);
			int coordx = coord.X;
			int coordz = coord.Z;

			int left = hexGrid.GetCellIndexFromCoord (coordx - 1, coordz);
			string leftEntity = hexGrid.GetEntity(index);
			string cleanleftEntity = Regex.Replace(leftEntity, @"[\d-]", string.Empty);
			if (playerEntities.Contains(cleanleftEntity)) {
				nearbyPlayerEntities.Add (leftEntity);
				nearbyPlayerEntitiesDistance.Add (usedDistance + 1);
			}

			int right = hexGrid.GetCellIndexFromCoord (coordx + 1, coordz);
			string rightEntity = hexGrid.GetEntity(index);
			string cleanrightEntity = Regex.Replace(rightEntity, @"[\d-]", string.Empty);
			if (playerEntities.Contains(cleanrightEntity)) {
				nearbyPlayerEntities.Add (rightEntity);
				nearbyPlayerEntitiesDistance.Add (usedDistance + 1);
			}

			int uleft = hexGrid.GetCellIndexFromCoord (coordx - 1, coordz + 1);
			string uleftEntity = hexGrid.GetEntity(index);
			string cleanuleftEntity = Regex.Replace(uleftEntity, @"[\d-]", string.Empty);
			if (playerEntities.Contains(cleanuleftEntity)) {
				nearbyPlayerEntities.Add (uleftEntity);
				nearbyPlayerEntitiesDistance.Add (usedDistance + 1);
			}

			int uright = hexGrid.GetCellIndexFromCoord (coordx, coordz + 1);
			string urightEntity = hexGrid.GetEntity(index);
			string cleanurightEntity = Regex.Replace(urightEntity, @"[\d-]", string.Empty);
			if (playerEntities.Contains(cleanurightEntity)) {
				nearbyPlayerEntities.Add (urightEntity);
				nearbyPlayerEntitiesDistance.Add (usedDistance + 1);
			}

			int lleft = hexGrid.GetCellIndexFromCoord (coordx, coordz - 1);
			string lleftEntity = hexGrid.GetEntity(index);
			string cleanlleftEntity = Regex.Replace(lleftEntity, @"[\d-]", string.Empty);
			if (playerEntities.Contains(cleanlleftEntity)) {
				nearbyPlayerEntities.Add (lleftEntity);
				nearbyPlayerEntitiesDistance.Add (usedDistance + 1);
			}

			int lright = hexGrid.GetCellIndexFromCoord (coordx + 1, coordz - 1);
			string lrightEntity = hexGrid.GetEntity(index);
			string cleanlrightEntity = Regex.Replace(lrightEntity, @"[\d-]", string.Empty);
			if (playerEntities.Contains(cleanlrightEntity)) {
				nearbyPlayerEntities.Add (lrightEntity);
				nearbyPlayerEntitiesDistance.Add (usedDistance + 1);
			}

			//left and right 
			if (left >= 0) {
				if (hexGrid.GetEntity (left) == "Empty") {
					if (hexGrid.GetTerrain (left) == "Mountain") {
						int newmovementpoints = maxDistance - 2;
						int newusedmovementpoints = usedDistance + 2;
						ScanEntitiesHelper (left, newmovementpoints, newusedmovementpoints);
					} else {
						int newmovementpoints = maxDistance - 1;
						int newusedmovementpoints = usedDistance + 1;
						ScanEntitiesHelper (left, newmovementpoints, newusedmovementpoints);
					}
				}
			}
			if (right >= 0) {
				if (hexGrid.GetEntity (right) == "Empty") {
					if (hexGrid.GetTerrain (right) == "Mountain") {
						int newmovementpoints = maxDistance - 2;
						int newusedmovementpoints = usedDistance + 2;
						ScanEntitiesHelper (right, newmovementpoints, newusedmovementpoints);
					} else {
						int newmovementpoints = maxDistance - 1;
						int newusedmovementpoints = usedDistance + 1;
						ScanEntitiesHelper (right, newmovementpoints, newusedmovementpoints);
					}
				}
			}

			//upper left and right
			if (uleft >= 0) {
				if (hexGrid.GetEntity (uleft) == "Empty") {
					if (hexGrid.GetTerrain (uleft) == "Mountain") {
						int newmovementpoints = maxDistance - 2;
						int newusedmovementpoints = usedDistance + 2;
						ScanEntitiesHelper (uleft, newmovementpoints, newusedmovementpoints);
					} else {
						int newmovementpoints = maxDistance - 1;
						int newusedmovementpoints = usedDistance + 1;
						ScanEntitiesHelper (uleft, newmovementpoints, newusedmovementpoints);
					}
				}
			}
			if (uright >= 0) {
				if (hexGrid.GetEntity (uright) == "Empty") {
					if (hexGrid.GetTerrain (uright) == "Mountain") {
						int newmovementpoints = maxDistance - 2;
						int newusedmovementpoints = usedDistance + 2;
						ScanEntitiesHelper (uright, newmovementpoints, newusedmovementpoints);
					} else {
						int newmovementpoints = maxDistance - 1;
						int newusedmovementpoints = usedDistance + 1;
						ScanEntitiesHelper (uright, newmovementpoints, newusedmovementpoints);
					}
				}
			}

			//lower left and right
			if (lleft >= 0) {
				if (hexGrid.GetEntity (lleft) == "Empty") {
					if (hexGrid.GetTerrain (lleft) == "Mountain") {
						int newmovementpoints = maxDistance - 2;
						int newusedmovementpoints = usedDistance + 2;
						ScanEntitiesHelper (lleft, newmovementpoints, newusedmovementpoints);
					} else {
						int newmovementpoints = maxDistance - 1;
						int newusedmovementpoints = usedDistance + 1;
						ScanEntitiesHelper (lleft, newmovementpoints, newusedmovementpoints);
					}
				}
			}
			if (lright >= 0) {
				if (hexGrid.GetEntity (lright) == "Empty") {
					if (hexGrid.GetTerrain (lright) == "Mountain") {
						int newmovementpoints = maxDistance - 2;
						int newusedmovementpoints = usedDistance + 2;
						ScanEntitiesHelper (lright, newmovementpoints, newusedmovementpoints);
					} else {
						int newmovementpoints = maxDistance - 1;
						int newusedmovementpoints = usedDistance + 1;
						ScanEntitiesHelper (lright, newmovementpoints, newusedmovementpoints);
					}
				}
			}
		}
	}
		
	// given list of player entities and their size, decide if attack and which
	public void DecideAttack (int eindex, List<int> plist, List<int> psize) {
		enemyEntities.Add ("Militia");
		string eEntity = hexGrid.GetEntity(eindex);
		//TODO DecideAttack
	}

	// attack player entity on pindex
	public void Attack (int eindex, int pindex) {
		enemyEntities.Add ("Militia");
		playerEntities.Add ("Necromancer");
		playerEntities.Add ("Skeleton");
		playerEntities.Add ("Zombie");

		string pEntity = hexGrid.GetEntity(pindex);
		string cleanpEntity = Regex.Replace(pEntity, @"[\d-]", string.Empty);
		string eEntity = hexGrid.GetEntity(eindex);
		string cleaneEntity = Regex.Replace(eEntity, @"[\d-]", string.Empty);
		Vector3 cellcoord = hexGrid.GetCellPos(pindex);

		GameObject attacker = GameObject.Find (eEntity);
		GameObject defender = GameObject.Find (pEntity);

		GetAttackerInfo (cleaneEntity);
		GetDefenderInfo (cleanpEntity);

		int unitsdied = hexGrid.GetCorpses(eindex);
		int oldattackersize = attackersize;

		if (attackercurrattpoint == 0) {
			//do nothing
		} else {
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
					int addcorpses = oldattackersize - attackersize;
					unitsdied = unitsdied + addcorpses;
					hexGrid.CorpsesCellIndex (eindex, unitsdied);

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
					//set zero new corpses since ranged
					int defenderhealthmod = defenderhealthtotal % defenderhealth;
					if (defenderhealthmod == 0) {
						defenderlasthealth = defenderhealth;
					} else {
						defenderlasthealth = defenderhealthmod;
					}
				}

				if (defendersize <= 0) {
					Destroy (defender);
					hexGrid.EntityCellIndex (eindex, "Empty");
					GameObject defenderSizeText = GameObject.Find ("Size " + pEntity);
					Destroy (defenderSizeText);
				}
				if (attackersize <= 0) {
					Destroy (attacker);
					hexGrid.EntityCellIndex (eindex, "Empty");
					GameObject attackerSizeText = GameObject.Find ("Size " + eEntity);
					Destroy (attackerSizeText);
				} else if (attackersize > 0 && defendersize <= 0) {
					attacker.transform.position = cellcoord;
					hexGrid.EntityCellIndex (pindex, eEntity);
					hexGrid.EntityCellIndex (eindex, "Empty");
					GameObject defenderSizeText = GameObject.Find ("Size " + pEntity);
					Destroy (defenderSizeText);
					GameObject attackerSizeText = GameObject.Find ("Size " + eEntity);
					attackerSizeText.transform.position = new Vector3 (cellcoord.x, cellcoord.y + 0.1f, cellcoord.z);
				}

				//------Set New Info Defender------
				if (cleanpEntity == "Necromancer") {
					defender.GetComponent<NecromancerBehaviour> ().size = defendersize;
					defender.GetComponent<NecromancerBehaviour> ().lasthealth = defenderlasthealth;
					Text defsizetext = GameObject.Find ("Size " + eEntity).GetComponent<Text> ();
					defsizetext.text = attackersize.ToString ();
				} else if (cleanpEntity == "Skeleton") {
					defender.GetComponent<SkeletonBehaviour> ().size = defendersize;
					defender.GetComponent<SkeletonBehaviour> ().lasthealth = defenderlasthealth;
					Text defsizetext = GameObject.Find ("Size " + eEntity).GetComponent<Text> ();
					defsizetext.text = defendersize.ToString ();
				} else if (cleanpEntity == "Zombie") {
					defender.GetComponent<ZombieBehaviour> ().size = defendersize;
					defender.GetComponent<ZombieBehaviour> ().lasthealth = defenderlasthealth;
					Text defsizetext = GameObject.Find ("Size " + eEntity).GetComponent<Text> ();
					defsizetext.text = defendersize.ToString ();
				}

				//------Set New Info Defender------
				if (cleaneEntity == "Militia") {
					attacker.GetComponent<MilitiaBehaviour> ().size = attackersize;
					attacker.GetComponent<MilitiaBehaviour> ().lasthealth = attackerlasthealth;
					Text attsizetext = GameObject.Find ("Size " + pEntity).GetComponent<Text> ();
					attsizetext.text = attackersize.ToString ();
					attacker.GetComponent<MilitiaBehaviour> ().currattackpoint = attacker.GetComponent<MilitiaBehaviour> ().currattackpoint - 1;
				}
			}
		}
	}

	void GetDefenderInfo(string pEntity) {
		GameObject defender = GameObject.Find (pEntity);
		string cleanpEntity = Regex.Replace(pEntity, @"[\d-]", string.Empty);

		//------Grab Info Defender------
		if (cleanpEntity == "Necromancer") {
			defendersize = defender.GetComponent<NecromancerBehaviour> ().size;
			defenderdmg = defender.GetComponent<NecromancerBehaviour> ().attack;
			defenderhealth = defender.GetComponent<NecromancerBehaviour> ().health;
			defenderlasthealth = defender.GetComponent<NecromancerBehaviour> ().lasthealth;
		} else if (cleanpEntity == "Skeleton") {
			defendersize = defender.GetComponent<SkeletonBehaviour> ().size;
			defenderdmg = defender.GetComponent<SkeletonBehaviour> ().attack;
			defenderhealth = defender.GetComponent<SkeletonBehaviour> ().health;
			defenderlasthealth = defender.GetComponent<SkeletonBehaviour> ().lasthealth;
		} else if (cleanpEntity == "Zombie") {
			defendersize = defender.GetComponent<ZombieBehaviour> ().size;
			defenderdmg = defender.GetComponent<ZombieBehaviour> ().attack;
			defenderhealth = defender.GetComponent<ZombieBehaviour> ().health;
			defenderlasthealth = defender.GetComponent<ZombieBehaviour> ().lasthealth;
		}
	}

	void GetAttackerInfo(string eEntity) {
		GameObject attacker = GameObject.Find (eEntity);
		string cleaneEntity = Regex.Replace(eEntity, @"[\d-]", string.Empty);

		//------Grab Info Attacker------
		if (cleaneEntity == "Militia") {
			attackersize = attacker.GetComponent<MilitiaBehaviour> ().size;
			attackerdmg = attacker.GetComponent<MilitiaBehaviour> ().attack;
			attackerhealth = attacker.GetComponent<MilitiaBehaviour> ().health;
			attackerlasthealth = attacker.GetComponent<MilitiaBehaviour> ().lasthealth;
			attackerrange = attacker.GetComponent<MilitiaBehaviour> ().range;
			attackercurrattpoint = attacker.GetComponent<MilitiaBehaviour> ().currattackpoint;
		}
	}
}
