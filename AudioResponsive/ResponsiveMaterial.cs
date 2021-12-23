using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Responsive material
 * Wessel Roelofse
 * 23/12/2021
 * 
 * Change the intensity of the _Color property on a material based on the music intensity
 */
public class ResponsiveMaterial : MonoBehaviour
{
    public Material material;
    public Color objectcol;
    public float intensity = 4.0f;
    public ReactionType reactsAt;
    public bool doLerp = false;
    public float lerpSpeed = 25f;

    private Material copy;
    private Vector4 color;
    private float musicIntensity = 0f;

    void Start()
    {
        copy = new Material(material);
        GetComponent<MeshRenderer>().material = copy;
        color = material.color;
        objectcol = color;
    }

    void Update()
    {
        color = objectcol; //Allow the user to change the color on runtime (from the inspector or a script)
        float newmusicIntensity = 0f;
        switch(reactsAt)
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
        if(doLerp)
        {
            musicIntensity = Mathf.Lerp(musicIntensity, newmusicIntensity, lerpSpeed * Time.deltaTime);
        } else
        {
            musicIntensity = newmusicIntensity;
        }
        copy.color = color * Mathf.Clamp((intensity * musicIntensity), 0.5f, 100);
    }
}
