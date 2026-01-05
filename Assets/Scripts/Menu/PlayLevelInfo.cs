using UnityEngine;
using UnityEngine.UI;

public class PlayLevelInfo : MonoBehaviour
{

    Button PlayLevelButton;
    MainMenuManager MainMenuManager;
    public void Initialize(MainMenuManager MainMenuManager)
    {
        PlayLevelButton = GetComponentInChildren<Button>();
        this.MainMenuManager = MainMenuManager;
    }

    public void Activate(Level level)
    {
        gameObject.SetActive(true);
        PlayLevelButton.onClick.RemoveAllListeners();
        PlayLevelButton.onClick.AddListener(() => MainMenuManager.GoToLevelButton(level));

    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
