using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int startingScore = 0;
    private int score = 0;

    public int dotScore = 10;
    public int largeDotScore = 50;

    public bool poweredUp = false;
    public int poweredUpTime = 6;

    SpriteRenderer spriteRenderer;

    public string movementDirection = "Right";

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        score = startingScore;
    }

    void Update()
    {
        HandleMovementInput();
        Movement();
        //AnimateSprite(); // the rotations mess up the movement code
        // maybe i can rotate the sprite? need to figure out the collisions
    }

    private void HandleMovementInput()
    {
        if (Input.GetAxis("Horizontal") > 0)
        {
            Debug.Log("R");
            movementDirection = "Right";
        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            Debug.Log("L");
            movementDirection = "Left";
        }
        else if (Input.GetAxis("Vertical") > 0)
        {
            Debug.Log("U");
            movementDirection = "Up";
        }
        else if (Input.GetAxis("Vertical") < 0)
        {
            Debug.Log("D");
            movementDirection = "Down";
        }
    }

    private void Movement()
    {
        if (movementDirection.Equals("Right"))
        {
            transform.Translate(0.08f, 0f, 0f);
        }
        else if (movementDirection.Equals("Left"))
        {
            transform.Translate(-0.08f, 0f, 0f);
        }
        else if (movementDirection.Equals("Up"))
        {
            transform.Translate(0f, 0.08f, 0f);
        }
        else if (movementDirection.Equals("Down"))
        {
            transform.Translate(0f, -0.08f, 0f);
        }
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
            Invoke("PowerDown", poweredUpTime);
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
