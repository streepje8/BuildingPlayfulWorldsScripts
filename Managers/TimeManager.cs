using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Time Manager
 * Wessel Roelofse
 * 23/12/2021
 * 
 * This script manages the timescale, it was supposed to get more functionality added, but due to lack of time it only holds one variable 0-o
 */
public class TimeManager : Singleton<TimeManager>
{
    public float timeScale = 1f;

    private void Awake()
    {
        Instance = this;
    }
}
