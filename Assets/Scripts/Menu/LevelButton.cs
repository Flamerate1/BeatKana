using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    // This is the level to be loaded and save data referenced to in the menu
    [SerializeField] Level level;

    Button button;

    void Start()
    {
        MainMenuManager MainMenuManager = GetComponentInParent<MainMenuManager>();
        button = GetComponent<Button>();
        button.onClick.AddListener(() => MainMenuManager.GoToLevelButton(level));
    }
}
