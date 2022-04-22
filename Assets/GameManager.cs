using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    bool checkLevelEnd = false; // Check wether the level has ended
    public float restartDelay = 1f; // Level restart delay
    public GameObject levelCompleteUI;
    public GameObject levelFailedUI;
    public ScoreManager scoreManager;

    public void CompleteLevel()
    {
        if (checkLevelEnd == false)
        {
            checkLevelEnd = true;
            levelCompleteUI.SetActive(true);
        }
    }
    public void FailedLevel()
    {
        if (checkLevelEnd == false)
        {
            checkLevelEnd = true;
            levelFailedUI.SetActive(true);
        }
    }
}
