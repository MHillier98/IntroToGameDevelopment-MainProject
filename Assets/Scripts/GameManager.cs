using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip startSound = null;

    public GhostController[] ghosts;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(startSound);
        ghosts = FindObjectsOfType<GhostController>();
    }

    public void ScatterAllGhosts()
    {
        for (int i = 0; i < ghosts.Length; i++)
        {
            ghosts[i].SendMessage("InvokeScatter");
        }
    }
}
