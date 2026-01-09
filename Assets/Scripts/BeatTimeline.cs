using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static FeedbackGraphic;

public class BeatTimeline : Timeline
{
    #region Vars

    // BeatTimeline specific 

    float BPS; // Calculate from BPM / 60 seconds per minute;
    int previousBeatIndex = 0; // Used to play code at every beat increment.
    float beatTime = 0f; // Accumulated actual time that has passed
    float aroundBeatApex = 0f; // time between now and the actual current beat point.
    bool canStartRecognition = false; // Enables single flip of duringKeyRecognition to true at start of input period. 
    bool canStopRecognition = false; // Enables single flip of duringKeyRecognition to false at end of input period.  
    bool duringKeyRecognition = false; // true during the moments when the player can input for the letter and score. 
    bool scoredSuccessfully = false; // resets during each input period. 
    AudioClip tickAudioClip; // Metronome sound effect. 
    AudioSource AudioSource;
    #endregion

    #region Start(), LoadTimeline(), GenerateBeatListSequential() & MakeBeats()
    public override void PlayManagerSetFields(PlayManager.TLFields tlFields)
    {
        this.progressBar = tlFields.progressBar;
        this.InputString = tlFields.InputString;
        this.SummaryScreen = tlFields.SummaryScreen;
        this.FeedbackGraphic = tlFields.FeedbackGraphic;
        this.currentWordTMP = tlFields.currentWordTMP;
        this.currentKanaTMP = tlFields.currentKanaTMP;
        this.scoreDisplay = tlFields.scoreDisplay;
    }
    public override void StartGame()
    {
        InputString.Initialize();
        InputString.UpdateStringEvent += CheckBeat;
        InputString.ResetString();
        currentKanaTMP.text = string.Empty;
        currentWordTMP.text = string.Empty;
        SummaryScreen.Initialize(); // Just sets its child to inactive and gets itself ready for activation
        SummaryScreen.gameObject.SetActive(false);
        FeedbackGraphic.gameObject.SetActive(false);

        AudioSource = GetComponent<AudioSource>();
        tickAudioClip = Resources.Load<AudioClip>("Synth_Tick_A_hi");

        LoadTimeline();
    }

    void LoadTimeline()
    {
        // Grab GameManager level if it has one
        if (GameManager.currentLevel != null)
        {
            levelObject = GameManager.GetLevel();
            Debug.Log("Grabbing GameManager's recorded level");
        }
        else
        {
            Debug.Log("Using stored level");
        }

        // Inititialize level data. 

        levelObject.GetLevelData(
            ref BPM,
            ref BeatDistance,
            ref maxBeatError,
            ref beatElementsBank,

            ref levelPreBeats,
            ref betweenBeats,
            ref levelPostBeats
        );


        // BPM based on level data
        BPS = 60f / BPM;

        // Load beatLine
        beatLine = Resources.Load<GameObject>("BeatLine");
        beatLineBlack = Resources.Load<GameObject>("BeatLine BlackVariant");

        // Load TimelineBeat prefab, create beats list and generate TimelineBeats
        timelineBeatPrefab = Resources.Load<GameObject>("TimelineBeatPrefab");
        beatList = new List<Beat>();

        // Loop through data and create beats
        GenerateBeatListSequential(ref beatList);

        // Make BeatObjects
        beatObjects = MakeBeats(beatList);
        isLevelLoaded = true;
        GameManager.PauseGame(false);
        NewBeat();
    }

    // this method creates the beat class instances in a list in sequential order that they were created in the array
    void GenerateBeatListSequential(ref List<Beat> beatsList)
    {
        Beat.AddEmptyBeats(ref beatsList, levelPreBeats);

        for (int i = 0; i < beatElementsBank.Length; i++)
        {
            //Beat.ProcessElement(ref beatsList, beatElementsBank[i]);
            beatElementsBank[i].ProcessToBeat(ref beatsList);
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
            Vector3 pos1 = Vector3.right * BeatDistance * i + offset;
            Vector3 pos2 = Vector3.right * BeatDistance * i;
            
            Instantiate(beatLine, pos1, Quaternion.identity, transform); // Create beatLine guide objects
            Instantiate(beatLineBlack, pos2, Quaternion.identity, transform); // Create beatLine guide objects

            if (beatList[i].text == string.Empty) continue; // skip if empty string beat class
            GameObject gameObject = Instantiate(timelineBeatPrefab, pos1, Quaternion.identity, transform); // Create the object
            beatObjects[i] = gameObject.GetComponent<TimelineBeat>(); // Grab the component
            beatObjects[i].SetBeat(beatList[i]); 
        }

        return beatObjects;
    }

    #endregion

