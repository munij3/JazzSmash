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
    public AudioSource audioSource;
    public Columns[] columns; 
    public float songDelayInSeconds;
    public double perfectMargin; // Maximum time margin in seconds for a perfect note
    public double goodMargin; // Maximum time margin in seconds for a good note
    public int inputDelay; // delay in milioseconds for keyboard inputs
    public string fileLocation; // Streams midi file from the MidiFiles folder 
    public float note_t; // Note duration on screen
    public float note_yspawn; // Note y spawn position 
    public float note_yattack; // Note y attack position 
    public static MidiFile midiFile; // Location on memory where the midi file will load
    public float note_ydespawn {
        get {
            return note_yattack - (note_yspawn - note_yattack);
        }
    }

    void Start(){
        trackManager = this;
        // Specify midi file location 
        midiFile = MidiFiles.Read(Application.MidiFile + "/" + fileLocation);
        // Use midi data after it has been loaded
        GetDataFromMidi();
    }

    public void GetDataFromMidi(){
        // Obtain notes and a count of notes from the midi file
        var notes = midiFile.GetNotes();
        var array = new Melanchall.DryWetMidi.Interaction.Note[notes.Count];
        // Copy midi notes to empty array
        notes.CopyTo(array, 0);

        // Set time stams for each column
        foreach (var column in columns) column.SetTimeStamps(array);

        // Invoke StartSong or audio source after a delay
        Invoke(nameof(StartSong), songDelayInSeconds);
    }
    public void StartSong(){
        audioSource.Play();
    }
    public static double sourceTime(){
        // Divides source samples by the frequency of the song to obtain the time
        return (double)trackManager.audioSource.timeSamples / trackManager.audioSource.clip.frequency;
    }

    void Update(){}
}