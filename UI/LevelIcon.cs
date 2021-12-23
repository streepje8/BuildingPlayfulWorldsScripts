using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * Level Icon
 * Wessel Roelofse
 * 23/12/2021
 * 
 * This script manages the level icons
 */
[RequireComponent(typeof(MeshRenderer))]
public class LevelIcon : MonoBehaviour
{
    public string levelName;
    public Texture normalIcon;
    public Texture lockedIcon;

    private Material mat;

    void Start()
    {
        mat = GetComponent<MeshRenderer>().material;        
    }

    void Update()
    {
        if(GameProgression.Instance.getUnlocked(levelName))
        {
            mat.SetTexture("_MainTex", normalIcon);
        } else
        {
            mat.SetTexture("_MainTex", lockedIcon);
        }
    }

    public void onHitByBullet()
    {
        if (GameProgression.Instance.getUnlocked(levelName))
        {
            StartCoroutine(LoadLevel());
        }
    }

    IEnumerator LoadLevel()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(levelName);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
