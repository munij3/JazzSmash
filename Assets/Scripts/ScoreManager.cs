using System;
using System.Collections;
using UnityEngine;
using TMPro;
using Melanchall.DryWetMidi.Interaction;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager scoreManager; // score manager instance for public access
    // public AudioSource missEffect;
    // public AudioSource goodEffect;
    // public AudioSource perfectEffect;
    public TMP_Text scoreText;
    public TMP_Text comboText;
    public TMP_Text healthText;
    public TMP_Text accuracyText;
    public TMP_Text finalScoreText;
    public TMP_Text notesHitText;
    public TMP_Text statusText;
    public int totalHitCount; // Keeps track of the number of good or perfect hits
    public int totalMissCount; // Keeps track of the number of good or perfect hits
    public int perfectHitCount;
    public int goodHitCount;
    static double totalErrorMargin; // Keeps track of the sum of the error margins for each enemy hit
    static double accuracy;
    static int combo;
    static int score;
    static int perfectTracker;
    static int goodTracker;
    static int playerHealth;
    static int timeStampCount;

    void Start()
    {
        scoreManager = this;
        enabled = true;

        combo = 1;
        score = 0;
        totalHitCount = 0;
        totalMissCount = 0;
        accuracy = 0;
        perfectTracker = 0;
        goodTracker = 0;
        totalErrorMargin = 0;
        playerHealth = 10;

        scoreText.text = $"Score: {score}";
        comboText.text = $"Score: {combo}";
        healthText.text = $"Score: {playerHealth}";
    }

    public void Good(double margin)
    {
        totalHitCount++;
        goodHitCount++;
        totalErrorMargin += margin;
        perfectTracker = 0;
        goodTracker++;
        if(playerHealth < 20)
        {
            playerHealth++;
        }
        StartCoroutine(HealthPulsation(playerHealth));
        if(goodTracker == 16)
        {
            combo++;
        }
        StartCoroutine(ComboPulsation(combo));
        score += 10 * combo;
        StartCoroutine(ScorePulsation(score));
        // scoreManager.goodEffect.Play();
    }
    public void Perfect(double margin)
    {
        totalHitCount++;
        perfectHitCount++;
        totalErrorMargin += margin;
        perfectTracker++;
        if(playerHealth < 20)
        {
            playerHealth++;
            StartCoroutine(HealthPulsation(playerHealth));
        }
        if(perfectTracker == 8)
        {
            combo++;
        }
        StartCoroutine(ComboPulsation(combo));
        score += 20 * combo;
        StartCoroutine(ScorePulsation(score));
        // scoreManager.perfectEffect.Play();
    }
    public void Miss()
    {
        //totalErrorMargin += TrackManager.trackManager.goodMargin;
        perfectTracker = 0;
        goodTracker = 0;
        playerHealth -= 2;
        StartCoroutine(HealthPulsation(playerHealth));
        if (score != 0)
        {
            combo = 1;
            StartCoroutine(ComboPulsation(combo));
            score -= 15;
            StartCoroutine(ScorePulsation(score));
        }
        // scoreManager.missEffect.Play();
    }
    void Update()
    {
        scoreText.text = $"Score: {score}";
        comboText.text = $"Combo: x{combo}";
        healthText.text = $"Health: {playerHealth}";
        
        /* Checks wether the players health has been depleted in order to display a failed level splash screen, and display obtained scores */
        if (playerHealth <= 0)
        {
            FindObjectOfType<GameManager>().FailedLevel();
            enabled = false;
        }
    }
    public void GetResults(bool levelStatus)
    {
        timeStampCount = TrackManager.midiFile.GetNotes().Count;

        if (levelStatus == true)
        {
            statusText.text = "Complete!";
        }
        else
        {
            statusText.text = "You failed!";
        }
        if (totalHitCount == 0)
        {
            accuracy = 0;
        }
        else
        {
            accuracy = Math.Round(CalculateAccuracy(), 2);
        }
        /* Calculate parameters regardless of level status set in GameManager */
        finalScoreText.text = $"Total score: {score}";
        accuracyText.text = $"Overal accuracy: {accuracy}%";
        notesHitText.text = $"Notes hit: {totalHitCount}/{timeStampCount}";
    }
    public double CalculateAccuracy()
    {
        double acceptedPerfectAccuracy = totalHitCount * FindObjectOfType<TrackManager>().perfectMargin; // Amount of possible perfect margins(
        double acceptedGoodAccuracy = totalHitCount * FindObjectOfType<TrackManager>().goodMargin; // Amount of possible good margins
        double marginRange = acceptedGoodAccuracy - acceptedPerfectAccuracy; // Margin range between accepted margins
        /* If the player accuracy ends up being less than the perfect margin, then an accuracy of 100% will be returned. Else, the player accuracy is calculated with a rule of three. */
        Debug.Log($"{totalErrorMargin}");
        if (totalErrorMargin < perfectHitCount)
        {
            return 100;
        }
        else
        {
            return (totalErrorMargin * 100) / marginRange;
        }
    }

    /* TEXT COROUTINES */
    private static IEnumerator ScorePulsation(int givenScore)
    {
        /* Co-routine for score pulsation when updated */

        for (float i = 1f; i < 1.2f; i += 0.05f)
        {
            scoreManager.scoreText.rectTransform.localScale = new Vector3(i, i, i);
            yield return new WaitForEndOfFrame();
        }
        scoreManager.scoreText.rectTransform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
        score = givenScore;

        for (float i = 1.2f; i >= 1f; i -= 0.05f)
        {
            scoreManager.scoreText.rectTransform.localScale = new Vector3(i, i, i);
            yield return new WaitForEndOfFrame();
        }
        scoreManager.scoreText.rectTransform.localScale = new Vector3(1f, 1f, 1f);
    }
    private static IEnumerator ComboPulsation(int givenCombo)
    {
        /* Co-routine for combo pulsation when updated */

        for (float i = 1f; i < 1.2f; i += 0.05f)
        {
            scoreManager.comboText.rectTransform.localScale = new Vector3(i, i, i);
            yield return new WaitForEndOfFrame();
        }
        scoreManager.comboText.rectTransform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
        accuracy = givenCombo;

        for (float i = 1.2f; i >= 1f; i -= 0.05f)
        {
            scoreManager.comboText.rectTransform.localScale = new Vector3(i, i, i);
            yield return new WaitForEndOfFrame();
        }
        scoreManager.comboText.rectTransform.localScale = new Vector3(1f, 1f, 1f);
    }
    private static IEnumerator HealthPulsation(int givenHealth)
    {
        /* Co-routine for health pulsation when updated */

        for (float i = 1f; i < 1.2f; i += 0.05f)
        {
            scoreManager.healthText.rectTransform.localScale = new Vector3(i, i, i);
            yield return new WaitForEndOfFrame();
        }
        scoreManager.healthText.rectTransform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
        playerHealth = givenHealth;

        for (float i = 1.2f; i >= 1f; i -= 0.05f)
        {
            scoreManager.healthText.rectTransform.localScale = new Vector3(i, i, i);
            yield return new WaitForEndOfFrame();
        }
        scoreManager.healthText.rectTransform.localScale = new Vector3(1f, 1f, 1f);
    }
}