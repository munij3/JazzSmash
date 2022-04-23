using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;
    public GameObject levelCompleteUI;
    public GameObject levelFailedUI;
    public GameObject levelSelectMenu;
    public GameObject mainMenuUI;
    public ScoreManager scoreManager;

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
    public void LoadLevelSelect()
    {
        SceneManager.LoadScene(1);
    }
    public void RestartLevel()
    {
        levelCompleteUI.SetActive(false);
        levelFailedUI.SetActive(false);  
        SceneManager.LoadScene(2);
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void LoadLevel1Scene()
    {
        SceneManager.LoadScene(2);
    }
    public void FailedLevel()
    {
        Time.timeScale = 0f;
        levelFailedUI.SetActive(true);
    }
    public void CompleteLevel()
    {
        Time.timeScale = 0f;
        levelCompleteUI.SetActive(true);
    }
}
