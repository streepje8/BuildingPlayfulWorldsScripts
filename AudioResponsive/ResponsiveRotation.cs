using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/*
 * Responsive rotation
 * Wessel Roelofse
 * 23/12/2021
 * 
 * Rotate an object based on music activity
 */
public class ResponsiveRotation : MonoBehaviour
{
    public Vector3 axis = Vector3.up;
    public ReactionType reactsAt;
    public bool doLerp = false;
    public float lerpSpeed = 25f;

    private float musicIntensity = 0f;

    void Update()
    {
        float newmusicIntensity = 0f;
        switch (reactsAt)
        {
            case ReactionType.MainActivity:
                newmusicIntensity = MainGameManager.Instance.musicManager.activity;
                break;
            case ReactionType.Bass:
                newmusicIntensity = MainGameManager.Instance.musicManager.bass;
                break;
            case ReactionType.Voices:
                newmusicIntensity = MainGameManager.Instance.musicManager.bass;
                break;
        }
        if (doLerp)
        {
            musicIntensity = Mathf.Lerp(musicIntensity, newmusicIntensity, lerpSpeed * Time.deltaTime);
        }
        else
        {
            musicIntensity = newmusicIntensity;
        }
        transform.Rotate(axis * musicIntensity);
    }
}
