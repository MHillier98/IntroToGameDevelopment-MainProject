using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LargeDotParticleController : MonoBehaviour
{
    private float destroyTimer = 3.0f; // how long this gameobject will exist for

    /*
     * Destroy this gameobject a few seconds after it is instantiated
     */
    private void Start()
    {
        Destroy(this, destroyTimer);
    }
}
