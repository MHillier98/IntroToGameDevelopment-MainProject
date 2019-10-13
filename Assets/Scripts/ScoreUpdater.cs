using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreUpdater : MonoBehaviour
{
    private TextMeshProUGUI scorerText;
    private PlayerController playerController;

    public string modeType = "Recreation_";
    private string highScorePrefName = "HighScore";

    public bool isHighScorer = false;
    private int highScore = 0;

    private void Start()
    {
        scorerText = GetComponent<TextMeshProUGUI>();
        playerController = GameObject.FindGameObjectWithTag("Ms Pac-Man").gameObject.GetComponent<PlayerController>();
        highScore = PlayerPrefs.GetInt(modeType + highScorePrefName, highScore);

        if (isHighScorer)
        {
            if (highScore <= 100)
            {
                PlayerPrefs.SetInt(modeType + highScorePrefName, 5000);
            }
            UpdateText(highScore);
        }
    }

    private void Update()
    {
        int playerScore = playerController.GetScore();

        if (isHighScorer)
        {
            if (playerScore > highScore)
            {
                highScore = playerScore;
                PlayerPrefs.SetInt(modeType + highScorePrefName, highScore);
                UpdateText(playerScore);
            }
        }
        else
        {
            UpdateText(playerScore);
        }
    }

    private void UpdateText(int score)
    {
        scorerText.text = score.ToString();
    }
}
