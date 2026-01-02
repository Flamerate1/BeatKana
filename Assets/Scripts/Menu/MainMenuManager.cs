using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] Vector3 mainMenuButtonPos = new Vector3(-250f, -125f);
    [SerializeField] Vector3 backMenuButtonPos = new Vector3(250f, -125f);

    [SerializeField] Camera Camera;

    public enum Menu 
    { 
        MainMenu, LevelMenu, SettingsMenu,
        Chapter1, Chapter2, Chapter3, Chapter4,
        Previous
    }

    GameObject[] menus;
    Menu currentMenu;
    Menu previousMenu;
    private void Awake()
    {
        GameManager.cam = Camera;
    }

    private void Start()
    {
        var childCount = transform.childCount;
        menus = new GameObject[childCount];

        GameObject menuButtonPrefab = Resources.Load<GameObject>("MainMenuButton");
        GameObject backButtonPrefab = Resources.Load<GameObject>("BackMenuButton");
        //var pos = GameManager.cam.ScreenToWorldPoint(mainMenuButtonPos);
        // Loop through children to grab base menus
        for (int i = 0; i < childCount; i++) 
        {
            // Still grab the menu
            menus[i] = transform.GetChild(i).gameObject;
            if (i != 0) 
            {   
                GameObject instance = Instantiate(menuButtonPrefab, mainMenuButtonPos, Quaternion.identity);
                instance.transform.SetParent(menus[i].transform, false);
                //GameObject backInstance = Instantiate(backButtonPrefab, backMenuButtonPos, Quaternion.identity);
                //backInstance.transform.SetParent(menus[i].transform, false);
            }
        }

        // Instantiate
        GoToMenuButton(Menu.MainMenu);
        currentMenu = Menu.LevelMenu;
    }

    // The following are functions used EXCLUSIVELY by buttons!

    public void GoToMenuButton(Menu menu)
    {
        if (menu == Menu.Previous)
        {
            GoToMenuButton(previousMenu);
            return;
        }
        previousMenu = currentMenu;
        currentMenu = menu;

        int index = (int)menu;
        Debug.Log("Going to menu: " + index.ToString());
        for (int i = 0; i < menus.Length; i++)
        {
            bool isSelectedMenu = i == index;
            menus[i].SetActive(isSelectedMenu);
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
