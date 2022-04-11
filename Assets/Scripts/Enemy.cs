using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    double instanceTime; // Time when the note is instantiated
    public float attackTime; // Time when the note must be attacked
    void Start(){
        instanceTime = TrackManager.sourceTime();
    }

    // Update is called once per frame
    void Update(){
        // Calculated elapsed time for note placement
        double elapsedTimeFromInstance = TrackManager.sourceTime() - instanceTime;
        // Divide elapsed time with the time between both spawn and attack times
        float t = (float)(elapsedTimeFromInstance / (TrackManager.trackManager.note_t * 2));

        if(t > 1){
            Destroy(gameObject);
        }
        else{
            // Generate transform for the note with a Lerp vector using the note spawn position, note despawn position, and 't' which represents the time between the note spawn position and the note despawn position (any value between 0 and 1)
            transform.localPosition = Vector3.Lerp(Vector3.up * TrackManager.trackManager.note_yspawn, Vector3.up * TrackManager.trackManager.note_ydespawn, t);
            GetComponent<SpriteRenderer>().enabled = true;
        }
    }
}