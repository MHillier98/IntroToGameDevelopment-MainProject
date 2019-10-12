using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    private string gameSceneName = "Game Scene";
    private string iterationSceneName = "Iteration Scene";

    public void StartGame()
    {
        SceneManager.LoadScene(gameSceneName, LoadSceneMode.Single);
    }

    public void StartDesignIteration()
    {
        SceneManager.LoadScene(iterationSceneName, LoadSceneMode.Single);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
