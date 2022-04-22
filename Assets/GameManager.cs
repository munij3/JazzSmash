using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    bool checkLevelEnd = false; // Check wether the level has ended
    public float restartDelay = 1f; // Level restart delay
    public GameObject levelCompleteUI;
    public void CompleteLevel()
    {
        levelCompleteUI.SetActive(true);
    }
    public void EndGame()
    {
        if (checkLevelEnd == false)
        {
            checkLevelEnd = true;
            Debug.Log("Game over.");
            Invoke("Restart", restartDelay);
        }
    }
}
