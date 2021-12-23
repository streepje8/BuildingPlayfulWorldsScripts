using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Player Status
 * Wessel Roelofse
 * 23/12/2021
 * 
 * A Script to keep track of all the player's stats
 */
public class PlayerStatus : MonoBehaviour
{
    public float HP = 100f;
    public float regenSpeed = 1f;
    public bool isAlive = true;

    void Update()
    {
        if(HP <= 0f)
        {
            isAlive = false;
        }
        if(HP < 100f)
        {
            HP += regenSpeed * Time.deltaTime;
        }
    }

    public void hit()
    {
        HP -= 10f;
    }
}
