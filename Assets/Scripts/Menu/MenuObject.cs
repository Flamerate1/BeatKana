using UnityEngine;

public class MenuObject : MonoBehaviour
{
    [SerializeField] MainMenuManager.Menu menu;
    public MainMenuManager.Menu Menu => menu;

    public void Activate()
    {
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
