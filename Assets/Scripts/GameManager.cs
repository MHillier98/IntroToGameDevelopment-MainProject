using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip startSound = null;

    public GhostController[] ghosts;

    public TextMeshProUGUI beginText;
    public TextMeshProUGUI endText;

    public bool isMainMenu = false;

    private void Awake()
    {
        Time.timeScale = 1f;
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(startSound);

        if (!isMainMenu)
        {
            ghosts = FindObjectsOfType<GhostController>();

            StartCoroutine(StartGame());
        }
    }

    public void ScatterAllGhosts()
    {
        for (int i = 0; i < ghosts.Length; i++)
        {
            ghosts[i].SendMessage("InvokeScatter");
        }
    }

    private IEnumerator StartGame()
    {
        Time.timeScale = 0f;
        float pauseEndTime = Time.realtimeSinceStartup + 3.0f;
        while (Time.realtimeSinceStartup < pauseEndTime)
        {
            yield return 0;
        }
        Time.timeScale = 1f;

        beginText.gameObject.SetActive(false);
    }

    public void EndGame()
    {
        endText.gameObject.SetActive(true);
    }
}
