using UnityEngine;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;

public class TrackManager : MonoBehaviour
{
    public static TrackManager trackManager; // track manager instance for public access
    public static MidiFile midiFile; // Location on memory where the midi file will load
    public AudioSource audioSource; // Selected level track
    public Columns[] columns; // Columns for each set of recorded midi keys
    public string midiName; // Streams midi file from the MidiFiles folder 
    public int inputDelay; // delay in milioseconds for keyboard inputs
    public double perfectMargin; // Maximum time margin in seconds for a perfect note
    public double goodMargin; // Maximum time margin in seconds for a good note
    public float songDelayInSeconds; // Song start delay
    public float currentSongTime; // Current audio source time when calling the SourceTime function
    public float audioSourceDuration; // Duration of the provided audio source
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

        midiFile = MidiFile.Read(Application.streamingAssetsPath  + "/" + midiName);

        audioSourceDuration = audioSource.clip.length;

        /* Obtain notes and a count of notes from the midi file, then copy them to an array and set timestamps for each column */
        var notes = midiFile.GetNotes();
        var array = new Melanchall.DryWetMidi.Interaction.Note[notes.Count];
        notes.CopyTo(array, 0);

        // Debug.Log("Audio clip length : " + audioSource.clip.length);

        foreach (var column in columns) column.SetTimeStamps(array); // Set time stams for each column

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
            FindObjectOfType<GameManager>().CompleteLevel();
        }
    }
}