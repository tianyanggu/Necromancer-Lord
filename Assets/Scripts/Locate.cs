using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class Locate : MonoBehaviour {

	public EntityStorage entityStorage;
    public EntityStats entityStats;

    public void SetAllMovementPoints () {
        foreach (List<GameObject> playerEntityList in entityStorage.activePlayersEntityList)
        {
            foreach (GameObject entity in playerEntityList)
            {
                entityStats.SetCurrMovementPoint(entity, entityStats.GetCurrMaxMovementPoint(entity));
            }
        }
	}

	public void SetAllAttackPoints () {
        foreach (List<GameObject> playerEntityList in entityStorage.activePlayersEntityList)
        {
            foreach (GameObject entity in playerEntityList)
            {
                entityStats.SetCurrAttackPoint(entity, entityStats.GetCurrMaxAttackPoint(entity));
            }
        }
	}

	public void SetAllIdleStatus (bool idleStatus, char playerID) {
		foreach (GameObject entity in entityStorage.PlayerEntityList(playerID)) {
            entityStats.SetIdle(entity, idleStatus);
		}
	}

	public bool CheckAllPoints (char playerID) {
        foreach (GameObject entity in entityStorage.PlayerEntityList(playerID)) {
            if (entityStats.GetCurrMovementPoint(entity) != 0 || entityStats.GetCurrAttackPoint(entity) != 0)
            {
                if (entityStats.GetIdle(entity) == false)
                {
                    return false;
                }
            }
        }
		return true;
	}
}
