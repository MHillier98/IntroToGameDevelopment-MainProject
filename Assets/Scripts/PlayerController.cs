using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;

    [SerializeField] private int startingScore = 0;
    [SerializeField] private int currentScore = 0;

    [SerializeField] private int dotScore = 10; // maybe move this to be on the dot itself, and get from it?
    [SerializeField] private int largeDotScore = 50;

    [SerializeField] private bool poweredUp = false;
    [SerializeField] private int poweredUpTimeMax = 6;
    [SerializeField] private int poweredUpTimeCurrent;

    [SerializeField] private bool canMove = true;
    [SerializeField] private float movementSpeed = 0.1f;
    [SerializeField] private string movementDirection = "Right";

    [SerializeField] private AudioClip chomp1;
    [SerializeField] private AudioClip chomp2;
    [SerializeField] private bool playedChomp1 = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();

        //audioSource.clip = Resources.Load<AudioClip>("Audio/Chomp");

        currentScore = startingScore;
        poweredUpTimeCurrent = poweredUpTimeMax;
    }

    void Update()
    {
        HandleMovementInput();
        if (CheckCanMove(movementDirection))
        {
            Move();
        }
        AnimateSprite();
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

        Debug.Log(movementDirection);
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

        RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDir, 1f);
        Debug.DrawRay(transform.position, rayDir, Color.red);

        if (hit.collider != null && hit.collider.tag != null && hit.collider.tag == "Walls")
        {
            return false;
        }

        return true;
    }

    void FixedUpdate()
    {
        //Debug.DrawRay(transform.position, Vector3.up, Color.red);
        //Debug.DrawRay(transform.position, Vector3.down, Color.red);
        //Debug.DrawRay(transform.position, Vector3.left, Color.red);
        //Debug.DrawRay(transform.position, Vector3.right, Color.red);

        //Vector3 hitDirection1 = transform.TransformDirection(new Vector3(1f, 1f));
        //Vector3 hitDirection2 = transform.TransformDirection(new Vector3(1f, -1f));

        //RaycastHit2D hit1 = Physics2D.Raycast(transform.position, hitDirection1);
        //RaycastHit2D hit2 = Physics2D.Raycast(transform.position, hitDirection2);

        //Debug.DrawRay(transform.position, hitDirection1, Color.red);
        //Debug.DrawRay(transform.position, hitDirection2, Color.black);

        //if (hit1.collider != null && hit1.collider.tag != null && hit1.collider.tag == "Walls" && hit2.collider != null && hit2.collider.tag != null && hit2.collider.tag == "Walls")
        //{
        //    Debug.Log(hit1.collider.tag);
        //    Debug.Log(hit2.collider.tag);
        //    canMove = false;
        //}
        //else
        //{
        //    canMove = true;
        //}
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
