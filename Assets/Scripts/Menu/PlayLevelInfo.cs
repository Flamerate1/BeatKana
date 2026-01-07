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
        else if (GameManager.PlayerSaveData.IsLevelLocked(level, out PlayerSaveData.PrereqStats stats))
        {
            textBox.text =
                level.LevelName + "\n" +
                "LOCKED";

            if (!stats.scoreSufficient)
            {
                Debug.Log(level.LevelName + " insufficient minScore!");
                textBox.text = textBox.text + "\n" +
                    "You require " + (5).ToString() + " more points.";
            }

            for (int i = 0; i < stats.levelSufficient.Length; i++)
            {
                if (!stats.levelSufficient[i])
                {
                    textBox.text = textBox.text + "\n" +
                        level.prereqs.requiredLevels[i].LevelName + " has not been completed.";
                }
            }

            PlayLevelButton.gameObject.SetActive(false);
        }
        else
        {
            textBox.text =
                level.LevelName + "\n" +
                "No completed succesfful run.";
            // This is also where'd I'd like to add a lock icon as greying out of the play button and menu

            PlayLevelButton.gameObject.SetActive(true);
            Debug.Log("No completed successful run");
        }
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
