using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupBase : MonoBehaviour
{
    public GameObject[] powerups; // a list of powerups to spawn
    private GameObject lastPowerup; // the last powerup spawn, used to make sure we don't spawn multiple powerups on the same spawn point

    private void SpawnPowerUp()
    {
        // make sure powerups have been set and that there isn't a powerup previously spawned at this position
        if (powerups.Length > 0 && lastPowerup == null)
        {
            int randNum = Random.Range(0, powerups.Length); // choose a random powerup
            lastPowerup = Instantiate(powerups[randNum], transform.position, Quaternion.identity);
        }
    }
}
