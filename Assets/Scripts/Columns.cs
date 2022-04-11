using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Plugin framework to parse the midi file
using Melanchall.DryWetMidi.Interaction;

public class Columns : MonoBehaviour
{
    public Melanchall.DryWetMidi.MusicTheory.NoteName noteRestriction; // Restricts parts to their corresponding key press
    public KeyCode input; // Input for any given lane
    public GameObject enemyPrefab; // Prefabs to spawn 
    List<Enemy> parts = new List<Enemy>(); // Keeps track of spawned parts
    public List<double> timeStamps = new List<double>(); // Timestamps of the song when the player needs to input keys for attack

    // Indexes that keep track of which timespamps from the list need to be spawned, and which input needs to be pressed
    int spawn_i = 0;
    int input_i = 0;

    void Start(){}

    public void SetTimeStamps(Melanchall.DryWetMidi.Interaction.Note[] array){
        // This function iterates every note in the notes array generated with the midi file in the track manager, and obtains and converts note times to seconds.
        foreach(var note in array){
            if(note.NoteName == noteRestriction){
                // Convert note time to seconds by feeding the tempo map from the track manager
                var metricTimeStamps = TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, TrackManager.midiFile.GetTempoMap());
                // Add metric times to the time stamps list by converting and adding minutes, seconds, and miliseconds
                timeStamps.Add((double)metricTimeStamps.Minutes * 60f + metricTimeStamps.Seconds + (double)metricTimeStamps.Milliseconds / 1000f);
            }
        }
    }

    void Update(){
        // Check if note spawn index (amount of parts spawned) isn't greater than the amount of timestamps of the track
        if(spawn_i < timeStamps.Count){
            // Obtain time in which note must be spawned by substracting the given time stamp with the note duration on screen
            if(TrackManager.sourceTime() >= timeStamps[spawn_i] - TrackManager.trackManager.note_t){
                // Instantiate note prefab and its transform
                var note = Instantiate(enemyPrefab, transform);
                // Add note to the list of instantiated parts
                parts.Add(note.GetComponent<Enemy>());
                // Obtain not component and assign its corresponding time stamp 
                note.GetComponent<Enemy>().attackTime = (float)timeStamps[spawn_i];
                spawn_i++; // Obtain next note spawn index
            }
        }

        if(input_i < timeStamps.Count){
            double timeStamp = timeStamps[input_i]; // Enemy time stamp
            double perfectMargin = TrackManager.trackManager.perfectMargin; // Perfect margin
            double goodMargin = TrackManager.trackManager.goodMargin; // Good margin
            double audioTime = TrackManager.sourceTime() - (TrackManager.trackManager.inputDelay / 1000.0);

            if(Input.GetKeyDown(input)){
                // Check for hits within margins of error
                double absoluteTime = Math.Abs(audioTime - timeStamp);
                if(absoluteTime <= perfectMargin){
                    Perfect(absoluteTime);
                    print("Perfect!");
                    Destroy(parts[spawn_i].gameObject);
                    input_i++;
                }else if(absoluteTime > perfectMargin && absoluteTime <= goodMargin){
                    Good(absoluteTime);
                    print("Good!");
                    Destroy(parts[spawn_i].gameObject);
                    input_i++;
                }
            }
            if(audioTime >= (timeStamp + perfectMargin + goodMargin)){
                // If the audio source time is greater than the margin the player has missed
                Miss();
                print($"Miss!");
                input_i++;
            }
        }       
    }
    private void Good(double margin){
        ScoreManager.Good(margin);
    }
    private void Perfect(double margin){
        ScoreManager.Perfect(margin);
    }
    private void Miss(){
        ScoreManager.Miss();
    }
}
