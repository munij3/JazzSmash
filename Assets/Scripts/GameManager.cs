using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;
    public GameObject levelCompleteUI;
    public GameObject levelFailedUI;
    public GameObject levelSelectMenu;
    public GameObject mainMenuUI;
    public bool paused;

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
        levelFailedUI.SetActive(false);  
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
        Time.timeScale = 0f;
        levelFailedUI.SetActive(true);
    }
    public void CompleteLevel()
    {
        paused = true;
        Time.timeScale = 0f;
        levelCompleteUI.SetActive(true);
    }
}
