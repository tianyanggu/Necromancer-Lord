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
            switch (cleanEntity)
            {
                case "Zombie":
                    entity.GetComponent<ZombieBehaviour>().currmovementpoint = entity.GetComponent<ZombieBehaviour>().movementpoint;
                    break;
                case "Skeleton":
                    entity.GetComponent<SkeletonBehaviour>().currmovementpoint = entity.GetComponent<SkeletonBehaviour>().movementpoint;
                    break;
                case "Necromancer":
                    entity.GetComponent<NecromancerBehaviour>().currmovementpoint = entity.GetComponent<NecromancerBehaviour>().movementpoint;
                    break;
                case "SkeletonArcher":
                    entity.GetComponent<SkeletonArcherBehaviour>().currmovementpoint = entity.GetComponent<SkeletonArcherBehaviour>().movementpoint;
                    break;
                case "ArmoredSkeleton":
                    entity.GetComponent<ArmoredSkeletonBehaviour>().currmovementpoint = entity.GetComponent<ArmoredSkeletonBehaviour>().movementpoint;
                    break;
                case "DeathKnight":
                    entity.GetComponent<DeathKnightBehaviour>().currmovementpoint = entity.GetComponent<DeathKnightBehaviour>().movementpoint;
                    break;
            }
        }

        //TODO human class update
        //update on both sides for each class

		foreach (GameObject entity in entityStorage.activeEnemyEntities) {
			string cleanEntity = Regex.Replace (entity.name, @"[\d-]", string.Empty);
            switch (cleanEntity)
            {
                case "Militia":
                    entity.GetComponent<MilitiaBehaviour>().currmovementpoint = entity.GetComponent<MilitiaBehaviour>().movementpoint;
                    break;
                case "Archer":
                    entity.GetComponent<ArcherBehaviour>().currmovementpoint = entity.GetComponent<ArcherBehaviour>().movementpoint;
                    break;
                case "Longbowman":
                    entity.GetComponent<LongbowmanBehaviour>().currmovementpoint = entity.GetComponent<LongbowmanBehaviour>().movementpoint;
                    break;
                case "Crossbowman":
                    entity.GetComponent<CrossbowmanBehaviour>().currmovementpoint = entity.GetComponent<CrossbowmanBehaviour>().movementpoint;
                    break;
                case "Footman":
                    entity.GetComponent<FootmanBehaviour>().currmovementpoint = entity.GetComponent<FootmanBehaviour>().movementpoint;
                    break;
                case "MountedKnight":
                    entity.GetComponent<MountedKnightBehaviour>().currmovementpoint = entity.GetComponent<MountedKnightBehaviour>().movementpoint;
                    break;
                case "HeroKing":
                    entity.GetComponent<HeroKingBehaviour>().currmovementpoint = entity.GetComponent<HeroKingBehaviour>().movementpoint;
                    break;
            }
		}
	}

	public void SetAllAttackPoints () {
		foreach (GameObject entity in entityStorage.activePlayerEntities) {
			string cleanEntity = Regex.Replace(entity.name, @"[\d-]", string.Empty);
            switch (cleanEntity)
            {
                case "Zombie":
                    entity.GetComponent<ZombieBehaviour>().currattackpoint = entity.GetComponent<ZombieBehaviour>().attackpoint;
                    break;
                case "Skeleton":
                    entity.GetComponent<SkeletonBehaviour>().currattackpoint = entity.GetComponent<SkeletonBehaviour>().attackpoint;
                    break;
                case "Necromancer":
                    entity.GetComponent<NecromancerBehaviour>().currattackpoint = entity.GetComponent<NecromancerBehaviour>().attackpoint;
                    break;
                case "SkeletonArcher":
                    entity.GetComponent<SkeletonArcherBehaviour>().currattackpoint = entity.GetComponent<SkeletonArcherBehaviour>().attackpoint;
                    break;
                case "ArmoredSkeleton":
                    entity.GetComponent<ArmoredSkeletonBehaviour>().currattackpoint = entity.GetComponent<ArmoredSkeletonBehaviour>().attackpoint;
                    break;
                case "DeathKnight":
                    entity.GetComponent<DeathKnightBehaviour>().currattackpoint = entity.GetComponent<DeathKnightBehaviour>().attackpoint;
                    break;
            }
		}

        //TODO human class update
        //update on both sides for each class

		foreach (GameObject entity in entityStorage.activeEnemyEntities) {
			string cleanEntity = Regex.Replace(entity.name, @"[\d-]", string.Empty);
            switch (cleanEntity)
            {
                case "Militia":
                    entity.GetComponent<MilitiaBehaviour>().currattackpoint = entity.GetComponent<MilitiaBehaviour>().attackpoint;
                    break;
                case "Archer":
                    entity.GetComponent<ArcherBehaviour>().currattackpoint = entity.GetComponent<ArcherBehaviour>().attackpoint;
                    break;
                case "Longbowman":
                    entity.GetComponent<LongbowmanBehaviour>().currattackpoint = entity.GetComponent<LongbowmanBehaviour>().attackpoint;
                    break;
                case "Crossbowman":
                    entity.GetComponent<FootmanBehaviour>().currattackpoint = entity.GetComponent<FootmanBehaviour>().attackpoint;
                    break;
                case "Footman":
                    entity.GetComponent<FootmanBehaviour>().currattackpoint = entity.GetComponent<FootmanBehaviour>().attackpoint;
                    break;
                case "MountedKnight":
                    entity.GetComponent<MountedKnightBehaviour>().currattackpoint = entity.GetComponent<MountedKnightBehaviour>().attackpoint;
                    break;
                case "HeroKing":
                    entity.GetComponent<HeroKingBehaviour>().currattackpoint = entity.GetComponent<HeroKingBehaviour>().attackpoint;
                    break;
            }
		}
	}

	public void SetAllIdleStatus (bool idleStatus) {
		foreach (GameObject entity in entityStorage.activePlayerEntities) {
			string cleanEntity = Regex.Replace(entity.name, @"[\d-]", string.Empty);
            switch (cleanEntity)
            {
                case "Zombie":
                    entity.GetComponent<ZombieBehaviour>().idle = idleStatus;
                    break;
                case "Skeleton":
                    entity.GetComponent<SkeletonBehaviour>().idle = idleStatus;
                    break;
                case "Necromancer":
                    entity.GetComponent<NecromancerBehaviour>().idle = idleStatus;
                    break;
                case "SkeletonArcher":
                    entity.GetComponent<SkeletonArcherBehaviour>().idle = idleStatus;
                    break;
                case "ArmoredSkeleton":
                    entity.GetComponent<ArmoredSkeletonBehaviour>().idle = idleStatus;
                    break;
                case "DeathKnight":
                    entity.GetComponent<DeathKnightBehaviour>().idle = idleStatus;
                    break;
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
            switch (cleanEntity)
            {
                case "Zombie":
                    if (entity.GetComponent<ZombieBehaviour>().currmovementpoint != 0)
                    {
                        return false;
                    }
                    break;
                case "Skeleton":
                    if (entity.GetComponent<SkeletonBehaviour>().currmovementpoint != 0)
                    {
                        return false;
                    }
                    break;
                case "Necromancer":
                    if (entity.GetComponent<NecromancerBehaviour>().currmovementpoint != 0)
                    {
                        return false;
                    }
                    break;
                case "SkeletonArcher":
                    if (entity.GetComponent<SkeletonArcherBehaviour>().currmovementpoint != 0)
                    {
                        return false;
                    }
                    break;
                case "ArmoredSkeleton":
                    if (entity.GetComponent<ArmoredSkeletonBehaviour>().currmovementpoint != 0)
                    {
                        return false;
                    }
                    break;
                case "DeathKnight":
                    if (entity.GetComponent<DeathKnightBehaviour>().currmovementpoint != 0)
                    {
                        return false;
                    }
                    break;


                case "Militia":
                    if (entity.GetComponent<MilitiaBehaviour>().currmovementpoint != 0)
                    {
                        return false;
                    }
                    break;
                case "Archer":
                    if (entity.GetComponent<ArcherBehaviour>().currmovementpoint != 0)
                    {
                        return false;
                    }
                    break;
                case "Longbowman":
                    if (entity.GetComponent<LongbowmanBehaviour>().currmovementpoint != 0)
                    {
                        return false;
                    }
                    break;
                case "Crossbowman":
                    if (entity.GetComponent<CrossbowmanBehaviour>().currmovementpoint != 0)
                    {
                        return false;
                    }
                    break;
                case "Footman":
                    if (entity.GetComponent<FootmanBehaviour>().currmovementpoint != 0)
                    {
                        return false;
                    }
                    break;
                case "MountedKnight":
                    if (entity.GetComponent<MountedKnightBehaviour>().currmovementpoint != 0)
                    {
                        return false;
                    }
                    break;
                case "HeroKing":
                    if (entity.GetComponent<HeroKingBehaviour>().currmovementpoint != 0)
                    {
                        return false;
                    }
                    break;
            }
		}
		return true;
	}

	public bool CheckAllAttack () {
		foreach (GameObject entity in entityStorage.activePlayerEntities) {
            string cleanEntity = Regex.Replace(entity.name, @"[\d-]", string.Empty);
            switch (cleanEntity)
            {
                case "Zombie":
                    if (entity.GetComponent<ZombieBehaviour>().currmovementpoint != 0 || entity.GetComponent<ZombieBehaviour>().currattackpoint != 0)
                    {
                        if (entity.GetComponent<ZombieBehaviour>().idle == false)
                        {
                            return false;
                        }
                    }
                    break;
                case "Skeleton":
                    if (entity.GetComponent<SkeletonBehaviour>().currmovementpoint != 0 || entity.GetComponent<SkeletonBehaviour>().currattackpoint != 0)
                    {
                        if (entity.GetComponent<SkeletonBehaviour>().idle == false)
                        {
                            return false;
                        }
                    }
                    break;
                case "Necromancer":
                    if (entity.GetComponent<NecromancerBehaviour>().currmovementpoint != 0 || entity.GetComponent<NecromancerBehaviour>().currattackpoint != 0)
                    {
                        if (entity.GetComponent<NecromancerBehaviour>().idle == false)
                        {
                            return false;
                        }
                    }
                    break;
                case "SkeletonArcher":
                    if (entity.GetComponent<SkeletonArcherBehaviour>().currmovementpoint != 0 || entity.GetComponent<SkeletonArcherBehaviour>().currattackpoint != 0)
                    {
                        if (entity.GetComponent<SkeletonArcherBehaviour>().idle == false)
                        {
                            return false;
                        }
                    }
                    break;
                case "ArmoredSkeleton":
                    if (entity.GetComponent<ArmoredSkeletonBehaviour>().currmovementpoint != 0 || entity.GetComponent<ArmoredSkeletonBehaviour>().currattackpoint != 0)
                    {
                        if (entity.GetComponent<ArmoredSkeletonBehaviour>().idle == false)
                        {
                            return false;
                        }
                    }
                    break;
                case "DeathKnight":
                    if (entity.GetComponent<DeathKnightBehaviour>().currmovementpoint != 0 || entity.GetComponent<DeathKnightBehaviour>().currattackpoint != 0)
                    {
                        if (entity.GetComponent<DeathKnightBehaviour>().idle == false)
                        {
                            return false;
                        }
                    }
                    break;
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
