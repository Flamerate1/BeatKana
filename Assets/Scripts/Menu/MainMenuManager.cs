using System;
using System.Collections.Generic;
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

    /*
    [Serializable] 
    public struct MenuObject
    {
        public Menu menu;
        public GameObject gameObject;
    }
    [SerializeField] MenuObject[] MenuObjectStructs;
    Dictionary<Menu, GameObject> menuObjects = new Dictionary<Menu, GameObject>();
    */
    MenuObject[] MenuObjects;
    PlayLevelInfo PlayLevelInfo;
    

    //GameObject[] menus;
    Menu currentMenu = Menu.MainMenu;
    Menu previousMenu;


    private void Awake()
    {
        GameManager.cam = Camera;
    }

    private void Start()
    {
        PlayLevelInfo = GetComponentInChildren<PlayLevelInfo>();
        PlayLevelInfo.Initialize(this);
        PlayLevelInfo.Deactivate();

        MenuObjects = GetComponentsInChildren<MenuObject>(); 
        GameObject menuButtonPrefab = Resources.Load<GameObject>("MainMenuButton");
        GameObject backButtonPrefab = Resources.Load<GameObject>("BackMenuButton");
        foreach (MenuObject menuObject in MenuObjects)
        {
            if (menuObject.Menu != Menu.MainMenu)
            {
                GameObject instance = Instantiate(menuButtonPrefab, mainMenuButtonPos, Quaternion.identity);
                instance.transform.SetParent(menuObject.transform, false);
            }
        }

        // Instantiate
        GoToMenuButton(Menu.MainMenu);

        /*
        var menuCount = MenuObjectStructs.Length;
        menus = new GameObject[menuCount];

        GameObject menuButtonPrefab = Resources.Load<GameObject>("MainMenuButton");
        GameObject backButtonPrefab = Resources.Load<GameObject>("BackMenuButton");

        // loop over menuObjects and place them into a dictionary
        foreach (MenuObject menuObject in MenuObjectStructs)
        {
            menuObjects.Add(menuObject.menu, menuObject.gameObject);
        }

        // Instantiate
        GoToMenuButton(Menu.MainMenu);
        */

        /*
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
        currentMenu = Menu.LevelMenu;*/
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

        foreach (MenuObject menuObject in MenuObjects)
        {
            if (menuObject.Menu == menu) 
                menuObject.Activate(); 
            else 
                menuObject.Deactivate(); 
        }

        /*
        foreach (KeyValuePair<Menu, GameObject> menuObject in menuObjects)
        {
            if (menuObject.Key == menu)
            {
                Debug.Log(menuObject.Key.ToString() + " was set on");
                menuObject.Value.SetActive(true);
            }
            else
            {
                Debug.Log(menuObject.Key.ToString() + " was set off");
                menuObject.Value.SetActive(false);
            }
        }*/


        /*
        // If previous menu, cut the method short
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
        }*/
    }


    public void QuitGameButton()
    {

    }

    public void ActivatePlayLevelInfo(Level level)
    {
        PlayLevelInfo.Activate(level);
    }

    public void GoToLevelButton(Level level)
    {
        GameManager.SetLevel(level);
        SceneManager.LoadScene("Scenes/PlayScene", LoadSceneMode.Single);
            
    }

}
