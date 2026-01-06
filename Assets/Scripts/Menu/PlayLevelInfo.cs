using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayLevelInfo : MonoBehaviour
{
    [SerializeField] TMP_Text textBox;


    Button PlayLevelButton;
    MainMenuManager MainMenuManager;

    public void Initialize(MainMenuManager MainMenuManager)
    {
        PlayLevelButton = GetComponentInChildren<Button>();
        //textBox = GetComponentInChildren<TMP_Text>();
        this.MainMenuManager = MainMenuManager;
    }

    public void Activate(Level level)
    {
        gameObject.SetActive(true);
        PlayLevelButton.onClick.RemoveAllListeners();
        PlayLevelButton.onClick.AddListener(() => MainMenuManager.GoToLevelButton(level));

        // Conditions for: have saved data, level is locked, level is unlocked but unplayed. 
        if (GameManager.PlayerSaveData.GetBestLevel(level.LevelName, out LevelSaveData savedLevel))
        {
            textBox.text = savedLevel.InfoString();
            PlayLevelButton.gameObject.SetActive(true);
        }
        else
        {
            textBox.text = 
                "Level Info: " + level.LevelName + 
                "No completed succesfful run.";
            // This is also where'd I'd like to add a lock icon as greying out of the play button and menu


            PlayLevelButton.gameObject.SetActive(false);

            Debug.Log("No completed successful run");
        }
        
        

            

    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
