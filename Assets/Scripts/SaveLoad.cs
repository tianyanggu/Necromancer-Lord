using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveLoad {

    //TODO select from list of savedGames
    //public static List<GameMemento> savedGames = new List<GameMemento>();
    public static GameMemento savedGame = new GameMemento();

    public static void Save()
    {
        //TODO add function to allow change names
        SaveLoad.savedGame = GameMemento.current;
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/savedGames.gd");
        bf.Serialize(file, SaveLoad.savedGame);
        file.Close();
    }

    public static void Load()
    {
        //displays files and their names
        string filepath = Application.persistentDataPath;
        DirectoryInfo d = new DirectoryInfo(filepath);
        foreach (var file in d.GetFiles("*.gd"))
        {
            Debug.Log(file.Name); 
        }

        //TODO after user selects the file, load the game
        if (File.Exists(Application.persistentDataPath + "/savedGames.gd"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/savedGames.gd", FileMode.Open);
            SaveLoad.savedGame = (GameMemento)bf.Deserialize(file);
            file.Close();
        }
    }
}
