using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EntityStorage : MonoBehaviour {

    public PlayerManager playerManager;
    public EntityStats entityStats;

    public List<GameObject> activePlayerAEntities = new List<GameObject>();
    public List<GameObject> activePlayerBEntities = new List<GameObject>();
    public List<GameObject> activePlayerCEntities = new List<GameObject>();

    public List<List<string>> factionEntityList = new List<List<string>>();
    public List<List<GameObject>> activePlayersEntityList = new List<List<GameObject>>();

    void Start () {
        ListActivePlayerEntities ();

        factionEntityList.Add(entityStats.humanEntities);
        factionEntityList.Add(entityStats.undeadEntities);

        activePlayersEntityList.Add(activePlayerAEntities);
        activePlayersEntityList.Add(activePlayerBEntities);
        activePlayersEntityList.Add(activePlayerCEntities);
    }

    public void ListActivePlayerEntities () {
		foreach (var player in playerManager.activePlayersFaction) {
            string playerID = player.Key;
            //get which faction entities needs to be checked for
            foreach (string entity in EntityFactionLists(player.Value))
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

    public List<string> EntityFactionLists(string factionName)
    {
        //------Determine Faction Entity List------
        switch (factionName)
        {
            case "undead":
                return entityStats.undeadEntities;
            case "human":
                return entityStats.humanEntities;
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
}
