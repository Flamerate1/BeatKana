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
        public InputString InputString;
        public SummaryScreen SummaryScreen;
        public FeedbackGraphic FeedbackGraphic;
        public TMP_Text currentWordTMP;
        public TMP_Text currentKanaTMP;
        public TMP_Text scoreDisplay;
    }

    [SerializeField] GMFields gmFields;
    [SerializeField] TLFields tlFields;

    [SerializeField] Timeline Timeline;
    [SerializeField] KanaKeyboard KanaKeyboard;

    private void Awake()
    {
        GameManager.PlayManagerSetFields(gmFields); 
        GameManager.InitializePlayScene();

        Timeline.PlayManagerSetFields(tlFields); 
        Timeline.StartGame();

        KanaKeyboard.PlayManagerSetFields(tlFields);
        KanaKeyboard.Initialize();
    }
}
