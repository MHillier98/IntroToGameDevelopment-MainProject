using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreUpdater : MonoBehaviour
{
    private TextMeshProUGUI scorerText; // the text object to update
    private PlayerController playerController; // the player script to get data from

    public string modeType = "Recreation_"; // the name of the mode to update
    private string highScorePrefName = "HighScore"; // the name of the player preference to update

    public bool isHighScorer = false; // if this scorer script is saving and updating the high score
    private int highScore = 0; // what the current high score is

    private void Start()
    {
        scorerText = GetComponent<TextMeshProUGUI>();
        playerController = GameObject.FindGameObjectWithTag("Ms Pac-Man").gameObject.GetComponent<PlayerController>(); // find the player object in the scene
        highScore = PlayerPrefs.GetInt(modeType + highScorePrefName, highScore);

        if (isHighScorer)
        {
            if (highScore <= 100)
            {
                PlayerPrefs.SetInt(modeType + highScorePrefName, 5000); // set a high-ish high score for a new player to try and beat
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
                PlayerPrefs.SetInt(modeType + highScorePrefName, highScore); // save the high score under the mode / preference name
                UpdateText(playerScore);
            }
        }
        else
        {
            UpdateText(playerScore);
        }
    }

    /* 
     * Update the scoring object with the score parameter
     */
    private void UpdateText(int score)
    {
        scorerText.text = score.ToString();
    }
}
