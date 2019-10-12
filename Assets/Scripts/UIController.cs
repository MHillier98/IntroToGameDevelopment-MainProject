using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    private string menuSceneName = "Main Menu";

    public void ExitToMenu()
    {
        SceneManager.LoadScene(menuSceneName, LoadSceneMode.Single);
    }
}
