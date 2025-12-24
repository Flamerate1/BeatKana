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
}
