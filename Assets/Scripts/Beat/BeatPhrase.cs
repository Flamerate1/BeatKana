using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BeatPhrase", menuName = "Scriptable Objects/BeatPhrase")]
public class BeatPhrase : BeatElement
{
    public override ElementType GetElementType() { return ElementType.Phrase; }
    public int level; // level equivalent
    public BeatWord[] beatWords;
    public Sprite sprite;

    public override void ProcessToBeat(ref List<Beat> beatList)
    {
        int beatListStartIndex = beatList.Count; // Get the starting point. 

        for (int i = 0; i < this.beatWords.Length; i++) // Iterate through characters and add them to beatlist. 
        {
            this.beatWords[i].ProcessToBeat(ref beatList);
        }

        for (int i =  beatListStartIndex; i < beatList.Count; i++)
        {
            beatList[i].sprite = this.sprite;
        }

    }

    public override void ProcessToComboBeat(ref List<Beat> beatList)
    {
        int beatListStartIndex = beatList.Count; // Get the starting point. 

        for (int i = 0; i < this.beatWords.Length; i++) // Iterate through characters and add them to beatlist. 
        {
            this.beatWords[i].ProcessToComboBeat(ref beatList);
        }

        for (int i = beatListStartIndex; i < beatList.Count; i++)
        {
            beatList[i].sprite = this.sprite;
        }
    }
}
