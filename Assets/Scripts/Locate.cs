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
			} else if (cleanEntity == "SkeletonArcher") {
				GameObject gameEntity = GameObject.Find (entity);
				gameEntity.GetComponent<SkeletonArcherBehaviour> ().currmovementpoint = gameEntity.GetComponent<SkeletonArcherBehaviour> ().movementpoint;
			} else if (cleanEntity == "ArmoredSkeleton") {
				GameObject gameEntity = GameObject.Find (entity);
				gameEntity.GetComponent<ArmoredSkeletonBehaviour> ().currmovementpoint = gameEntity.GetComponent<ArmoredSkeletonBehaviour> ().movementpoint;
			} else if (cleanEntity == "DeathKnight") {
				GameObject gameEntity = GameObject.Find (entity);
				gameEntity.GetComponent<DeathKnightBehaviour> ().currmovementpoint = gameEntity.GetComponent<DeathKnightBehaviour> ().movementpoint;
			}
		}

        //TODO human class update
        //update on both sides for each class

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
			} else if (cleanEntity == "SkeletonArcher") {
				GameObject gameEntity = GameObject.Find (entity);
				gameEntity.GetComponent<SkeletonArcherBehaviour> ().currattackpoint = gameEntity.GetComponent<SkeletonArcherBehaviour> ().attackpoint;
			} else if (cleanEntity == "ArmoredSkeleton") {
				GameObject gameEntity = GameObject.Find (entity);
				gameEntity.GetComponent<ArmoredSkeletonBehaviour> ().currattackpoint = gameEntity.GetComponent<ArmoredSkeletonBehaviour> ().attackpoint;
			} else if (cleanEntity == "DeathKnight") {
				GameObject gameEntity = GameObject.Find (entity);
				gameEntity.GetComponent<DeathKnightBehaviour> ().currattackpoint = gameEntity.GetComponent<DeathKnightBehaviour> ().attackpoint;
			}
		}

        //TODO human class update
        //update on both sides for each class

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
			} else if (cleanEntity == "SkeletonArcher") {
				GameObject gameEntity = GameObject.Find (entity);
				gameEntity.GetComponent<SkeletonArcherBehaviour> ().idle = true;
			} else if (cleanEntity == "ArmoredSkeleton") {
				GameObject gameEntity = GameObject.Find (entity);
				gameEntity.GetComponent<ArmoredSkeletonBehaviour> ().idle = true;
			} else if (cleanEntity == "DeathKnight") {
				GameObject gameEntity = GameObject.Find (entity);
				gameEntity.GetComponent<DeathKnightBehaviour> ().idle = true;
			}

            //TODO human class update
            //else if (cleanEntity == "Militia") {
			//	GameObject gameEntity = GameObject.Find (entity);
			//	gameEntity.GetComponent<Militia> ().idle = true;
			//}
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
			} else if (cleanEntity == "SkeletonArcher") {
				GameObject gameEntity = GameObject.Find (entity);
				gameEntity.GetComponent<SkeletonArcherBehaviour> ().idle = false;
			} else if (cleanEntity == "ArmoredSkeleton") {
				GameObject gameEntity = GameObject.Find (entity);
				gameEntity.GetComponent<ArmoredSkeletonBehaviour> ().idle = false;
			} else if (cleanEntity == "DeathKnight") {
				GameObject gameEntity = GameObject.Find (entity);
				gameEntity.GetComponent<DeathKnightBehaviour> ().idle = false;
			}

            //TODO human class update
            //else if (cleanEntity == "Militia") {
			//	GameObject gameEntity = GameObject.Find (entity);
			//	gameEntity.GetComponent<Militia> ().idle = true;
			//}
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
			} else if (cleanEntity == "SkeletonArcher") {
				GameObject gameEntity = GameObject.Find (entity);
				if (gameEntity.GetComponent<SkeletonArcherBehaviour> ().currmovementpoint != 0) {
					return false;
				}
			} else if (cleanEntity == "ArmoredSkeleton") {
				GameObject gameEntity = GameObject.Find (entity);
				if (gameEntity.GetComponent<ArmoredSkeletonBehaviour> ().currmovementpoint != 0) {
					return false;
				}
			} else if (cleanEntity == "DeathKnight") {
				GameObject gameEntity = GameObject.Find (entity);
				if (gameEntity.GetComponent<DeathKnightBehaviour> ().currmovementpoint != 0) {
					return false;
				}
			}

            else if (cleanEntity == "Militia") {
				GameObject gameEntity = GameObject.Find (entity);
				if (gameEntity.GetComponent<MilitiaBehaviour> ().currmovementpoint != 0) {
					return false;
				}
			}
		}
		return true;
	}

	public bool CheckAllAttack () {
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
			} else if (cleanEntity == "SkeletonArcher") {
				GameObject gameEntity = GameObject.Find (entity);
				if (gameEntity.GetComponent<SkeletonArcherBehaviour> ().currmovementpoint != 0 || gameEntity.GetComponent<SkeletonArcherBehaviour> ().currattackpoint != 0) {
					if (gameEntity.GetComponent<SkeletonArcherBehaviour> ().idle == false) {
						return false;
					}
				}
			} else if (cleanEntity == "ArmoredSkeleton") {
				GameObject gameEntity = GameObject.Find (entity);
				if (gameEntity.GetComponent<ArmoredSkeletonBehaviour> ().currmovementpoint != 0 || gameEntity.GetComponent<ArmoredSkeletonBehaviour> ().currattackpoint != 0) {
					if (gameEntity.GetComponent<ArmoredSkeletonBehaviour> ().idle == false) {
						return false;
					}
				}
			} else if (cleanEntity == "DeathKnight") {
				GameObject gameEntity = GameObject.Find (entity);
				if (gameEntity.GetComponent<DeathKnightBehaviour> ().currmovementpoint != 0 || gameEntity.GetComponent<DeathKnightBehaviour> ().currattackpoint != 0) {
					if (gameEntity.GetComponent<DeathKnightBehaviour> ().idle == false) {
						return false;
					}
				}
			}

            //TODO human update
            //else if (cleanEntity == "Militia") {
			//	GameObject gameEntity = GameObject.Find (entity);
			//	if (gameEntity.GetComponent<MilitiaBehaviour> ().currmovementpoint != 0 || gameEntity.GetComponent<MilitiaBehaviour> ().currattackpoint != 0) {
			//		if (gameEntity.GetComponent<MilitiaBehaviour> ().idle == false) {
			//  		return false;
			//		}
			//	}
			//} 
		}
		return true;
	}
}
