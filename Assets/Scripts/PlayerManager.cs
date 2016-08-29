using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerManager : MonoBehaviour {

    public List<string> activePlayers = new List<string>();
    public string currPlayer;

    // Use this for initialization
    void Start () {
        ListActivePlayers();
        currPlayer = activePlayers[0];
    }
	
    public void ListActivePlayers()
    {
        for (int i = 1; i <= 26; i++)
        {
            string checkActivePlayer = PlayerPrefs.GetString("Player" + i);
            if (checkActivePlayer != string.Empty)
            {
                activePlayers.Add(checkActivePlayer);
            }
        }
    }

    public void NextActivePlayer()
    {
        for (int i = 0; i <= 26; i++){
            if (currPlayer == activePlayers[i]) {
                currPlayer = activePlayers[i + 1];
                return;
            }
        }
        currPlayer = activePlayers[0];
    }
}
