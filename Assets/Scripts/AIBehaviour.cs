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
    public EntityStats entityStats;

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
	void Start () {
        //		List<string> scannedentities = ScanEntities (15);
        //		string decideattentity = DecideAttack (15, nearbyPlayerEntities, nearbyPlayerEntitiesIndex, nearbyPlayerEntitiesDistance, nearbyPlayerEntitiesHealth);
        //
        //		Debug.Log (decideattentity);
        //Debug.Log(ClosestEntity(12));
    }
	public void Actions (int eindex) {
		ScanEntities (eindex);
		int decideAttEntity = DecideAttack (eindex, nearbyPlayerEntities, nearbyPlayerEntitiesIndex, nearbyPlayerEntitiesDistance, nearbyPlayerEntitiesHealth);
        //TODO decideAttEntity for ranged units

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
			//battle.Attack (attackPos[0], decideAttEntity);
		}
        //if cannot reach right away, move closest possible
        //TODO move closest function
	}

	//eindex is the current enemy entity that scans for player entities within movement range of it
	public List<string> ScanEntities (int aiIndex) {
		aiMovementIndexes.Clear ();
		nearbyPlayerEntities.Clear ();
		nearbyPlayerEntitiesIndex.Clear ();
		nearbyPlayerEntitiesDistance.Clear ();
		nearbyPlayerEntitiesHealth.Clear ();

        GameObject eEntity = hexGrid.GetEntityObject(aiIndex);
        GetAIInfo (eEntity);

		aiMovementIndexes = movement.GetCellIndexesBlockers (aiIndex, aicurrmovepoint);
		ScanEntitiesHelper (aiIndex, aicurrmovepoint, 0);

		if (nearbyPlayerEntities.Any ()) {
			List<string> scan = nearbyPlayerEntities;
			return scan;
		}

        //if cannot find any that can be reached then find nearest nearest one
        else
        {
            ClosestEntity(aiIndex);
        }

		return new List<string> {"Empty"};
	}

    public GameObject ClosestEntity(int aiIndex)
    {
        GameObject currClosestObj = null;
        int currClosest = 999999;
        //TODO change activePlayerAEntities to enemies of AI
        foreach (GameObject aiEntity in entityStorage.activePlayerAEntities)
        {
            int currEntityIndex = hexGrid.GetCellIndexFromGameObject(aiEntity);
            int currDist = movement.GetDistance(aiIndex, currEntityIndex);
            if (currDist < currClosest)
            {
                currClosest = currDist;
                currClosestObj = aiEntity;
            }
        }
        return currClosestObj;
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
                    GameObject dirEntityObject = hexGrid.GetEntityObject(direction);
                    string cleandirEntity = entityStats.GetType(dirEntityObject);
					if (dirEntityObject == null) {
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
					} else if (entityStats.WhichFactionEntity(cleandirEntity) == FactionNames.Undead) {
                        //TODO player entities different each time, not undead entities
                        //char playerChar = playerManager.currPlayer[0];
                        //EntityFactionLists(playerChar) instead of undeadEntities
                        if (entityStats.undeadEntities.Contains (cleandirEntity)) {
							nearbyPlayerEntities.Add (cleandirEntity);
							nearbyPlayerEntitiesIndex.Add (direction);
							nearbyPlayerEntitiesDistance.Add (usedDistance + 1);
							int playerEntityHealth = GetPlayerEntityHealth (dirEntityObject);
							nearbyPlayerEntitiesHealth.Add (playerEntityHealth);
						}
					}
				}
			}
		}
	}

	// given list of player entities, decide if attack and which
	public int DecideAttack (int eindex, List<string> plist, List<int> pindex, List<int> pdist, List<int> phealth) {
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
                if (hexGrid.GetEntityObject(direction) == null)
                {
                    availableIndex.Add(direction);
                }
                //TODO if entity has no available attack indexes because of entity occupying space but after one of entities occupying empty space dies, it may have space. Recalculate
            }
        }
		return availableIndex;
	}

	void GetAIInfo(GameObject eEntity) {
        aicurrattpoint = entityStats.GetCurrAttackPoint(eEntity);
        aimovepoint = entityStats.GetCurrMaxMovementPoint(eEntity);
        aicurrmovepoint = entityStats.GetCurrMovementPoint(eEntity);
	}
    
	private int GetPlayerEntityHealth(GameObject pEntity) {
        return entityStats.GetCurrHealth(pEntity);
	}

    void NewMovementPoints(GameObject entity, int change)
    {
        entityStats.SetCurrMovementPoint(entity, aicurrmovepoint - change);
    }
}
