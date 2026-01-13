using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimelineBeatObject : MonoBehaviour
{
    TMP_Text textMeshPro;

    // This is executed at every instances creation, so this replaces the functionality of Start()
    public void SetBeat(Beat beat) 
    {
        textMeshPro = GetComponent<TMP_Text>();
        textMeshPro.text = beat.text;
    }
}
