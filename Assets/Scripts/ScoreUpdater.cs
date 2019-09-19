using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreUpdater : MonoBehaviour
{
    private TextMeshProUGUI scorerText;
    private PlayerController playerController;

    [SerializeField] private bool isHighScorer = false;
    [SerializeField] private int highScore = 0;

    void Start()
    {
        scorerText = GetComponent<TextMeshProUGUI>();
        playerController = GameObject.FindGameObjectWithTag("Ms Pac-Man").gameObject.GetComponent<PlayerController>();

        highScore = PlayerPrefs.GetInt("High Score", highScore);

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
                PlayerPrefs.SetInt("High Score", highScore);
                scorerText.text = playerScore.ToString();
            }
        }
        else
        {
            scorerText.text = playerScore.ToString();
        }
    }
}
