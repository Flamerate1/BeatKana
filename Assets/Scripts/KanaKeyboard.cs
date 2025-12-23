using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KanaKeyboard : MonoBehaviour
{
    TMP_InputField inputField;

    private void Start()
    {
        inputField = GameManager.inputField;
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
        //inputField.onValueChanged.Invoke(inputField.text);
    }
}
