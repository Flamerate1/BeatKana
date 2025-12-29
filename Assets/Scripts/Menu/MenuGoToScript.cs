using UnityEngine;
using UnityEngine.UI;

public class MenuGoToScript : MonoBehaviour
{
    Button button; 
    MenuScript menuScript;
    [SerializeField] MenuScript.Menu menu;

    void Start()
    {
        button = GetComponent<Button>();
        menuScript = GetComponentInParent<MenuScript>();
        button.onClick.AddListener(() => menuScript.GoToMenuButton(menu));
    }
}
