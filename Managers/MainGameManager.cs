using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Main Game Manager
 * Wessel Roelofse
 * 23/12/2021
 * 
 * Used to manage the current state of a level and keep track of global variables
 */
public class MainGameManager : Singleton<MainGameManager>
{
    public Transform player;
    public FirstPersonController playerController;
    public MusicManager musicManager;
    public float countdown = 4f;
    public bool started = false;
    public PlayerStatus playerStatus;
    public float bossHP = 100f;
    public bool finished = false;
    public bool currentSceneIsMainmenu = false;
    public float levelPlaytime = 0f;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if(countdown > 0f)
        {
            countdown -= Time.deltaTime;
            if(countdown < 0)
            {
                levelPlaytime = 0f;
                countdown = 0f;
                started = true;
            }
        } else
        {
            levelPlaytime += Time.deltaTime;
        }
    }
}
