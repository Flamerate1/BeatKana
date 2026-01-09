using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseScreen : MonoBehaviour
{
    Image overlayImage;
    Color overlayColor;

    //GameObject unpauseButton;

    AudioSource AudioSource;

    public void Init(AudioSource AudioSource)
    {
        // Deactivate stuff and change alpha of the primary image. 
        //overlayColor = Color.black;
        //overlayColor.a = 0f;
        overlayImage = GetComponent<Image>();
        //overlayImage.color = overlayColor;
        //overlayColor.a = 156f / 255f;

        this.AudioSource = AudioSource;

        //unpauseButton = transform.GetChild(0).GetChild(0).gameObject; // Always top of panel object
    }

    public void Activate()
    {
        //unpauseButton.SetActive(false);
        // everything that happens when the summary screen is being used. 
    }
    public void Pause(bool doPause)
    {
        gameObject.SetActive(doPause);
    }

    public void GoToMainMenu() { SceneManager.LoadScene("Scenes/MainMenu", LoadSceneMode.Single); }
    public void RestartLevel() { SceneManager.LoadScene("Scenes/PlayScene", LoadSceneMode.Single); }

    public void UpdateTickVolume()
    {

    }
    public void UpdateKanaVolume()
    {

    }
}
