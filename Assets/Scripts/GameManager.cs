using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;
    public float restartDelay = 1f; // Level restart delay
    public GameObject levelCompleteUI;
    public GameObject levelFailedUI;
    public ScoreManager scoreManager;
    bool gameEnded = false;
    public void LoadMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void LoadLevelScene()
    {
        SceneManager.LoadScene("LevelScene");
    }

    public void CompleteLevel()
    {
        if (gameEnded == false)
        {
            gameEnded = true;
            levelCompleteUI.SetActive(true);
        }
    }
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void FailedLevel()
    {
        levelFailedUI.SetActive(true);
    }
}
