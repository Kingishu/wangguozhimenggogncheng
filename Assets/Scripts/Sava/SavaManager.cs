using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    //单例模式
    public static SaveManager instance;
    public List<SOSavePair> savePairs = new List<SOSavePair>(); // 在Inspector中配置
    public Player player;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public bool HasSaveFile
    {
        get
        {
            string filePath = Path.Combine(Application.persistentDataPath, "game_SaveData", "save.dat");
            return File.Exists(filePath);
        }
    }
    
    
    [ContextMenu("测试保存")]
    public void SavaGame()
    {
        //延迟进行,因为代码执行顺序的原因
        StartCoroutine(DelaySave());
    }

    [ContextMenu("测试加载")]
    public void LoadGame()
    {
        string filePath = Application.persistentDataPath + "/game_SaveData/save.dat";
        if (!File.Exists(filePath)) return;

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(filePath, FileMode.Open);

        // 反序列化数据
        GameSaveData loadedData = JsonUtility.FromJson<GameSaveData>(
            (string)bf.Deserialize(file)
        );

        // 应用数据到SO
        foreach (var entry in loadedData.dataEntries)
        {
            var pair = savePairs.Find(p => p.key == entry.key);
            if (pair != null)
            {
                JsonUtility.FromJsonOverwrite(entry.jsonData, pair.so);
                Debug.Log($"已加载 {pair.key}");
            }
        }
        // 恢复玩家数值（新增逻辑）
        if(player != null)
        {
            player.CurrentHp = loadedData.PlayerHP;
            CoinManager.coins = loadedData.PlayerGold;
            Debug.Log($"已加载：血量 {loadedData.PlayerHP} 金币 {loadedData.PlayerGold} ");
        }

        file.Close();
    }

    [System.Serializable]
    public class GameSaveData
    {
        public List<SODataEntry> dataEntries = new List<SODataEntry>();

        public int PlayerHP;
        public int PlayerGold;
    }

    [System.Serializable]
    public class SODataEntry
    {
        public string key; // SO的唯一标识符
        public string jsonData; // SO的JSON序列化数据
    }

    [System.Serializable]
    public class SOSavePair
    {
        public string key; // 唯一标识符（如："MapLayout"）
        public ScriptableObject so; // 要保存的SO引用
    }

    IEnumerator DelaySave()
    {
        yield return new WaitForSeconds(1f);
        string dirPath = Application.persistentDataPath + "/game_SaveData";
        if (!Directory.Exists(dirPath)) Directory.CreateDirectory(dirPath);

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = File.Create(dirPath + "/save.dat");
  
        // 构建保存数据
        GameSaveData saveData = new GameSaveData();
        foreach (var pair in savePairs)
        {
            saveData.dataEntries.Add(new SODataEntry
            {
                key = pair.key,
                jsonData = JsonUtility.ToJson(pair.so)
            });
        }
        //保存玩家数值
        if(player != null)
        {
            saveData.PlayerHP = player.CurrentHp;
            saveData.PlayerGold = CoinManager.coins;
        }

        // 序列化保存
        var json = JsonUtility.ToJson(saveData);
        formatter.Serialize(file, json);
        file.Close();
        print("我保存了");
    }
    
}

