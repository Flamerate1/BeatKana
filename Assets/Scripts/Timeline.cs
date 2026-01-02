using TMPro;
using UnityEngine;

public abstract class Timeline : MonoBehaviour
{

    public abstract void PlayManagerSetFields(PlayManager.TLFields tlFields);
    public abstract void StartGame();
}
