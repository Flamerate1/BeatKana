using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Scriptable Objects/Level")]
public class Level : ScriptableObject
{
    [Serializable]
    public struct DifficultyFields
    {
        public int BPM;
        public float BeatDistance;
        public float maxBeatError;
        public bool showKana;
        public DifficultyFields(int BPM, float BeatDistance, float maxBeatError, bool showKana)
        {
             this.BPM = BPM;
             this.BeatDistance = BeatDistance;
             this.maxBeatError =maxBeatError;
             this.showKana = showKana;
        }
    }
    [Serializable]
    public struct BeatCounts
    {
        public int levelPreBeats;
        public int betweenBeats;
        public int levelPostBeats;
        public BeatCounts(int levelPreBeats, int betweenBeats, int levelPostBeats)
        {
            this.levelPreBeats = levelPreBeats;
            this.betweenBeats = betweenBeats;
            this.levelPostBeats = levelPostBeats;
        }
    }

    [Serializable]
    public enum LevelType { Beat, Queue }

    [Serializable] 
    public struct Prereqs
    {
        public int minScore;
        public Level[] requiredLevels;
        public Prereqs(int minScore, Level[] requiredLevels)
        {
            this.minScore = minScore;
            this.requiredLevels = requiredLevels;
        }
    }


    // Name used to assist in saved data referencing
    // Used as an ID to make sure that where ever a level is being loaded (the button, the level, etc), it can also use the proper saved data there. 
    public string LevelName = string.Empty;
    public LevelType levelType = LevelType.Beat;
    public DifficultyFields difficulty = new DifficultyFields(60, 1.0f, 0.2f, true);
    public BeatElement[] beatElementsBank; // Assume in order for now. 
    public BeatCounts beatCounts = new BeatCounts(3, 1, 2);
    public Prereqs prereqs = new Prereqs(0, new Level[0]);


    // method WITHOUT Level Name
    public void GetLevelData(
        ref int BPM,
        ref float BeatDistance,
        ref float maxBeatError,
        ref BeatElement[] beatElementsBank,

        ref int levelPreBeats,
        ref int betweenBeats,
        ref int levelPostBeats
        )
    {
        BPM = this.difficulty.BPM;
        BeatDistance = this.difficulty.BeatDistance;
        maxBeatError = this.difficulty.maxBeatError;

        beatElementsBank = this.beatElementsBank;

        levelPreBeats = this.beatCounts.levelPreBeats;
        betweenBeats = this.beatCounts.betweenBeats;
        levelPostBeats = this.beatCounts.levelPostBeats;
    }

    // method WITH Level Name
    public void GetLevelData(
        ref string LevelName,
        ref int BPM,
        ref float BeatDistance,
        ref float maxBeatError,
        ref BeatElement[] beatElementsBank,

        ref int levelPreBeats,
        ref int betweenBeats,
        ref int levelPostBeats
        )
    {
        LevelName = this.LevelName;
        GetLevelData(
                ref BPM,
                ref BeatDistance,
                ref maxBeatError,
                ref beatElementsBank,

                ref levelPreBeats,
                ref betweenBeats,
                ref levelPostBeats
            );
    }
}
