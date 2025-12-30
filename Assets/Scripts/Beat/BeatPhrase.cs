using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BeatPhrase", menuName = "Scriptable Objects/BeatPhrase")]
public class BeatPhrase : BeatElement
{
    public override ElementType GetElementType() { return ElementType.Phrase; }
    //public Character character;
    public string text; // Japanese text; hiragana or katakana
    public int level; // level equivalent
    public string romaji; // romanized characters
    public AudioClip clip; // associated sound file. (need additional for special katakana)

    public override void ProcessToBeat(ref List<Beat> beatList)
    {
        //
    }
}
