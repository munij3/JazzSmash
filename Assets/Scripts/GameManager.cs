using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;
    public GameObject levelCompleteUI;
    public GameObject levelSelectMenu;
    public GameObject mainMenuUI;
    public bool paused;
    public bool levelStatus = false;

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
        paused = false;
        levelCompleteUI.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void LoadLevel1Scene()
    {
        paused = false;
        SceneManager.LoadScene(2);
    }
    public void FailedLevel()
    {
        paused = true;
        FindObjectOfType<ScoreManager>().GetResults(levelStatus);
        FindObjectOfType<TrackManager>().audioSource.Stop();
        FindObjectOfType<Columns>().enabled = false;
        FindObjectOfType<ScoreManager>().enabled = false;
        levelCompleteUI.SetActive(true);
    }
    public void CompleteLevel()
    {
        paused = true;
        levelStatus = true;
        FindObjectOfType<ScoreManager>().GetResults(levelStatus);
        FindObjectOfType<Columns>().enabled = false;
        FindObjectOfType<ScoreManager>().enabled = false;
        levelCompleteUI.SetActive(true);
    }
}
