using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LivesCounterController : MonoBehaviour
{
    private TextMeshProUGUI livesText;
    private PlayerController playerController;

    public int playerLivesCurrent = 0;
    public int playerLivesMax = 0;

    void Start()
    {
        livesText = GetComponent<TextMeshProUGUI>();
        playerController = GameObject.FindGameObjectWithTag("Ms Pac-Man").gameObject.GetComponent<PlayerController>();

        playerLivesCurrent = playerController.GetCurrentLives();
        playerLivesMax = playerController.GetMaxLives();

        UpdateLifeCounter(playerLivesCurrent, playerLivesMax);
    }

    void Update()
    {
        playerLivesCurrent = playerController.GetCurrentLives();
        UpdateLifeCounter(playerLivesCurrent, playerLivesMax);
    }

    private void UpdateLifeCounter(int current, int max)
    {
        if (livesText != null)
        {
            livesText.text = current.ToString() + " / " + max.ToString();
        }
    }
}
