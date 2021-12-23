using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Display Condition
 * Wessel Roelofse
 * 23/12/2021
 * 
 * This script only enables all child objects if a certain level is passed.
 */
public class DisplayCondition : MonoBehaviour
{
    public string ClearedLevel = "";
    void Start()
    {
        if(GameProgression.Instance.getCompleted(ClearedLevel))
        {
            for(int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }
        } else
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}
