using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelCompletionRecord
{
    public static void SetNextId(int nextId) { LevelCompletionRecord.nextId = nextId; Debug.Log("LevelCompletionRecord.nextId set to " + nextId.ToString()); }
    private static int nextId = 0; 
    public int id;
    public LevelCompletionRecord(string levelName, int score, bool completed)
    {
        this.levelName = levelName;
        this.score = score;
        this.completed = completed;

        this.dateTimeISO = DateTime.Now.ToString("o");
        this.id = nextId++;
    }

    public string levelName; // Will be the primary string used to index in Dictionary
    public int score;
    public bool completed;
    public string dateTimeISO;

    public LevelSaveData ConvertToLevelSaveData()
    {
        LevelSaveData levelSaveData = new LevelSaveData(this.levelName, this.score);
        return levelSaveData;
    }
    
}