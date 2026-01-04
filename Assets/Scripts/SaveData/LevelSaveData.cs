using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelSaveData
{
    public LevelSaveData(string name, int score)
    {
        this.name = name;
        this.score = score;
    }

    public string name = "lvl"; // Will be the primary string used to index in Dictionary
    public int score = 0;
}
