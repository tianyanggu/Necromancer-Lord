using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class AIBehaviour : MonoBehaviour {

	public Movement movement;
	public HexGrid hexGrid;

	private List<string> playerEntities = new List<string> ();
	private List<string> enemyEntities = new List<string> ();

	private List<string> nearbyPlayerEntities = new List<string> ();
	private List<int> nearbyPlayerEntitiesDistance = new List<int> ();
	private List<int> nearbyPlayerEntitiesSize = new List<int> ();

	// Use this for initialization
	public List<string> ScanEntities (int index) {
		playerEntities.Add ("Necromancer");
		playerEntities.Add ("Skeleton");
		playerEntities.Add ("Zombie");

		ScanEntitiesHelper (index, 3, 0);
		List<string> scan = nearbyPlayerEntities;

		return scan;
	}

	public void ScanEntitiesHelper (int index, int maxDistance, int usedDistance) {
		if (maxDistance > 0) {
			HexCoordinates coord = hexGrid.GetCellCoord (index);
			int coordx = coord.X;
			int coordz = coord.Z;

			int left = hexGrid.GetCellIndexFromCoord (coordx - 1, coordz);
			string leftEntity = hexGrid.GetEntity(index);
			string cleanleftEntity = Regex.Replace(leftEntity, @"[\d-]", string.Empty);
			if (playerEntities.Contains(cleanleftEntity)) {
				nearbyPlayerEntities.Add (leftEntity);
				nearbyPlayerEntitiesDistance.Add (usedDistance + 1);
			}

			int right = hexGrid.GetCellIndexFromCoord (coordx + 1, coordz);
			string rightEntity = hexGrid.GetEntity(index);
			string cleanrightEntity = Regex.Replace(rightEntity, @"[\d-]", string.Empty);
			if (playerEntities.Contains(cleanrightEntity)) {
				nearbyPlayerEntities.Add (rightEntity);
				nearbyPlayerEntitiesDistance.Add (usedDistance + 1);
			}

			int uleft = hexGrid.GetCellIndexFromCoord (coordx - 1, coordz + 1);
			string uleftEntity = hexGrid.GetEntity(index);
			string cleanuleftEntity = Regex.Replace(uleftEntity, @"[\d-]", string.Empty);
			if (playerEntities.Contains(cleanuleftEntity)) {
				nearbyPlayerEntities.Add (uleftEntity);
				nearbyPlayerEntitiesDistance.Add (usedDistance + 1);
			}

			int uright = hexGrid.GetCellIndexFromCoord (coordx, coordz + 1);
			string urightEntity = hexGrid.GetEntity(index);
			string cleanurightEntity = Regex.Replace(urightEntity, @"[\d-]", string.Empty);
			if (playerEntities.Contains(cleanurightEntity)) {
				nearbyPlayerEntities.Add (urightEntity);
				nearbyPlayerEntitiesDistance.Add (usedDistance + 1);
			}

			int lleft = hexGrid.GetCellIndexFromCoord (coordx, coordz - 1);
			string lleftEntity = hexGrid.GetEntity(index);
			string cleanlleftEntity = Regex.Replace(lleftEntity, @"[\d-]", string.Empty);
			if (playerEntities.Contains(cleanlleftEntity)) {
				nearbyPlayerEntities.Add (lleftEntity);
				nearbyPlayerEntitiesDistance.Add (usedDistance + 1);
			}

			int lright = hexGrid.GetCellIndexFromCoord (coordx + 1, coordz - 1);
			string lrightEntity = hexGrid.GetEntity(index);
			string cleanlrightEntity = Regex.Replace(lrightEntity, @"[\d-]", string.Empty);
			if (playerEntities.Contains(cleanlrightEntity)) {
				nearbyPlayerEntities.Add (lrightEntity);
				nearbyPlayerEntitiesDistance.Add (usedDistance + 1);
			}

			//left and right 
			if (left >= 0) {
				if (hexGrid.GetEntity (left) == "Empty") {
					if (hexGrid.GetTerrain (left) == "Mountain") {
						int newmovementpoints = maxDistance - 2;
						int newusedmovementpoints = usedDistance + 2;
						ScanEntitiesHelper (left, newmovementpoints, newusedmovementpoints);
					} else {
						int newmovementpoints = maxDistance - 1;
						int newusedmovementpoints = usedDistance + 1;
						ScanEntitiesHelper (left, newmovementpoints, newusedmovementpoints);
					}
				}
			}
			if (right >= 0) {
				if (hexGrid.GetEntity (right) == "Empty") {
					if (hexGrid.GetTerrain (right) == "Mountain") {
						int newmovementpoints = maxDistance - 2;
						int newusedmovementpoints = usedDistance + 2;
						ScanEntitiesHelper (right, newmovementpoints, newusedmovementpoints);
					} else {
						int newmovementpoints = maxDistance - 1;
						int newusedmovementpoints = usedDistance + 1;
						ScanEntitiesHelper (right, newmovementpoints, newusedmovementpoints);
					}
				}
			}

			//upper left and right
			if (uleft >= 0) {
				if (hexGrid.GetEntity (uleft) == "Empty") {
					if (hexGrid.GetTerrain (uleft) == "Mountain") {
						int newmovementpoints = maxDistance - 2;
						int newusedmovementpoints = usedDistance + 2;
						ScanEntitiesHelper (uleft, newmovementpoints, newusedmovementpoints);
					} else {
						int newmovementpoints = maxDistance - 1;
						int newusedmovementpoints = usedDistance + 1;
						ScanEntitiesHelper (uleft, newmovementpoints, newusedmovementpoints);
					}
				}
			}
			if (uright >= 0) {
				if (hexGrid.GetEntity (uright) == "Empty") {
					if (hexGrid.GetTerrain (uright) == "Mountain") {
						int newmovementpoints = maxDistance - 2;
						int newusedmovementpoints = usedDistance + 2;
						ScanEntitiesHelper (uright, newmovementpoints, newusedmovementpoints);
					} else {
						int newmovementpoints = maxDistance - 1;
						int newusedmovementpoints = usedDistance + 1;
						ScanEntitiesHelper (uright, newmovementpoints, newusedmovementpoints);
					}
				}
			}

			//lower left and right
			if (lleft >= 0) {
				if (hexGrid.GetEntity (lleft) == "Empty") {
					if (hexGrid.GetTerrain (lleft) == "Mountain") {
						int newmovementpoints = maxDistance - 2;
						int newusedmovementpoints = usedDistance + 2;
						ScanEntitiesHelper (lleft, newmovementpoints, newusedmovementpoints);
					} else {
						int newmovementpoints = maxDistance - 1;
						int newusedmovementpoints = usedDistance + 1;
						ScanEntitiesHelper (lleft, newmovementpoints, newusedmovementpoints);
					}
				}
			}
			if (lright >= 0) {
				if (hexGrid.GetEntity (lright) == "Empty") {
					if (hexGrid.GetTerrain (lright) == "Mountain") {
						int newmovementpoints = maxDistance - 2;
						int newusedmovementpoints = usedDistance + 2;
						ScanEntitiesHelper (lright, newmovementpoints, newusedmovementpoints);
					} else {
						int newmovementpoints = maxDistance - 1;
						int newusedmovementpoints = usedDistance + 1;
						ScanEntitiesHelper (lright, newmovementpoints, newusedmovementpoints);
					}
				}
			}
		}
	}
	
	// attack player entity on pindex
	public void Attack (int eindex, int pindex) {
		enemyEntities.Add ("Militia");
		string eEntity = hexGrid.GetEntity(eindex);
		//TODO attack
	}
		
	// given list of player entities and their size, decide if attack and which
	public void DecideAttack (int eindex, List<int> plist, List<int> psize) {
		enemyEntities.Add ("Militia");
		string eEntity = hexGrid.GetEntity(eindex);
		//TODO DecideAttack
	}
}
