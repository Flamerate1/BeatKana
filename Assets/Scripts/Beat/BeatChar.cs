using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "BeatChar", menuName = "Scriptable Objects/BeatChar")]
public class BeatChar : BeatElement
{
    public override ElementType GetElementType() { return ElementType.Char; }
    //public Character character;
    public string text; // Japanese text; hiragana or katakana
    public int level; // level equivalent
    public string romaji; // romanized characters
    public AudioClip clip; // associated sound file. (need additional for special katakana)
    
    public override void ProcessToBeat(ref List<Beat> beatList)
    {
        beatList.Add(new Beat(this.text, this.romaji, this.clip)); // Add character to beatlist 
    }

    public override void ProcessToComboBeat(ref List<Beat> beatList)
    {
        // possibilities:
        // あ、が、きゃ、ぎゃ、ぴゃ、っ

        //if (KanaData.IsDakuten(this.text[0]))
        bool dakuten_present = false;
        if (KanaData.FromKomoji(this.text[0], out char oppositeTsu))
        {
            beatList.Add(new Beat(oppositeTsu.ToString(), this.romaji, this.clip));
            beatList.Add(new Beat("小", this.romaji, null));
            dakuten_present = true;
        }
        else if (KanaData.FromDakuten(this.text[0], out char opposite))
        {
            beatList.Add(new Beat(opposite.ToString(), this.romaji, this.clip));
            beatList.Add(new Beat("゛", this.romaji, null));
            dakuten_present = true;
        }
        else if (KanaData.FromHandakuten(this.text[0], out char oppositeH))
        {
            beatList.Add(new Beat(oppositeH.ToString(), this.romaji, this.clip));
            beatList.Add(new Beat("゜", this.romaji, null));
            dakuten_present = true;
        }
        else 
        {
            beatList.Add(new Beat(this.text[0].ToString(), this.romaji, this.clip));
        }

        if (this.text.Length >= 2 && KanaData.FromKomoji(this.text[1], out char oppositeK))
        {
            beatList.Add(new Beat(oppositeK.ToString(), this.romaji, this.clip));
            beatList.Add(new Beat("小", this.romaji, null));
        } 

        if (!dakuten_present) 
            Beat.AddEmptyBeats(ref beatList, 1); 

        // If by self, Add an extra
        // If with dakuten, DON'T add extra. 
        // If with combo, add extra. 
        // if with dakuten and combo, DON'T add extra. 


        //beatList.Add(new Beat(this.text, this.romaji, this.clip)); // Add character to beatlist 

        // Special keys: ゛小゜
        //Beat.AddEmptyBeats(ref beatList, 1);
    }
}
