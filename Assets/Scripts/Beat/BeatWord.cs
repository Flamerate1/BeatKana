using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BeatWord", menuName = "Scriptable Objects/BeatWord")]
public class BeatWord : BeatElement
{
    public override ElementType GetElementType() { return ElementType.Word; }
    //public Character character;
    public string text; // Either kana or kanji depending on if kanji is present or not
    public int level; // Associated level based on used hiragana or katana
    public int pitch; // 0 is heiban, above 0 is the drop point. 
    public string definition;

    public BeatChar[] beatChars;

    public override void ProcessToBeat(ref List<Beat> beatList)
    {
        int beatListStartIndex = beatList.Count; // Get the starting point. 
        for (int i = 0; i < this.beatChars.Length; i++) // Iterate through characters and add them to beatlist. 
        {
            this.beatChars[i].ProcessToBeat(ref beatList);
            //ProcessElement(ref beatList, this.beatChars[i]); // Add each character to beatlist. Populate list
            beatList[beatListStartIndex + i].word = this.text;
        }

        if (this.pitch == 0) // heiban word
        {
            beatList[beatListStartIndex].pitchIsHigh = false;
            for (int i = 1; i < this.beatChars.Length; i++)
            {
                beatList[beatListStartIndex + i].pitchIsHigh = true;
            }
        }
        else // nakadaka or oodaka
        {
            for (int i = 0; i < this.beatChars.Length; i++)
            {
                bool isHigh = (i + 1) < this.pitch; // Switches to low 
                beatList[beatListStartIndex + i].pitchIsHigh = isHigh;
            }
        }
    }

    public override void ProcessToComboBeat(ref List<Beat> beatList)
    {
        int beatListStartIndex = beatList.Count; // Get the starting point. 
        for (int i = 0; i < this.beatChars.Length; i++) // Iterate through characters and add them to beatlist. 
        {
            this.beatChars[i].ProcessToComboBeat(ref beatList);
            //ProcessElement(ref beatList, this.beatChars[i]); // Add each character to beatlist. Populate list
            beatList[beatListStartIndex + i].word = this.text;
        }

        if (this.pitch == 0) // heiban word
        {
            beatList[beatListStartIndex].pitchIsHigh = false;
            for (int i = 1; i < this.beatChars.Length; i++)
            {
                beatList[beatListStartIndex + i].pitchIsHigh = true;
            }
        }
        else // nakadaka or oodaka
        {
            for (int i = 0; i < this.beatChars.Length; i++)
            {
                bool isHigh = (i + 1) < this.pitch; // Switches to low 
                beatList[beatListStartIndex + i].pitchIsHigh = isHigh;
            }
        }
    }
}
