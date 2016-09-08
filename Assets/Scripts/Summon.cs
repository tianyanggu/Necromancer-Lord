using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

public class Summon : MonoBehaviour {
	public HexGrid hexGrid;
	public LoadMap loadMap;
	public EntityStorage entityStorage;
	public Currency currency;
    public EntityStats entityStats;

    //given an index and the type of summon, summons that entity with the next available name
    public void SummonEntity (int cellindex, string summonname, string playerid) {
		Vector3 summonindex = hexGrid.GetCellPos(cellindex);
		summonindex.y = 0.2f;
		string availableNum = AvailableName (summonname, playerid);
		string availableName = playerid + summonname + availableNum;
		int health = entityStats.GetHealthInfo(summonname);

        //Instantiate the prefab from the resources folder
        GameObject entity = (GameObject)Instantiate(Resources.Load(summonname), summonindex, Quaternion.identity);
        entity.name = availableName;
        char playerChar = playerid[0];
        entityStorage.PlayerEntityList(playerChar).Add(entity);
        hexGrid.SetEntityObject(cellindex, entity);
        hexGrid.SetEntityName(cellindex, availableName);
        loadMap.CreateHealthLabel(cellindex, health, availableName);

        //stores info of new summon to playerprefs for saving
        string ppName = AvailablePlayerPrefsName ();

		PlayerPrefs.SetString ("HexEntity" + ppName, availableName);
		PlayerPrefs.SetString (availableName, "HexEntity" + ppName);
		PlayerPrefs.SetInt ("HexEntityHealth" + ppName, health);
		PlayerPrefs.SetInt ("HexEntityIndex" + ppName, cellindex);
	}

	//Check for next available entity number
	string AvailableName (string summonname, string playerid) {
		for (int i = 1; i <= 999; i++) {
			string num = i.ToString ();
            bool nameExists = false;
            char playerChar = playerid[0];
            foreach (GameObject playerEntity in entityStorage.PlayerEntityList(playerChar))
            {
                if (playerEntity.name == playerid + summonname + num)
                {
                    nameExists = true;
                }
            }
            if (!nameExists)
            {
                return num;
            }
        } 
		return null;
	}

	//Check for next available setstring number
	string AvailablePlayerPrefsName () {
		for (int i = 0; i < hexGrid.size; i++) {
			string num = i.ToString ();
			string allEntity = PlayerPrefs.GetString ("HexEntity" + i);
			if (allEntity == string.Empty) {
				return num;
			}
		}
		return null; //TODO error message if no available spaces, should not be possible to give null
	}

    //TODO for human entities
	public bool ValidSummon(string entity) {
        string faction = entityStorage.WhichFaction(entity);
        switch (faction)
        {
            case "undead":
                int souls = PlayerPrefs.GetInt("Souls");
                int cost = entityStorage.summonSoulCost(entity);
                if (souls >= cost)
                {
                    currency.ChangeSouls(-cost);
                    return true;
                }
                return false;
            case "humans":
                return true;
        }
        return false;
    }

    public void KillEntity(int cellindex)
    {
        string entityName = hexGrid.GetEntityName(cellindex);
        GameObject entityObj = hexGrid.GetEntityObject(cellindex);
        char playerFirstLetter = entityName[0];
        entityStorage.PlayerEntityList(playerFirstLetter).Remove(entityObj);
        Destroy(entityObj);

        hexGrid.SetEntityName(cellindex, "Empty");
        hexGrid.SetEntityObject(cellindex, null);
        GameObject attackerHealthText = GameObject.Find("Health " + entityName);
        Destroy(attackerHealthText);

        //delete from playerprefs
        string playerprefsName = PlayerPrefs.GetString(entityName);
        string playerprefsNum = Regex.Replace(playerprefsName, "[^0-9 -]", string.Empty);
        PlayerPrefs.DeleteKey("HexEntity" + playerprefsNum);
        PlayerPrefs.DeleteKey("HexEntityHealth" + playerprefsNum);
        PlayerPrefs.DeleteKey("HexEntityIndex" + playerprefsNum);
        PlayerPrefs.DeleteKey(entityName);

        //add to corpses if not undead
        string cleanEntity = Regex.Replace(entityName.Substring(2), @"[\d-]", string.Empty);
        if (entityStorage.WhichFaction(cleanEntity) != "undead")
        {
            hexGrid.SetCorpses(cellindex, entityName);
        }
    }
}
