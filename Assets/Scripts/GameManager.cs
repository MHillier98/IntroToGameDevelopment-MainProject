using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using TMPro;

public class GameManager : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip startSound = null;

    public GhostController[] ghosts;
    public PlayerController player;

    public GameObject dots;
    public GameObject largeDots;

    public GameObject dotPrefab = null;
    public GameObject largeDotPrefab = null;

    public TextMeshProUGUI beginText;
    public TextMeshProUGUI endText;

    public TilemapRenderer tilemapRenderer;

    public PowerupBase[] powerupBases;
    public float timer = 0f;

    public bool isMainMenu = false;

    private void Awake()
    {
        Time.timeScale = 1f;
        timer = Time.time;
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(startSound);

        if (!isMainMenu)
        {
            ghosts = FindObjectsOfType<GhostController>();
            powerupBases = FindObjectsOfType<PowerupBase>();

            StartCoroutine(StartGame());
        }
    }

    private void Update()
    {
        if (!isMainMenu)
        {
            if (powerupBases.Length > 0 && timer + 15f <= Time.time)
            {
                int randNum = Random.Range(0, powerupBases.Length);
                powerupBases[randNum].SendMessage("SpawnPowerUp");
                timer = Time.time;
            }

            if (dots != null && largeDots != null)
            {
                if (dots.transform.childCount == 0 && largeDots.transform.childCount == 0)
                {
                    if (dotPrefab != null && largeDotPrefab != null)
                    {
                        Destroy(dots);
                        Destroy(largeDots);

                        dots = Instantiate(dotPrefab, transform.position, transform.rotation);
                        largeDots = Instantiate(largeDotPrefab, transform.position, transform.rotation);

                        player.SendMessage("ResetPosition");

                        for (int i = 0; i < ghosts.Length; i++)
                        {
                            ghosts[i].SendMessage("ResetPosition");
                        }
                    }
                }
            }
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

    private IEnumerator RevealWalls(float time)
    {
        float pauseEndTime = Time.realtimeSinceStartup + time;
        while (Time.realtimeSinceStartup < pauseEndTime)
        {
            yield return 0;
        }
        ShowWalls();
    }

    public void HideWalls()
    {
        if (tilemapRenderer != null)
        {
            tilemapRenderer.enabled = false;
            StartCoroutine(RevealWalls(4.0f));
        }
    }

    public void ShowWalls()
    {
        if (tilemapRenderer != null)
        {
            tilemapRenderer.enabled = true;
        }
    }
}
