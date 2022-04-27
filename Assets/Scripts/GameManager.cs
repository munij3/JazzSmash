using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;
    public GameObject levelCompleteUI;
    public GameObject levelSelectMenu;
    public GameObject mainMenuUI;
    public GameObject PauseMenuUI;
    public Columns[] columns;
    public bool paused;
    static ScoreManager scoreManager;
    static TrackManager trackManager;
    static bool levelStatus = false;

    void Start()
    {
        gameManager = this;
        paused = false;
        scoreManager = FindObjectOfType<ScoreManager>();
        trackManager = FindObjectOfType<TrackManager>();
    }
    public void SetPause()
    {
        if (paused == false)
        {
            paused = true;
            PauseMenuUI.SetActive(true);
            Time.timeScale = 0f;
            // AudioListener.paused = true;
            foreach (var column in columns) column.enabled = false;
        }
        else
        {
            paused = false;
            PauseMenuUI.SetActive(false);
            Time.timeScale = 1f;
            // AudioListener.paused = false;
            foreach (var column in columns) column.enabled = true;
        }
    }
    public void LoadMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void LoadLevelSelect()
    {
        SceneManager.LoadScene(1);
    }
    public void RestartLevel()
    {
        Time.timeScale = 1f;
        paused = false;
        levelCompleteUI.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void LoadLevel1Scene()
    {
        Time.timeScale = 1f;
        paused = false;
        SceneManager.LoadScene(2);
    }
    public void FailedLevel()
    {
        Time.timeScale = 0f;
        paused = true;
        scoreManager.GetResults(levelStatus);
        trackManager.audioSource.Stop();
        levelCompleteUI.SetActive(true);
    }
    public void CompleteLevel()
    {
        paused = true;
        levelStatus = true;
        scoreManager.GetResults(levelStatus);
        levelCompleteUI.SetActive(true);
    }
    void Update() 
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameManager.SetPause();
        }
    }
}
