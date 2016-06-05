using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class LoadMap : MonoBehaviour {
	public HexGrid hexGrid;
	public Text sizeLabel;
	public EntityStorage entityStorage;
	Canvas gridCanvas;

	public GameObject Necromancer;
	public GameObject Skeleton;
	public GameObject Militia;
	public GameObject Zombie;

	public void LoadHexTiles () {

		hexGrid.SetSize (12,12);

	}

	public void LoadEntities () {
		gridCanvas = GetComponentInChildren<Canvas>();

		Vector3 start = hexGrid.GetCellPos(14);
		GameObject playerNecromancer = (GameObject)Instantiate (Necromancer, start, Quaternion.identity);
		playerNecromancer.name = "Necromancer1";
		hexGrid.EntityCellIndex (14, "Necromancer1");
		playerNecromancer.GetComponent<NecromancerBehaviour> ().size = 1;
		CreateSizeLabel (14, 1, "Necromancer1");

		Vector3 militiastart = hexGrid.GetCellPos(12);
		GameObject militia1 = (GameObject)Instantiate (Militia, militiastart, Quaternion.identity);
		militia1.name = "Militia1";
		hexGrid.EntityCellIndex (12, "Militia1");
		militia1.GetComponent<MilitiaBehaviour> ().size = 10;
		CreateSizeLabel (12, 10, "Militia1");

		Vector3 militiastart2 = hexGrid.GetCellPos(15);
		GameObject militia2 = (GameObject)Instantiate (Militia, militiastart2, Quaternion.identity);
		militia2.name = "Militia2";
		hexGrid.EntityCellIndex (15, "Militia2");
		militia2.GetComponent<MilitiaBehaviour> ().size = 15;
		CreateSizeLabel (15, 15, "Militia2");

		Vector3 start2 = hexGrid.GetCellPos(3);
		GameObject playerSkeleton = (GameObject)Instantiate (Skeleton, start2, Quaternion.identity);
		playerSkeleton.name = "Skeleton1";
		hexGrid.EntityCellIndex (3, "Skeleton1");
		playerSkeleton.GetComponent<SkeletonBehaviour> ().size = 10;
		CreateSizeLabel (3, 10, "Skeleton1");

		Vector3 start3 = hexGrid.GetCellPos(18);
		GameObject playerZombie = (GameObject)Instantiate (Zombie, start3, Quaternion.identity);
		playerZombie.name = "Zombie1";
		hexGrid.EntityCellIndex (18, "Zombie1");
		playerZombie.GetComponent<ZombieBehaviour> ().size = 10;
		CreateSizeLabel (18, 10, "Zombie1");

		for (int i = 0; i < hexGrid.size; i++) {
			string allEntity = PlayerPrefs.GetString ("HexEntity" + i);
			string cleanEntity = Regex.Replace(allEntity, @"[\d-]", string.Empty);
			int allSize = PlayerPrefs.GetInt ("HexEntitySize" + i);
			int allIndex = PlayerPrefs.GetInt ("HexEntityIndex" + i);

			if (cleanEntity == "Necromancer") {
				Vector3 spawn = hexGrid.GetCellPos(allIndex);
				GameObject pNecromancer = (GameObject)Instantiate (Necromancer, spawn, Quaternion.identity);
				pNecromancer.name = allEntity;
				hexGrid.EntityCellIndex (allIndex, allEntity);
				pNecromancer.GetComponent<NecromancerBehaviour> ().size = allSize;
				CreateSizeLabel (allIndex, allSize, allEntity);
			} else if (cleanEntity == "Zombie") {
				Vector3 spawn = hexGrid.GetCellPos(allIndex);
				GameObject pZombie = (GameObject)Instantiate (Zombie, spawn, Quaternion.identity);
				pZombie.name = allEntity;
				hexGrid.EntityCellIndex (allIndex, allEntity);
				pZombie.GetComponent<ZombieBehaviour> ().size = allSize;
				CreateSizeLabel (allIndex, allSize, allEntity);
			} else if (cleanEntity == "Skeleton") {
				Vector3 spawn = hexGrid.GetCellPos(allIndex);
				GameObject pSkeleton = (GameObject)Instantiate (Skeleton, spawn, Quaternion.identity);
				pSkeleton.name = allEntity;
				hexGrid.EntityCellIndex (allIndex, allEntity);
				pSkeleton.GetComponent<SkeletonBehaviour> ().size = allSize;
				CreateSizeLabel (allIndex, allSize, allEntity);
			} else if (cleanEntity == "Militia") {
				Vector3 spawn = hexGrid.GetCellPos(allIndex);
				GameObject pMilitia = (GameObject)Instantiate (Militia, spawn, Quaternion.identity);
				pMilitia.name = allEntity;
				hexGrid.EntityCellIndex (allIndex, allEntity);
				pMilitia.GetComponent<MilitiaBehaviour> ().size = allSize;
				CreateSizeLabel (allIndex, allSize, allEntity);
			}
		}
	}

	public void CreateSizeLabel (int index, int size, string entity) {
		Text label = Instantiate<Text>(sizeLabel);
		label.rectTransform.SetParent(gridCanvas.transform, false);
		Vector3 sizepos = hexGrid.GetCellPos (index);
		label.rectTransform.anchoredPosition = new Vector2(sizepos.x, sizepos.z);
		//int? sizestring = playerSkeleton.GetComponent<SkeletonBehaviour> ().size;
		label.text = size.ToString();
		label.name = "Size " + entity;
	}

	public void LoadTerrain () {
		
		for (int i = 0; i < hexGrid.size; i++) {
			string allTerrain = PlayerPrefs.GetString ("Hex" + i);

			if (allTerrain == "Grass") {
				hexGrid.ColorCellIndex (i, Color.green);
				hexGrid.TerrainCellIndex (i, "Grass");
			} else if (allTerrain == "Water") {
				hexGrid.ColorCellIndex (i, Color.blue);
				hexGrid.TerrainCellIndex (i, "Water");
			} else if (allTerrain == "Mountain") {
				hexGrid.ColorCellIndex (i, Color.red);
				hexGrid.TerrainCellIndex (i, "Mountain");
			}
		}
	}

	public void LoadBuildings () {

		for (int i = 0; i < hexGrid.size; i++) {
			string allBuildings = PlayerPrefs.GetString ("Hex" + i);

			if (allBuildings == "Village") {
				hexGrid.ColorCellIndex (i, Color.yellow);
				hexGrid.BuildingCellIndex (i, "Village");
			}
		}
	}
}
