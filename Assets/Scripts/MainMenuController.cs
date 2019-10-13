using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    private string gameSceneName = "Game Scene"; // the recreation scene's name
    private string iterationSceneName = "Iteration Scene"; // the design iteration scene's name

    /*
     * Load the Recreation scene
     */
    public void StartGame()
    {
        SceneManager.LoadScene(gameSceneName, LoadSceneMode.Single);
    }

    /*
     * Load the Design Iteration scene
     */
    public void StartDesignIteration()
    {
        SceneManager.LoadScene(iterationSceneName, LoadSceneMode.Single);
    }

    /*
     * Quit the game
     */
    public void QuitGame()
    {
        Application.Quit();
    }
}
