using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData
{
    public LevelData(string name, int score)
    {
        this.name = name;
        this.score = score;
    }

    public string name = "lvl";
    public int score = 0;
}

[System.Serializable]
public class PlayerSaveData
{
    [SerializeField] int saveVersion; // Initialized in constructor: increment when notable changes are made. 
    [SerializeField] string testString;
    [SerializeField] List<LevelData> levelDataList;

    [System.NonSerialized]
    public Dictionary<string, LevelData> levelDataDictionary;

    public PlayerSaveData() 
    {
        saveVersion = 0; // increment when notable changes are made. 
        testString = "seven";
        levelDataList = new List<LevelData> { new LevelData("cool", 50), new LevelData("cool5", 60), new LevelData("cool7", 70) };
        // Make automated code detect all of the level data and stuff. 
    }

    public void InitLevelDataDictionary()
    {
        foreach (LevelData level in levelDataList)
        {
            levelDataDictionary.Add(level.name, level);
        }
    }

    public void SaveLevelResult(LevelData levelData)
    {
        this.levelDataList.Add(levelData);
    }
    // make sure variables have default values
}
