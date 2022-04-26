using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;
    public GameObject levelCompleteUI;
    public GameObject levelSelectMenu;
    public GameObject mainMenuUI;
    public bool paused;
    static ScoreManager scoreManager;
    static TrackManager trackManager;
    static bool levelStatus = false;

    void Start()
    {
        gameManager = this;
        scoreManager = FindObjectOfType<ScoreManager>();
        trackManager = FindObjectOfType<TrackManager>();
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
}
