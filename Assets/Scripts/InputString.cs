using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputString : MonoBehaviour
{
    public delegate void UpdateEvent(string text);
    public event UpdateEvent UpdateStringEvent;

    TMP_Text tmpText;

    public void Init()
    {
        tmpText = GetComponent<TMP_Text>();
    }

    public void ResetString()
    {
        tmpText.text = string.Empty;
    }
    public void AddString(string addition)
    {
        tmpText.text += addition;
        UpdateStringEvent?.Invoke(tmpText.text);
    }
    public char FinalChar()
    {
        return tmpText.text[tmpText.text.Length - 1];
    }

    public void RemoveFromEnd(int amount)
    {
        int len = tmpText.text.Length;
        if (len - amount < 0) { return; }
        tmpText.text = tmpText.text.Remove(len - amount);
    }
    public void RemoveFromEnd() { RemoveFromEnd(1); }

    public int Length() { return tmpText.text.Length; }
    public bool IsEmpty() { return Length() == 0; }
}
