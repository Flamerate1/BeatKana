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
}
