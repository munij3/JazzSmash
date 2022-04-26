using UnityEngine;

public class Enemy : MonoBehaviour
{
    double instanceTime; // Time when the note is instantiated
    public float attackTime; // Time when the note must be attacked
    
    void Start()
    {
        instanceTime = TrackManager.SourceTime();
    }

    // Update is called once per frame
    void Update()
    {
        // Calculated elapsed time for note placement
        double elapsedTimeFromInstance = TrackManager.SourceTime() - instanceTime;
        // Divide elapsed time with the time between both spawn and attack times
        float t = (float)(elapsedTimeFromInstance / (TrackManager.trackManager.noteTimeOnScreen * 2));

        // Since t will take values from 0 to 1, therefore enemies must be destroyed when t is greater than 1.
        if(t > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            /* Generate transform for the note with a Lerp vector using the note spawn position, note despawn position, and 't' which represents the time between the note spawn position and the note despawn position */
            transform.localPosition = Vector3.Lerp(Vector3.up * TrackManager.trackManager.noteSpawnPos, Vector3.up * TrackManager.trackManager.noteDespawnPos, t);
        }
        // Generate sprite after position has been set
        GetComponent<SpriteRenderer>().enabled = true;
    }
}