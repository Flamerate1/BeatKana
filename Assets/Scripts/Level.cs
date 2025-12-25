using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Scriptable Objects/Level")]
public class Level : ScriptableObject
{
    public int BPM = 60;
    public float BeatDistance = 1.0f;
    public float maxBeatError = 0.2f; // Can't be equal to or more than 0.25f;
    public BeatElement[] beatElementsBank; // Assume in order for now. 

    public int levelPreBeats = 3;
    public int betweenBeats = 1;
    public int levelPostBeats = 2;

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
}
