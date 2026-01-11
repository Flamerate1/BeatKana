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

    MenuObject[] MenuObjects;
    PlayLevelInfo PlayLevelInfo;
    

    //GameObject[] menus;
    Menu currentMenu = Menu.MainMenu;
    Menu previousMenu;


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

        totalScoreText.text = "Total Score: " +
            GameManager.PlayerSaveData.GetTotalScore().ToString();
    }

    // The following are functions used EXCLUSIVELY by buttons!

    public void GoToMenuButton(Menu menu)
    {
        if (menu == Menu.Previous)
        {
            GoToMenuButton(previousMenu);
            return;
        }

        // Save PlayerPrefs when exiting SettingsMenu
        if (currentMenu == Menu.SettingsMenu) { PlayerPrefs.Save(); Debug.Log("PlayerPrefs saved!"); } 
        
        previousMenu = currentMenu;
        currentMenu = menu;

        foreach (MenuObject menuObject in MenuObjects)
        {
            if (menuObject.Menu == menu) 
                menuObject.Activate(); 
            else 
                menuObject.Deactivate(); 
        }
        PlayLevelInfo.gameObject.SetActive(false);
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
