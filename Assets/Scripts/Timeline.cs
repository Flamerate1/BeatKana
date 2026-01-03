using TMPro;
using UnityEngine;

public abstract class Timeline : MonoBehaviour
{

    public abstract void PlayManagerSetFields(PlayManager.TLFields tlFields);
    public abstract void StartGame();



    protected bool isGameOver = false;
    protected bool isLevelLoaded = false;
    private void Update()
    {
        if (isGameOver) return;
        if (!isLevelLoaded) return;
        if (GameManager.gamePaused) { return; } // don't update if game is paused
    }
}
