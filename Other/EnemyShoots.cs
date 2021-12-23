using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Enemy Shoots
 * Wessel Roelofse
 * 23/12/2021
 * 
 * A script that makes enemies shoot.
 */
public class EnemyShoots : MonoBehaviour
{
    public Vector3 relativeOrigin = Vector3.zero; //From where does the enemy shoot?
    public Vector3 inAccuracy = Vector3.zero;
    public float ShootMinum = 0f;
    public float reloadDuration = 2f;
    public ReactionType ShootsAt = ReactionType.Bass;
    public GameObject bulletPrefab;


    private float cooldown = 0f;
    void Update()
    {
        if(cooldown <= 0f && !MainGameManager.Instance.finished) //if the level has started
        {
            bool peformShoot = false;
            switch (ShootsAt)
            {
                case ReactionType.MainActivity:
                    peformShoot = MainGameManager.Instance.musicManager.activity > ShootMinum;
                    break;
                case ReactionType.Bass:
                    peformShoot = MainGameManager.Instance.musicManager.bass > ShootMinum;
                    break;
                case ReactionType.Voices:
                    peformShoot = MainGameManager.Instance.musicManager.voices > ShootMinum;
                    break;
            }
            if(peformShoot)
            {
                //Create a bullet and shoot it towards the player
                GameObject bullet = Instantiate(bulletPrefab);
                bullet.transform.position = transform.position + relativeOrigin;
                BulletScript bs = bullet.GetComponent<BulletScript>();
                bs.direction = (MainGameManager.Instance.player.position - transform.position).normalized + new Vector3(Random.Range(-inAccuracy.x, inAccuracy.x), Random.Range(-inAccuracy.y, inAccuracy.y), Random.Range(-inAccuracy.z, inAccuracy.z));
                cooldown = reloadDuration;
            }
        } else {
            cooldown -= Time.deltaTime;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position + relativeOrigin, 0.1f);
    }
}
