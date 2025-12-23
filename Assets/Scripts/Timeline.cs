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

    

    List<Beat> beats; /// List of beat classes
    GameObject timelineBeatPrefab; // the actual visual beat prefab that reacts to input
    TimelineBeat[] beatObjects; // the list of beat objects that can be manipulated

    TMP_InputField inputField;
    TMP_Text currentWordTMP;
    TMP_Text currentKanaTMP;


    void Start()
    {
        inputField = GameManager.inputField;
        currentWordTMP = GameObject.FindWithTag("CurrentWord").GetComponent<TMP_Text>();
        currentKanaTMP = GameObject.FindWithTag("CurrentKana").GetComponent<TMP_Text>();

        // BPM based on level data
        BPS = BPM / 60;

        // BeatDistance based on level data (maybe)
        timelineSpeed = (BPM / 60) * BeatDistance;

        // Load TimelineBeat prefab, create beats list and generate TimelineBeats
        timelineBeatPrefab = Resources.Load<GameObject>("TimelineBeatPrefab");
        beats = new List<Beat>();

        // Loop through data and create beats
        GenerateBeatListSequential(ref beats);

        // Make BeatObjects
        beatObjects = MakeBeats(beats);
    }

    // this method creates the beat class instances in a list in sequential order that they were created in the array
    void GenerateBeatListSequential(ref List<Beat> beatsList)
    {
        Beat.AddEmptyBeats(ref beatsList, levelPreBeats);

        for (int i = 0; i < beatElementsBank.Length; i++)
        {
            Beat.ProcessElement(ref beatsList, beatElementsBank[i]);
            Beat.AddEmptyBeats(ref beatsList, betweenBeats);
        }
    }

    // This method instantiates the beat prefabs into child objects of the Timeline. 
    TimelineBeat[] MakeBeats(List<Beat> beatList)
    {
        // TimelineBeat component array to return
        TimelineBeat[] beatObjects = new TimelineBeat[beatList.Count];

        // Iterate over all of the beat class instances
        for (int i = 0; i < beatList.Count; i++)
        {
            if (beatList[i].text == string.Empty) continue; // skip if empty string beat class
            Vector3 offset = Vector3.right * BeatDistance * 0.5f; // Offset by half a BeatDistance (places beat square in the center of the correct bounds)
            Vector3 pos = Vector3.right * BeatDistance * i + offset; 

            GameObject gameObject = Instantiate(timelineBeatPrefab, pos, Quaternion.identity, transform); // Create the object
            beatObjects[i] = gameObject.GetComponent<TimelineBeat>(); // Grab the component
            beatObjects[i].SetBeat(beatList[i]); 
        }

        return beatObjects;
    }

    public void KeyUpdate()
    {
        if (!duringKeyRecognition) return; // Quit if can't score now.

        string text = inputField.text;
        
        // check text match!
        //
        //

        if (true) // if text match
        {
            inputField.text = string.Empty;
            float accuracy = 1f - (Mathf.Abs(aroundBeatTime) / maxBeatError);
            float round = Mathf.Floor(accuracy * 100f) / 100f;
            accuracy = Mathf.Clamp01(round);
        }
    }

    void Update()
    {
        if (GameManager.gamePaused) return; // don't update if game is paused

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
