using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseScreen : MonoBehaviour
{
    Image overlayImage;
    Color overlayColor;

    public void Init()
    {
        overlayImage = GetComponent<Image>();
    }

    public void Activate()
    {
        //unpauseButton.SetActive(false);
        // everything that happens when the summary screen is being used. 
    }
    public void Pause(bool doPause)
    {
        PlayerPrefs.Save();
        gameObject.SetActive(doPause);
    }

    public void GoToMainMenu() { SceneManager.LoadScene("Scenes/MainMenu", LoadSceneMode.Single); }
    public void RestartLevel() { SceneManager.LoadScene("Scenes/PlayScene", LoadSceneMode.Single); }
}
