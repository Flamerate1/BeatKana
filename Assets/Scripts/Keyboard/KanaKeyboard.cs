using TMPro;
using UnityEngine;

public class KanaKeyboard : MonoBehaviour
{
    //TMP_InputField inputField;
    InputString inputString;
    KanaButtonGuide kanaButtonGuide;

    public void SetDakutenSpecialKeys(string[] keys) { this.dakutenSpecialKeys = keys; }
    string[] dakutenSpecialKeys = new string[5]; 

    private void Start()
    {
        //inputField = GameManager.inputField;
        inputString = GameManager.inputString;
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


    public void InputToField(string key, string text)
    {
        if (key == "dakuten") 
            text = DakutenProcess(text);

        //inputField.text = inputField.text + text;
        inputString.AddString(text);
        InputFieldUpdated();
    }

    string DakutenProcess(string text)
    {
        if (inputString.IsEmpty()) return "";
        string switchButton = this.dakutenSpecialKeys[0];
        string dakutenButton = this.dakutenSpecialKeys[1];
        string komojiButton = this.dakutenSpecialKeys[2];
        string handakutenButton = this.dakutenSpecialKeys[3];
        string nothingButton = this.dakutenSpecialKeys[4];
        if (text == nothingButton) return "";


        char lastChar = inputString.FinalChar();

        switch (text)
        {
            case var x when x == switchButton:
                Debug.Log("Special key: " + switchButton);
                if (KanaData.SwitchKana(lastChar, out char next))
                {
                    inputString.RemoveFromEnd();
                    return next.ToString();
                }
                break;
            case var x when x == dakutenButton:
                Debug.Log("Special key: " + dakutenButton);
                if (KanaData.ToFromDakuten(lastChar, out char dakuten))
                {
                    inputString.RemoveFromEnd();
                    return dakuten.ToString();
                }
                break;
            case var x when x == komojiButton:
                Debug.Log("Special key: " + komojiButton);
                if (KanaData.ToFromKomoji(lastChar, out char komoji))
                {
                    inputString.RemoveFromEnd();
                    return komoji.ToString();
                }
                break;
            case var x when x == handakutenButton:
                Debug.Log("Special key: " + handakutenButton);
                if (KanaData.ToFromHandakuten(lastChar, out char handakuten))
                {
                    inputString.RemoveFromEnd();
                    return handakuten.ToString();
                }
                break;
        }

        return lastChar.ToString();
    }

    public void BackspaceOnField()
    {
        inputString.RemoveFromEnd();
        /*
        if (inputField.text.Length > 0)
        {
            inputField.text = inputField.text.Remove(inputField.text.Length - 1);
            InputFieldUpdated();
        }*/
    }
}
