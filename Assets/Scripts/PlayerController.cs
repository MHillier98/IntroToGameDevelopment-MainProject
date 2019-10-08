using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;

    private static int startingScore = 0;
    private int currentScore = 0;

    private static int dotScore = 10;
    private static int largeDotScore = 50;
    private static int ghostScore = 100;

    private int ghostsEatenCounter = 1;

    private bool poweredUp = false;
    private static int poweredUpTimeMax = 6;
    private int poweredUpTimeCurrent;

    public float movementSpeed = 6.0f;
    private string movementDirection = "Right";

    private bool playedChomp1 = false;
    public AudioClip chomp1 = null;
    public AudioClip chomp2 = null;

    public GameObject particleObject = null;

    private bool canMove = true;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();

        currentScore = startingScore;
        poweredUp = false;
        poweredUpTimeCurrent = poweredUpTimeMax;
        movementDirection = "Right";
        playedChomp1 = false;
    }

    private void Update()
    {
        HandleMovementInput();

        if (canMove)
        {
            Move();
        }

        AnimateSprite();
    }

    private void FixedUpdate()
    {
        canMove = CheckCanMove(movementDirection);
    }

    private void HandleMovementInput()
    {
        if (Input.GetAxis("Horizontal") > 0)
        {
            if (CheckCanMove("Right"))
            {
                movementDirection = "Right";
            }
        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            if (CheckCanMove("Left"))
            {
                movementDirection = "Left";
            }
        }
        else if (Input.GetAxis("Vertical") > 0)
        {
            if (CheckCanMove("Up"))
            {
                movementDirection = "Up";
            }
        }
        else if (Input.GetAxis("Vertical") < 0)
        {
            if (CheckCanMove("Down"))
            {
                movementDirection = "Down";
            }
        }
    }

    private bool CheckCanMove(string direction)
    {
        float rayOffset = 0.9f;
        float rayOffsetX = 0.0f;
        float rayOffsetY = 0.0f;
        float checkDistance = 1.1f;
        Vector3 rayDir = Vector3.zero;

        switch (direction)
        {
            case "Right":
                rayOffsetX = 0.0f;
                rayOffsetY = rayOffset;
                rayDir = Vector3.right;
                break;

            case "Left":
                rayOffsetX = 0.0f;
                rayOffsetY = rayOffset;
                rayDir = Vector3.left;
                break;

            case "Up":
                rayOffsetX = rayOffset;
                rayOffsetY = 0.0f;
                rayDir = Vector3.up;
                break;

            case "Down":
                rayOffsetX = rayOffset;
                rayOffsetY = 0.0f;
                rayDir = Vector3.down;
                break;

            default: // this should never happen as direction should always be set
                return false;
        }

        Vector2 vectorOffsetLeft = new Vector2(transform.position.x - rayOffsetX, transform.position.y - rayOffsetY);
        //Debug.DrawRay(vectorOffsetLeft, rayDir * checkDistance, Color.red);
        RaycastHit2D hitLeft = Physics2D.Raycast(vectorOffsetLeft, rayDir, checkDistance);

        Vector2 vectorOffsetMiddle = new Vector2(transform.position.x, transform.position.y);
        //Debug.DrawRay(vectorOffsetMiddle, rayDir * checkDistance, Color.green);
        RaycastHit2D hitMiddle = Physics2D.Raycast(vectorOffsetMiddle, rayDir, checkDistance);

        Vector2 vectorOffsetRight = new Vector2(transform.position.x + rayOffsetX, transform.position.y + rayOffsetY);
        //Debug.DrawRay(vectorOffsetRight, rayDir * checkDistance, Color.cyan);
        RaycastHit2D hitRight = Physics2D.Raycast(vectorOffsetRight, rayDir, checkDistance);


        if (hitLeft.collider != null && hitLeft.collider.tag != null && hitLeft.collider.tag == "Walls")
        {
            return false;
        }

        if (hitMiddle.collider != null && hitMiddle.collider.tag != null && hitMiddle.collider.tag == "Walls")
        {
            return false;
        }

        if (hitRight.collider != null && hitRight.collider.tag != null && hitRight.collider.tag == "Walls")
        {
            return false;
        }

        return true;
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
            transform.localEulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0.0f);
        }
        else if (movementDirection.Equals("Up"))
        {
            spriteRenderer.flipY = false;
            transform.localEulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 90.0f);
        }
        else if (movementDirection.Equals("Left"))
        {
            spriteRenderer.flipY = true;
            transform.localEulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 180.0f);
        }
        else if (movementDirection.Equals("Down"))
        {
            spriteRenderer.flipY = false;
            transform.localEulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 270.0f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Debug.Log(collision.tag);
        // could set the dots to not be active, if i want to reactivate them for new levels

        if (collision.tag.Equals("Dots"))
        {
            PlayEatSound();
            Destroy(collision.gameObject);
            AddScore(dotScore);
        }
        else if (collision.tag.Equals("Large Dots"))
        {
            if (particleObject != null)
            {
                Instantiate(particleObject, transform.position, transform.rotation);
            }

            PlayEatSound();
            Destroy(collision.gameObject);
            AddScore(largeDotScore);
            PowerUp();
            Invoke("PowerDown", poweredUpTimeCurrent);
        }
        else if (collision.tag.Equals("Ghosts"))
        {
            if (poweredUp)
            {
                Destroy(collision.gameObject);
                AddScore(ghostScore * ghostsEatenCounter);
                ghostsEatenCounter += 1;
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
    }

    public void PlayEatSound()
    {
        if (playedChomp1 && chomp2 != null)
        {
            audioSource.PlayOneShot(chomp2);
            playedChomp1 = false;
        }
        else if (!playedChomp1 && chomp1 != null)
        {
            audioSource.PlayOneShot(chomp1);
            playedChomp1 = true;
        }
    }

    public void SetScore(int newScore)
    {
        currentScore = newScore;
    }

    public void AddScore(int addScore)
    {
        currentScore += addScore;
    }

    public void IncrementScore()
    {
        currentScore += 1;
    }

    public int GetScore()
    {
        return currentScore;
    }

    public void PowerUp()
    {
        poweredUp = true;
    }

    public void PowerDown()
    {
        poweredUp = false;
        ghostsEatenCounter = 1;
    }
}
