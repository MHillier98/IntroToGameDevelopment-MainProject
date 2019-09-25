﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LargeDotParticleController : MonoBehaviour
{
    [SerializeField] private float destroyTimer = 3.0f;
    
    void Start()
    {
        Destroy(this, destroyTimer);
    }
}