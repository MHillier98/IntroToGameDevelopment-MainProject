using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LivesCounterController : MonoBehaviour
{
    private TextMeshProUGUI livesText; // the text object to update
    private PlayerController playerController; // the player script to get data from

    public int playerLivesCurrent = 0; // the current life count
    public int playerLivesMax = 0; // the max/starting counting value

    private void Start()
    {
        livesText = GetComponent<TextMeshProUGUI>();
        playerController = GameObject.FindGameObjectWithTag("Ms Pac-Man").gameObject.GetComponent<PlayerController>();

        playerLivesCurrent = playerController.GetCurrentLives();
        playerLivesMax = playerController.GetMaxLives();

        UpdateLifeCounter(playerLivesCurrent, playerLivesMax);
    }

    private void Update()
    {
        playerLivesCurrent = playerController.GetCurrentLives();
        UpdateLifeCounter(playerLivesCurrent, playerLivesMax);
    }

    /*
     * Update the life counter text object
     */
    private void UpdateLifeCounter(int current, int max)
    {
        if (livesText != null)
        {
            livesText.text = current.ToString() + " / " + max.ToString(); // format the info as "current lives / max lives"
        }
    }
}
