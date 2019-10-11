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

    void Start()
    {
        scorerText = GetComponent<TextMeshProUGUI>();
        playerController = GameObject.FindGameObjectWithTag("Ms Pac-Man").gameObject.GetComponent<PlayerController>();

        highScore = PlayerPrefs.GetInt(modeType + highScorePrefName, highScore);

        if (isHighScorer)
        {
            scorerText.text = highScore.ToString();
        }
    }

    void Update()
    {
        int playerScore = playerController.GetScore();

        if (isHighScorer)
        {
            if (playerScore > highScore)
            {
                highScore = playerScore;
                PlayerPrefs.SetInt(modeType + highScorePrefName, highScore);
                scorerText.text = playerScore.ToString();
            }
        }
        else
        {
            scorerText.text = playerScore.ToString();
        }
    }
}
