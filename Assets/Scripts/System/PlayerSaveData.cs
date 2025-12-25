using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData
{
    public LevelData(string name, bool completed, int score)
    {
        this.name = name;
        this.completed = completed;
        this.score = score;
    }

    public string name = "lvl";
    public bool completed = false;
    public int score = 0;
}

[System.Serializable]
public class PlayerSaveData
{
    public int saveVersion; // Initialized in constructor: increment when notable changes are made. 
    public string testString;
    public List<LevelData> levelData; 

    public PlayerSaveData() 
    {
        saveVersion = 0; // increment when notable changes are made. 
        testString = "seven";
        levelData = new List<LevelData> { new LevelData("cool", false, 50), new LevelData("cool5", false, 60), new LevelData("cool7", false, 70) };
        // Make automated code detect all of the level data and stuff. 
    }
    // make sure variables have default values
}
