using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private static SpriteRenderer spriteRenderer;
    private static AudioSource audioSource;

    [SerializeField] private static int startingScore = 0;
    [SerializeField] private int currentScore = 0;

    [SerializeField] private static int dotScore = 10; // maybe move this to be on the dot itself, and get from it?
    [SerializeField] private static int largeDotScore = 50;

    [SerializeField] private bool poweredUp = false;
    [SerializeField] private static int poweredUpTimeMax = 6;
    [SerializeField] private int poweredUpTimeCurrent;

    [SerializeField] private static float movementSpeed = 0.1f;
    [SerializeField] private string movementDirection = "Right";

    [SerializeField] private bool playedChomp1 = false;
    [SerializeField] public static AudioClip chomp1 = null;
    [SerializeField] public static AudioClip chomp2 = null;

    [SerializeField] public static GameObject particleObject = null;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();

        currentScore = startingScore;
        poweredUp = false;
        poweredUpTimeCurrent = poweredUpTimeMax;
        movementSpeed = 0.1f;
        movementDirection = "Right";
        playedChomp1 = false;
    }

    void Update()
    {
        HandleMovementInput();

        if (CheckCanMoveMoving(movementDirection))
        {
            Move();
        }

        AnimateSprite();
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
        Vector3 rayDir = Vector3.zero;
        switch (direction)
        {
            case "Right":
                rayDir = Vector3.right;
                break;
            case "Left":
                rayDir = Vector3.left;
                break;
            case "Up":
                rayDir = Vector3.up;
                break;
            case "Down":
                rayDir = Vector3.down;
                break;
            default: break;
        }

        float checkDistance = 1.1f;
        float rayOffset = 0.87f;

        RaycastHit2D hit = Physics2D.Raycast(transform.position + transform.up * rayOffset, rayDir, checkDistance);
        Debug.DrawRay(transform.position + transform.up * rayOffset, rayDir * checkDistance, Color.red);

        if (hit.collider != null && hit.collider.tag != null && hit.collider.tag == "Walls")
        {
            return false;
        }

        RaycastHit2D hit2 = Physics2D.Raycast(transform.position + transform.up * -rayOffset, rayDir, checkDistance);
        Debug.DrawRay(transform.position + transform.up * -rayOffset, rayDir * checkDistance, Color.yellow);

        if (hit2.collider != null && hit2.collider.tag != null && hit2.collider.tag == "Walls")
        {
            return false;
        }

        return true;
    }

    private bool CheckCanMoveMoving(string direction)
    {
        Vector3 rayDir = Vector3.zero;
        switch (direction)
        {
            case "Right":
                rayDir = Vector3.right;
                break;
            case "Left":
                rayDir = Vector3.left;
                break;
            case "Up":
                rayDir = Vector3.up;
                break;
            case "Down":
                rayDir = Vector3.down;
                break;
            default: break;
        }

        float checkDistance = 1.1f;
        float rayOffset = 0.87f;

        RaycastHit2D hit = Physics2D.Raycast(transform.position + transform.up * rayOffset, rayDir, checkDistance);
        Debug.DrawRay(transform.position + transform.up * rayOffset, rayDir * checkDistance, Color.green);

        if (hit.collider != null && hit.collider.tag != null && hit.collider.tag == "Walls")
        {
            return false;
        }

        RaycastHit2D hit2 = Physics2D.Raycast(transform.position + transform.up * -rayOffset, rayDir, checkDistance);
        Debug.DrawRay(transform.position + transform.up * -rayOffset, rayDir * checkDistance, Color.magenta);

        if (hit2.collider != null && hit2.collider.tag != null && hit2.collider.tag == "Walls")
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
            AddScore(dotScore);
        }
        else if (collision.tag.Equals("Large Dots"))
        {
            PlayEatSound();
            Destroy(collision.gameObject);
            Instantiate(particleObject, transform.position, transform.rotation);
            AddScore(largeDotScore);
            PowerUp();
            Invoke("PowerDown", poweredUpTimeCurrent);
        }
        else if (collision.tag.Equals("Ghosts"))
        {
            if (poweredUp)
            {

            }
            else
            {
                Destroy(this);
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
    }
}
