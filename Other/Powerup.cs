using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Powerup
 * Wessel Roelofse
 * 23/12/2021
 * 
 * This script handles all powerup behaviour
 * (sadly its not too scaleable)
 */
public class Powerup : MonoBehaviour
{
    public enum powerupType
    {
        doubleBPM,
        halfBPM,
        fullHP
    }

    public float speed = 1;
    public powerupType type;
    private Dictionary<powerupType, System.Action> powerups = new Dictionary<powerupType, System.Action>();

    private void Awake()
    {
        //Program in all the power up types with their respective effects
        powerups.Add(powerupType.doubleBPM, () => { MainGameManager.Instance.player.gameObject.GetComponent<GunScript>().BPM *= 2; });
        powerups.Add(powerupType.halfBPM, () => { MainGameManager.Instance.player.gameObject.GetComponent<GunScript>().BPM /= 2; });
        powerups.Add(powerupType.fullHP, () => { MainGameManager.Instance.playerStatus.HP = 100f; });
    }

    void Update()
    {
        transform.Rotate(Vector3.one * speed * Time.deltaTime * TimeManager.Instance.timeScale); //The rotation animation
    }

    public void collect()
    {
        powerups[type]?.Invoke();
        Destroy(gameObject);    
    }
}
