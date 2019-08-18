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


    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        score = startingScore;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            spriteRenderer.flipY = false;
            transform.eulerAngles = new Vector3(0, 0, 90);
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            spriteRenderer.flipY = true;
            transform.eulerAngles = new Vector3(0, 0, 180);
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            spriteRenderer.flipY = false;
            transform.eulerAngles = new Vector3(0, 0, 270);
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            spriteRenderer.flipY = false;
            transform.eulerAngles = new Vector3(0, 0, 0);
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
