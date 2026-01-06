using System;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    // Camera Positioning
    public static Camera cam;
    public static event Action OnResolutionChanged;
    static int lastWidth = 0;
    static int lastHeight = 0;
    public static float cam_z;
    public static Vector3[] camWorldCorners;

    // 


    //public static TMP_InputField inputField;
    public static InputString inputString;
    public static RectTransform CanvasRectTransform;
    static RectTransform TimelineCameraPosition;

    public static bool gamePaused = false;

    // Current Level Data
    // Selected in main menu and grabbed by timeline in the playgame scene
    public static Level currentLevel;
    public static Level GetLevel() { Level level = currentLevel; return level; } // Was GetLevelThenNull to avoid next playthrough keeping the same level data.
    public static void SetLevel(Level level) { currentLevel = level; }


    private static string saveDataPath; // 
    public static PlayerSaveData PlayerSaveData; // Stores level progression, currencies, etc

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        saveDataPath = Path.Combine(Application.persistentDataPath, "save.json");
        PlayerSaveData = PlayerSaveData.LoadFromJson(saveDataPath);
    }

    public static void PlayManagerSetFields(PlayManager.GMFields gmFields)
    {
        GameManager.cam = gmFields.Camera;
        GameManager.CanvasRectTransform = gmFields.canvasRectTransform;
        GameManager.TimelineCameraPosition = gmFields.timelineCameraPosition;
    }
    public static void InitializePlayScene()
    {
        Debug.Log("Scene is now PlayScene");
        cam_z = cam.transform.position.z;
        lastHeight = 0; lastWidth = 0;
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
        TimelineCameraPosition.GetPositionAndRotation(out Vector3 pos2, out Quaternion quat2);
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
        // Perform live screen updating
        if (SceneManager.GetActiveScene().name != "PlayScene") { return; }

        // Detect screen orientation
        if (Screen.width != lastWidth || Screen.height != lastHeight)
        {
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

    static void LoadLevelData()
    {
        Level[] allLevels = Resources.LoadAll<Level>("ScriptableObjects/Levels/");
    }
}
