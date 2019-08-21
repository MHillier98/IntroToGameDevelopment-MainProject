using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LargeDotController : MonoBehaviour
{
    public GameObject particleObject;

    private void OnDestroy()
    {
        Instantiate(particleObject, transform.position, transform.rotation);
    }
}
