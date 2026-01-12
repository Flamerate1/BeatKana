using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] Vector3 mainMenuButtonPos = new Vector3(-250f, -125f);
    [SerializeField] Vector3 backMenuButtonPos = new Vector3(250f, -125f);

    [SerializeField] AudioManager AudioManager;
    [SerializeField] Camera Camera;
    [SerializeField] TMP_Text totalScoreText;


    public enum Menu
    {
        MainMenu, LevelMenu, GuideMenu, SettingsMenu,
        Chapter1, Chapter2, Chapter3, Chapter4,
        Previous
    }

    
    PlayLevelInfo PlayLevelInfo;

    MenuObject[] MenuObjects;
    Stack<MenuObject> menuStack = new Stack<MenuObject>();
    MenuObject currentMenu;

    [SerializeField] GameObject mainMenuButton;
    [SerializeField] GameObject backButton;



    private void Awake()
    {
        GameManager.AudioManager = AudioManager;
        GameManager.cam = Camera;
    }

    private void Start()
    {
        AudioManager.Init();
        foreach (VolumeSlider slider in GetComponentsInChildren<VolumeSlider>())
        {
            slider.Init();
        }

        PlayLevelInfo = GetComponentInChildren<PlayLevelInfo>();
        PlayLevelInfo.Initialize(this);
        PlayLevelInfo.Deactivate();

        MenuObjects = GetComponentsInChildren<MenuObject>();

        MenuObject _startMenu = MenuObjects[0];
        currentMenu = _startMenu;
        foreach (MenuObject menuObject in MenuObjects)
        {
            if (menuObject.Menu == Menu.MainMenu)
            {
                _startMenu = menuObject;
                Debug.Log("Main menu MenuObject found");
                break;
            }
        }

        // Instantiate
        GoToMenu(_startMenu);

        totalScoreText.text = "Total Score: " +
            GameManager.PlayerSaveData.GetTotalScore().ToString();
    }
    
    public void GoToMenu(MenuObject menuObject)
    {
        // Save PlayerPrefs when exiting SettingsMenu
        if (currentMenu.Menu == Menu.SettingsMenu) { PlayerPrefs.Save(); Debug.Log("PlayerPrefs saved!"); }
        bool isMainMenu = menuObject.Menu == Menu.MainMenu;
        mainMenuButton.SetActive(!isMainMenu); 
        backButton.SetActive(!isMainMenu);

        menuStack.Push(currentMenu);
        currentMenu = menuObject;

        foreach (MenuObject menu in MenuObjects)
        {
            menu.Deactivate();
        }
        PlayLevelInfo.gameObject.SetActive(false);
        menuObject.Activate();
    }
    public void GoToPreviousMenu()
    {
        GoToMenu(menuStack.Pop()); // Take the previous menu to go to. 
        menuStack.Pop(); // Remove the added menu from going to the previous.
    }

    public void QuitGameButton()
    {
        Application.Quit();
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
