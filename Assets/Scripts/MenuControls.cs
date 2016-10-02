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
            GUILayout.TextField(GameMemento.current.gameID, 20);
            Debug.Log(GameMemento.current.hexGridMemento);
            //GUILayout.TextField(GameMemento.current.hexGridMemento.cells[0].terrain, 20);
            GUILayout.TextField(GameMemento.current.hexGridMemento.size.ToString(), 20);
        }
    }
}
