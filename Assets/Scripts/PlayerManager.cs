using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class FactionNames
{
    public const string Undead = "Undead";
    public const string Human = "Human";
}

public class PlayerManager : MonoBehaviour {

    //Players are assigned a code for the game to recognize them
    //e.g. CA is player C and is on team A

    public Dictionary<string, string> activePlayersName = new Dictionary<string, string>();
    public Dictionary<string, string> activePlayersFaction = new Dictionary<string, string>();
    public Dictionary<int, string> activePlayersOrder = new Dictionary<int, string>();
    public int currPlayerOrder = 0;
    public string currPlayer = string.Empty;

    public void SetActivePlayers()
    {
        activePlayersName = GameMemento.current.activePlayersName;
        activePlayersFaction = GameMemento.current.activePlayersFaction;
        activePlayersOrder = GameMemento.current.activePlayersOrder;
        currPlayerOrder = GameMemento.current.currPlayerOrder;
        currPlayer = GameMemento.current.currPlayer;

        //TODO throw exception and return players to start screen if activeplayers is empty
        //TEST CODE
        if (activePlayersName.Count == 0)
        {
            activePlayersName.Add("AA", "Lolpolice");
            activePlayersName.Add("BB", "Noob");
            activePlayersName.Add("CA", "Noob2");
        }
        if (activePlayersFaction.Count == 0)
        {
            activePlayersFaction.Add("AA", FactionNames.Undead);
            activePlayersFaction.Add("BB", FactionNames.Human);
            activePlayersFaction.Add("CA", FactionNames.Human);
        }
        if (activePlayersOrder.Count == 0)
        {
            activePlayersOrder.Add(1, "AA");
            activePlayersOrder.Add(2, "BB");
            activePlayersOrder.Add(3, "CA");
        }
        if (string.IsNullOrEmpty(currPlayer) || currPlayerOrder == 0)
        {
            Debug.Log("changed");
            currPlayer = "AA";
            currPlayerOrder = 1;
        }
    }

    public void NextActivePlayer()
    {
        currPlayerOrder++;
        if (currPlayerOrder > activePlayersOrder.Count)
        {
            currPlayerOrder = 1;
        }
        currPlayer = activePlayersOrder[currPlayerOrder];
    }
}
