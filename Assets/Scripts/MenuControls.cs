using UnityEngine;
using System.Collections;

public class MenuControls : MonoBehaviour {

    public bool testdisplay = false;
    public Game game;

    void OnGUI()
    {
        if (GUI.Button(new Rect(850, 200, 120, 20), "Save"))
        {
            game.SetMemento();
            SaveLoad.Save();
        }

        if (GUI.Button(new Rect(850, 150, 120, 20), "Load"))
        {
            SaveLoad.Load();
            testdisplay = true;
        }

        if (testdisplay)
        {
            /*
            foreach (GameMemento savedGame in SaveLoad.savedGames)
            {
                GUILayout.TextField(savedGame.gameID, 20);
            }
            GameMemento.current = SaveLoad.savedGames[0];
            */
            GameMemento.current = SaveLoad.savedGame;
            Debug.Log(GameMemento.current.hexGridMemento);
            GUILayout.TextField(GameMemento.current.hexGridTerrainList[0], 20);
            GUILayout.TextField(GameMemento.current.hexGridMemento.size.ToString(), 20);
        }
    }
}
