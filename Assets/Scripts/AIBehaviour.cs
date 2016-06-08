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

	void GetAttackerInfo(string eEntity) {
		GameObject attacker = GameObject.Find (eEntity);
		string cleaneEntity = Regex.Replace(eEntity, @"[\d-]", string.Empty);

		//------Grab Info Attacker------
		if (cleaneEntity == "Militia") {
			attackercurrattpoint = attacker.GetComponent<MilitiaBehaviour> ().currattackpoint;
			attackermovepoint = attacker.GetComponent<MilitiaBehaviour> ().movementpoint;
			attackercurrmovepoint = attacker.GetComponent<MilitiaBehaviour> ().currmovementpoint;
		}
	}

//	void SetAttackerInfo(string eEntity) {
//		GameObject attacker = GameObject.Find (eEntity);
//		string cleaneEntity = Regex.Replace(eEntity, @"[\d-]", string.Empty);
//
//		//------Set New Info Attacker------
//		if (cleaneEntity == "Militia") {
//			attacker.GetComponent<MilitiaBehaviour> ().currattackpoint = attacker.GetComponent<MilitiaBehaviour> ().currattackpoint - 1;
//		}
//	}

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
