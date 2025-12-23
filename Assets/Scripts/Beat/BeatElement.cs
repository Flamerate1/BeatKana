using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;


public abstract class BeatElement : ScriptableObject
{
    public enum ElementType
    {
        Char,
        Word, 
        Phrase
    }

    ElementType elementType;
    public abstract ElementType GetElementType();
}
