using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public enum Menu
    {
        MainMenu, LevelMenu, SettingsMenu
    }

    GameObject[] menus;

    private void Awake()
    {
        var childCount = transform.childCount;
        menus = new GameObject[childCount];
        for (int i = 0; i < childCount; i++) 
        {
            menus[i] = transform.GetChild(i).gameObject;
        }

        GoToMenuButton(Menu.MainMenu);
    }

    // The following are functions used EXCLUSIVELY by buttons!

    public void GoToMenuButton(Menu menu)
    {
        int index = (int)menu;
        Debug.Log("Going to menu: " + index.ToString());
        for (int i = 0; i < menus.Length; i++)
        {
            menus[i].SetActive(i == index);
        }
    }

    /*
    public void GoToLevelMenuButton()
    {

    }

    public void GoToMainMenuButton()
    {

    }

    public void GoToSettingsMenuButton()
    {

    }*/

    public void QuitGameButton()
    {

    }

    public void GoToLevelButton(Level level)
    {
        GameManager.SetLevel(level);
        // GoToScene
        if (GameManager.instance.isBeatTimeline)
        {
            SceneManager.LoadScene("Scenes/PlayScene", LoadSceneMode.Single);
        }
        else
        {
            SceneManager.LoadScene("Scenes/AltPlayScene", LoadSceneMode.Single);
        }
            
    }

}
