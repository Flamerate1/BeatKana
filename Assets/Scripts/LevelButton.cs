using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    // This is the level to be loaded and save data referenced to in the menu
    [SerializeField] Level level;

    Button button;

    void Start()
    {
        MenuScript menuScript = GetComponentInParent<MenuScript>();
        button = GetComponent<Button>();
        button.onClick.AddListener(() => menuScript.GoToLevelButton(level));
    }
}
