using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public Vector3 teleportToPoint;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ms Pac-Man")
        {
            if (teleportToPoint != null)
            {
                collision.gameObject.transform.position = teleportToPoint;
            }
        }
    }
}
