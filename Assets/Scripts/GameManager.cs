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
    public bool pausedGame;
    public static bool pause;
    static ScoreManager scoreManager;
    static TrackManager trackManager;
    static bool levelStatus = false;

    void Start()
    {
        gameManager = this;
        pausedGame = false;
        scoreManager = FindObjectOfType<ScoreManager>();
        trackManager = FindObjectOfType<TrackManager>();
    }
    public void SetPause()
    {
        if (pausedGame == false)
        {
            pausedGame = true;
            PauseMenuUI.SetActive(true);
            Time.timeScale = 0f;
            AudioListener.pause = true;
            foreach (var column in columns) column.enabled = false;
        }
        else
        {
            pausedGame = false;
            PauseMenuUI.SetActive(false);
            Time.timeScale = 1f;
            AudioListener.pause = false;
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
        pausedGame = false;
        levelCompleteUI.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void LoadLevel1Scene()
    {
        Time.timeScale = 1f;
        pausedGame = false;
        SceneManager.LoadScene(2);
    }
    public void FailedLevel()
    {
        Time.timeScale = 0f;
        pausedGame = true;
        scoreManager.GetResults(levelStatus);
        trackManager.audioSource.Stop();
        levelCompleteUI.SetActive(true);
    }
    public void CompleteLevel()
    {
        pausedGame = true;
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
