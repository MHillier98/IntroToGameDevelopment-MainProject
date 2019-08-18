using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int score = 0;

    void Start()
    {
        score = 0;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.eulerAngles = new Vector3(0, 0, 90);
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.eulerAngles = new Vector3(0, 0, 180);
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            transform.eulerAngles = new Vector3(0, 0, 270);
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Debug.Log(collision.name);

        // I can set the dots to not be active, if i want to reactivate them for new levels
        if (collision.tag.Equals("Dots"))
        {
            Destroy(collision.gameObject);
            // collision.gameObject.SetActive(false);
            AddScore(10);
        }
        else if (collision.tag.Equals("Large Dots"))
        {
            Destroy(collision.gameObject);
            // collision.gameObject.SetActive(false);
            AddScore(50);
        }
    }
}
