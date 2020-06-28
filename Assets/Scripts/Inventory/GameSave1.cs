using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class GameSave1 : MonoBehaviour
{
    public Inventory EqInventory;
    public Inventory GoodsInventory;
    public Inventory EquipPanel;

    public void SaveGame(string path, string filename, Inventory thisbag)
    {
        Debug.Log(Application.persistentDataPath);
        if (!Directory.Exists(Application.persistentDataPath + path))
        {
            Directory.CreateDirectory(Application.persistentDataPath + path);
        }

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + path + filename);

        var json = JsonUtility.ToJson(thisbag);

        formatter.Serialize(file, json);
        file.Close();
    }

    public void LoadGame(string path, string filename, Inventory thisbag)
    {
        BinaryFormatter bf = new BinaryFormatter();
        if (File.Exists(Application.persistentDataPath + path + filename))
        {
            FileStream file = File.Open(Application.persistentDataPath + path + filename, FileMode.Open);

            JsonUtility.FromJsonOverwrite((string)bf.Deserialize(file), thisbag);
            file.Close();
        }
    }

    public void Save()    ////保存背包数据接口，在Button中调用，挂在GameSaves上
    {
        SaveGame("GameSave/", "EquipBag.txt", EqInventory);
        SaveGame("GameSave/", "GoodsBag.txt", GoodsInventory);
        SaveGame("GameSave/", "EquipPanel.txt", EquipPanel);
    }

    public void Load()   ////加载游戏背包数据接口，在Button中调用，挂在GameSaves上
    {
        LoadGame("GameSave/", "EquipBag.txt", EqInventory);
        LoadGame("GameSave/", "GoodsBag.txt", GoodsInventory);
        LoadGame("GameSave/", "EquipPanel.txt", EquipPanel);
    }

}
