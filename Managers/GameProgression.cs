using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Game Progression
 * Wessel Roelofse
 * 23/12/2021
 * 
 * A object that is set to not destroy to keep track of the player their progression
 */
public class GameProgression : Singleton<GameProgression>
{
    public float PlayTime = 0f;
    private Dictionary<string, bool> levelComplete = new Dictionary<string, bool>();
    private Dictionary<string, bool> levelUnlocked = new Dictionary<string, bool>();

    public void setCompleted(string level)
    {
        if(levelComplete.ContainsKey(level))
        {
            levelComplete.Remove(level);
        }
        levelComplete.Add(level, true);
    }

    public void setCompleted(string level, bool state)
    {
        if (levelComplete.ContainsKey(level))
        {
            levelComplete.Remove(level);
        }
        levelComplete.Add(level, state);
    }

    public bool getCompleted(string level)
    {
        if(!levelComplete.ContainsKey(level))
        {
            levelComplete.Add(level, false);
        }
        return levelComplete[level];
    }

    public void Unlock(string level)
    {
        if (!levelUnlocked.ContainsKey(level))
        {
            levelUnlocked.Remove(level);
        }
        levelUnlocked.Add(level, true);
    }

    public bool getUnlocked(string level)
    {
        if (!levelUnlocked.ContainsKey(level))
        {
            return false;
        }
        return levelUnlocked[level];
    }

    private void Awake()
    {
        if (Instance == null) 
        {    
            DontDestroyOnLoad(gameObject);
            Instance = this;
            levelUnlocked.Add("VillaVolta", true);
        }
    }

    private void Update()
    {
        PlayTime += Time.deltaTime;
    }
}
