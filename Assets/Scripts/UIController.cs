using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    private string menuSceneName = "Main Menu"; // the scene to load

    /*
     * Load the main menu scene (unloading the current scene)
     */
    public void ExitToMenu()
    {
        SceneManager.LoadScene(menuSceneName, LoadSceneMode.Single);
    }
}
