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
		foreach (GameObject entity in entityStorage.activePlayerEntities) {
			string cleanEntity = Regex.Replace(entity.name, @"[\d-]", string.Empty);
			if (cleanEntity == "Necromancer") {
                entity.GetComponent<NecromancerBehaviour> ().currmovementpoint = entity.GetComponent<NecromancerBehaviour> ().movementpoint;
			} else if (cleanEntity == "Skeleton") {
                entity.GetComponent<SkeletonBehaviour> ().currmovementpoint = entity.GetComponent<SkeletonBehaviour> ().movementpoint;
			} else if (cleanEntity == "Zombie") {
                entity.GetComponent<ZombieBehaviour> ().currmovementpoint = entity.GetComponent<ZombieBehaviour> ().movementpoint;
			} else if (cleanEntity == "SkeletonArcher") {
                entity.GetComponent<SkeletonArcherBehaviour> ().currmovementpoint = entity.GetComponent<SkeletonArcherBehaviour> ().movementpoint;
			} else if (cleanEntity == "ArmoredSkeleton") {
                entity.GetComponent<ArmoredSkeletonBehaviour> ().currmovementpoint = entity.GetComponent<ArmoredSkeletonBehaviour> ().movementpoint;
			} else if (cleanEntity == "DeathKnight") {
                entity.GetComponent<DeathKnightBehaviour> ().currmovementpoint = entity.GetComponent<DeathKnightBehaviour> ().movementpoint;
			}
		}

        //TODO human class update
        //update on both sides for each class

		foreach (GameObject entity in entityStorage.activeEnemyEntities) {
			string cleanEntity = Regex.Replace (entity.name, @"[\d-]", string.Empty);
			if (cleanEntity == "Militia") {
                entity.GetComponent<MilitiaBehaviour> ().currmovementpoint = entity.GetComponent<MilitiaBehaviour> ().movementpoint;
			} else if (cleanEntity == "Archer") {
                entity.GetComponent<ArcherBehaviour> ().currmovementpoint = entity.GetComponent<ArcherBehaviour> ().movementpoint;
			} else if (cleanEntity == "Longbowman") {
                entity.GetComponent<LongbowmanBehaviour> ().currmovementpoint = entity.GetComponent<LongbowmanBehaviour> ().movementpoint;
			} else if (cleanEntity == "Crossbowman") {
                entity.GetComponent<CrossbowmanBehaviour> ().currmovementpoint = entity.GetComponent<CrossbowmanBehaviour> ().movementpoint;
			} else if (cleanEntity == "Footman") {
                entity.GetComponent<FootmanBehaviour> ().currmovementpoint = entity.GetComponent<FootmanBehaviour> ().movementpoint;
			} else if (cleanEntity == "MountedKnight") {
                entity.GetComponent<MountedKnightBehaviour> ().currmovementpoint = entity.GetComponent<MountedKnightBehaviour> ().movementpoint;
			} else if (cleanEntity == "HeroKing") {
                entity.GetComponent<HeroKingBehaviour> ().currmovementpoint = entity.GetComponent<HeroKingBehaviour> ().movementpoint;
			}
		}
	}

	public void SetAllAttackPoints () {
		foreach (GameObject entity in entityStorage.activePlayerEntities) {
			string cleanEntity = Regex.Replace(entity.name, @"[\d-]", string.Empty);
			if (cleanEntity == "Necromancer") {
                entity.GetComponent<NecromancerBehaviour> ().currattackpoint = entity.GetComponent<NecromancerBehaviour> ().attackpoint;
			} else if (cleanEntity == "Skeleton") {
                entity.GetComponent<SkeletonBehaviour> ().currattackpoint = entity.GetComponent<SkeletonBehaviour> ().attackpoint;
			} else if (cleanEntity == "Zombie") {
                entity.GetComponent<ZombieBehaviour> ().currattackpoint = entity.GetComponent<ZombieBehaviour> ().attackpoint;
			} else if (cleanEntity == "SkeletonArcher") {
                entity.GetComponent<SkeletonArcherBehaviour> ().currattackpoint = entity.GetComponent<SkeletonArcherBehaviour> ().attackpoint;
			} else if (cleanEntity == "ArmoredSkeleton") {
                entity.GetComponent<ArmoredSkeletonBehaviour> ().currattackpoint = entity.GetComponent<ArmoredSkeletonBehaviour> ().attackpoint;
			} else if (cleanEntity == "DeathKnight") {
                entity.GetComponent<DeathKnightBehaviour> ().currattackpoint = entity.GetComponent<DeathKnightBehaviour> ().attackpoint;
			}
		}

        //TODO human class update
        //update on both sides for each class

		foreach (GameObject entity in entityStorage.activeEnemyEntities) {
			string cleanEntity = Regex.Replace(entity.name, @"[\d-]", string.Empty);
			if (cleanEntity == "Militia") {
                entity.GetComponent<MilitiaBehaviour> ().currattackpoint = entity.GetComponent<MilitiaBehaviour> ().attackpoint;
			} else if (cleanEntity == "Archer") {
                entity.GetComponent<ArcherBehaviour> ().currmovementpoint = entity.GetComponent<ArcherBehaviour> ().attackpoint;
			} else if (cleanEntity == "Longbowman") {
                entity.GetComponent<LongbowmanBehaviour> ().currmovementpoint = entity.GetComponent<LongbowmanBehaviour> ().attackpoint;
			} else if (cleanEntity == "Crossbowman") {
                entity.GetComponent<CrossbowmanBehaviour> ().currmovementpoint = entity.GetComponent<CrossbowmanBehaviour> ().attackpoint;
			} else if (cleanEntity == "Footman") {
                entity.GetComponent<FootmanBehaviour> ().currmovementpoint = entity.GetComponent<FootmanBehaviour> ().attackpoint;
			} else if (cleanEntity == "MountedKnight") {
                entity.GetComponent<MountedKnightBehaviour> ().currmovementpoint = entity.GetComponent<MountedKnightBehaviour> ().attackpoint;
			} else if (cleanEntity == "HeroKing") {
                entity.GetComponent<HeroKingBehaviour> ().currmovementpoint = entity.GetComponent<HeroKingBehaviour> ().attackpoint;
			}
		}
	}

	public void SetAllIdle () {
		foreach (GameObject entity in entityStorage.activePlayerEntities) {
			string cleanEntity = Regex.Replace(entity.name, @"[\d-]", string.Empty);
			if (cleanEntity == "Necromancer") {
                entity.GetComponent<NecromancerBehaviour> ().idle = true;
			} else if (cleanEntity == "Skeleton") {
                entity.GetComponent<SkeletonBehaviour> ().idle = true;
			} else if (cleanEntity == "Zombie") {
                entity.GetComponent<ZombieBehaviour> ().idle = true;
			} else if (cleanEntity == "SkeletonArcher") {
                entity.GetComponent<SkeletonArcherBehaviour> ().idle = true;
			} else if (cleanEntity == "ArmoredSkeleton") {
                entity.GetComponent<ArmoredSkeletonBehaviour> ().idle = true;
			} else if (cleanEntity == "DeathKnight") {
                entity.GetComponent<DeathKnightBehaviour> ().idle = true;
			}

            //TODO human class update
            //else if (cleanEntity == "Militia") {
			//	GameObject gameEntity = GameObject.Find (entity);
			//	gameEntity.GetComponent<Militia> ().idle = true;
			//}
		}
	}

	public void SetAllActive () {
		foreach (GameObject entity in entityStorage.activePlayerEntities) {
			string cleanEntity = Regex.Replace(entity.name, @"[\d-]", string.Empty);
			if (cleanEntity == "Necromancer") {
                entity.GetComponent<NecromancerBehaviour> ().idle = false;
			} else if (cleanEntity == "Skeleton") {
                entity.GetComponent<SkeletonBehaviour> ().idle = false;
			} else if (cleanEntity == "Zombie") {
                entity.GetComponent<ZombieBehaviour> ().idle = false;
			} else if (cleanEntity == "SkeletonArcher") {
                entity.GetComponent<SkeletonArcherBehaviour> ().idle = false;
			} else if (cleanEntity == "ArmoredSkeleton") {
                entity.GetComponent<ArmoredSkeletonBehaviour> ().idle = false;
			} else if (cleanEntity == "DeathKnight") {
                entity.GetComponent<DeathKnightBehaviour> ().idle = false;
			}

            //TODO human class update
            //else if (cleanEntity == "Militia") {
			//	GameObject gameEntity = GameObject.Find (entity);
			//	gameEntity.GetComponent<Militia> ().idle = true;
			//}
		}
	}

	public bool CheckAllMovementPoints () {
		foreach (GameObject entity in entityStorage.activePlayerEntities) {
			string cleanEntity = Regex.Replace(entity.name, @"[\d-]", string.Empty);
			if (cleanEntity == "Necromancer") {
				if (entity.GetComponent<NecromancerBehaviour> ().currmovementpoint != 0) {
					return false;
				}
			} else if (cleanEntity == "Skeleton") {
				if (entity.GetComponent<SkeletonBehaviour> ().currmovementpoint != 0) {
					return false;
				}
			} else if (cleanEntity == "Zombie") {
				if (entity.GetComponent<ZombieBehaviour> ().currmovementpoint != 0) {
					return false;
				}
			} else if (cleanEntity == "SkeletonArcher") {
				if (entity.GetComponent<SkeletonArcherBehaviour> ().currmovementpoint != 0) {
					return false;
				}
			} else if (cleanEntity == "ArmoredSkeleton") {
				if (entity.GetComponent<ArmoredSkeletonBehaviour> ().currmovementpoint != 0) {
					return false;
				}
			} else if (cleanEntity == "DeathKnight") {
				if (entity.GetComponent<DeathKnightBehaviour> ().currmovementpoint != 0) {
					return false;
				}
			}

            else if (cleanEntity == "Militia") {
				if (entity.GetComponent<MilitiaBehaviour> ().currmovementpoint != 0) {
					return false;
				}
			} else if (cleanEntity == "Archer") {
				if (entity.GetComponent<ArcherBehaviour> ().currmovementpoint != 0) {
					return false;
				}
			} else if (cleanEntity == "Longbowman") {
				if (entity.GetComponent<LongbowmanBehaviour> ().currmovementpoint != 0) {
					return false;
				}
			} else if (cleanEntity == "Crossbowman") {
				if (entity.GetComponent<CrossbowmanBehaviour> ().currmovementpoint != 0) {
					return false;
				}
			} else if (cleanEntity == "Footman") {
				if (entity.GetComponent<FootmanBehaviour> ().currmovementpoint != 0) {
					return false;
				}
			} else if (cleanEntity == "MountedKnight") {
				if (entity.GetComponent<MountedKnightBehaviour> ().currmovementpoint != 0) {
					return false;
				}
			} else if (cleanEntity == "HeroKing") {
				if (entity.GetComponent<HeroKingBehaviour> ().currmovementpoint != 0) {
					return false;
				}
			}
		}
		return true;
	}

	public bool CheckAllAttack () {
		foreach (GameObject entity in entityStorage.activePlayerEntities) {
			string cleanEntity = Regex.Replace(entity.name, @"[\d-]", string.Empty);
			if (cleanEntity == "Necromancer") {
				if (entity.GetComponent<NecromancerBehaviour> ().currmovementpoint != 0 || entity.GetComponent<NecromancerBehaviour> ().currattackpoint != 0) {
					if (entity.GetComponent<NecromancerBehaviour> ().idle == false) {
						return false;
					}
				}
			} else if (cleanEntity == "Skeleton") {
				if (entity.GetComponent<SkeletonBehaviour> ().currmovementpoint != 0 || entity.GetComponent<SkeletonBehaviour> ().currattackpoint != 0) {
					if (entity.GetComponent<SkeletonBehaviour> ().idle == false) { 
						return false;
					}
				}
			} else if (cleanEntity == "Zombie") {
				if (entity.GetComponent<ZombieBehaviour> ().currmovementpoint != 0 || entity.GetComponent<ZombieBehaviour> ().currattackpoint != 0) {
					if (entity.GetComponent<ZombieBehaviour> ().idle == false) {
						return false;
					}
				}
			} else if (cleanEntity == "SkeletonArcher") {
				if (entity.GetComponent<SkeletonArcherBehaviour> ().currmovementpoint != 0 || entity.GetComponent<SkeletonArcherBehaviour> ().currattackpoint != 0) {
					if (entity.GetComponent<SkeletonArcherBehaviour> ().idle == false) {
						return false;
					}
				}
			} else if (cleanEntity == "ArmoredSkeleton") {
				if (entity.GetComponent<ArmoredSkeletonBehaviour> ().currmovementpoint != 0 || entity.GetComponent<ArmoredSkeletonBehaviour> ().currattackpoint != 0) {
					if (entity.GetComponent<ArmoredSkeletonBehaviour> ().idle == false) {
						return false;
					}
				}
			} else if (cleanEntity == "DeathKnight") {
				if (entity.GetComponent<DeathKnightBehaviour> ().currmovementpoint != 0 || entity.GetComponent<DeathKnightBehaviour> ().currattackpoint != 0) {
					if (entity.GetComponent<DeathKnightBehaviour> ().idle == false) {
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
