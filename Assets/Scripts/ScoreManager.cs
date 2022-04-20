using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager scoreManager;
    // public AudioSource goodEffect;
    // public AudioSource perfectEffect;
    public AudioSource missEffect;
    static double combo;
    static double score;
    static double accuracy;
    static double totalMargin;

    void Start(){
        scoreManager = this;
        combo = 0;
        score = 0;
        accuracy = 0;
    }

    public static void Good(double margin){
        combo += 1;
        score += 10 * combo;
        // scoreManager.goodEffect.Play();
        totalMargin += margin;
    }
    public static void Perfect(double margin){
        combo += 1.5;
        score += 20 * combo;
        // scoreManager.perfectEffect.Play();
        totalMargin += margin;
    }
    public static void Miss(){
        combo = 0;
        scoreManager.missEffect.Play(); 
    }

    public static void Accuracy(){
        // accuracy =
    }
    
    private void Update(){
        // Set score updating text or
        // scoreText.text = combo.ToString();
    }
}