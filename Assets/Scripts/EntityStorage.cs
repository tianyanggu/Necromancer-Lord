using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EntityStorage : MonoBehaviour {

    public PlayerManager playerManager;

	public List<string> undeadEntities = new List<string> ();
	public List<string> humanEntities = new List<string> ();

    public List<GameObject> activePlayerAEntities = new List<GameObject>();
    public List<GameObject> activePlayerBEntities = new List<GameObject>();
    public List<GameObject> activePlayerCEntities = new List<GameObject>();

    public List<List<GameObject>> activePlayersList = new List<List<GameObject>>();

    void Start () {
		//undead entities
		undeadEntities.Add ("Necromancer");
        undeadEntities.Add ("Skeleton");
        undeadEntities.Add ("Zombie");
        undeadEntities.Add ("SkeletonArcher");
        undeadEntities.Add ("ArmoredSkeleton");
        undeadEntities.Add ("DeathKnight");
		//human entities
		humanEntities.Add ("Militia");
        humanEntities.Add ("Archer");
        humanEntities.Add ("Longbowman");
        humanEntities.Add ("Crossbowman");
        humanEntities.Add ("Footman");
        humanEntities.Add ("MountedKnight");
        humanEntities.Add ("HeroKing");

        //TODO make function to check number of players at start of game and their team
        //add active players to list
        ListActivePlayerEntities ();

        activePlayersList.Add(activePlayerAEntities);
        activePlayersList.Add(activePlayerBEntities);
        activePlayersList.Add(activePlayerCEntities);
    }

    public void ListActivePlayerEntities () {
		foreach (string playerID in playerManager.activePlayers) {
            //get which faction entities needs to be checked for
            foreach (string entity in PlayerFactionList(PlayerPrefs.GetString(playerID)))
            {
                for (int i = 1; i <= 99; i++)
                {
                    string num = i.ToString();
                    GameObject gameEntity = GameObject.Find(playerID + entity + num);
                    if (gameEntity != null)
                    {
                        char playerFirstLetter = playerID[0];
                        PlayerEntityList(playerFirstLetter).Add(gameEntity);
                    }
                }
            }
		}
	}

	//returns faction
	public string WhichFaction(string entity) {
		//------Determine Faction------
        switch (entity)
        {
            case "Zombie":
                return "undead";
            case "Skeleton":
                return "undead";
            case "Necromancer":
                return "undead";
            case "SkeletonArcher":
                return "undead";
            case "ArmoredSkeleton":
                return "undead";
            case "DeathKnight":
                return "undead";

            case "Militia":
                return "human";
            case "Archer":
                return "human";
            case "Longbowman":
                return "human";
            case "Crossbowman":
                return "human";
            case "Footman":
                return "human";
            case "MountedKnight":
                return "human";
            case "HeroKing":
                return "human";
        }
		return "unknown";
	}

    public List<string> PlayerFactionList(string factionName)
    {
        //------Determine Faction Entity List------
        switch (factionName)
        {
            case "undead":
                return undeadEntities;
            case "human":
                return humanEntities;
        }
        return new List<string>();
    }

    public List<GameObject> PlayerEntityList(char playerID)
    {
        //------Determine Faction Entity List------
        switch (playerID)
        {
            case 'A':
                return activePlayerAEntities;
            case 'B':
                return activePlayerBEntities;
            case 'C':
                return activePlayerCEntities;
        }
        return new List<GameObject>();
    }

    //returns summon soul cost
    public int summonSoulCost(string entity) {
        //------Determine Cost------
        switch (entity)
        {
            case "Zombie":
                return 100;
            case "Skeleton":
                return 150;
            case "Necromancer":
                return 10000;
            case "SkeletonArcher":
                return 150;
            case "ArmoredSkeleton":
                return 200;
            case "DeathKnight":
                return 250;
		}
		return 0;
	}
}
