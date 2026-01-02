using System;
using TMPro;
using UnityEngine;

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
        // progressBar, inputString
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

    [SerializeField] GMFields gmFields;
    [SerializeField] TLFields tlFields;

    [SerializeField] Timeline BeatTimeline;
    [SerializeField] Timeline QueueTimeline;
    Timeline Timeline;

    [SerializeField] KanaKeyboard KanaKeyboard;

    private void Awake()
    {
        GameManager.PlayManagerSetFields(gmFields); 
        GameManager.InitializePlayScene();

        if (GameManager.currentLevel != null)
        {
            levelType = GameManager.currentLevel.levelType;
            Debug.Log("Grabbing GM's LevelType");
        }

        switch (levelType) {
            case Level.LevelType.Beat:
                Timeline = BeatTimeline;
                Destroy(QueueTimeline.gameObject);
                //QueueTimeline.gameObject.SetActive(false);
                break;

            case Level.LevelType.Queue:
                Timeline = QueueTimeline;
                Destroy(BeatTimeline.gameObject);
                //Timeline.gameObject.SetActive(false);
                break;
        }

        Timeline.PlayManagerSetFields(tlFields);
        Timeline.StartGame();

        KanaKeyboard.PlayManagerSetFields(tlFields);
        KanaKeyboard.Initialize();
    }
}
