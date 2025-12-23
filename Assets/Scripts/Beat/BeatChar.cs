using UnityEngine;

[CreateAssetMenu(fileName = "BeatChar", menuName = "Scriptable Objects/BeatChar")]
public class BeatChar : BeatElement
{
    public override ElementType GetElementType() {  return ElementType.Char; }
    //public Character character;
    public string text;
    public string romaji;
    public AudioClip clip;
}
