using System;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;

public class PlayManager : MonoBehaviour
{
    [Serializable]
    public struct GMFields // GameManager Fields
    {
        public Camera Camera;
        public RectTransform canvasRectTransform;
        public RectTransform timelineCameraPosition;

    }
    [Serializable]
    public struct TLFields // Timeline Fields
    {
        public LineRenderer progressBar;
        public LineRenderer timeRemainingBar;
        public InputString InputString;
        public SummaryScreen SummaryScreen;
        public FeedbackGraphic FeedbackGraphic;
        public TMP_Text currentWordTMP;
        public TMP_Text currentKanaTMP;
        public TMP_Text scoreDisplay;
    }

    [SerializeField] Level.LevelType levelType;

    [SerializeField] GMFields gmFields; // Variables to set for the GameManager
    [SerializeField] TLFields tlFields; // Variables to set for the Timeline

    [SerializeField] Timeline BeatTimeline;
    [SerializeField] Timeline QueueTimeline;
    Timeline Timeline;

    [SerializeField] KanaKeyboard KanaKeyboard;

    [SerializeField] PauseScreen PauseScreen;

    private void Awake()
    {
        // Initialize GameManager
        GameManager.PlayManagerSetFields(gmFields); 
        GameManager.InitializePlayScene();

        if (GameManager.currentLevel != null)
        {
            levelType = GameManager.currentLevel.levelType;
            Debug.Log("Grabbing GM's LevelType");
        }

        // Set the current timeline. 
        switch (levelType) {
            case Level.LevelType.Beat:
                Timeline = BeatTimeline;
                Destroy(QueueTimeline.gameObject);
                break;

            case Level.LevelType.Queue:
                Timeline = QueueTimeline;
                Destroy(BeatTimeline.gameObject);
                break;
        }

        Timeline.PlayManagerSetFields(tlFields);
        Timeline.StartGame();

        KanaKeyboard.PlayManagerSetFields(tlFields);
        KanaKeyboard.Initialize();
        
        PauseScreen.Init();
        PauseScreen.gameObject.SetActive(false);
    }

    public void PauseGame() { PauseGame(!GameManager.gamePaused); }
    public void PauseGame(bool doPause)
    {
        GameManager.PauseGame(doPause);
        //tlFields.SummaryScreen.Pause(doPause);
        PauseScreen.Pause(doPause);
    }
}