    #region Runtime: CheckBeat(), Update()
    public void CheckBeat(string text) 
    {
        if (!base.CheckBeatGuards(text)) { return; }

        // Punish player if inputted during wrong moments
        if ((tlState != TLState.Input || // Quit if can't score now.
            scoredSuccessfully || // Don't search any longer if scored already.
            currentBeat.text == string.Empty) && // Don't search if currently an empty beat
            text != string.Empty
            )
        /*if ((!duringKeyRecognition || // Quit if can't score now.
            scoredSuccessfully || // Don't search any longer if scored already.
            currentBeat.text == string.Empty) && // Don't search if currently an empty beat
            text != string.Empty
            )*/
        { 
            Debug.Log("Can't input. Wrong time!");
            // punish player code
            totalPoints -= 20;

            // Update ScoreDisplay
            scoreDisplay.text = "Score: " + totalPoints.ToString();

            FeedbackGraphic.InitiateFeedback(FeedbackGraphic.Degree.WrongTime);

            //inputField.text = string.Empty;
            InputString.ResetString();
            return;
        }

        // Correct Input
        if (text.Contains(currentBeat.text)) // if text match
        {
            scoredSuccessfully = true;
            //inputField.text = string.Empty;
            InputString.ResetString();
            float accuracy = 1f - (Mathf.Abs(aroundBeatApex) / maxBeatError); // Accuracy depending on distance to beat. 
            float round = Mathf.Floor(accuracy * 100f) / 100f; // reduce to 2 decimal places
            accuracy = Mathf.Clamp01(round); // Clamp between 0 and 1

            var points = 20 + Mathf.RoundToInt(accuracy * 100f);
            Debug.Log("Scored: " + points.ToString());

            // Add to total score. 
            totalPoints += points;

            // Update ScoreDisplay
            scoreDisplay.text = "Score: " + totalPoints.ToString();

            // Categorize Score, then InitiateFeedback for graphic. 
            FeedbackGraphic.InitiateFeedback(FeedbackGraphic.Degree.Perfect);
        }
    }


    enum TLState { Before, Input, After }
    [SerializeField] TLState tlState = TLState.Before;
    void NewBeat()
    {
        previousBeatIndex = currentBeatIndex;

        currentBeat = beatList[currentBeatIndex]; // The actual current Beat class instance being focused on in the current beat window
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
    void InputStart()
    {

    }
    void InputEnd()
    {
        if (currentBeat.text != string.Empty &&
                !scoredSuccessfully)
        {
            FeedbackGraphic.InitiateFeedback(FeedbackGraphic.Degree.Miss);
        }

        scoredSuccessfully = false;

        //inputField.text = string.Empty;
        InputString.ResetString();
    }

    void Update()
    {
        if (!base.UpdateGuards()) return;
        beatTime += Time.deltaTime / BPS; // Time counted as amount of beats (float, not discrete int)
        currentBeatIndex = Mathf.FloorToInt(beatTime);  // time as discrete int
        if (currentBeatIndex >= beatList.Count) { LevelEnd(true); return; } // End level when beat index hits end of beatList count

        //currentBeat = beatList[currentBeatIndex]; // The actual current Beat class instance being focused on in the current beat window
        aroundBeatApex = (beatTime - currentBeatIndex) - 0.5f; // Amount of time between now and the apex of the current beat. 

        levelProgress = beatTime / beatList.Count;

        // Use beatTime, currentBeatIndex, and aroundBeatApex to compute beat states. 
        switch (tlState)
        {
            case TLState.Before:
                if (aroundBeatApex >= -maxBeatError)
                {
                    InputStart();
                    tlState = TLState.Input;
                }
                break;
            case TLState.Input:
                if (aroundBeatApex >= maxBeatError)
                {
                    InputEnd();
                    tlState = TLState.After;
                }
                break;
            case TLState.After:
                if (currentBeatIndex != previousBeatIndex)
                {
                    NewBeat();
                    tlState = TLState.Before;
                }
                break;
        }

        /*
        // transition to new beat
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

        // Transition to input state
        if (canStartRecognition && aroundBeatApex >= -maxBeatError)
        {
            canStartRecognition = false;
            duringKeyRecognition = true;
        }

        // Transition to non input state
        if (canStopRecognition && aroundBeatApex >= maxBeatError)
        {
            if (currentBeat.text != string.Empty &&
                !scoredSuccessfully)
            {
                FeedbackGraphic.InitiateFeedback(FeedbackGraphic.Degree.Miss);
            }

            canStopRecognition = false;
            duringKeyRecognition = false;

            scoredSuccessfully = false;

            //inputField.text = string.Empty;
            InputString.ResetString();
        }*/

        

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
    #endregion
}
