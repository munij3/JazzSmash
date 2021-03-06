using Unity;
using UnityEngine;
using UnityEngine.Audio;
using System;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;

public class TrackManager : MonoBehaviour
{
    public static TrackManager trackManager; // track manager instance for public access
    public static MidiFile midiFile; // Location on memory where the midi file will load
    public AudioSource audioSource; // Selected level track object
    public GameManager gameManager;
    public GameObject apiTest;
    APITest api;
    public Columns[] columns; // Columns for each set of recorded midi keys
    public string midiName; // Streams midi file from the MidiFiles folder 
    public int inputDelay; // delay in milioseconds for keyboard inputs
    public double perfectMargin; // Maximum time margin in seconds for a perfect note
    public double goodMargin; // Maximum time margin in seconds for a good note
    public float songDelayInSeconds; // Song start delay
    public float currentSongTime; // Current audio source time when calling the SourceTime function
    public float audioSourceDuration; // Duration of the provided audio source
    public int noteCount;
    public float noteTimeOnScreen; // Note duration on screen in seconds
    public float noteSpawnPos; // Note spawn Y position
    public float noteAttackPos; // Note attack Y position
    public float noteDespawnPos 
    {
        get 
        {
            return noteAttackPos - (noteSpawnPos - noteAttackPos);
        }
    } // Note despawn Y position for the notes

    void Start()
    {
        trackManager = this;

        audioSourceDuration = audioSource.clip.length;

        gameManager = FindObjectOfType<GameManager>();

        api = apiTest.GetComponent<APITest>();

        midiFile = MidiFile.Read(Application.streamingAssetsPath  + "/" + midiName);

        /* Obtain notes and a count of notes from the midi file, then copy them to an array and set timestamps for each column */
        var notes = midiFile.GetNotes();
        var array = new Melanchall.DryWetMidi.Interaction.Note[notes.Count];
        notes.CopyTo(array, 0);

        foreach (var column in columns) column.SetTimeStamps(array); // Set time stams for each column
        foreach (var column in columns) noteCount += column.timeStamps.Count;
        
        Debug.Log("Audio clip name : " + audioSource.clip.ToString());
        Debug.Log("Audio clip length : " + (int)Math.Floor(audioSource.clip.length));
        Debug.Log($"Amount of notes: {noteCount}");

        api.AddMusicDataMethod(audioSource.clip.ToString(), (int)Math.Floor(audioSourceDuration), noteCount);

        Invoke(nameof(StartSong), songDelayInSeconds); // Invoke StartSong or audio source after a delay
    }
    public void StartSong()
    {
        audioSource.Play();
    }
    public static double SourceTime()
    {
        // Divides source samples by the frequency of the song to obtain the time
        return (double)trackManager.audioSource.timeSamples / trackManager.audioSource.clip.frequency;
    }
    void Update()
    {
        currentSongTime = (float)SourceTime();

        /* If the song duration has been elapsed, then the player has completed the level */
        if ((float)currentSongTime == audioSourceDuration)
        {
            gameManager.CompleteLevel();
        }
    }
}