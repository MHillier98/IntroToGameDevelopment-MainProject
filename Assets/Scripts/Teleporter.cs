using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField] private static Vector3 teleportToPoint = new Vector3(0, 0, 0);

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ms Pac-Man")
        {
            collision.gameObject.transform.position = teleportToPoint;
        }
    }
}
