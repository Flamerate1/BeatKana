using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Scriptable Objects/Level")]
public class Level : ScriptableObject
{
    // Name used to assist in saved data referencing
    // Used as an ID to make sure that where ever a level is being loaded (the button, the level, etc), it can also use the proper saved data there. 
    public string LevelName = string.Empty; 
    public int BPM = 60;
    public float BeatDistance = 1.0f;
    public float maxBeatError = 0.2f; // Can't be equal to or more than 0.25f;
    public BeatElement[] beatElementsBank; // Assume in order for now. 

    public int levelPreBeats = 3;
    public int betweenBeats = 1;
    public int levelPostBeats = 2;

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
        BPM = this.BPM;
        BeatDistance = this.BeatDistance;
        maxBeatError = this.maxBeatError;
        beatElementsBank = this.beatElementsBank;

        levelPreBeats = this.levelPreBeats;
        betweenBeats = this.betweenBeats;
        levelPostBeats = this.levelPostBeats;
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
