using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

/*
 * Boss Navigator
 * Wessel Roelofse
 * 23/12/2021
 * 
 * This script adds behaviour to the boss by using different states
 * [All references to boss in this script refer to the boss in the MainScene level]
 */
public enum BossState
{
    patrol,
    attackA,
    attackB
}

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(BulletSpawner))]
public class BossNavigator : MonoBehaviour
{
    public bool navigateInOrder = false;
    public List<Vector3> patrolPoints = new List<Vector3>();
    public BossState state;
    public Dictionary<BossState, BossNavigatorState> bossStates = new Dictionary<BossState, BossNavigatorState>();
    public Dictionary<BossState, RuntimeAnimatorController> bossAnimations = new Dictionary<BossState, RuntimeAnimatorController>();
    public GameObject bossBullet;
    public RuntimeAnimatorController walkingController;
    public RuntimeAnimatorController idleController;
    public ParticleSystem explosionParticles;
    public bool isDead = false;
    public TMP_Text winlossText;

    public NavMeshAgent agent { get; set; }
    public Vector3 currentDestination { get; set; } = Vector3.zero;
    public int currentDestinationInt { get; set; } = 0;
    public Animator anim { get; set; }
    public BulletSpawner bulletSpawner { get; set; }

    private float time = 0f;
    private float transitionTime = 0f;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        bossStates.Add(BossState.patrol, new PatrolState());
        bossStates.Add(BossState.attackA, new AttackA());
        bossStates.Add(BossState.attackB, new AttackB());
        bossAnimations.Add(BossState.patrol, walkingController);
        bossAnimations.Add(BossState.attackA, idleController);
        bossAnimations.Add(BossState.attackB, idleController);
        bulletSpawner = GetComponent<BulletSpawner>();
    }

    private void Start()
    {
        currentDestination = patrolPoints[0];
        agent.SetDestination(currentDestination);
    }

    void Update()
    {
        time += Time.deltaTime;
        if (MainGameManager.Instance.countdown <= 0) //Only work if the level has started
        {
            //Update the current state and animation
            if(!isDead) { 
                bossStates[state]?.UpdateAgent(this);
                anim.runtimeAnimatorController = bossAnimations[state]; //i'm still learning animations, so this aweful code works for now
            } else
            {
                anim.runtimeAnimatorController = idleController;
            }

            //Code that detects when the level should finish
            if (MainGameManager.Instance.bossHP <= 0) //The boss dies
            {
                if (!isDead)
                {
                    explosionParticles.Play();
                    transitionTime = explosionParticles.main.duration;
                    isDead = true;
                    time = 0f;
                    winlossText.text = "-= Victory =-";
                    GameProgression.Instance.setCompleted("MainScene");
                    MainGameManager.Instance.player.gameObject.GetComponent<GunScript>().BPM = 0f;
                    MainGameManager.Instance.playerController.BlockMovement = true;
                    MainGameManager.Instance.finished = true;
                    agent.isStopped = true;
                }
            }
            if(!MainGameManager.Instance.musicManager.source.isPlaying && MainGameManager.Instance.levelPlaytime > 10f && MainGameManager.Instance.bossHP > 0 && !isDead) //The music finishes before the boss dies
            {
                winlossText.text = "-= Defeat =-";
                MainGameManager.Instance.bossHP = 100;
                transitionTime = 6f;
                time = 0f;
                isDead = true;
                MainGameManager.Instance.player.gameObject.GetComponent<GunScript>().BPM = 0f;
                MainGameManager.Instance.playerController.BlockMovement = true;
                MainGameManager.Instance.finished = true;
                agent.isStopped = true;
            }
            if (MainGameManager.Instance.playerStatus.HP <= 0f && MainGameManager.Instance.bossHP > 0 && !isDead) //The player dies before the boss dies
            {
                winlossText.text = "-= Defeat =-";
                MainGameManager.Instance.bossHP = 100;
                transitionTime = 6f;
                time = 0f;
                isDead = true;
                MainGameManager.Instance.player.gameObject.GetComponent<GunScript>().BPM = 0f;
                MainGameManager.Instance.playerController.BlockMovement = true;
                MainGameManager.Instance.finished = true;
                agent.isStopped = true;
            }
        }
        //Fade out the music if the boss dies
        if (isDead) { 
            if(MainGameManager.Instance.musicManager.source.volume > 0f) { 
                MainGameManager.Instance.musicManager.source.volume -= 1f * Time.deltaTime;
            }
        }
        //Wait for the cutscene to end and switch back to the level select (only if the level finishes of course)
        if (time > transitionTime && isDead)
        {
            StartCoroutine(BackToLevelSelect());
        }
    }

    private void OnDrawGizmosSelected()
    {
        foreach (Vector3 point in patrolPoints)
        {
            Gizmos.DrawWireSphere(point, 1f);
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

//The boss state template
public abstract class BossNavigatorState
{
    public abstract void UpdateAgent(BossNavigator bn);
}


//All the states for the boss/////////////////////////////////////////////////////////////////////////////
class PatrolState : BossNavigatorState
{
    public override void UpdateAgent(BossNavigator bn)
    {
        bn.agent.isStopped = false;
        if (Vector3.Distance(bn.transform.position, bn.currentDestination) < 0.5)
        {
            if (bn.navigateInOrder)
            {
                bn.currentDestinationInt++;
                if (bn.currentDestinationInt >= bn.patrolPoints.Count)
                {
                    bn.currentDestinationInt = 0;
                }
            }
            else
            {
                bn.currentDestinationInt = Mathf.RoundToInt(Random.Range(0, bn.patrolPoints.Count - 1));
                if (bn.currentDestinationInt >= bn.patrolPoints.Count)
                {
                    bn.currentDestinationInt--;
                }
            }
            //Walk to a (random) point, then do a random attack
            bn.currentDestination = bn.patrolPoints[bn.currentDestinationInt];
            bn.agent.SetDestination(bn.currentDestination);
            bn.state = (new System.Random().NextDouble() > 0.5) ? BossState.attackA : BossState.attackB;
        }
    }
}

class AttackA : BossNavigatorState
{
    int bulletcount = 0;
    float timeboi = 0f;
    public override void UpdateAgent(BossNavigator bn)
    {
        timeboi += Time.deltaTime;
        bn.agent.isStopped = true;
        if(timeboi > 0.03f) { 
            bulletcount++;
            if(bulletcount < 10)
            {
                //Shoot bullets in all directions (this code still seems to have a bug in it but i have no idea why. It should shoot everywhere but for some reason it sometimes ignores one axis)
                BulletScript newBullet = bn.bulletSpawner.spawnBullet(bn.bossBullet);
                newBullet.transform.position = bn.transform.position + Vector3.up * 2f;
                newBullet.direction = new Vector3(Random.Range(-1, 1), Random.Range(-0.4f, 1),Random.Range(-1, 1));
            }
            if(bulletcount == 10)
            {
                bn.state = BossState.patrol;
            }
            if(bulletcount > 10)
            {
                bulletcount = 0;
            }
            timeboi = 0f;
        }
    }
}

class AttackB : BossNavigatorState
{
    int bulletcount = 0;
    float timeboi = 0f;
    public override void UpdateAgent(BossNavigator bn)
    {
        timeboi += Time.deltaTime;
        bn.agent.isStopped = true;
        if (timeboi > 0.1f)
        {
            bulletcount++;
            if (bulletcount < 10)
            {
                //Shoot towards the player in a shotgun like style
                BulletScript newBullet = bn.bulletSpawner.spawnBullet(bn.bossBullet);
                newBullet.transform.position = bn.transform.position + Vector3.up * 2f;
                newBullet.direction = (MainGameManager.Instance.player.position - (bn.transform.position + Vector3.up * 2f)).normalized + ((new Vector3(Random.Range(-1, 1), Random.Range(-0.4f, 1), Random.Range(-1, 1))).normalized * 0.1f);
            }
            if (bulletcount == 10)
            {
                bn.state = BossState.patrol;
            }
            if (bulletcount > 10)
            {
                bulletcount = 0;
            }
            timeboi = 0f;
        }
    }
}