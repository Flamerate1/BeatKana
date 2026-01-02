using UnityEngine;
using UnityEngine.UI;

public class MenuGoToScript : MonoBehaviour
{
    Button button; 
    MainMenuManager menuScript;
    [SerializeField] MainMenuManager.Menu menu;

    void Start()
    {
        button = GetComponent<Button>();
        menuScript = GetComponentInParent<MainMenuManager>();
        button.onClick.AddListener(() => menuScript.GoToMenuButton(menu));
    }
}
