﻿// Decompiled with JetBrains decompiler
// Type: Enemy
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A28AF8C8-695A-49DE-887A-AA1AA02D690F
// Assembly location: E:\Tower_Defense_Builds\14-10-2020\Tower Defense_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using Enemies;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float currentHealth;
    public Image healthBar;
    public EnemyScriptableObject enemyData;
    public StringLiterals.singularItemDrops drops;
    private GameManager _manager;

    public float CurrentHealth
    {
        get => currentHealth;
        set => currentHealth = value;
    }

    public float Health
    {
        get => enemyData.maxHealth;
        set => enemyData.maxHealth = value;
    }

    private void Start()
    {
        _manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        currentHealth = enemyData.maxHealth;
    }

    private void Update()
    {
    }

    public void loseHealth(float value)
    {
        currentHealth -= value;
        healthBar.fillAmount = currentHealth / enemyData.maxHealth;
        if (currentHealth > 0.0)
            return;
        KillEnemy();
    }

    private void KillEnemy()
    {
        bool flag = false;
        foreach (KeyValuePair<EnemyScriptableObject, int> killedEnemy in _manager.KilledEnemies)
        {
            if (killedEnemy.Key.id == gameObject.GetComponent<Enemy>().enemyData.id)
                flag = true;
        }

        if (!flag){
            _manager.KilledEnemies.Add(gameObject.GetComponent<Enemy>().enemyData, 0);
        }

        _manager.KilledEnemies[gameObject.GetComponent<Enemy>().enemyData]++;
        _manager.AddGold(enemyData.goldValue);
        Destroy(gameObject);
    }
}