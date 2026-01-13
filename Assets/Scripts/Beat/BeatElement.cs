using System.Collections.Generic;
using UnityEngine;


public abstract class BeatElement : ScriptableObject
{
    public enum ElementType
    {
        Char,
        Word, 
        Phrase, 
        Particle
    }

    ElementType elementType;
    public abstract ElementType GetElementType();
    /* example
     * public override ElementType GetElementType() {  return ElementType.Char; }
     */

    public abstract void ProcessToBeat(ref List<Beat> beatList);

    public abstract void ProcessToComboBeat(ref List<Beat> beatList);
}
