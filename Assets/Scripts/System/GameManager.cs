using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static Camera cam;
    public static TMP_InputField inputField;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        cam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        inputField = GameObject.FindWithTag("InputField").GetComponent<TMP_InputField>();
    }


    private void Update()
    {
    }
}
