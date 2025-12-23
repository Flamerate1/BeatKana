using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class Timeline : MonoBehaviour
{
    [SerializeField] int BPM = 60;
    [SerializeField] float BeatDistance = 1.0f;
    [SerializeField] float maxBeatError = 0.2f; // Can't be equal to or more than 0.25f;
    [SerializeField] BeatElement[] beatElementsBank; // Assume in order for now. 

    [SerializeField] int levelPreBeats = 3;
    [SerializeField] int betweenBeats = 1;
    //[SerializeField] int eachBeatElementCount = 2;
    //[SerializeField] int levelBeatElementAmount = 10;


    float BPS; // Calculate from BPM / 60 seconds per minute;
    float timelineSpeed; // Calculated from BPM and BeatDistance. Physical speed that the positions moves with
    int previousBeat = 0; // Used to play code at every beat increment.
    [SerializeField] float beatTime = 0f; // Accumulated actual time that has passed
    [SerializeField] int currentBeat = 0; // The current beat which is seconds / BPM
    [SerializeField] float aroundBeatTime = 0f; // time between now and the actual current beat point.
    bool canStartRecognition = false;
    bool canStopRecognition = false;
    bool duringKeyRecognition = false;

    

    // Beat[] beats;
    List<Beat> beats;

    TMP_InputField inputField;
    TextMeshPro currentWordTMP;
    TextMeshPro currentKanaTMP;

    GameObject timelineBeatPrefab;
    List<TimelineBeat> timelineBeats;

    void Start()
    {
        inputField = GameManager.inputField;
        currentWordTMP = GameObject.FindWithTag("CurrentWord").GetComponent<TextMeshPro>();
        currentKanaTMP = GameObject.FindWithTag("CurrentKana").GetComponent<TextMeshPro>();

        // BPM based on level data
        BPS = BPM / 60;

        // BeatDistance based on level data (maybe)
        timelineSpeed = (BPM / 60) * BeatDistance;

        // Load TimelineBeat prefab, create beats list and generate TimelineBeats
        timelineBeatPrefab = Resources.Load<GameObject>("TimelineBeatPrefab");
        beats = new List<Beat>();

        // Loop through data and create beats
        beats = GenerateBeatsSequential();
    }

    // this method creates beats on the timeline in sequential order that they were created in the array
    List<Beat> GenerateBeatsSequential()
    {
        List<Beat> result = new List<Beat>();

        for (int i = 0; i < beatElementsBank.Length; i++)
        {
            Beat.ProcessElement(ref beats, beatElementsBank[i]);
            Beat.AddEmptyBeats(ref beats, betweenBeats);
        }
        return result;
    }

    public void KeyUpdate()
    {
        if (!duringKeyRecognition) return; // Quit if can't score now.

        string text = inputField.text;
        Debug.Log("KeyUpdate: " + text);
        
        // check text match!
        //
        //

        if (true) // if text match
        {
            inputField.text = string.Empty;
            float accuracy = 1f - (Mathf.Abs(aroundBeatTime) / maxBeatError);
            float round = Mathf.Floor(accuracy * 100f) / 100f;
            accuracy = Mathf.Clamp01(round);
            Debug.Log(accuracy.ToString());
        }
    }

    void Update()
    {
        beatTime += Time.deltaTime / BPS;
        currentBeat = Mathf.FloorToInt(beatTime);
        aroundBeatTime = (beatTime - currentBeat) - 0.5f;

        if (currentBeat != previousBeat)
        {
            // Code to switch text, image, etc about beat
            previousBeat = currentBeat;
            canStartRecognition = true;
            canStopRecognition = true;
        }

        if (canStartRecognition && aroundBeatTime >= -maxBeatError)
        {
            canStartRecognition = false;
            duringKeyRecognition = true;
            KeyUpdate();// Key update after turn on key recognition
        }

        if (canStopRecognition && aroundBeatTime >= maxBeatError)
        {
            canStopRecognition = false;
            KeyUpdate(); // Key update before turn off key recognition
            duringKeyRecognition = false;
            
            // If keyupdate fails, then fail the beat
        }

        
        // Update Timeline position over time
        // assumes seconds = meters
        float TimelinePos = beatTime * timelineSpeed;
        var pos = Vector3.right * (-TimelinePos);
        transform.position = pos;
    }
}
