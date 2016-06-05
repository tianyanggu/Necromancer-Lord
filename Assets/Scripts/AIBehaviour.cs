using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class AIBehaviour : MonoBehaviour {

	public Movement movement;
	public HexGrid hexGrid;
	public EntityStorage entityStorage;

	private List<string> nearbyPlayerEntities = new List<string> ();
	private List<int> nearbyPlayerEntitiesIndex = new List<int> ();
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

	private int attackermovepoint = 0;
	private int attackercurrattpoint = 0;
	private int attackercurrmovepoint = 0;

	//eindex is the current enemy entity that scans for player entities within movement range of it
	public List<string> ScanEntities (int eindex) {
		string eEntity = hexGrid.GetEntity(eindex);
		GetAttackerInfo (eEntity);

		ScanEntitiesHelper (eindex, attackermovepoint, 0);
		List<string> scan = nearbyPlayerEntities;

		return scan;
	}

	public void ScanEntitiesHelper (int index, int maxDistance, int usedDistance) {
		if (maxDistance > 0) {
			HexCoordinates coord = hexGrid.GetCellCoord (index);
			int coordx = coord.X;
			int coordz = coord.Z;

			int left = hexGrid.GetCellIndexFromCoord (coordx - 1, coordz);
			int right = hexGrid.GetCellIndexFromCoord (coordx + 1, coordz);
			int uleft = hexGrid.GetCellIndexFromCoord (coordx - 1, coordz + 1);
			int uright = hexGrid.GetCellIndexFromCoord (coordx, coordz + 1);
			int lleft = hexGrid.GetCellIndexFromCoord (coordx, coordz - 1);
			int lright = hexGrid.GetCellIndexFromCoord (coordx + 1, coordz - 1);
			int[] hexdirections = new int[] { left, right, uleft, uright, lleft, lright };

			foreach (int direction in hexdirections) {
				string dirEntity = hexGrid.GetEntity (direction);
				string cleandirEntity = Regex.Replace (dirEntity, @"[\d-]", string.Empty);
				if (entityStorage.playerEntities.Contains (cleandirEntity)) {
					nearbyPlayerEntities.Add (dirEntity);
					nearbyPlayerEntitiesIndex.Add (direction);
					nearbyPlayerEntitiesDistance.Add (usedDistance + 1);
					int playerEntitySize = GetPlayerEntitySize (dirEntity);
					nearbyPlayerEntitiesSize.Add (playerEntitySize);
				}
			}

			foreach (int direction in hexdirections) {
				if (direction >= 0) {
					if (hexGrid.GetEntity (direction) == "Empty") {
						if (hexGrid.GetTerrain (direction) == "Mountain" && maxDistance > 1) {
							int newmovementpoints = maxDistance - 2;
							int newusedmovementpoints = usedDistance + 2;
							ScanEntitiesHelper (direction, newmovementpoints, newusedmovementpoints);
						} else if (hexGrid.GetTerrain (direction) != "Mountain") {
							int newmovementpoints = maxDistance - 1;
							int newusedmovementpoints = usedDistance + 1;
							ScanEntitiesHelper (direction, newmovementpoints, newusedmovementpoints);
						}
					}
				}
			}
		}
	}

	// given list of player entities and their size, decide if attack and which
	public void DecideAttack (int eindex, List<int> plist, List<int> pindex, List<int> pdist, List<int> psize) {
		string eEntity = hexGrid.GetEntity(eindex);
		foreach (int index in pindex) {
			//if can attack enemy
			availableAttackIndexes (index);
		}

		//TODO DecideAttack
	}

	public List<int> availableAttackIndexes (int pindex) {
		List <int> availableIndex = new List<int> ();
		HexCoordinates coord = hexGrid.GetCellCoord (pindex);
		
		int coordx = coord.X;
		int coordz = coord.Z;

		int left = hexGrid.GetCellIndexFromCoord (coordx - 1, coordz);
		int right = hexGrid.GetCellIndexFromCoord (coordx + 1, coordz);
		int uleft = hexGrid.GetCellIndexFromCoord (coordx - 1, coordz + 1);
		int uright = hexGrid.GetCellIndexFromCoord (coordx, coordz + 1);
		int lleft = hexGrid.GetCellIndexFromCoord (coordx, coordz - 1);
		int lright = hexGrid.GetCellIndexFromCoord (coordx + 1, coordz - 1);
		int[] hexdirections = new int[] { left, right, uleft, uright, lleft, lright };

		foreach (int direction in hexdirections) {
			string dirEntity = hexGrid.GetEntity (direction);
			if (dirEntity == "Empty") {
				availableIndex.Add (direction);
			}
		}
		return availableIndex;
	}



	// attack player entity on pindex
	public void Attack (int eindex, int pindex) {
		string pEntity = hexGrid.GetEntity(pindex);
		string cleanpEntity = Regex.Replace(pEntity, @"[\d-]", string.Empty);
		string eEntity = hexGrid.GetEntity(eindex);
		string cleaneEntity = Regex.Replace(eEntity, @"[\d-]", string.Empty);
		Vector3 cellcoord = hexGrid.GetCellPos(pindex);

		GameObject attacker = GameObject.Find (eEntity);
		GameObject defender = GameObject.Find (pEntity);

		GetAttackerInfo (eEntity);
		GetDefenderInfo (pEntity);

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
					entityStorage.RemoveActivePlayerEntity (pEntity);
				}
				if (attackersize <= 0) {
					Destroy (attacker);
					hexGrid.EntityCellIndex (eindex, "Empty");
					GameObject attackerSizeText = GameObject.Find ("Size " + eEntity);
					Destroy (attackerSizeText);
					entityStorage.RemoveActiveEnemyEntity (eEntity);
				} else if (attackersize > 0 && defendersize <= 0) {
					attacker.transform.position = cellcoord;
					hexGrid.EntityCellIndex (pindex, eEntity);
					hexGrid.EntityCellIndex (eindex, "Empty");
					GameObject defenderSizeText = GameObject.Find ("Size " + pEntity);
					Destroy (defenderSizeText);
					GameObject attackerSizeText = GameObject.Find ("Size " + eEntity);
					attackerSizeText.transform.position = new Vector3 (cellcoord.x, cellcoord.y + 0.1f, cellcoord.z);
				}

				SetDefenderInfo (pEntity);
				SetAttackerInfo (eEntity);
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
			attackermovepoint = attacker.GetComponent<MilitiaBehaviour> ().movementpoint;
			attackercurrmovepoint = attacker.GetComponent<MilitiaBehaviour> ().currmovementpoint;
		}
	}

	void SetDefenderInfo(string pEntity) {
		GameObject defender = GameObject.Find (pEntity);
		string cleanpEntity = Regex.Replace(pEntity, @"[\d-]", string.Empty);

		//------Set New Info Defender------
		if (cleanpEntity == "Necromancer") {
			defender.GetComponent<NecromancerBehaviour> ().size = defendersize;
			defender.GetComponent<NecromancerBehaviour> ().lasthealth = defenderlasthealth;
			Text defsizetext = GameObject.Find ("Size " + pEntity).GetComponent<Text> ();
			defsizetext.text = attackersize.ToString ();
		} else if (cleanpEntity == "Skeleton") {
			defender.GetComponent<SkeletonBehaviour> ().size = defendersize;
			defender.GetComponent<SkeletonBehaviour> ().lasthealth = defenderlasthealth;
			Text defsizetext = GameObject.Find ("Size " + pEntity).GetComponent<Text> ();
			defsizetext.text = defendersize.ToString ();
		} else if (cleanpEntity == "Zombie") {
			defender.GetComponent<ZombieBehaviour> ().size = defendersize;
			defender.GetComponent<ZombieBehaviour> ().lasthealth = defenderlasthealth;
			Text defsizetext = GameObject.Find ("Size " + pEntity).GetComponent<Text> ();
			defsizetext.text = defendersize.ToString ();
		}
	}

	void SetAttackerInfo(string eEntity) {
		GameObject attacker = GameObject.Find (eEntity);
		string cleaneEntity = Regex.Replace(eEntity, @"[\d-]", string.Empty);

		//------Set New Info Attacker------
		if (cleaneEntity == "Militia") {
			attacker.GetComponent<MilitiaBehaviour> ().size = attackersize;
			attacker.GetComponent<MilitiaBehaviour> ().lasthealth = attackerlasthealth;
			Text attsizetext = GameObject.Find ("Size " + eEntity).GetComponent<Text> ();
			attsizetext.text = attackersize.ToString ();
			attacker.GetComponent<MilitiaBehaviour> ().currattackpoint = attacker.GetComponent<MilitiaBehaviour> ().currattackpoint - 1;
		}
	}

	private int GetPlayerEntitySize(string pEntity) {
		GameObject pEntityObject = GameObject.Find (pEntity);
		string cleanpEntity = Regex.Replace(pEntity, @"[\d-]", string.Empty);

		//------Grab Info Defender------
		if (cleanpEntity == "Necromancer") {
			return pEntityObject.GetComponent<NecromancerBehaviour> ().size;
		} else if (cleanpEntity == "Skeleton") {
			return pEntityObject.GetComponent<SkeletonBehaviour> ().size;
		} else if (cleanpEntity == "Zombie") {
			return pEntityObject.GetComponent<ZombieBehaviour> ().size;
		}
		return 0;
	}
}
