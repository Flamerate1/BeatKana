using TMPro;
using UnityEngine;

public class TimelineBeat : MonoBehaviour
{
    TextMeshPro textMeshPro;

    private void Start()
    {
        textMeshPro = GetComponent<TextMeshPro>();
    }

    public void SetBeat(Beat beat)
    {
        textMeshPro.text = beat.text;
    }
}
