using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Locate : MonoBehaviour {

	private List<string> playerEntities = new List<string> ();
	private List<string> enemyEntities = new List<string> ();

	//		else if (entity == "Militia") {
	//			GameObject gameEntity = GameObject.Find (entity);
	//			gameEntity.GetComponent<MilitiaBehaviour> ().currmovementpoint = gameEntity.GetComponent<MilitiaBehaviour> ().movementpoint;
	//		}

	public void SetAllMovementPoints () {
		//player controlled entities
		playerEntities.Add ("Necromancer");
		playerEntities.Add ("Skeleton");
		playerEntities.Add ("Zombie");

		foreach (string entity in playerEntities) {
			for (int i = 1; i <= 99; i++) {
				string num = i.ToString ();
				if (GameObject.Find (entity) != null) {
					if (entity == "Necromancer") {
						GameObject gameEntity = GameObject.Find (entity);
						gameEntity.GetComponent<NecromancerBehaviour> ().currmovementpoint = gameEntity.GetComponent<NecromancerBehaviour> ().movementpoint;
					} else if (entity == "Skeleton") {
						GameObject gameEntity = GameObject.Find (entity);
						gameEntity.GetComponent<SkeletonBehaviour> ().currmovementpoint = gameEntity.GetComponent<SkeletonBehaviour> ().movementpoint;
					} else if (entity == "Zombie") {
						GameObject gameEntity = GameObject.Find (entity);
						gameEntity.GetComponent<ZombieBehaviour> ().currmovementpoint = gameEntity.GetComponent<ZombieBehaviour> ().movementpoint;
					}
				} else if (GameObject.Find (entity + num) != null) {
					if (entity == "Necromancer") {
						GameObject gameEntity = GameObject.Find (entity + num);
						gameEntity.GetComponent<NecromancerBehaviour> ().currmovementpoint = gameEntity.GetComponent<NecromancerBehaviour> ().movementpoint;
					} else if (entity == "Skeleton") {
						GameObject gameEntity = GameObject.Find (entity + num);
						gameEntity.GetComponent<SkeletonBehaviour> ().currmovementpoint = gameEntity.GetComponent<SkeletonBehaviour> ().movementpoint;
					} else if (entity == "Zombie") {
						GameObject gameEntity = GameObject.Find (entity + num);
						gameEntity.GetComponent<ZombieBehaviour> ().currmovementpoint = gameEntity.GetComponent<ZombieBehaviour> ().movementpoint;
					}
				}
			}
		}
	}

	public void SetAllAttackPoints () {
		//player controlled entities
		playerEntities.Add ("Necromancer");
		playerEntities.Add ("Skeleton");
		playerEntities.Add ("Zombie");

		foreach (string entity in playerEntities) {
			for (int i = 1; i <= 99; i++) {
				string num = i.ToString ();
				if (GameObject.Find (entity) != null) {
					if (entity == "Necromancer") {
						GameObject gameEntity = GameObject.Find (entity);
						gameEntity.GetComponent<NecromancerBehaviour> ().currattackpoint = gameEntity.GetComponent<NecromancerBehaviour> ().attackpoint;
					} else if (entity == "Skeleton") {
						GameObject gameEntity = GameObject.Find (entity);
						gameEntity.GetComponent<SkeletonBehaviour> ().currattackpoint = gameEntity.GetComponent<SkeletonBehaviour> ().attackpoint;
					} else if (entity == "Zombie") {
						GameObject gameEntity = GameObject.Find (entity);
						gameEntity.GetComponent<ZombieBehaviour> ().currattackpoint = gameEntity.GetComponent<ZombieBehaviour> ().attackpoint;
					}
				} else if (GameObject.Find (entity + num) != null) {
					if (entity == "Necromancer") {
						GameObject gameEntity = GameObject.Find (entity + num);
						gameEntity.GetComponent<NecromancerBehaviour> ().currattackpoint = gameEntity.GetComponent<NecromancerBehaviour> ().attackpoint;
					} else if (entity == "Skeleton") {
						GameObject gameEntity = GameObject.Find (entity + num);
						gameEntity.GetComponent<SkeletonBehaviour> ().currattackpoint = gameEntity.GetComponent<SkeletonBehaviour> ().attackpoint;
					} else if (entity == "Zombie") {
						GameObject gameEntity = GameObject.Find (entity + num);
						gameEntity.GetComponent<ZombieBehaviour> ().currattackpoint = gameEntity.GetComponent<ZombieBehaviour> ().attackpoint;
					}
				}
			}
		}
	}

	public void SetAllIdle () {
		//player controlled entities
		playerEntities.Add ("Necromancer");
		playerEntities.Add ("Skeleton");
		playerEntities.Add ("Zombie");

		foreach (string entity in playerEntities) {
			for (int i = 1; i <= 99; i++) {
				string num = i.ToString ();
				if (GameObject.Find (entity) != null) {
					if (entity == "Necromancer") {
						GameObject gameEntity = GameObject.Find (entity);
						gameEntity.GetComponent<NecromancerBehaviour> ().idle = true;
					} else if (entity == "Skeleton") {
						GameObject gameEntity = GameObject.Find (entity);
						gameEntity.GetComponent<SkeletonBehaviour> ().idle = true;
					} else if (entity == "Zombie") {
						GameObject gameEntity = GameObject.Find (entity);
						gameEntity.GetComponent<ZombieBehaviour> ().idle = true;
					}
				} else if (GameObject.Find (entity + num) != null) {
					if (entity == "Necromancer") {
						GameObject gameEntity = GameObject.Find (entity + num);
						gameEntity.GetComponent<NecromancerBehaviour> ().idle = true;
					} else if (entity == "Skeleton") {
						GameObject gameEntity = GameObject.Find (entity + num);
						gameEntity.GetComponent<SkeletonBehaviour> ().idle = true;
					} else if (entity == "Zombie") {
						GameObject gameEntity = GameObject.Find (entity + num);
						gameEntity.GetComponent<ZombieBehaviour> ().idle = true;
					}
				}
			}
		}
	}

	public void SetAllActive () {
		//player controlled entities
		playerEntities.Add ("Necromancer");
		playerEntities.Add ("Skeleton");
		playerEntities.Add ("Zombie");

		foreach (string entity in playerEntities) {
			for (int i = 1; i <= 99; i++) {
				string num = i.ToString ();
				if (GameObject.Find (entity) != null) {
					if (entity == "Necromancer") {
						GameObject gameEntity = GameObject.Find (entity);
						gameEntity.GetComponent<NecromancerBehaviour> ().idle = false;
					} else if (entity == "Skeleton") {
						GameObject gameEntity = GameObject.Find (entity);
						gameEntity.GetComponent<SkeletonBehaviour> ().idle = false;
					} else if (entity == "Zombie") {
						GameObject gameEntity = GameObject.Find (entity);
						gameEntity.GetComponent<ZombieBehaviour> ().idle = false;
					}
				} else if (GameObject.Find (entity + num) != null) {
					if (entity == "Necromancer") {
						GameObject gameEntity = GameObject.Find (entity + num);
						gameEntity.GetComponent<NecromancerBehaviour> ().idle = false;
					} else if (entity == "Skeleton") {
						GameObject gameEntity = GameObject.Find (entity + num);
						gameEntity.GetComponent<SkeletonBehaviour> ().idle = false;
					} else if (entity == "Zombie") {
						GameObject gameEntity = GameObject.Find (entity + num);
						gameEntity.GetComponent<ZombieBehaviour> ().idle = false;
					}
				}
			}
		}
	}

	public bool CheckAllMovementPoints () {
		//player controlled entities
		playerEntities.Add ("Necromancer");
		playerEntities.Add ("Skeleton");
		playerEntities.Add ("Zombie");

		foreach (string entity in playerEntities) {
			for (int i = 1; i <= 99; i++) {
				string num = i.ToString ();
				if (GameObject.Find (entity) != null) {
					if (entity == "Necromancer") {
						GameObject gameEntity = GameObject.Find (entity);
						if (gameEntity.GetComponent<NecromancerBehaviour> ().currmovementpoint != 0) {
							return false;
						}
					} else if (entity == "Skeleton") {
						GameObject gameEntity = GameObject.Find (entity);
						if (gameEntity.GetComponent<SkeletonBehaviour> ().currmovementpoint != 0) {
							return false;
						}
					} else if (entity == "Zombie") {
						GameObject gameEntity = GameObject.Find (entity);
						if (gameEntity.GetComponent<ZombieBehaviour> ().currmovementpoint != 0) {
							return false;
						}
					}
				} else if (GameObject.Find (entity + num) != null) {
					if (entity == "Necromancer") {
						GameObject gameEntity = GameObject.Find (entity + num);
						if (gameEntity.GetComponent<NecromancerBehaviour> ().currmovementpoint != 0) {
							return false;
						}
					} else if (entity == "Skeleton") {
						GameObject gameEntity = GameObject.Find (entity + num);
						if (gameEntity.GetComponent<SkeletonBehaviour> ().currmovementpoint != 0) {
							return false;
						}
					} else if (entity == "Zombie") {
						GameObject gameEntity = GameObject.Find (entity + num);
						if (gameEntity.GetComponent<ZombieBehaviour> ().currmovementpoint != 0) {
							return false;
						}
					}
				}
			}
		}
		return true;
	}

	public bool CheckAll () {
		//player controlled entities
		playerEntities.Add ("Necromancer");
		playerEntities.Add ("Skeleton");
		playerEntities.Add ("Zombie");

		foreach (string entity in playerEntities) {
			for (int i = 1; i <= 99; i++) {
				string num = i.ToString ();
				if (GameObject.Find (entity) != null) {
					if (entity == "Necromancer") {
						GameObject gameEntity = GameObject.Find (entity);
						if (gameEntity.GetComponent<NecromancerBehaviour> ().currmovementpoint != 0 || gameEntity.GetComponent<NecromancerBehaviour> ().currattackpoint != 0) {
							if (gameEntity.GetComponent<NecromancerBehaviour> ().idle == false) {
								return false;
							}
						}
					} else if (entity == "Skeleton") {
						GameObject gameEntity = GameObject.Find (entity);
						if (gameEntity.GetComponent<SkeletonBehaviour> ().currmovementpoint != 0 || gameEntity.GetComponent<SkeletonBehaviour> ().currattackpoint != 0) {
							if (gameEntity.GetComponent<SkeletonBehaviour> ().idle == false) { 
								return false;
							}
						}
					} else if (entity == "Zombie") {
						GameObject gameEntity = GameObject.Find (entity);
						if (gameEntity.GetComponent<ZombieBehaviour> ().currmovementpoint != 0 || gameEntity.GetComponent<ZombieBehaviour> ().currattackpoint != 0) {
							if (gameEntity.GetComponent<ZombieBehaviour> ().idle == false) {
								return false;
							}
						}
					}
				} else if (GameObject.Find (entity + num) != null) {
					if (entity == "Necromancer") {
						GameObject gameEntity = GameObject.Find (entity + num);
						if (gameEntity.GetComponent<NecromancerBehaviour> ().currmovementpoint != 0 || gameEntity.GetComponent<NecromancerBehaviour> ().currattackpoint != 0) {
							if (gameEntity.GetComponent<NecromancerBehaviour> ().idle == false) {
								return false;
							}
						}
					} else if (entity == "Skeleton") {
						GameObject gameEntity = GameObject.Find (entity + num);
						if (gameEntity.GetComponent<SkeletonBehaviour> ().currmovementpoint != 0 || gameEntity.GetComponent<SkeletonBehaviour> ().currattackpoint != 0) {
							if (gameEntity.GetComponent<SkeletonBehaviour> ().idle == false) {
								return false;
							}
						}
					} else if (entity == "Zombie") {
						GameObject gameEntity = GameObject.Find (entity + num);
						if (gameEntity.GetComponent<ZombieBehaviour> ().currmovementpoint != 0 || gameEntity.GetComponent<ZombieBehaviour> ().currattackpoint != 0) {
							if (gameEntity.GetComponent<ZombieBehaviour> ().idle == false) {
								return false;
							}
						}
					}
				}
			}
		}
		return true;
	}
}
