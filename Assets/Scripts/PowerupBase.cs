using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupBase : MonoBehaviour
{
    public GameObject[] powerups;
    private GameObject lastPowerup;

    private void SpawnPowerUp()
    {
        if (powerups.Length > 0 && lastPowerup == null)
        {
            int randNum = Random.Range(0, powerups.Length);
            lastPowerup = Instantiate(powerups[randNum], transform.position, Quaternion.identity);
        }
    }
}
