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
	private List<int> nearbyPlayerEntitiesHealth = new List<int> ();

	private int attackerdmg = 0;
	//private int attackerhealth = 0;
	private int attackerlasthealth = 0;
	private int attackerrange = 0;
	private int attackerrangedmg = 0;
	private int defenderdmg = 0;
	//private int defenderhealth = 0;
	private int defenderlasthealth = 0;

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
					int playerEntityHealth = GetPlayerEntityHealth (dirEntity);
					nearbyPlayerEntitiesHealth.Add (playerEntityHealth);
				}
			}

			foreach (int direction in hexdirections) {
				if (direction >= 0 && direction < hexGrid.size) {
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

	// given list of player entities, decide if attack and which
	public void DecideAttack (int eindex, List<int> plist, List<int> pindex, List<int> pdist, List<int> phealth) {
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
		string eEntity = hexGrid.GetEntity(eindex);
		Vector3 cellcoord = hexGrid.GetCellPos(pindex);

		GameObject attacker = GameObject.Find (eEntity);
		GameObject defender = GameObject.Find (pEntity);

		GetAttackerInfo (eEntity);
		GetDefenderInfo (pEntity);

		int unitsdied = hexGrid.GetCorpses(eindex);
		int oldattackerhealth = attackerlasthealth;

		if (attackercurrattpoint == 0) {
			//do nothing
		} else {
			if (attackerlasthealth > 0) {
				//if melee attack or range attack
				if (attackerrange == 1) {
					//calc dmg to attacker and defender health
					defenderlasthealth = defenderlasthealth - attackerdmg;
					attackerlasthealth = attackerlasthealth - defenderdmg;

					//set new corpses created on tile
					int addcorpses = oldattackerhealth - attackerlasthealth;
					unitsdied = unitsdied + addcorpses;
					hexGrid.CorpsesCellIndex (eindex, unitsdied);
				} else if (attackerrange >= 2) {
					//calc dmg to defender health
					defenderlasthealth = defenderlasthealth - attackerrangedmg;

					//set zero new corpses since ranged
				}

				//check new status
				if (defenderlasthealth <= 0) {
					Destroy (defender);
					hexGrid.EntityCellIndex (eindex, "Empty");
					GameObject defenderHealthText = GameObject.Find ("Health " + pEntity);
					Destroy (defenderHealthText);
					entityStorage.RemoveActivePlayerEntity (pEntity);
				}
				if (attackerlasthealth <= 0) {
					Destroy (attacker);
					hexGrid.EntityCellIndex (eindex, "Empty");
					GameObject attackerHealthText = GameObject.Find ("Health " + eEntity);
					Destroy (attackerHealthText);
					entityStorage.RemoveActiveEnemyEntity (eEntity);
				} 
				if (attackerlasthealth > 0 && defenderlasthealth <= 0) {
					//move to defender's position if have enough movement points
					int minmove = movement.GetMovementPointsUsed (eindex, pindex, attackercurrmovepoint);
					if (attackercurrmovepoint > 0 && attackerrange == 1) {
						attacker.transform.position = cellcoord;
						hexGrid.EntityCellIndex (pindex, eEntity);
						hexGrid.EntityCellIndex (eindex, "Empty");
						GameObject attackerHealthText = GameObject.Find ("Health " + eEntity);
						attackerHealthText.transform.position = new Vector3 (cellcoord.x, cellcoord.y + 0.1f, cellcoord.z);
						NewMovementPoints (eEntity, minmove);
					} else if (attackerrange == 2) {
						//do nothing
					}
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
			defenderdmg = defender.GetComponent<NecromancerBehaviour> ().attack;
			//defenderhealth = defender.GetComponent<NecromancerBehaviour> ().health;
			defenderlasthealth = defender.GetComponent<NecromancerBehaviour> ().lasthealth;
		} else if (cleanpEntity == "Skeleton") {
			defenderdmg = defender.GetComponent<SkeletonBehaviour> ().attack;
			//defenderhealth = defender.GetComponent<SkeletonBehaviour> ().health;
			defenderlasthealth = defender.GetComponent<SkeletonBehaviour> ().lasthealth;
		} else if (cleanpEntity == "Zombie") {
			defenderdmg = defender.GetComponent<ZombieBehaviour> ().attack;
			//defenderhealth = defender.GetComponent<ZombieBehaviour> ().health;
			defenderlasthealth = defender.GetComponent<ZombieBehaviour> ().lasthealth;
		}
	}

	void GetAttackerInfo(string eEntity) {
		GameObject attacker = GameObject.Find (eEntity);
		string cleaneEntity = Regex.Replace(eEntity, @"[\d-]", string.Empty);

		//------Grab Info Attacker------
		if (cleaneEntity == "Militia") {
			attackerdmg = attacker.GetComponent<MilitiaBehaviour> ().attack;
			//attackerhealth = attacker.GetComponent<MilitiaBehaviour> ().health;
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
			defender.GetComponent<NecromancerBehaviour> ().lasthealth = defenderlasthealth;
			Text defhealthtext = GameObject.Find ("Health " + pEntity).GetComponent<Text> ();
			defhealthtext.text = attackerlasthealth.ToString ();
		} else if (cleanpEntity == "Skeleton") {
			defender.GetComponent<SkeletonBehaviour> ().lasthealth = defenderlasthealth;
			Text defhealthtext = GameObject.Find ("Health " + pEntity).GetComponent<Text> ();
			defhealthtext.text = defenderlasthealth.ToString ();
		} else if (cleanpEntity == "Zombie") {
			defender.GetComponent<ZombieBehaviour> ().lasthealth = defenderlasthealth;
			Text defhealthtext = GameObject.Find ("Health " + pEntity).GetComponent<Text> ();
			defhealthtext.text = defenderlasthealth.ToString ();
		}
	}

	void SetAttackerInfo(string eEntity) {
		GameObject attacker = GameObject.Find (eEntity);
		string cleaneEntity = Regex.Replace(eEntity, @"[\d-]", string.Empty);

		//------Set New Info Attacker------
		if (cleaneEntity == "Militia") {
			attacker.GetComponent<MilitiaBehaviour> ().lasthealth = attackerlasthealth;
			Text atthealthtext = GameObject.Find ("Health " + eEntity).GetComponent<Text> ();
			atthealthtext.text = attackerlasthealth.ToString ();
			attacker.GetComponent<MilitiaBehaviour> ().currattackpoint = attacker.GetComponent<MilitiaBehaviour> ().currattackpoint - 1;
		}
	}

	private int GetPlayerEntityHealth(string pEntity) {
		GameObject pEntityObject = GameObject.Find (pEntity);
		string cleanpEntity = Regex.Replace(pEntity, @"[\d-]", string.Empty);

		//------Grab Info Defender------
		if (cleanpEntity == "Necromancer") {
			return pEntityObject.GetComponent<NecromancerBehaviour> ().lasthealth;
		} else if (cleanpEntity == "Skeleton") {
			return pEntityObject.GetComponent<SkeletonBehaviour> ().lasthealth;
		} else if (cleanpEntity == "Zombie") {
			return pEntityObject.GetComponent<ZombieBehaviour> ().lasthealth;
		}
		return 0;
	}

	void NewMovementPoints(string selectedentity, int change) {
		GameObject eEntity = GameObject.Find (selectedentity);
		string cleaneEntity = Regex.Replace (selectedentity, @"[\d-]", string.Empty);
		//set movement points
		if (cleaneEntity == "Militia") {
			eEntity.GetComponent<MilitiaBehaviour> ().currmovementpoint = eEntity.GetComponent<MilitiaBehaviour> ().currmovementpoint - change;
		}
	}
}
