using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Bullet script
 * Wessel Roelofse
 * 23/12/2021
 * 
 * This script handles the behaviour of the bullets
 */
public class BulletScript : MonoBehaviour
{
    public float speed = 1f;
    public float lifetime = 3f;
    public Vector3 direction = Vector3.zero;
    public bool isPlayerbullet = false;

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime * TimeManager.Instance.timeScale; //Move
        lifetime -= Time.deltaTime;
        if (lifetime <= 0f || MainGameManager.Instance.finished) //Despawn after a certain amount of time
        {
            Destroy(gameObject);
        }

        //Check if something is hit
        RaycastHit hit;
        bool objhit = Physics.Raycast(transform.position, direction, out hit, speed * Time.deltaTime * TimeManager.Instance.timeScale);
        if(objhit)
        {
            //Check for object that have special properties when hit (would use an interface in the future)
            if (hit.collider.CompareTag("Player") && !isPlayerbullet)
            {
                hit.collider.GetComponent<PlayerStatus>().hit();
            }
            Powerup pu = hit.collider.GetComponent<Powerup>();
            if (pu != null)
            {
                pu.collect();
            }
            BossNavigator bs = hit.collider.GetComponent<BossNavigator>();
            if(bs != null)
            {
                MainGameManager.Instance.bossHP -= 1f;
            }
            VoltaBossScript vbs = hit.collider.GetComponent<VoltaBossScript>();
            if (vbs != null)
            {
                MainGameManager.Instance.bossHP -= 1f;
            }
            LevelIcon li = hit.collider.GetComponent<LevelIcon>();
            if(li != null)
            {
                li.onHitByBullet();
            }

            //Destroy the bullet
            Destroy(gameObject);
        }
    }
}
