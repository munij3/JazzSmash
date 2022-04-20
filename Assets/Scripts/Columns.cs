using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Plugin framework to parse the midi file
using Melanchall.DryWetMidi.Interaction;

public class Columns : MonoBehaviour
{
    public Melanchall.DryWetMidi.MusicTheory.NoteName noteRestriction; // Restricts notes to their corresponding key press
    public KeyCode input; // Input for any given lane
    public GameObject enemyPrefab; // Prefabs to spawn 
    public GameObject goodPrefab; // Feedback message prefab instance
    public GameObject perfectPrefab; // Feedback message prefab instance
    public GameObject missPrefab; // Feedback message prefab instance
    List<Enemy> notes = new List<Enemy>(); // Keeps track of spawned notes
    public List<double> timeStamps = new List<double>(); // Timestamps of the song when the player needs to input keys for attack

    public double timeStamp;
    public double audioTime;
    public double sourceTime;

    /* Indexes that keep track of which timespamps from the list need to be spawned, and which input needs to be pressed */
    int spawn_i = 0;
    int input_i = 0;

    Vector3 offset = new Vector3(0, -6.5f, 0);

    void Start(){}

    public void SetTimeStamps(Melanchall.DryWetMidi.Interaction.Note[] array)
    {
        /* This function iterates every note in the notes array generated with the midi file in the track manager, and obtains and converts note times to seconds. */
        foreach(var note in array)
        {
            if(note.NoteName == noteRestriction)
            {
                var metricTimeSpan = TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, TrackManager.midiFile.GetTempoMap()); // Convert note time to seconds by feeding the tempo map from the track manager
                timeStamps.Add((double)metricTimeSpan.Minutes * 60f + metricTimeSpan.Seconds + (double)metricTimeSpan.Milliseconds / 1000f); // Add metric times to the time stamps list by converting and adding minutes, seconds, and miliseconds
            }
        }
    }

    void Update()
    {
        sourceTime = TrackManager.sourceTime();
        /* Check if note spawn index (amount of notes spawned) isn't greater than the amount of timestamps of the track */
        if(spawn_i < timeStamps.Count)
        {
            // Obtain time in which note must be spawned by substracting the given time stamp with the note duration on screen
            if(TrackManager.sourceTime() >= timeStamps[spawn_i] - TrackManager.trackManager.noteTimeOnScreen)
            {
                var note = Instantiate(enemyPrefab, transform); // Instantiate note prefab and its transform
                notes.Add(note.GetComponent<Enemy>()); // Add note to the list of instantiated notes
                note.GetComponent<Enemy>().attackTime = (float)timeStamps[spawn_i]; // Obtain note component and assign its corresponding time stamp 
                spawn_i++; // Obtain next note spawn index
            }
        }

        if(input_i < timeStamps.Count)
        {
            timeStamp = timeStamps[input_i]; // Enemy time stamp
            double perfectMargin = TrackManager.trackManager.perfectMargin; // Perfect margin
            double goodMargin = TrackManager.trackManager.goodMargin; // Good margin
            audioTime = TrackManager.sourceTime() - (TrackManager.trackManager.inputDelay / 1000.0); // Song duration

            if(Input.GetKeyDown(input))
            {
                /* Check if the key press in the current song audio time does not exceed the given margins */
                double absHitTime = Math.Abs(audioTime - timeStamp);
                print(absHitTime);
                if(absHitTime < perfectMargin)
                {
                    Perfect(absHitTime - timeStamp);
                    print($"Perfect! hit {input_i}");
                    var message = Instantiate(perfectPrefab, transform.position + offset, Quaternion.identity);
                    Destroy(notes[input_i].gameObject);
                    input_i++;
                }
                else if(absHitTime < goodMargin)
                {
                    Good(absHitTime - timeStamp);
                    print($"Good! hit {input_i}");
                    var message = Instantiate(goodPrefab, transform.position + offset, Quaternion.identity);
                    Destroy(notes[input_i].gameObject);
                    input_i++;
                }
                else
                {
                    Miss();
                }
            }
            if(timeStamp + goodMargin <= audioTime)
            {
                /* If the audio source time is greater than the margin, then the player has missed */
                Miss();
                print($"Missed {input_i} note");
                var message = Instantiate(missPrefab, transform.position + offset, Quaternion.identity);
                input_i++;
            }
        }       
    }
    private void Good(double margin)
    {
        ScoreManager.Good(margin);
    }
    private void Perfect(double margin)
    {
        ScoreManager.Perfect(margin);
    }
    private void Miss()
    {
        ScoreManager.Miss();
    }
}
