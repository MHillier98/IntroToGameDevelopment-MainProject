using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public Vector3 teleportToPoint; // the point where to teleport to

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // only teleport the player and ghosts
        if (collision.tag.Equals("Ms Pac-Man") || collision.tag.Equals("Ghosts"))
        {
            if (teleportToPoint != null)
            {
                collision.gameObject.transform.position = teleportToPoint;
            }
        }
    }
}
