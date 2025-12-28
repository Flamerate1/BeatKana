using UnityEngine;

[CreateAssetMenu(fileName = "BeatChar", menuName = "Scriptable Objects/BeatChar")]
public class BeatChar : BeatElement
{
    public override ElementType GetElementType() {  return ElementType.Char; }
    //public Character character;
    public string text; // Japanese text; hiragana or katakana
    public int level; // level equivalent
    public string romaji; // romanized characters
    public AudioClip clip; // associated sound file. (need additional for special katakana)
}
