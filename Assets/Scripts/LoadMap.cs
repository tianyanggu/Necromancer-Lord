using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class LoadMap : MonoBehaviour {
	public HexGrid hexGrid;
	public Text healthLabel;
	public EntityStorage entityStorage;
	public Resources resources;
	Canvas gridCanvas;

	public GameObject Necromancer;
	public GameObject Skeleton;
	public GameObject Militia;
	public GameObject Zombie;
	public GameObject Village;
	public GameObject Necropolis;

	public GameObject Corpses;


	public void LoadHexTiles () {
		hexGrid.SetSize (12,12);
	}

	public void LoadResources () {
		int corpseAmount = PlayerPrefs.GetInt ("Corpses");
		resources.SetCorpses (corpseAmount);
	}

	public void LoadEntities () {
		gridCanvas = GetComponentInChildren<Canvas>();

		Vector3 start = hexGrid.GetCellPos(14);
		start.y = 0.2f;
		GameObject playerNecromancer = (GameObject)Instantiate (Necromancer, start, Quaternion.identity);
		playerNecromancer.name = "Necromancer1";
		hexGrid.EntityCellIndex (14, "Necromancer1");
		int ncurrhealth = playerNecromancer.GetComponent<NecromancerBehaviour> ().health;
		CreateHealthLabel (14, ncurrhealth, "Necromancer1");

		Vector3 militiastart = hexGrid.GetCellPos(12);
		militiastart.y = 0.2f;
		GameObject militia1 = (GameObject)Instantiate (Militia, militiastart, Quaternion.identity);
		militia1.name = "Militia1";
		hexGrid.EntityCellIndex (12, "Militia1");
		int mcurrhealth = militia1.GetComponent<MilitiaBehaviour> ().health;
		CreateHealthLabel (12, mcurrhealth, "Militia1");

		Vector3 militiastart2 = hexGrid.GetCellPos(15);
		militiastart2.y = 0.2f;
		GameObject militia2 = (GameObject)Instantiate (Militia, militiastart2, Quaternion.identity);
		militia2.name = "Militia2";
		hexGrid.EntityCellIndex (15, "Militia2");
		int m2currhealth = militia2.GetComponent<MilitiaBehaviour> ().health;
		CreateHealthLabel (15, m2currhealth, "Militia2");

		Vector3 start2 = hexGrid.GetCellPos(3);
		start2.y = 0.2f;
		GameObject playerSkeleton = (GameObject)Instantiate (Skeleton, start2, Quaternion.identity);
		playerSkeleton.name = "Skeleton1";
		hexGrid.EntityCellIndex (3, "Skeleton1");
		int scurrhealth = playerSkeleton.GetComponent<SkeletonBehaviour> ().health;
		CreateHealthLabel (3, scurrhealth, "Skeleton1");

		Vector3 start3 = hexGrid.GetCellPos(18);
		start3.y = 0.2f;
		GameObject playerZombie = (GameObject)Instantiate (Zombie, start3, Quaternion.identity);
		playerZombie.name = "Zombie1";
		hexGrid.EntityCellIndex (18, "Zombie1");
		int zcurrhealth = playerZombie.GetComponent<ZombieBehaviour> ().health;
		CreateHealthLabel (18, zcurrhealth, "Zombie1");

		Vector3 build1 = hexGrid.GetCellPos(15);
		build1.y = 0.1f;
		GameObject eVillage = (GameObject)Instantiate (Village, build1, Quaternion.Euler(90,0,0));
		eVillage.name = "Village1";
		hexGrid.BuildingCellIndex (15, "Village1");

		Vector3 build2 = hexGrid.GetCellPos(14);
		build2.y = 0.1f;
		GameObject pNecropolis = (GameObject)Instantiate (Necropolis, build2, Quaternion.Euler(90,0,0));
		pNecropolis.name = "Necropolis1";
		hexGrid.BuildingCellIndex (14, "Necropolis1");

		for (int i = 0; i < hexGrid.size; i++) {
			string allEntity = PlayerPrefs.GetString ("HexEntity" + i);
			int allHealth = PlayerPrefs.GetInt ("HexEntityHealth" + i);
			string cleanEntity = Regex.Replace(allEntity, @"[\d-]", string.Empty);
			int allIndex = PlayerPrefs.GetInt ("HexEntityIndex" + i);

			if (cleanEntity == "Necromancer") {
				Vector3 spawn = hexGrid.GetCellPos(allIndex);
				spawn.y = 0.2f;
				GameObject pNecromancer = (GameObject)Instantiate (Necromancer, spawn, Quaternion.identity);
				pNecromancer.name = allEntity;
				hexGrid.EntityCellIndex (allIndex, allEntity);
				pNecromancer.GetComponent<NecromancerBehaviour> ().lasthealth = allHealth;
				CreateHealthLabel (allIndex, allHealth, allEntity);
			} else if (cleanEntity == "Zombie") {
				Vector3 spawn = hexGrid.GetCellPos(allIndex);
				spawn.y = 0.2f;
				GameObject pZombie = (GameObject)Instantiate (Zombie, spawn, Quaternion.identity);
				pZombie.name = allEntity;
				hexGrid.EntityCellIndex (allIndex, allEntity);
				pZombie.GetComponent<ZombieBehaviour> ().lasthealth = allHealth;
				CreateHealthLabel (allIndex, allHealth, allEntity);
			} else if (cleanEntity == "Skeleton") {
				Vector3 spawn = hexGrid.GetCellPos(allIndex);
				spawn.y = 0.2f;
				GameObject pSkeleton = (GameObject)Instantiate (Skeleton, spawn, Quaternion.identity);
				pSkeleton.name = allEntity;
				hexGrid.EntityCellIndex (allIndex, allEntity);
				pSkeleton.GetComponent<SkeletonBehaviour> ().lasthealth = allHealth;
				CreateHealthLabel (allIndex, allHealth, allEntity);
			} else if (cleanEntity == "Militia") {
				Vector3 spawn = hexGrid.GetCellPos(allIndex);
				spawn.y = 0.2f;
				GameObject pMilitia = (GameObject)Instantiate (Militia, spawn, Quaternion.identity);
				pMilitia.name = allEntity;
				hexGrid.EntityCellIndex (allIndex, allEntity);
				pMilitia.GetComponent<MilitiaBehaviour> ().lasthealth = allHealth;
				CreateHealthLabel (allIndex, allHealth, allEntity);
			}
		}
	}

	public void CreateHealthLabel (int index, int health, string entity) {
		Text label = Instantiate<Text>(healthLabel);
		label.rectTransform.SetParent(gridCanvas.transform, false);
		Vector3 healthpos = hexGrid.GetCellPos (index);
		label.rectTransform.anchoredPosition = new Vector2(healthpos.x, healthpos.z);
		label.text = health.ToString();
		label.name = "Health " + entity;
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
			} else if (allBuildings == "Necropolis") {
				hexGrid.ColorCellIndex (i, Color.black);
				hexGrid.BuildingCellIndex (i, "Necropolis");
			}
		}
	}
}
