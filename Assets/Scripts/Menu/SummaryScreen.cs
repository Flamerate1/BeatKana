using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SummaryScreen : MonoBehaviour
{
    Image overlayImage;
    Color overlayColor;

    GameObject unpauseButton;

    public void Initialize()
    {
        // Deactivate stuff and change alpha of the primary image. 
        //overlayColor = Color.black;
        //overlayColor.a = 0f;
        overlayImage = GetComponent<Image>();
        //overlayImage.color = overlayColor;
        //overlayColor.a = 156f / 255f;

        unpauseButton = transform.GetChild(0).GetChild(0).gameObject; // Always top of panel object
    }

    public void Activate()
    {
        unpauseButton.SetActive(false);
        // everything that happens when the summary screen is being used. 
    }
    public void Pause(bool doPause)
    {
        gameObject.SetActive(doPause);
    }

    public void GoToMainMenu() { SceneManager.LoadScene("Scenes/MainMenu", LoadSceneMode.Single); }
    public void RestartLevel() { SceneManager.LoadScene("Scenes/PlayScene", LoadSceneMode.Single); }
}
