using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/*
 * Countdown Text
 * Wessel Roelofse
 * 23/12/2021
 * 
 * This script manages the countdown text and ready prompt
 */
[RequireComponent(typeof(TMP_Text))]
public class CountdownText : MonoBehaviour
{
    private TMP_Text text;
    private bool started = false;
    private bool ready = false;


    void Start()
    {
        text = GetComponent<TMP_Text>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            ready = true;
        }
        if(!ready)
        {
            MainGameManager.Instance.countdown = 4;
            text.text = "Press shift when ready!";
        }
        else { 
            int c = roundUpwards(MainGameManager.Instance.countdown);
            if(c > 3)
            {
                text.text = "3";
            } else if(c > 2)
            {
                text.text = "2";
            } else if (c > 1)
            {
                text.text = "1";
            } else if (c > 0)
            {
                text.text = "GO!";
            }
            if(c == 0 && !started)
            {
                text.text = "";
                MainGameManager.Instance.musicManager.startMusic();
                started = true;
            }
        }
    }

    private int roundUpwards(float countdown) //This function always rounds floats upwards, would make it an extension if i had more time.
    {
        int rounded = Mathf.RoundToInt(countdown);
        if(rounded < countdown)
        {
            return rounded + 1;
        }
        return rounded;
    }
}
