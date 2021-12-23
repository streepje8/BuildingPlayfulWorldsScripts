using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Gun Script
 * Wessel Roelofse
 * 23/12/2021
 * 
 * This script makes the gun shoot, and also implements the recoil animation
 */
public class GunScript : MonoBehaviour
{
    public float BPM = 100;
    public Transform shootingOrigin;
    public Transform GunObject;
    public GameObject bulletPrefab;
    public float ShootingDepth = 10f;
    public float kickStrength = 0.2f;

    private float time = 0f;

    void Update()
    {
        if(MainGameManager.Instance.started) { 
            time += Time.deltaTime;
            if(time > 1/ (BPM / 60f)) //BPM to Beats per second (or how i like to call it, bullets per second)
            {
                //Shoot the bullet
                GameObject bullet = Instantiate(bulletPrefab);
                Vector3 cameraforward = Camera.main.transform.forward.normalized;
                Vector3 gunPos = shootingOrigin.transform.position;
                bullet.transform.position = gunPos;
                BulletScript bulletScript = bullet.GetComponent<BulletScript>();
                bulletScript.isPlayerbullet = true;
                bulletScript.direction = ((Camera.main.transform.position + cameraforward * ShootingDepth) - gunPos).normalized;
                GunObject.localPosition = Vector3.forward * -kickStrength; //Apply recoil
                time = 0f;
            }
            GunObject.localPosition = Vector3.Lerp(GunObject.localPosition, Vector3.zero, 10f * Time.deltaTime); //Lerp the gun back to place
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position + Camera.main.transform.forward * ShootingDepth, 1f);
    }
}
