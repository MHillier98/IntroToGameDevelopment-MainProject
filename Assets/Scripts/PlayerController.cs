using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;

    public enum PoweredStates { PoweredUp, PoweredDown }
    private PoweredStates powerState = PoweredStates.PoweredDown;

    private static int poweredUpTimeMax = 6;
    private int poweredUpTimeCurrent;
    
    public enum MovementDirections { Up, Down, Left, Right };
    public MovementDirections movementDirection = MovementDirections.Right;
    
    public Vector3 startingPosition = new Vector3(0f, -2f, 0f);
    public float movementSpeed = 6.0f;
    
    public AudioClip chompSound1 = null;
    public AudioClip chompSound2 = null;
    private bool playedChomp1 = false;

    private static int startingScore = 0;
    private int currentScore = 0;

    private static int dotScore = 10;
    private static int largeDotScore = 50;
    private static int ghostScore = 100;

    private int ghostsEatenCounter = 1;
    
    public GameObject dotParticleObject = null;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();

        transform.position = startingPosition;
        currentScore = startingScore;
        poweredUpTimeCurrent = poweredUpTimeMax;
        powerState = PoweredStates.PoweredDown;
        movementSpeed = 6.0f;
        movementDirection = MovementDirections.Right;
        playedChomp1 = false;
    }

    private void Update()
    {
        HandleMovementInput();
        AnimateSprite();

        if (CheckCanMove(movementDirection))
        {
            Move();
        }
    }

    private void HandleMovementInput()
    {
        if (Input.GetAxis("Horizontal") > 0)
        {
            if (CheckCanMove(MovementDirections.Right))
            {
                movementDirection = MovementDirections.Right;
            }
        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            if (CheckCanMove(MovementDirections.Left))
            {
                movementDirection = MovementDirections.Left;
            }
        }
        else if (Input.GetAxis("Vertical") > 0)
        {
            if (CheckCanMove(MovementDirections.Up))
            {
                movementDirection = MovementDirections.Up;
            }
        }
        else if (Input.GetAxis("Vertical") < 0)
        {
            if (CheckCanMove(MovementDirections.Down))
            {
                movementDirection = MovementDirections.Down;
            }
        }
    }

    private bool CheckCanMove(MovementDirections direction)
    {
        float rayOffsetX = 0.0f;
        float rayOffsetY = 0.0f;
        float rayOffset = 0.9f;
        Vector3 rayDir = Vector3.zero;
        float checkDistance = 1.1f;

        switch (direction)
        {
            case MovementDirections.Right:
                rayOffsetX = 0.0f;
                rayOffsetY = rayOffset;
                rayDir = Vector3.right;
                break;

            case MovementDirections.Left:
                rayOffsetX = 0.0f;
                rayOffsetY = rayOffset;
                rayDir = Vector3.left;
                break;

            case MovementDirections.Up:
                rayOffsetX = rayOffset;
                rayOffsetY = 0.0f;
                rayDir = Vector3.up;
                break;

            case MovementDirections.Down:
                rayOffsetX = rayOffset;
                rayOffsetY = 0.0f;
                rayDir = Vector3.down;
                break;

            default: // this should never happen as direction should always be set
                return false;
        }

        Vector2 vectorOffsetLeft = new Vector2(transform.position.x - rayOffsetX, transform.position.y - rayOffsetY);
        RaycastHit2D hitLeft = Physics2D.Raycast(vectorOffsetLeft, rayDir, checkDistance);
        //Debug.DrawRay(vectorOffsetLeft, rayDir * checkDistance, Color.red);

        if (hitLeft.collider != null && hitLeft.collider.tag != null && hitLeft.collider.tag == "Walls")
        {
            return false;
        }

        Vector2 vectorOffsetMiddle = new Vector2(transform.position.x, transform.position.y);
        RaycastHit2D hitMiddle = Physics2D.Raycast(vectorOffsetMiddle, rayDir, checkDistance);
        //Debug.DrawRay(vectorOffsetMiddle, rayDir * checkDistance, Color.green);

        if (hitMiddle.collider != null && hitMiddle.collider.tag != null && hitMiddle.collider.tag == "Walls")
        {
            return false;
        }

        Vector2 vectorOffsetRight = new Vector2(transform.position.x + rayOffsetX, transform.position.y + rayOffsetY);
        RaycastHit2D hitRight = Physics2D.Raycast(vectorOffsetRight, rayDir, checkDistance);
        //Debug.DrawRay(vectorOffsetRight, rayDir * checkDistance, Color.cyan);

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
        switch (movementDirection)
        {
            case MovementDirections.Right:
                spriteRenderer.flipY = false;
                transform.localEulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0.0f);
                break;

            case MovementDirections.Up:
                spriteRenderer.flipY = false;
                transform.localEulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 90.0f);
                break;

            case MovementDirections.Left:
                spriteRenderer.flipY = true;
                transform.localEulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 180.0f);
                break;

            case MovementDirections.Down:
                spriteRenderer.flipY = false;
                transform.localEulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 270.0f);
                break;

            default:
                return;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Dots"))
        {
            PlayEatSound();
            Destroy(collision.gameObject);
            AddScore(dotScore);
        }
        else if (collision.tag.Equals("Large Dots"))
        {
            if (dotParticleObject != null)
            {
                Instantiate(dotParticleObject, transform.position, transform.rotation);
            }

            PlayEatSound();
            Destroy(collision.gameObject);
            AddScore(largeDotScore);
            PowerUp();
            Invoke("PowerDown", poweredUpTimeCurrent);
        }
        else if (collision.tag.Equals("Ghosts"))
        {
            if (powerState.Equals(PoweredStates.PoweredUp))
            {
                Destroy(collision.gameObject);
                AddScore(ghostScore * ghostsEatenCounter);
                ghostsEatenCounter += 1;
            }
            else
            {
                transform.position = startingPosition;
            }
        }
    }

    public void PlayEatSound()
    {
        if (playedChomp1 && chompSound2 != null)
        {
            audioSource.PlayOneShot(chompSound2);
            playedChomp1 = false;
        }
        else if (!playedChomp1 && chompSound1 != null)
        {
            audioSource.PlayOneShot(chompSound1);
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
        powerState = PoweredStates.PoweredUp;
    }

    public void PowerDown()
    {
        powerState = PoweredStates.PoweredDown;
        ghostsEatenCounter = 1;
    }
}
