using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class GameSave : MonoBehaviour
{
    public Inventory myInventory;

    public void SaveGame()
    {
        Debug.Log(Application.persistentDataPath);
        if (!Directory.Exists(Application.persistentDataPath + "/GameSaveData"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/GameSaveData");
        }

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/GameSaveData/Inventory.txt");

        var json = JsonUtility.ToJson(myInventory);

        formatter.Serialize(file, json);
        file.Close();
    }

    public void LoadGame()
    {
        BinaryFormatter bf = new BinaryFormatter();
        if (File.Exists(Application.persistentDataPath + "/GameSaveData/Inventory.txt"))
        {
            FileStream file = File.Open(Application.persistentDataPath + "/GameSaveData/Inventory.txt", FileMode.Open);

            JsonUtility.FromJsonOverwrite((string)bf.Deserialize(file), myInventory);
            file.Close();
        }
    }
}
