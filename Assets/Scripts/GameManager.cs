using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using TMPro;

public class GameManager : MonoBehaviour
{
    private AudioSource audioSource; // the AudioSource component to play audio from
    public AudioClip startSound = null; // the AudioClip to play

    public GhostController[] ghosts; // an array of ghosts in the scene
    public PlayerController player; // the player script object

    public GameObject dots; // the dots in the scene
    public GameObject largeDots; // the large dots in the scene

    public GameObject dotPrefab = null; // the dots prefab
    public GameObject largeDotPrefab = null; // the large dots prefab

    public TextMeshProUGUI beginText; // the text object to show at the start of the scene
    public TextMeshProUGUI endText; // the text object to show at the end of the scene

    public TilemapRenderer tilemapRenderer; // the tilemap object to show/hide

    public PowerupBase[] powerupBases; // the powerup base spawners
    public float timer = 0f; // the timer for spawning powerups

    public bool isMainMenu = false; // if this manager is in the main menu or not

    private void Awake()
    {
        Time.timeScale = 1f; // make sure the timescale is correct
        timer = Time.time;
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(startSound); // play the starting sound

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
            if (powerupBases.Length > 0 && timer + 12f <= Time.time) // if the timer has run for long enough
            {
                int randNum = Random.Range(0, powerupBases.Length);
                powerupBases[randNum].SendMessage("SpawnPowerUp"); // tell the selected powerup base to spawn a powerup
                timer = Time.time;
            }

            if (dots != null && largeDots != null)
            {
                if (dots.transform.childCount == 0 && largeDots.transform.childCount == 0)
                {
                    if (dotPrefab != null && largeDotPrefab != null)
                    {
                        // here we want to destroy the current dots objects and make new ones, as well as reset the positions of the player and ghosts

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

    /*
     * Make the ghosts run away to their respective corners
     */
    public void ScatterAllGhosts()
    {
        for (int i = 0; i < ghosts.Length; i++)
        {
            ghosts[i].SendMessage("InvokeScatter");
        }
    }

    /*
     * Pause the game for 3 seconds, then hide the beginText object
     */
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

    /*
     * Show the end of game text object
     */
    public void EndGame()
    {
        endText.gameObject.SetActive(true);
    }

    /*
     * Hide the walls renderer
     */
    public void HideWalls()
    {
        if (tilemapRenderer != null)
        {
            tilemapRenderer.enabled = false;
            StartCoroutine(RevealWalls(4.0f));
        }
    }

    /*
     * Show the walls after they have been hidden
     */
    private IEnumerator RevealWalls(float time)
    {
        float pauseEndTime = Time.realtimeSinceStartup + time;
        while (Time.realtimeSinceStartup < pauseEndTime)
        {
            yield return 0;
        }
        ShowWalls();
    }

    /*
     * Show the walls renderer
     */
    public void ShowWalls()
    {
        if (tilemapRenderer != null)
        {
            tilemapRenderer.enabled = true;
        }
    }
}
