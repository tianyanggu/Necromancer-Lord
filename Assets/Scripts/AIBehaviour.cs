using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System.Linq;

public class AIBehaviour : MonoBehaviour {

	public Movement movement;
	public HexGrid hexGrid;
	public EntityStorage entityStorage;
	public Battle battle;

	private List<int> aiMovementIndexes = new List<int> ();

	private List<string> nearbyPlayerEntities = new List<string> ();
	private List<int> nearbyPlayerEntitiesIndex = new List<int> ();
	private List<int> nearbyPlayerEntitiesDistance = new List<int> ();
	private List<int> nearbyPlayerEntitiesHealth = new List<int> ();

	private int aimovepoint = 0;
	private int aicurrattpoint = 0;
	private int aicurrmovepoint = 0;

	private List<string> plistAtt = new List<string>();
	private List<int> pindexAtt = new List<int>();
	private List<int> pdistAtt = new List<int>();
	private List<int> phealthAtt = new List<int>();

	//testing remove after
//	void Start () {
//		List<string> scannedentities = ScanEntities (15);
//		string decideattentity = DecideAttack (15, nearbyPlayerEntities, nearbyPlayerEntitiesIndex, nearbyPlayerEntitiesDistance, nearbyPlayerEntitiesHealth);
//
//		Debug.Log (decideattentity);
//	}
	public void Actions (int eindex) {
		ScanEntities (eindex);
		int decideAttEntity = DecideAttack (eindex, nearbyPlayerEntities, nearbyPlayerEntitiesIndex, nearbyPlayerEntitiesDistance, nearbyPlayerEntitiesHealth);

		//find postions where player entity can be attacked at then attacks it
		List<int> attackPos = new List<int>();
		if (decideAttEntity != -1) {
			List<int> attIndexesList = availableAttackIndexes (decideAttEntity);
			foreach (int attIndex in attIndexesList) {
				if (aiMovementIndexes.Contains (attIndex)) {					
					attackPos.Add (attIndex);
				}
			}
			battle.Attack (eindex, attackPos[0]);
			battle.Attack (attackPos[0], decideAttEntity);
		}
	}

	//eindex is the current enemy entity that scans for player entities within movement range of it
	public List<string> ScanEntities (int eindex) {
		aiMovementIndexes.Clear ();
		nearbyPlayerEntities.Clear ();
		nearbyPlayerEntitiesIndex.Clear ();
		nearbyPlayerEntitiesDistance.Clear ();
		nearbyPlayerEntitiesHealth.Clear ();

		string eEntity = hexGrid.GetEntity(eindex);
		GetAIInfo (eEntity);

		aiMovementIndexes = movement.GetCellIndexesBlockers (eindex, aicurrmovepoint);
		ScanEntitiesHelper (eindex, aicurrmovepoint, 0);

		if (nearbyPlayerEntities.Any ()) {
			List<string> scan = nearbyPlayerEntities;
			return scan;
		}
		return new List<string> {"Empty"};
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
				//ensures no index error from index being out of bounds in hexgrid
				if (direction >= 0 && direction < hexGrid.size) {
					string dirEntity = hexGrid.GetEntity (direction);
					string cleandirEntity = Regex.Replace (dirEntity, @"[\d-]", string.Empty);
					if (dirEntity == "Empty") {
						if (hexGrid.GetTerrain (direction) == "Mountain" && maxDistance > 1) {
							int newmovementpoints = maxDistance - 2;
							int newusedmovementpoints = usedDistance + 2;
							ScanEntitiesHelper (direction, newmovementpoints, newusedmovementpoints);
						} else if (hexGrid.GetTerrain (direction) != "Mountain") {
							int newmovementpoints = maxDistance - 1;
							int newusedmovementpoints = usedDistance + 1;
							ScanEntitiesHelper (direction, newmovementpoints, newusedmovementpoints);
						}
					//if index not empty and is from undead faction, get the entity
					} else if (entityStorage.whichFaction(cleandirEntity) == "undead") {
						if (entityStorage.playerEntities.Contains (cleandirEntity)) {
							nearbyPlayerEntities.Add (dirEntity);
							nearbyPlayerEntitiesIndex.Add (direction);
							nearbyPlayerEntitiesDistance.Add (usedDistance + 1);
							int playerEntityHealth = GetPlayerEntityHealth (dirEntity);
							nearbyPlayerEntitiesHealth.Add (playerEntityHealth);
						}
					}
				}
			}
		}
	}

	// given list of player entities, decide if attack and which
	public int DecideAttack (int eindex, List<string> plist, List<int> pindex, List<int> pdist, List<int> phealth) {
		string eEntity = hexGrid.GetEntity(eindex);
		//list of the index of each list to get the corresponding plist, pindex, pdist, and phealth values
		List<int> canBeAttListPos = new List<int>();
		foreach (int index in pindex) {
			//if can attack enemy
			List<int> attIndexesList = availableAttackIndexes (index);
			foreach (int attIndex in attIndexesList) {
				if (aiMovementIndexes.Contains (attIndex)) {					
					int posIndex = pindex.IndexOf(index);
					canBeAttListPos.Add (posIndex);
				}
			}
		}

		//add entities that can be attacked into new list
		if (canBeAttListPos.Any ()) {
			foreach (int posIndex in canBeAttListPos) {
				plistAtt.Add (plist [posIndex]);
				pindexAtt.Add (pindex [posIndex]);
				pdistAtt.Add (pdist [posIndex]);
				phealthAtt.Add (phealth [posIndex]);
			}
		}

		//attack lowest health unit that can be attacked
		//check if phealth is null
		if (phealthAtt.Any()) {
			int posLowest = phealthAtt.IndexOf (phealthAtt.Min ());
			int healthLowestEntity = pindexAtt [posLowest];
				
			return healthLowestEntity;
		}
		
		//if no entities found
		return -1;
		//TODO also return the positions to attack from or move the player to attack
		//TODO DecideAttack kill most valuable player unit if possible
	}

	//avaliable tiles that pindex entity can be attacked from
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
			if (direction >= 0 && direction < hexGrid.size) {
				string dirEntity = hexGrid.GetEntity (direction);
				if (dirEntity == "Empty") {
					availableIndex.Add (direction);
				}
			}
		}
		return availableIndex;
	}

	void GetAIInfo(string eEntity) {
		GameObject atentity = GameObject.Find (eEntity);
		string cleaneEntity = Regex.Replace(eEntity, @"[\d-]", string.Empty);

		//------Grab Info Attacker------
		if (cleaneEntity == "Militia") {
			aicurrattpoint = atentity.GetComponent<MilitiaBehaviour> ().currattackpoint;
			aimovepoint = atentity.GetComponent<MilitiaBehaviour> ().movementpoint;
			aicurrmovepoint = atentity.GetComponent<MilitiaBehaviour> ().currmovementpoint;
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
		} else if (cleanpEntity == "SkeletonArcher") {
			return pEntityObject.GetComponent<SkeletonArcherBehaviour> ().lasthealth;
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
