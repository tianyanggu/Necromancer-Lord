using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Movement : MonoBehaviour {

	public HexGrid hexGrid;


//	public List<int> GetCellIndexesOneHexAway (int index) {
//		int width = hexGrid.width;
//		int height = hexGrid.height;
//
//		List<int> positions = new List<int> ();
//		//right case
//		if ((index % width) == (width - 1)) {
//			positions.Add (index - 1);
//		}
//		//left case
//		else if ((index % width) == 0) {
//			positions.Add (index + 1);
//		} else {
//			positions.Add (index - 1);
//			positions.Add (index + 1);
//		}
//		//determine borders by even or odd rows
//		int row = index / width;
//		//Odd rows
//		if (row % 2 == 1) {
//			if ((index + width) >= (height * width - 1)) {
//				//upper is empty, only add lower cells
//				if ((index % width) != (width - 1)) {
//					//left side
//					positions.Add (index - width); //lower left
//					positions.Add (index - width + 1); //lower right
//				} else {
//					//right side
//					positions.Add (index - width); //lower left
//				}
//			} else {
//				if ((index % width) != (width - 1)) {
//					//left side
//					positions.Add (index + width); //upper left
//					positions.Add (index + width + 1); //upper right
//					positions.Add (index - width); //lower left
//					positions.Add (index - width + 1); //lower right
//				} else {
//					//right side
//					positions.Add (index + width); //upper left
//					positions.Add (index - width); //lower left
//				}
//			}
//
//		}		
//		if (row % 2 == 0) {
//
//			if ((index + width) >= (height * width - 1)) {
//				//upper is empty, only add lower cells
//				if ((index % width) != (width - 1)) {
//					//left side
//					positions.Add (index - width); //lower right
//				} else {
//					positions.Add (index - width - 1); //lower left
//					positions.Add (index - width); //lower right
//				}
//			} else if (row == 0) {
//				//lower is empty, only add upper cells
//				if ((index % width) != (width - 1)) {
//					//left side
//					positions.Add (index + width); //upper right
//				} else {
//					positions.Add (index + width - 1); //upper left
//					positions.Add (index + width); //upper right
//				}
//			} else {
//				if ((index % width) == 0) {
//					//left side
//					positions.Add (index + width); //upper right
//					positions.Add (index - width); //lower right
//				} else {
//					positions.Add (index + width - 1); //upper left
//					positions.Add (index + width); //upper right
//					positions.Add (index - width - 1); //lower left
//					positions.Add (index - width); //lower right
//				}
//			}
//		}
//		return positions;
//	}


	public List<int> GetCellIndexesOneHexAway (int index) {

		List<int> positions = new List<int> ();
		HexCoordinates coord = hexGrid.GetCellCoord (index);
		int coordx = coord.X;
		int coordz = coord.Z;

		//left and right 
		positions.Add(hexGrid.GetCellIndexFromCoord (coordx - 1, coordz));
		positions.Add(hexGrid.GetCellIndexFromCoord (coordx + 1, coordz));

		//upper left and right
		positions.Add(hexGrid.GetCellIndexFromCoord (coordx - 1, coordz + 1));
		positions.Add(hexGrid.GetCellIndexFromCoord (coordx, coordz + 1));

		//lower left and right
		positions.Add(hexGrid.GetCellIndexFromCoord (coordx, coordz - 1));
		positions.Add(hexGrid.GetCellIndexFromCoord (coordx + 1, coordz - 1));

		return positions;
	}

	public List<int> GetCellIndexesTwoHexAway (int index) {

		List<int> positions = new List<int> ();
		HexCoordinates coord = hexGrid.GetCellCoord (index);
		int coordx = coord.X;
		int coordz = coord.Z;

		//left and right 
		positions.Add(hexGrid.GetCellIndexFromCoord (coordx - 1, coordz));
		positions.Add(hexGrid.GetCellIndexFromCoord (coordx + 1, coordz));
		//far left and right 
		positions.Add(hexGrid.GetCellIndexFromCoord (coordx - 2, coordz));
		positions.Add(hexGrid.GetCellIndexFromCoord (coordx + 2, coordz));

		//one level upper left and right
		positions.Add(hexGrid.GetCellIndexFromCoord (coordx - 1, coordz + 1));
		positions.Add(hexGrid.GetCellIndexFromCoord (coordx, coordz + 1));
		//one level upper far left and right
		positions.Add(hexGrid.GetCellIndexFromCoord (coordx - 2, coordz + 1));
		positions.Add(hexGrid.GetCellIndexFromCoord (coordx + 1, coordz + 1));

		//two level upper center
		positions.Add(hexGrid.GetCellIndexFromCoord (coordx - 1, coordz + 2));
		//two level upper left and right
		positions.Add(hexGrid.GetCellIndexFromCoord (coordx - 2, coordz + 2));
		positions.Add(hexGrid.GetCellIndexFromCoord (coordx, coordz + 2));

		//one level lower left and right
		positions.Add(hexGrid.GetCellIndexFromCoord (coordx, coordz - 1));
		positions.Add(hexGrid.GetCellIndexFromCoord (coordx + 1, coordz - 1));
		//one level lower far left and right
		positions.Add(hexGrid.GetCellIndexFromCoord (coordx - 1, coordz - 1));
		positions.Add(hexGrid.GetCellIndexFromCoord (coordx + 2, coordz - 1));

		//two level lower center
		positions.Add(hexGrid.GetCellIndexFromCoord (coordx + 1, coordz - 2));
		//two level lower left and right
		positions.Add(hexGrid.GetCellIndexFromCoord (coordx, coordz - 2));
		positions.Add(hexGrid.GetCellIndexFromCoord (coordx + 2, coordz - 2));

		return positions;
	}

	public List<int> GetCellIndexesTwoHexAwayBlockers (int index) {

		List<int> positions = new List<int> ();
		HexCoordinates coord = hexGrid.GetCellCoord (index);
		int coordx = coord.X;
		int coordz = coord.Z;

		//left and right
		positions.Add(hexGrid.GetCellIndexFromCoord (coordx - 1, coordz));
		int left = hexGrid.GetCellIndexFromCoord (coordx - 1, coordz);
		positions.Add(hexGrid.GetCellIndexFromCoord (coordx + 1, coordz));
		int right = hexGrid.GetCellIndexFromCoord (coordx + 1, coordz);
		//far left and right 
		positions.Add(hexGrid.GetCellIndexFromCoord (coordx - 2, coordz));
		positions.Add(hexGrid.GetCellIndexFromCoord (coordx + 2, coordz));

		//one level upper left and right
		int uleft = hexGrid.GetCellIndexFromCoord (coordx - 1, coordz + 1);
		positions.Add(hexGrid.GetCellIndexFromCoord (coordx - 1, coordz + 1));
		int uright = hexGrid.GetCellIndexFromCoord (coordx, coordz + 1);
		positions.Add(hexGrid.GetCellIndexFromCoord (coordx, coordz + 1));
		//one level upper far left and right
		int ufleft = hexGrid.GetCellIndexFromCoord (coordx - 2, coordz + 1);
//		if (hexGrid.GetTerrain (left) != "Mountain" || hexGrid.GetTerrain (uleft) != "Mountain") {
//			if (hexGrid.GetEntity (left) == "Empty" || hexGrid.GetEntity (uleft) == "Empty") {
				positions.Add (hexGrid.GetCellIndexFromCoord (coordx - 2, coordz + 1));
//			}
//		}
		positions.Add(hexGrid.GetCellIndexFromCoord (coordx + 1, coordz + 1));
		//two level upper center
		positions.Add(hexGrid.GetCellIndexFromCoord (coordx - 1, coordz + 2));
		//two level upper left and right
		int uuleft = hexGrid.GetCellIndexFromCoord (coordx - 2, coordz + 2);
//		if (hexGrid.GetTerrain (uuleft) != "Mountain" && hexGrid.GetTerrain (uleft) != "Mountain") {
//			if (hexGrid.GetEntity (uleft) == "Empty") {
				positions.Add (hexGrid.GetCellIndexFromCoord (coordx - 2, coordz + 2));
//			}
//		}
		int uuright = hexGrid.GetCellIndexFromCoord (coordx, coordz + 2);
//		if (hexGrid.GetTerrain (uuright) != "Mountain" && hexGrid.GetTerrain (uright) != "Mountain") {
//			if (hexGrid.GetEntity (uright) == "Empty") {
				positions.Add (hexGrid.GetCellIndexFromCoord (coordx, coordz + 2));
//			}
//		}

		//one level lower left and right
		positions.Add(hexGrid.GetCellIndexFromCoord (coordx, coordz - 1));
		int lleft = hexGrid.GetCellIndexFromCoord (coordx, coordz - 1);
		positions.Add(hexGrid.GetCellIndexFromCoord (coordx + 1, coordz - 1));
		int lright = hexGrid.GetCellIndexFromCoord (coordx + 1, coordz - 1);
		//one level lower far left and right
		positions.Add(hexGrid.GetCellIndexFromCoord (coordx - 1, coordz - 1));
		positions.Add(hexGrid.GetCellIndexFromCoord (coordx + 2, coordz - 1));

		//two level lower center
		positions.Add(hexGrid.GetCellIndexFromCoord (coordx + 1, coordz - 2));
		//two level lower left and right

		positions.Add(hexGrid.GetCellIndexFromCoord (coordx, coordz - 2));
		positions.Add(hexGrid.GetCellIndexFromCoord (coordx + 2, coordz - 2));

		return positions;
	}
}
