using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Bullet Spawner
 * Wessel Roelofse
 * 23/12/2021
 * 
 * The only reason this script exists was because i could not instanciate bullets from the boss states (because they are not monobehaviours). 
 * But since i now pass BossNavigator as an argument, this script is no longer needed.
 */
public class BulletSpawner : MonoBehaviour
{
    public BulletScript spawnBullet(GameObject bullet)
    {
        return Instantiate(bullet).GetComponent<BulletScript>();
    }
}
