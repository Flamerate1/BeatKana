using System;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;
    public static TMP_InputField inputField;

    public bool isBeatTimeline = true;

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

    // Current Level Data
    // Selected in main menu and grabbed by timeline in the playgame scene
    public static Level currentLevel;
    public static Level GetLevel() { Level level = currentLevel; return level; } // Was GetLevelThenNull to avoid next playthrough keeping the same level data.
    public static void SetLevel(Level level) { currentLevel = level; }


    private static string path;
    public static PlayerSaveData playerSaveData; // Stores level progression, currencies, etc

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        
        SceneManager.activeSceneChanged += OnSceneChange;

        path = Path.Combine(Application.persistentDataPath, "save.json");
        LoadGame();
        SaveGame(); // for testing save function
    }

    // SceneManager.activeSceneChanged += OnSceneChange; // In Awake method
    private void OnSceneChange(Scene current, Scene next)
    {
        string currentName = current.name;

        if (currentName == null)
        {
            // Scene1 has been removed
            currentName = "Replaced";
        }

        switch (next.name)
        {
            case "PlayScene":
                cam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
                cam_z = cam.transform.position.z;
                inputField = GameObject.FindWithTag("InputField").GetComponent<TMP_InputField>();
                CanvasRectTransform = GameObject.FindWithTag("Canvas").GetComponent<RectTransform>();
                TimelinePositionRectTransform = GameObject.FindWithTag("TimelineCameraPosition").GetComponent<RectTransform>();
                lastHeight = 0; lastWidth = 0;
                break;
            case "MainMenu":
                cam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
                break;
        }
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

    public static void SaveGame()
    {
        // Grab playerSaveData from GameManager and put it into json to place into a save file. 
        string json = JsonUtility.ToJson(playerSaveData, true);
        File.WriteAllText(path, json);
        Debug.Log(json);
        Debug.Log("Saved to: " + path);
    }

    static void LoadGame()
    {
        // Grab data from json and put it into GameManager's playerSaveData object. 
        // Update saveVersion

        

        if (!File.Exists(path))
        {
            Debug.LogWarning("No save file found, returning new save.");
            playerSaveData = new PlayerSaveData();
            return;
        }
        
        string json = File.ReadAllText(path);
        Debug.Log(json);
        playerSaveData = JsonUtility.FromJson<PlayerSaveData>(json);

        // If no save data create default save file. 
    }

    static void LoadLevelData()
    {
        Level[] allLevels = Resources.LoadAll<Level>("ScriptableObjects/Levels/");
    }
}
