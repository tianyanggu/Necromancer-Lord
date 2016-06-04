using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class Locate : MonoBehaviour {

	public EntityStorage entityStorage;

	//		else if (entity == "Militia") {
	//			GameObject gameEntity = GameObject.Find (entity);
	//			gameEntity.GetComponent<MilitiaBehaviour> ().currmovementpoint = gameEntity.GetComponent<MilitiaBehaviour> ().movementpoint;
	//		}

	public void SetAllMovementPoints () {
		foreach (string entity in entityStorage.activePlayerEntities) {
			string cleanEntity = Regex.Replace(entity, @"[\d-]", string.Empty);
			if (cleanEntity == "Necromancer") {
				GameObject gameEntity = GameObject.Find (entity);
				gameEntity.GetComponent<NecromancerBehaviour> ().currmovementpoint = gameEntity.GetComponent<NecromancerBehaviour> ().movementpoint;
			} else if (cleanEntity == "Skeleton") {
				GameObject gameEntity = GameObject.Find (entity);
				gameEntity.GetComponent<SkeletonBehaviour> ().currmovementpoint = gameEntity.GetComponent<SkeletonBehaviour> ().movementpoint;
			} else if (cleanEntity == "Zombie") {
				GameObject gameEntity = GameObject.Find (entity);
				gameEntity.GetComponent<ZombieBehaviour> ().currmovementpoint = gameEntity.GetComponent<ZombieBehaviour> ().movementpoint;
			}
		}

		foreach (string entity in entityStorage.activeEnemyEntities) {
			string cleanEntity = Regex.Replace (entity, @"[\d-]", string.Empty);
			if (cleanEntity == "Militia") {
				GameObject gameEntity = GameObject.Find (entity);
				gameEntity.GetComponent<MilitiaBehaviour> ().currmovementpoint = gameEntity.GetComponent<MilitiaBehaviour> ().movementpoint;
			}
		}
	}

	public void SetAllAttackPoints () {
		foreach (string entity in entityStorage.activePlayerEntities) {
			string cleanEntity = Regex.Replace(entity, @"[\d-]", string.Empty);
			if (cleanEntity == "Necromancer") {
				GameObject gameEntity = GameObject.Find (entity);
				gameEntity.GetComponent<NecromancerBehaviour> ().currattackpoint = gameEntity.GetComponent<NecromancerBehaviour> ().attackpoint;
			} else if (cleanEntity == "Skeleton") {
				GameObject gameEntity = GameObject.Find (entity);
				gameEntity.GetComponent<SkeletonBehaviour> ().currattackpoint = gameEntity.GetComponent<SkeletonBehaviour> ().attackpoint;
			} else if (cleanEntity == "Zombie") {
				GameObject gameEntity = GameObject.Find (entity);
				gameEntity.GetComponent<ZombieBehaviour> ().currattackpoint = gameEntity.GetComponent<ZombieBehaviour> ().attackpoint;
			}
		}

		foreach (string entity in entityStorage.activeEnemyEntities) {
			string cleanEntity = Regex.Replace(entity, @"[\d-]", string.Empty);
			if (cleanEntity == "Militia") {
				GameObject gameEntity = GameObject.Find (entity);
				gameEntity.GetComponent<MilitiaBehaviour> ().currattackpoint = gameEntity.GetComponent<MilitiaBehaviour> ().attackpoint;
			}
		}
	}

	public void SetAllIdle () {
		foreach (string entity in entityStorage.activePlayerEntities) {
			string cleanEntity = Regex.Replace(entity, @"[\d-]", string.Empty);
			if (cleanEntity == "Necromancer") {
				GameObject gameEntity = GameObject.Find (entity);
				gameEntity.GetComponent<NecromancerBehaviour> ().idle = true;
			} else if (cleanEntity == "Skeleton") {
				GameObject gameEntity = GameObject.Find (entity);
				gameEntity.GetComponent<SkeletonBehaviour> ().idle = true;
			} else if (cleanEntity == "Zombie") {
				GameObject gameEntity = GameObject.Find (entity);
				gameEntity.GetComponent<ZombieBehaviour> ().idle = true;
			}
		}
	}

	public void SetAllActive () {
		foreach (string entity in entityStorage.activePlayerEntities) {
			string cleanEntity = Regex.Replace(entity, @"[\d-]", string.Empty);
			if (cleanEntity == "Necromancer") {
				GameObject gameEntity = GameObject.Find (entity);
				gameEntity.GetComponent<NecromancerBehaviour> ().idle = false;
			} else if (cleanEntity == "Skeleton") {
				GameObject gameEntity = GameObject.Find (entity);
				gameEntity.GetComponent<SkeletonBehaviour> ().idle = false;
			} else if (cleanEntity == "Zombie") {
				GameObject gameEntity = GameObject.Find (entity);
				gameEntity.GetComponent<ZombieBehaviour> ().idle = false;
			}
		}
	}

	public bool CheckAllMovementPoints () {
		foreach (string entity in entityStorage.activePlayerEntities) {
			string cleanEntity = Regex.Replace(entity, @"[\d-]", string.Empty);
			if (cleanEntity == "Necromancer") {
				GameObject gameEntity = GameObject.Find (entity);
				if (gameEntity.GetComponent<NecromancerBehaviour> ().currmovementpoint != 0) {
					return false;
				}
			} else if (cleanEntity == "Skeleton") {
				GameObject gameEntity = GameObject.Find (entity);
				if (gameEntity.GetComponent<SkeletonBehaviour> ().currmovementpoint != 0) {
					return false;
				}
			} else if (cleanEntity == "Zombie") {
				GameObject gameEntity = GameObject.Find (entity);
				if (gameEntity.GetComponent<ZombieBehaviour> ().currmovementpoint != 0) {
					return false;
				}
			}
		}
		return true;
	}

	public bool CheckAll () {
		foreach (string entity in entityStorage.activePlayerEntities) {
			string cleanEntity = Regex.Replace(entity, @"[\d-]", string.Empty);
			if (cleanEntity == "Necromancer") {
				GameObject gameEntity = GameObject.Find (entity);
				if (gameEntity.GetComponent<NecromancerBehaviour> ().currmovementpoint != 0 || gameEntity.GetComponent<NecromancerBehaviour> ().currattackpoint != 0) {
					if (gameEntity.GetComponent<NecromancerBehaviour> ().idle == false) {
						return false;
					}
				}
			} else if (cleanEntity == "Skeleton") {
				GameObject gameEntity = GameObject.Find (entity);
				if (gameEntity.GetComponent<SkeletonBehaviour> ().currmovementpoint != 0 || gameEntity.GetComponent<SkeletonBehaviour> ().currattackpoint != 0) {
					if (gameEntity.GetComponent<SkeletonBehaviour> ().idle == false) { 
						return false;
					}
				}
			} else if (cleanEntity == "Zombie") {
				GameObject gameEntity = GameObject.Find (entity);
				if (gameEntity.GetComponent<ZombieBehaviour> ().currmovementpoint != 0 || gameEntity.GetComponent<ZombieBehaviour> ().currattackpoint != 0) {
					if (gameEntity.GetComponent<ZombieBehaviour> ().idle == false) {
						return false;
					}
				}
			} 
		}
		return true;
	}
}
