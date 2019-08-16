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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // this is all one big gameobject (the tilemap)
        // todo: remake this.

        // collision.gameObject.active = false;
        // Debug.Log("");
        
    }
}
