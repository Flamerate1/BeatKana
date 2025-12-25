using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timeline : MonoBehaviour
{
    [SerializeField] Level levelObject; // Optional Level scriptable object. 
    bool isLevelLoaded = false;

    [SerializeField] int BPM = 60;
    [SerializeField] float BeatDistance = 1.0f;
    [SerializeField] float maxBeatError = 0.2f; // Can't be equal to or more than 0.25f;
    [SerializeField] BeatElement[] beatElementsBank; // Assume in order for now. 

    [SerializeField] int levelPreBeats = 3;
    [SerializeField] int betweenBeats = 1;
    [SerializeField] int levelPostBeats = 2;


    float BPS; // Calculate from BPM / 60 seconds per minute;
    float timelineSpeed; // Calculated from BPM and BeatDistance. Physical speed that the positions moves with
    int previousBeatIndex = 0; // Used to play code at every beat increment.
    float beatTime = 0f; // Accumulated actual time that has passed
    int currentBeatIndex = 0; // The current beat which is seconds / BPM
    float aroundBeatTime = 0f; // time between now and the actual current beat point.
    bool canStartRecognition = false; // Enables single flip of duringKeyRecognition to true at start of input period. 
    bool canStopRecognition = false; // Enables single flip of duringKeyRecognition to false at end of input period.  
    bool duringKeyRecognition = false; // true during the moments when the player can input for the letter and score. 
    float levelProgress = 0f; // Between 0 and 1 for how far the level has progressed

    // Performance stats
    bool scoredSuccessfully = false; // resets during each input period. 
    int totalPoints = 0; // Accumulated points
    SummaryScreen summaryScreen;

    // Beat Processing and note keeping
    Beat currentBeat; // Reference to the current Beat class instance
    List<Beat> beatList; /// List of beat classes
    GameObject timelineBeatPrefab; // the actual visual beat prefab that reacts to input
    TimelineBeat[] beatObjects; // the list of beat objects that can be manipulated

    // Visual Section
    GameObject beatLine; // Visual guideline made at every beat position. 
    LineRenderer progressBar; // Bar that display how far through the level the player is. 
    TMP_InputField inputField;
    TMP_Text currentWordTMP;
    TMP_Text currentKanaTMP;
    TMP_Text scoreDisplay;


    void Start()
    {
        progressBar = GameObject.FindWithTag("ProgressBar").GetComponent<LineRenderer>();
        inputField = GameManager.inputField;
        currentWordTMP = GameObject.FindWithTag("CurrentWord").GetComponent<TMP_Text>();
        currentKanaTMP = GameObject.FindWithTag("CurrentKana").GetComponent<TMP_Text>();
        scoreDisplay = GameObject.FindWithTag("ScoreDisplay").GetComponent<TMP_Text>();
        summaryScreen = GameObject.FindWithTag("SummaryScreen").GetComponent<SummaryScreen>();
        summaryScreen.Initialize(); // Just sets its child to inactive and gets itself ready for activation
        summaryScreen.gameObject.SetActive(false);

        LoadTimeline();
    }

    void LoadTimeline()
    {
        // Grab GameManager level if it has one
        if (GameManager.currentLevel != null)
        {
            levelObject = GameManager.GetLevelThenNull();
            Debug.Log("Grabbing GameManager's recorded level");
        }
        else
        {
            Debug.Log("Using stored level");
        }

        // Inititialize level data. 

        if (levelObject != null)
        {
            Debug.Log("using either inspector stored or GameManager");
            levelObject.GetLevelData(
                ref BPM,
                ref BeatDistance,
                ref maxBeatError,
                ref beatElementsBank,

                ref levelPreBeats,
                ref betweenBeats,
                ref levelPostBeats
            );
        }
        else
        {
            Debug.Log("Using default inspector values");
        }


        // BPM based on level data
        BPS = 60f / BPM;

        // BeatDistance based on level data (maybe)
        timelineSpeed = BPS * BeatDistance;

        // Load beatLine
        beatLine = Resources.Load<GameObject>("BeatLine");

        // Load TimelineBeat prefab, create beats list and generate TimelineBeats
        timelineBeatPrefab = Resources.Load<GameObject>("TimelineBeatPrefab");
        beatList = new List<Beat>();

        // Loop through data and create beats
        GenerateBeatListSequential(ref beatList);

        // Make BeatObjects
        beatObjects = MakeBeats(beatList);
        isLevelLoaded = true;
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

        Beat.AddEmptyBeats(ref beatsList, levelPostBeats);
    }

    // This method instantiates the beat prefabs into child objects of the Timeline. 
    TimelineBeat[] MakeBeats(List<Beat> beatList)
    {
        // TimelineBeat component array to return
        TimelineBeat[] beatObjects = new TimelineBeat[beatList.Count];

        // Iterate over all of the beat class instances
        for (int i = 0; i < beatList.Count; i++)
        {
            Vector3 offset = Vector3.right * BeatDistance * 0.5f; // Offset by half a BeatDistance (places beat square in the center of the correct bounds)
            Vector3 pos = Vector3.right * BeatDistance * i + offset;
            
            Instantiate(beatLine, pos, Quaternion.identity, transform); // Create beatLine guide objects

            if (beatList[i].text == string.Empty) continue; // skip if empty string beat class
            GameObject gameObject = Instantiate(timelineBeatPrefab, pos, Quaternion.identity, transform); // Create the object
            beatObjects[i] = gameObject.GetComponent<TimelineBeat>(); // Grab the component
            beatObjects[i].SetBeat(beatList[i]); 
        }

        return beatObjects;
    }

    public void CheckBeat()
    {
        // Guard clauses
        if (GameManager.gamePaused || // Don't input if game is paused >:( CHEATER
            !duringKeyRecognition || // Quit if can't score now.
            scoredSuccessfully || // Don't search any longer if scored already. 
            currentBeat.text == string.Empty // Don't search if currently an empty beat
        ) return; 

        string text = inputField.text;
        
        if (text.Contains(currentBeat.text)) // if text match
        {
            scoredSuccessfully = true;
            inputField.text = string.Empty;
            float accuracy = 1f - (Mathf.Abs(aroundBeatTime) / maxBeatError); // Accuracy depending on distance to beat. 
            float round = Mathf.Floor(accuracy * 100f) / 100f; // reduce to 2 decimal places
            accuracy = Mathf.Clamp01(round); // Clamp between 0 and 1

            var points = 20 + Mathf.RoundToInt(accuracy * 100f);
            Debug.Log("Scored: " + points.ToString());

            // Add to total score. 
            totalPoints += points;

            // Update ScoreDisplay
            scoreDisplay.text = "Score: " + totalPoints.ToString();

        }
    }

    void Update()
    {
        if (!isLevelLoaded) return;
        if (GameManager.gamePaused) return; // don't update if game is paused

        beatTime += Time.deltaTime / BPS;
        currentBeatIndex = Mathf.FloorToInt(beatTime); 
        if (currentBeatIndex >= beatList.Count) { LevelEnd(); return; }
        currentBeat = beatList[currentBeatIndex];
        aroundBeatTime = (beatTime - currentBeatIndex) - 0.5f;

        levelProgress = beatTime / beatList.Count;

        if (currentBeatIndex != previousBeatIndex)
        {
            // Code to switch text, image, etc about beat
            previousBeatIndex = currentBeatIndex;
            canStartRecognition = true;
            canStopRecognition = true;

            // Update Visuals based on current Beat class
            if (currentBeatIndex < beatList.Count)
            {
                currentKanaTMP.text = beatList[currentBeatIndex].text;
                currentWordTMP.text = beatList[currentBeatIndex].word;
            }
            else
            {
                currentKanaTMP.text = string.Empty;
                currentWordTMP.text = string.Empty;
            }
        }

        if (canStartRecognition && aroundBeatTime >= -maxBeatError)
        {
            canStartRecognition = false;
            duringKeyRecognition = true;
            CheckBeat(); // Check Beat after turn on key recognition (duringKeyRecognition must be true)
        }

        if (canStopRecognition && aroundBeatTime >= maxBeatError)
        {
            canStopRecognition = false;
            CheckBeat(); // Check Beat before turn off key recognition (duringKeyRecognition must be true)
            duringKeyRecognition = false;

            scoredSuccessfully = false;

            // If keyupdate fails, then fail the beat
            inputField.text = string.Empty;
        }

        float TimelinePos = beatTime * BeatDistance;
        var pos = Vector3.right * (-TimelinePos);
        transform.position = pos;

        // Update progress bar
        var progressBarPos = progressBar.GetPosition(1);

        var cam_left = GameManager.camWorldCorners[0].x;
        var cam_right = GameManager.camWorldCorners[2].x;
        
        var progress_rel_x = (cam_right - cam_left) * levelProgress;
        progressBarPos.x = progress_rel_x + cam_left;
        progressBar.SetPosition(1, progressBarPos);
    }
    
    void LevelEnd()
    {
        GameManager.PauseGame(true);

        // Save stats
        // level ending transition
        // maybe goto a level success or failure screen with stats
        summaryScreen.gameObject.SetActive(true);
        summaryScreen.Activate();
        // switch to main menu with said conclusion screen
    }
}
