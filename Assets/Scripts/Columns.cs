using System;
using System.Collections.Generic;
using UnityEngine;
using Melanchall.DryWetMidi.Interaction;

public class Columns : MonoBehaviour
{
    public static Columns columns; // Columns instance for public access
    public ScoreManager scoreManager;
    public TrackManager trackManager;
    public GameManager gameManager;
    public Melanchall.DryWetMidi.MusicTheory.NoteName noteRestriction; // Restricts notes to their corresponding key press
    public KeyCode input; // Input for any given lane
    public GameObject enemyPrefab; // Prefabs to spawn 
    public GameObject goodPrefab; // Feedback message prefab instance
    public GameObject perfectPrefab; // Feedback message prefab instance
    public GameObject missPrefab; // Feedback message prefab instance
    List<Enemy> enemies = new List<Enemy>(); // Keeps track of spawned notes
    public List<double> timeStamps = new List<double>(); // Timestamps of the song when the player needs to input keys for attack 
    Vector3 offset = new Vector3(0, -5.6f, 0); // Feedback meesage instantiation transform offset
    int spawn_i = 0; // Keeps track of enemy timestamps that need to be spawned
    int input_i = 0; // Keeps track of key inputs that need to be pressed
    double perfectMargin;
    double goodMargin;

    void Start()
    {
        columns = this;
        gameManager = FindObjectOfType<GameManager>();
        scoreManager = FindObjectOfType<ScoreManager>();
        trackManager = FindObjectOfType<TrackManager>();

        perfectMargin = FindObjectOfType<TrackManager>().perfectMargin; 
        goodMargin = FindObjectOfType<TrackManager>().goodMargin; 
    }

    public void SetTimeStamps(Melanchall.DryWetMidi.Interaction.Note[] array)
    {
        /* This function iterates and filters every note in the notes array generated with the midi file in the track manager. If the name of the note corresponds with its corresponding key input restriction, then its span will be obtained */
        foreach (var note in array)
        {
            /* If the name of the note corresponds with its corresponding key input restriction, then its time span will be obtained */
            if (note.NoteName == noteRestriction)
            {
                var metricTimeSpan = TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, TrackManager.midiFile.GetTempoMap()); // Convert note time to seconds by feeding the tempo map from the track manager
                timeStamps.Add((double)metricTimeSpan.Minutes * 60f + metricTimeSpan.Seconds + (double)metricTimeSpan.Milliseconds / 1000f); // Add converted time stamps to the column's time stamps list
            }
        }
    }

    void Update()
    {
        /* Won't stop until the spawn index is greater than the amount of timestamps of the track */
        if (spawn_i < timeStamps.Count)
        {
            /* Check wether notes have to be spawned by evaluating if song duration is greater or equal than the timestamp spawning time minus the time the note is on screen. The note time on screen represents the time before the player attacks enemies. */
            if (TrackManager.SourceTime() >= timeStamps[spawn_i] - TrackManager.trackManager.noteTimeOnScreen)
            {
                var enemy = Instantiate(enemyPrefab, transform); // Instantiate enemy prefab and its transform
                enemies.Add(enemy.GetComponent<Enemy>()); // Add note to the list of instantiated enemies
                enemy.GetComponent<Enemy>().attackTime = (float)timeStamps[spawn_i]; // Obtain enemy component and assign its corresponding time stamp 
                spawn_i++; // Obtain next enemy spawn index
            }
        }

        if (gameManager.paused == false) 
        {
            if (input_i < timeStamps.Count)
            {
                /* Will run while the amount of inputs is lower then the amount of timestamps */
                double timeStamp = timeStamps[input_i]; // Enemy time stamp
                double audioTime = TrackManager.SourceTime() - (TrackManager.trackManager.inputDelay / 1000.0); // Current song time that accounts for given delay in seconds

                if (Input.GetKeyDown(input))
                {
                    /* Check if the key press in the current song audio time does not exceed the given margins */
                    double absHitTime = Math.Abs(audioTime - timeStamp);
            
                    if (absHitTime < perfectMargin)
                    {
                        /* Hit within the perfect margin */
                        Debug.Log($"Perfect! (+{Math.Round(absHitTime, 2)})");
                        scoreManager.Perfect(absHitTime);
                        Destroy(enemies[input_i].gameObject);
                        var message = Instantiate(perfectPrefab, transform.position + offset, Quaternion.identity);
                        input_i++;
                    }
                    else if (absHitTime < goodMargin)
                    {
                        /* Hit within the good margin */
                        Debug.Log($"Good! (+{Math.Round(absHitTime, 2)})");
                        scoreManager.Good(absHitTime);
                        Destroy(enemies[input_i].gameObject);
                        var message = Instantiate(goodPrefab, transform.position + offset, Quaternion.identity);
                        input_i++;
                    }
                    else
                    {
                        /* Undesired key input lowers player health eitherways */
                        scoreManager.Miss();
                        scoreManager.totalMissCount++;
                    }
                }
                if (timeStamp + goodMargin  < audioTime)
                {
                    /* If the current time in the song is greater than the timestamp conbined with the margin of error, then the player has missed */
                    Debug.Log($"Missed!");
                    scoreManager.Miss();
                    var message = Instantiate(missPrefab, transform.position + offset, Quaternion.identity);
                    scoreManager.totalMissCount++;
                    input_i++;
                }
            }
        }
    }
}
