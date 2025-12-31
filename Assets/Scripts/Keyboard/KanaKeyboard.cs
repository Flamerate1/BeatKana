using TMPro;
using UnityEngine;

public class KanaKeyboard : MonoBehaviour
{
    TMP_InputField inputField;
    KanaButtonGuide kanaButtonGuide;

    public void SetDakutenSpecialKeys(string[] keys) { this.dakutenSpecialKeys = keys; }
    string[] dakutenSpecialKeys = new string[5]; 

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
        string switchButton = this.dakutenSpecialKeys[0];
        string dakutenButton = this.dakutenSpecialKeys[1];
        string komojiButton = this.dakutenSpecialKeys[2];
        string handakutenButton = this.dakutenSpecialKeys[3];
        string nothingButton = this.dakutenSpecialKeys[4];
        switch (text)
        {
            case var x when x == switchButton:
                if (KanaData.SwitchKana(text.ToCharArray()[0], out char next)) { text = next.ToString(); }
                break;
            case var x when x == dakutenButton:
                if (KanaData.ToFromDakuten(text.ToCharArray()[0], out char dakuten)) { text = dakuten.ToString(); }
                break;
            case var x when x == komojiButton:
                if (KanaData.ToFromKomoji(text.ToCharArray()[0], out char komoji)) { text = komoji.ToString(); }
                break;
            case var x when x == handakutenButton:
                if (KanaData.ToFromHandakuten(text.ToCharArray()[0], out char handakuten)) { text = handakuten.ToString(); }
                break;
            case var x when x == nothingButton: text = ""; break;
        }
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
