using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    // This is the level to be loaded and save data referenced to in the menu
    [SerializeField] Level level;

    Button button;
    Image image;

    void Start()
    {
        MainMenuManager MainMenuManager = GetComponentInParent<MainMenuManager>();
        button = GetComponent<Button>();
        image = GetComponent<Image>();
        //button.onClick.AddListener(() => MainMenuManager.GoToLevelButton(level));
        button.onClick.AddListener(() => MainMenuManager.ActivatePlayLevelInfo(this.level));

        

        if (GameManager.PlayerSaveData.IsLevelLocked(level))
        {
            image.color = Color.grey;
        }
        else if (!GameManager.PlayerSaveData.IsLevelCompleted(level.LevelName))
        {
            image.color = Color.greenYellow;
        }

    }
}
