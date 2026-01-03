using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputString : MonoBehaviour
{
    public delegate void UpdateEvent(string text);
    public event UpdateEvent UpdateStringEvent;

    TMP_Text tmpText;

    public void Initialize()
    {
        tmpText = GetComponent<TMP_Text>();
        Debug.Log("InputString.Initialize");
        Debug.Log("The text: " + tmpText.text);
    }

    public void ResetString()
    {
        tmpText.text = string.Empty;
        Debug.Log("InputString.ResetString");
        //UpdateStringEvent?.Invoke(); //Not necessary to check empty
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
