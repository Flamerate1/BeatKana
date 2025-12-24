using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static TMP_InputField inputField;

    // Camera Positioning
    public static Camera cam;
    public static event Action OnResolutionChanged;
    static int lastWidth = 0;
    static int lastHeight = 0;
    public static float cam_z;
    public static Vector3[] camWorldCorners;

    public static RectTransform CanvasRectTransform;
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


        // Not fully decided functionality how this works
        // When does the camera get updated and all other things reliant on the camera? 
        UpdateCameraPosition();
    }

    // Keeps the camera's position at the designated UI location set by TimelinePositionRectTransform
    public static void UpdateCameraPosition()
    {
        lastWidth = Screen.width;
        lastHeight = Screen.height;

        // Get world position of canvas center
        CanvasRectTransform.GetPositionAndRotation(out Vector3 pos1, out Quaternion quat1);
        pos1 = cam.ScreenToWorldPoint(pos1);
        
        // Get world position of timeline position within the canvas
        TimelinePositionRectTransform.GetPositionAndRotation(out Vector3 pos2, out Quaternion quat2);
        pos2 = cam.ScreenToWorldPoint(pos2);

        // Get relative world position
        Vector3 relPos = pos1 - pos2;
        relPos.z = cam_z; // overwrite z coordinate 

        // apply position to camera
        cam.transform.position = relPos;

        // Now grab cam stats
        camWorldCorners = new Vector3[4];
        CanvasRectTransform.GetWorldCorners(camWorldCorners);
        for (int i = 0; i < camWorldCorners.Length; i++)
            camWorldCorners[i] = cam.ScreenToWorldPoint(camWorldCorners[i]);
    }

    private void Update()
    {
        // Detect screen orientation
        if (Screen.width != lastWidth || Screen.height != lastHeight)
        {
            //lastWidth = Screen.width;
            //lastHeight = Screen.height;
            UpdateCameraPosition();
            OnResolutionChanged?.Invoke();
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
