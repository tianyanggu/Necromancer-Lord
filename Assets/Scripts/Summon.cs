using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Summon : MonoBehaviour {
	public HexGrid hexGrid;
	public LoadMap loadMap;
	public EntityStorage entityStorage;
	public Currency currency;

	public GameObject Necromancer;
	public GameObject Skeleton;
	public GameObject Zombie;
    public GameObject SkeletonArcher;
    public GameObject ArmoredSkeleton;
    public GameObject DeathKnight;

    public GameObject Militia;
    public GameObject Archer;
    public GameObject Longbowman;
    public GameObject Crossbowman;
    public GameObject Footman;
    public GameObject MountedKnight;
    public GameObject HeroKing;

	//given an index and the type of summon, summons that entity with the next available name
	public void SummonEntity (int cellindex, string summonname) {
		Vector3 summonindex = hexGrid.GetCellPos(cellindex);
		summonindex.y = 0.2f;
		string faction = entityStorage.whichFaction (summonname);
		string availableNum = AvailableName (summonname, faction);
		string availableName = summonname + availableNum;
		int health = GetHealthInfo (summonname);

        //Instantiate the prefab from the resources folder
        GameObject entity = (GameObject)Instantiate(Resources.Load(summonname), summonindex, Quaternion.identity);
        entity.name = availableName;
        entityStorage.AddActivePlayerEntity(entity);
        hexGrid.SetEntityObject(cellindex, entity);

		//stores info of new summon to playerprefs for saving
		string ppName = AvailablePlayerPrefsName ();

		PlayerPrefs.SetString ("HexEntity" + ppName, availableName);
		PlayerPrefs.SetString (availableName, "HexEntity" + ppName);
		PlayerPrefs.SetInt ("HexEntityHealth" + ppName, health);
		PlayerPrefs.SetInt ("HexEntityIndex" + ppName, cellindex);
		hexGrid.SetEntityName (cellindex, availableName);
        loadMap.CreateHealthLabel (cellindex, health, availableName);
	}

	//Check for next available entity number
	string AvailableName (string summonname, string faction) {
		for (int i = 1; i <= 999; i++) {
			string num = i.ToString ();
			if (faction == "undead") {
                bool nameExists = false;
                foreach (GameObject playerEntity in entityStorage.activePlayerEntities) {
                    if (playerEntity.name == summonname + num)
                    {
                        nameExists = true;
                    }
				}
                if (!nameExists)
                {
                    return num;
                }
            }
			if (faction == "human") {
                bool nameExists = false;
                foreach (GameObject playerEntity in entityStorage.activePlayerEntities)
                {
                    if (playerEntity.name == summonname + num)
                    {
                        nameExists = true;
                    }
                }
                if (!nameExists)
                {
                    return num;
                }
            }
		} 
		return null;
	}

	//Check for next available setstring number
	string AvailablePlayerPrefsName () {
		for (int i = 0; i < hexGrid.size; i++) {
			string num = i.ToString ();
			string allEntity = PlayerPrefs.GetString ("HexEntity" + i);
			if (allEntity == "") {
				return num;
			}
		}
		return null; //TODO error message if no available spaces, should not be possible to give null
	}

	//grabs health info
	int GetHealthInfo(string entity) {
		//------Grab Info Attacker------
        switch (entity)
        {
            case "Zombie":
                return Zombie.GetComponent<ZombieBehaviour>().health;
            case "Skeleton":
                return Skeleton.GetComponent<SkeletonBehaviour>().health;
            case "Necromancer":
                return Necromancer.GetComponent<NecromancerBehaviour>().health;
            case "SkeletonArcher":
                return SkeletonArcher.GetComponent<SkeletonArcherBehaviour>().health;
            case "ArmoredSkeleton":
                return ArmoredSkeleton.GetComponent<ArmoredSkeletonBehaviour>().health;
            case "DeathKnight":
                return DeathKnight.GetComponent<DeathKnightBehaviour>().health;

            case "Militia":
                return Militia.GetComponent<MilitiaBehaviour>().health;
            case "Archer":
                return Archer.GetComponent<ArcherBehaviour>().health;
            case "Longbowman":
                return Longbowman.GetComponent<LongbowmanBehaviour>().health;
            case "Crossbowman":
                return Crossbowman.GetComponent<CrossbowmanBehaviour>().health;
            case "Footman":
                return Footman.GetComponent<FootmanBehaviour>().health;
            case "MountedKnight":
                return MountedKnight.GetComponent<MountedKnightBehaviour>().health;
            case "HeroKing":
                return HeroKing.GetComponent<HeroKingBehaviour>().health;	
		} 
		return 0;
	}

	public bool ValidSummon(string entity) {
		int souls = PlayerPrefs.GetInt ("Souls");
		int cost = entityStorage.summonSoulCost (entity);
		if (souls >= cost) {
            currency.ChangeSouls (-cost);
			return true;
		}
		return false;
	}
}
