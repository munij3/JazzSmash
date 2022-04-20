using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.Networking;

// Plugin frameworks to parse the midi file
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;

public class TrackManager : MonoBehaviour
{
    public static TrackManager trackManager; // track manager instance for public access
    public static MidiFile midiFile; // Location on memory where the midi file will load
    public AudioSource audioSource;
    public Columns[] columns;
    public string midiName; // Streams midi file from the MidiFiles folder 
    public int inputDelay; // delay in milioseconds for keyboard inputs
    public double perfectMargin; // Maximum time margin in seconds for a perfect note
    public double goodMargin; // Maximum time margin in seconds for a good note
    public float songDelayInSeconds;
    public float noteTimeOnScreen; // Note duration on screen in seconds
    public float noteSpawnPos; // Note spawn Y position 
    public float noteAttackPos; // Note attack Y position 
    public float noteDespawnPos 
    {
        get 
        {
            return noteAttackPos - (noteSpawnPos - noteAttackPos);
        }
    } // Note despawn Y position for the notes ()

    void Start()
    {
        trackManager = this;
        midiFile = MidiFile.Read(Application.streamingAssetsPath  + "/" + midiName);
        GetMidiNotes(); // Use the midi file data after it has been loaded
    }

    public void GetMidiNotes()
    {
        /* Obtain notes and a count of notes from the midi file, then copy them to an array and set timestamps for each column */
        var notes = midiFile.GetNotes();
        var array = new Melanchall.DryWetMidi.Interaction.Note[notes.Count];
        notes.CopyTo(array, 0);

        // Set time stams for each column
        foreach (var column in columns) column.SetTimeStamps(array);

        // Invoke StartSong or audio source after a delay
        Invoke(nameof(StartSong), songDelayInSeconds);
    }
    public void StartSong()
    {
        audioSource.Play();
    }
    public static double sourceTime()
    {
        // Divides source samples by the frequency of the song to obtain the time
        return (double)trackManager.audioSource.timeSamples / trackManager.audioSource.clip.frequency;
    }
    void Update(){}
}