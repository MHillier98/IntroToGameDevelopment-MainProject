using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;

    public int startingScore = 0;
    private int score = 0;

    public int dotScore = 10; // maybe move this to be on the dot itself, and get from it?
    public int largeDotScore = 50;

    public bool poweredUp = false;
    public int poweredUpTimeMax = 6;
    public int poweredUpTimeCurrent;

    public bool isMoving = true;
    public float movementSpeed = 0.1f;
    public string movementDirection = "Right";

    public AudioClip chomp1;
    public AudioClip chomp2;
    private bool playedChomp1 = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();

        //audioSource.clip = Resources.Load<AudioClip>("Audio/Chomp");

        score = startingScore;
        poweredUpTimeCurrent = poweredUpTimeMax;
    }

    void Update()
    {
        HandleMovementInput();
        AnimateSprite();
        if (isMoving)
        {
            Move();
        }
    }

    private void HandleMovementInput()
    {
        if (Input.GetAxis("Horizontal") > 0)
        {
            movementDirection = "Right";
        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            movementDirection = "Left";
        }
        else if (Input.GetAxis("Vertical") > 0)
        {
            movementDirection = "Up";
        }
        else if (Input.GetAxis("Vertical") < 0)
        {
            movementDirection = "Down";
        }
    }

    private void Move()
    {
        transform.Translate(new Vector3(movementSpeed, 0f, 0f) * Time.deltaTime, Space.Self);
    }

    private void AnimateSprite()
    {
        if (movementDirection.Equals("Right"))
        {
            spriteRenderer.flipY = false;
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (movementDirection.Equals("Left"))
        {
            spriteRenderer.flipY = true;
            transform.eulerAngles = new Vector3(0, 0, 180);
        }
        else if (movementDirection.Equals("Up"))
        {
            spriteRenderer.flipY = false;
            transform.eulerAngles = new Vector3(0, 0, 90);
        }
        else if (movementDirection.Equals("Down"))
        {
            spriteRenderer.flipY = false;
            transform.eulerAngles = new Vector3(0, 0, 270);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // i could set the dots to not be active, if i want to reactivate them for new levels
        if (collision.tag.Equals("Dots"))
        {
            PlayEatSound();

            Destroy(collision.gameObject);
            // collision.gameObject.SetActive(false);
            AddScore(dotScore);
        }
        else if (collision.tag.Equals("Large Dots"))
        {
            Destroy(collision.gameObject);
            // collision.gameObject.SetActive(false);
            AddScore(largeDotScore);
            PowerUp();
            Invoke("PowerDown", poweredUpTimeCurrent);
        }
        else if (collision.tag.Equals("Ghosts"))
        {
            Destroy(this);
        }
    }

    public void PlayEatSound()
    {
        if (playedChomp1)
        {
            audioSource.PlayOneShot(chomp2);
            playedChomp1 = false;
        }
        else
        {
            audioSource.PlayOneShot(chomp1);
            playedChomp1 = true;
        }
    }

    public void SetScore(int newScore)
    {
        score = newScore;
    }

    public void AddScore(int addScore)
    {
        score += addScore;
    }

    public void IncrementScore()
    {
        score += 1;
    }

    public int GetScore()
    {
        return score;
    }

    public void PowerUp()
    {
        poweredUp = true;
    }

    public void PowerDown()
    {
        poweredUp = false;
    }
}
