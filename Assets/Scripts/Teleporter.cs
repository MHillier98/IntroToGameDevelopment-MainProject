using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField] private Vector3 teleportToPoint;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ms Pac-Man")
        {
            collision.gameObject.transform.position = teleportToPoint;
        }
    }
}
