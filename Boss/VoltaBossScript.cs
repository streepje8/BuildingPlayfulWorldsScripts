using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


/*
 * Volta Boss Script
 * Wessel Roelofse
 * 23/12/2021
 * 
 * This code contains all the code for the VillaVolta level boss.
 * [All references with the word boss in this script reference to the VillaVolta boss]
 */
public class VoltaBossScript : MonoBehaviour
{
    public float attackCooldown = 1f;
    public bool isDead = false;
    public TMP_Text winlossText;
    public ParticleSystem explosionParticles;
    public GameObject bossBullet;

    private float myCooldown = 0f;
    private float time = 0f;
    private float transitionTime = 0f;
    private VoltaBossState state;
    private int bulletcount = 0;
    private float timeboi = 0f;

    public enum VoltaBossState
    {
        idle,
        attackA,
        attackB
    }

    void Update()
    {
        
        time += Time.deltaTime;
        if (MainGameManager.Instance.countdown <= 0)
        {
            if (!isDead)
            {
                switch(state)
                {
                    case VoltaBossState.idle:
                        myCooldown -= Time.deltaTime;
                        if (myCooldown <= 0)
                        {
                            //Pick a random state, then cooldown again
                            state = (new System.Random().NextDouble() > 0.5) ? VoltaBossState.attackA : VoltaBossState.attackB;
                            myCooldown = attackCooldown;
                        }
                        break;
                    case VoltaBossState.attackB:
                        timeboi += Time.deltaTime;
                        if (timeboi > 0.03f)
                        {
                            bulletcount++;
                            if (bulletcount < 40)
                            {
                                //Shoot everywere, for more info see the BossNavigator script.
                                BulletScript newBullet = Instantiate(bossBullet).GetComponent<BulletScript>();
                                newBullet.transform.position = transform.position + Vector3.up * 2f;
                                newBullet.direction = new Vector3(Random.Range(-1, 1), Random.Range(-0.4f, 1), Random.Range(-1, 1));
                            }
                            if (bulletcount == 10)
                            {
                                state = VoltaBossState.idle;
                            }
                            if (bulletcount > 10)
                            {
                                bulletcount = 0;
                            }
                            timeboi = 0f;
                        }
                        break;
                    case VoltaBossState.attackA:
                        timeboi += Time.deltaTime;
                        if (timeboi > 0.1f)
                        {
                            bulletcount++;
                            if (bulletcount < 10)
                            {
                                //Shotgun towards the player
                                BulletScript newBullet = Instantiate(bossBullet).GetComponent<BulletScript>();
                                newBullet.transform.position = transform.position + Vector3.up * 2f;
                                newBullet.direction = (MainGameManager.Instance.player.position - (transform.position + Vector3.up * 2f)).normalized + ((new Vector3(Random.Range(-1, 1), Random.Range(-0.4f, 1), Random.Range(-1, 1))).normalized * 0.1f);
                            }
                            if (bulletcount == 10)
                            {
                                state = VoltaBossState.idle;
                            }
                            if (bulletcount > 10)
                            {
                                bulletcount = 0;
                            }
                            timeboi = 0f;
                        }
                        break;
                }
            }

            //Check if the level is finished
            if (MainGameManager.Instance.bossHP <= 0) //Boss died
            {
                if (!isDead)
                {
                    explosionParticles.Play();
                    transitionTime = explosionParticles.main.duration;
                    isDead = true;
                    time = 0f;
                    winlossText.text = "-= Victory =-";
                    GameProgression.Instance.Unlock("MainScene");
                    MainGameManager.Instance.player.gameObject.GetComponent<GunScript>().BPM = 0f;
                    MainGameManager.Instance.playerController.BlockMovement = true;
                    MainGameManager.Instance.finished = true;
                }
            }
            if (!MainGameManager.Instance.musicManager.source.isPlaying && MainGameManager.Instance.levelPlaytime > 10f && MainGameManager.Instance.bossHP > 0 && !isDead) //Music finished before the boss died
            {
                winlossText.text = "-= Defeat =-";
                MainGameManager.Instance.bossHP = 100;
                transitionTime = 6f;
                time = 0f;
                isDead = true;
                MainGameManager.Instance.player.gameObject.GetComponent<GunScript>().BPM = 0f;
                MainGameManager.Instance.playerController.BlockMovement = true;
                MainGameManager.Instance.finished = true;
            }
            if (MainGameManager.Instance.playerStatus.HP <= 0f && MainGameManager.Instance.bossHP > 0 && !isDead) //Player died before the boss
            {
                winlossText.text = "-= Defeat =-";
                MainGameManager.Instance.bossHP = 100;
                transitionTime = 6f;
                time = 0f;
                isDead = true;
                MainGameManager.Instance.player.gameObject.GetComponent<GunScript>().BPM = 0f;
                MainGameManager.Instance.playerController.BlockMovement = true;
                MainGameManager.Instance.finished = true;
            }
        }
        //If the level finishes
        if (isDead)
        {
            if (MainGameManager.Instance.musicManager.source.volume > 0f)
            {
                MainGameManager.Instance.musicManager.source.volume -= 1f * Time.deltaTime;
            }
        }
        if (time > transitionTime && isDead)
        {
            StartCoroutine(BackToLevelSelect());
        }
    }

    IEnumerator BackToLevelSelect()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("LevelSelector");
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
