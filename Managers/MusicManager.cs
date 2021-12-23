using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Music Manager
 * Wessel Roelofse
 * 23/12/2021
 * 
 * This class plays the music on the specified audio source and splits the current activity up in to three types.
 */
public enum ReactionType
{
    MainActivity,
    Bass,
    Voices
}
public class MusicManager : MonoBehaviour
{
    public AudioSource source;
    public AudioClip levelaudio;
    public float activity = 0f;
    public float bass = 0f;
    public float voices = 0f;
    public float activityMultiplier = 1;
    public float activityToTimeMultiplayer = 1;

    public void startMusic()
    {
        source.clip = levelaudio;
        source.Play();
    }

    void Update()
    {
        if (source.isPlaying)
        {
            float[] spectrum = new float[256];

            AudioListener.GetSpectrumData(spectrum, 0, FFTWindow.Rectangular);

            activity = 0;
            for (int i = 1; i < spectrum.Length - 1; i++)
            {
                activity += spectrum[i];
                Debug.DrawLine(new Vector3(Mathf.Log(i - 1), spectrum[i - 1] - 10, 1) * 10, new Vector3(Mathf.Log(i), spectrum[i] - 10, 1) * 10, Color.green); //I recommend looking at this, its really cool 0-0
            }
            bass = spectrum[0] > 0.1f ? spectrum[0]-0.1f:0; //calculate the bass
            voices = 0f;
            for(int i = spectrum.Length / 2; i < (spectrum.Length/2) + (spectrum.Length/4); i++)
            {
                voices += spectrum[i];
            }
            voices = Mathf.InverseLerp(0, 1, voices); //Calculate the voices
            activity = Mathf.Clamp(((activity - 0.2f) * 30) / 40 * activityMultiplier, 0, 1); //calculate the main activity
        }
        else
        {
            activity = 0f;
            voices = 0f;
            bass = 0f;
        }
        //Change the time speed according to the main activity
        if(!MainGameManager.Instance.currentSceneIsMainmenu) { 
            TimeManager.Instance.timeScale = Mathf.Clamp(1 + ((activity - 0.5f) * activityToTimeMultiplayer),0.2f,2);
        } else
        {
            TimeManager.Instance.timeScale = 1f;
        }
    }
}
