using UnityEngine;

[CreateAssetMenu(fileName = "BeatWord", menuName = "Scriptable Objects/BeatWord")]
public class BeatWord : BeatElement
{
    public override ElementType GetElementType() { return ElementType.Word; }
    //public Character character;
    public string text;
    public int pitch; // 0 is heiban, above 0 is the drop point. 
    public BeatChar[] beatChars;
}
