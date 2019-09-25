﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public string gameSceneName = "Game Scene";

    public void StartGame()
    {
        SceneManager.LoadScene(gameSceneName, LoadSceneMode.Single);
    }
}
