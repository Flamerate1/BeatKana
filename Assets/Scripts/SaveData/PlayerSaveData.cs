using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class PlayerSaveData
{
    // internal use
    private string filePath; public void SetFilePath(string filePath) { this.filePath = filePath; }


    [SerializeField] string playerSaveId; 
    [SerializeField] int saveVersion; // Initialized in constructor: increment when notable changes are made. 
    [SerializeField] string testString;
    [SerializeField] List<LevelSaveData> bestLevelData; // Data to save into json
    [SerializeField] List<LevelCompletionRecord> LevelCompletionRecord_List; // Data to use at runtime

    [System.NonSerialized]
    public Dictionary<string, LevelSaveData> LevelSaveData_Dictionary;

    public PlayerSaveData() 
    {
        playerSaveId = Guid.NewGuid().ToString();
        saveVersion = 0; // increment when notable changes are made. 
        testString = "seven";
        //bestLevelData = new List<LevelSaveData> { new LevelSaveData("cool", 50), new LevelSaveData("cool5", 60), new LevelSaveData("cool7", 70) };
        bestLevelData = new List<LevelSaveData>();
        LevelSaveData_Dictionary = new Dictionary<string, LevelSaveData>();
        // Make automated code detect all of the level data and stuff. 
    }

    public void InitLevelDataDictionary()
    {
        foreach (LevelSaveData level in bestLevelData)
        {
            LevelSaveData_Dictionary.Add(level.name, level);
        }
        string debug_string = LevelSaveData_Dictionary.ToString();
        Debug.Log(debug_string);

        foreach (var kvp in LevelSaveData_Dictionary)
        {
            Debug.Log($"{kvp.Key} : {kvp.Value}");
        }
    }

    public void RecordLevelResult(LevelCompletionRecord levelCompletionRecord)
    {
        this.LevelCompletionRecord_List.Add(levelCompletionRecord);
        if (!levelCompletionRecord.completed) return; // No need to add to LevelSaveData_List

        // Compare previous and new LevelSaveData
        LevelSaveData newLevelSaveData = levelCompletionRecord.ConvertToLevelSaveData();
        if (LevelSaveData_Dictionary.TryGetValue(levelCompletionRecord.levelName, out LevelSaveData oldLevelSaveData))
        {
            if (newLevelSaveData.score > oldLevelSaveData.score)
            {
                // New high score! 
                //LevelSaveData_Dictionary.Add(newLevelSaveData.name, newLevelSaveData);
                LevelSaveData_Dictionary[levelCompletionRecord.levelName] = newLevelSaveData;
            }
        }
        else
        {
            // First time completion!
            LevelSaveData_Dictionary.Add(newLevelSaveData.name, newLevelSaveData);
        }

        UpdateBestLevelData();
    }

    public void UpdateBestLevelData()
    {
        bestLevelData.Clear();
        //foreach (LevelSaveData levelSaveData in LevelSaveData_Dictionary)
        foreach (KeyValuePair<string, LevelSaveData> pair in LevelSaveData_Dictionary)
        {
            bestLevelData.Add(pair.Value);
        }
    }



    public void SaveToJson()
    {
        // Grab PlayerSaveData from GameManager and put it into json to place into a save file. 
        string json = JsonUtility.ToJson(this, true);
        File.WriteAllText(this.filePath, json);
        Debug.Log(json);
        Debug.Log("Saved to: " + this.filePath);
    }

    public static PlayerSaveData LoadFromJson(string filePath)
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            Debug.Log(json);
            PlayerSaveData loaded = JsonUtility.FromJson<PlayerSaveData>(json);
            loaded.InitLevelDataDictionary();
            loaded.SetFilePath(filePath);
            
            int count = loaded.LevelCompletionRecord_List.Count;
            int nextId = loaded.LevelCompletionRecord_List[count-1].id;
            LevelCompletionRecord.SetNextId(nextId+1);

            return loaded;
        }
        else
        {
            Debug.LogWarning("No save file found, returning new save.");
            PlayerSaveData playeSaveData = new PlayerSaveData();
            playeSaveData.SetFilePath(filePath);
            return playeSaveData;
        }

    }
}
