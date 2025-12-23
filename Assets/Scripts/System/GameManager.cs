using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static Camera cam;
    static int lastWidth;
    static int lastHeight;
    public static float cam_z;
    public static TMP_InputField inputField;

    static RectTransform CanvasRectTransform;
    static RectTransform TimelinePositionRectTransform;

    public static bool gamePaused = false;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        cam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        cam_z = cam.transform.position.z;
        inputField = GameObject.FindWithTag("InputField").GetComponent<TMP_InputField>();
        CanvasRectTransform = GameObject.FindWithTag("Canvas").GetComponent <RectTransform>();
        TimelinePositionRectTransform = GameObject.FindWithTag("TimelineCameraPosition").GetComponent<RectTransform>();
        //Screen.orientation = ScreenOrientation.LandscapeLeft;

        UpdateCameraPosition();
    }
    
    public static void UpdateCameraPosition()
    {

        CanvasRectTransform.GetPositionAndRotation(out Vector3 pos1, out Quaternion quat1);
        pos1 = cam.ScreenToWorldPoint(pos1);
        TimelinePositionRectTransform.GetPositionAndRotation(out Vector3 pos2, out Quaternion quat2);
        pos2 = cam.ScreenToWorldPoint(pos2);

        Vector3 relPos = pos1 - pos2;
        relPos.z = cam_z;
        

        Debug.Log(relPos.ToString());

        cam.transform.position = relPos;
    }

    private void Update()
    {
        // Detect screen orientation
        if (Screen.width != lastWidth || Screen.height != lastHeight)
        {
            lastWidth = Screen.width;
            lastHeight = Screen.height;
            UpdateCameraPosition();
        }
    }

    public static void PauseGame() { PauseGame(!GameManager.gamePaused); }

    public static void PauseGame(bool doPause)
    {
        if (doPause) { GameManager.gamePaused = true; Time.timeScale = 0.0f; }
        else { GameManager.gamePaused = false; Time.timeScale = 1.0f; }
        Debug.Log("Game paused?: " + doPause.ToString());
    }
}
