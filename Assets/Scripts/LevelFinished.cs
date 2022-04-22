using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelFinished : MonoBehaviour
{
    public void LoadMenu()
    {
        // NEEDS SCENE NAME
        SceneManager.LoadScene("");
    }
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
