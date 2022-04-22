using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public Columns columns;
    public static ScoreManager scoreManager;
    // public AudioSource goodEffect;
    // public AudioSource perfectEffect;
    public AudioSource missEffect;
    public TMPro.TextMeshProUGUI scoreText;
    public TMPro.TextMeshProUGUI comboText;
    public TMPro.TextMeshProUGUI healthText;
    public TMPro.TextMeshProUGUI accuracyText;
    public TMPro.TextMeshProUGUI finalScoreText;
    public TMPro.TextMeshProUGUI notesHitText;
    static int combo;
    static int score;
    static double accuracy;
    static int perfectHitCount;
    static int goodHitCount;
    static double totalErrorMargin;
    static int playerHealth;

    private void Awake()
    {
        scoreText = GetComponent<TMPro.TextMeshProUGUI>();
        comboText = GetComponent<TMPro.TextMeshProUGUI>();
        healthText = GetComponent<TMPro.TextMeshProUGUI>();
        accuracyText = GetComponent<TMPro.TextMeshProUGUI>();
        finalScoreText = GetComponent<TMPro.TextMeshProUGUI>();
        notesHitText = GetComponent<TMPro.TextMeshProUGUI>();
    }
    void Start()
    {
        scoreManager = this;
        combo = 1;
        score = 0;
        accuracy = 0;
        perfectHitCount = 0;
        goodHitCount = 0;
        totalErrorMargin = 0;
        playerHealth = 10;
        columns = GetComponent<Columns>();
    }

    public static void Good(double margin)
    {
        playerHealth++;
        perfectHitCount = 0;
        goodHitCount++;
        if(goodHitCount == 16)
        {
            combo++;
        }
        score += 10 * combo;
        totalErrorMargin += margin;
        // scoreManager.goodEffect.Play();
    }
    public static void Perfect(double margin)
    {
        playerHealth++;
        perfectHitCount++;
        if(perfectHitCount == 8)
        {
            combo++;
        }
        score += 20 * combo;
        totalErrorMargin += margin;
        // scoreManager.perfectEffect.Play();
    }
    public static void Miss()
    {
        playerHealth -= 2;
        combo = 1;
        goodHitCount = 0;
        perfectHitCount = 0;
        // scoreManager.missEffect.Play();
    }
    
    private void Update()
    {
        scoreText.text = $"Score: {score.ToString()}";
        comboText.text = $"Combo: x{combo.ToString()}";
        healthText.text = $"Health: {playerHealth.ToString()}";

        // CAMBIAR AL FINAL DEL NIVEL
        finalScoreText.text = $"Total score: {score}";
        accuracyText.text = $"Overal accuracy: {CalculateAccuracy(columns.timeStamps.Count).ToString}%";
        notesHitText.text = $"Notes hit: {columns.amountOfNotesHit}/{columns.timeStamps.Count}";
    }
    public double CalculateAccuracy(int timeStampCount)
    {
        accuracy = (timeStampCount * TrackManager.trackManager.goodMargin) - totalErrorMargin;
    }
    // private IEnumerator ScorePulsation(int givenScore)
    // {
    //     /* Co-routine for score pulsation when updated */

    //     for (float i = 1f; i < 1.2f; i += 0.05f)
    //     {
    //         scoreText.rectTransform.localScale = new Vector3(i, i, i);
    //         yield return new WaitForEndOfFrame();
    //     }
    //     scoreText.rectTransform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
    //     score = givenScore;

    //     for (float i = 1.2f; i >= 1f; i -= 0.05f)
    //     {
    //         scoreText.rectTransform.localScale = new Vector3(i, i, i);
    //         yield return new WaitForEndOfFrame();
    //     }
    //     scoreText.rectTransform.localScale = new Vector3(1f, 1f, 1f);
    // }
}