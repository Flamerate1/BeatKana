using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KanaKeyboard : MonoBehaviour
{
    TMP_InputField inputField;
    KanaButtonGuide kanaButtonGuide;

    private void Start()
    {
        inputField = GameManager.inputField;
        kanaButtonGuide = gameObject.GetComponentInChildren<KanaButtonGuide>();
        KanaButtonGuideDeactivate();
    }


    public void KanaButtonGuideActivate(Vector3 pos, float y_diff, string[] keys) 
    {
        kanaButtonGuide.gameObject.SetActive(true); 
        kanaButtonGuide.SetActivate(pos, y_diff, keys);
        kanaButtonGuide.prevIndex = -1; // to ensure that a change in string index always initially occurs
    }
    public void KanaButtonGuideSetIndex(int index) { kanaButtonGuide.SetIndex(index); }
    public void KanaButtonGuideDeactivate()
    {
        kanaButtonGuide.gameObject.SetActive(false);
    }



    public void InputFieldUpdated() 
    { 
        // Empty and unused
    }

    public void ResetInputField() { inputField.text = string.Empty; }

    public string GetInputFieldText()
    {
        Debug.Log(inputField.text);
        return inputField.text;
    }
    public void InputToField(string text)
    {
        inputField.text = inputField.text + text;
        InputFieldUpdated();
    }
    public void BackspaceOnField()
    {
        if (inputField.text.Length > 0)
        {
            inputField.text = inputField.text.Remove(inputField.text.Length - 1);
            InputFieldUpdated();
        }
    }
}
