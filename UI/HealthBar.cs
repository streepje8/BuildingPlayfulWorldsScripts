using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Healthbar
 * Wessel Roelofse
 * 23/12/2021
 * 
 * This script manages the healthbars on screen.
 */
[RequireComponent(typeof(Slider))]
public class HealthBar : MonoBehaviour
{
    public enum BarType
    {
        player,
        boss
    }
    private Slider bar;
    public BarType type = BarType.player;
    private float startBossHP = 0f;


    void Start()
    {
        bar = GetComponent<Slider>();
        startBossHP = MainGameManager.Instance.bossHP;
    }

    void Update()
    {
        switch(type)
        {
            case BarType.player:
                bar.value = MainGameManager.Instance.playerStatus.HP;
                break;
            case BarType.boss:
                bar.value = (MainGameManager.Instance.bossHP / startBossHP) * 100f;
                break;
        }
    }
}
