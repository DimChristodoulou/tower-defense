// Decompiled with JetBrains decompiler
// Type: Enemy
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A28AF8C8-695A-49DE-887A-AA1AA02D690F
// Assembly location: E:\Tower_Defense_Builds\14-10-2020\Tower Defense_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using Enemies;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    private float currentHealth;
    public Image healthBar;
    public GameObject entireHealthBar;
    public EnemyScriptableObject enemyData;
    private GameManager _manager;

    [ShowInInspector]
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

        if (enemyData.isHealer){
            InvokeRepeating("healTargets", 0.0f, enemyData.healingCooldown);
        }
    }

    public void loseHealth(float value)
    {
        currentHealth -= value;

        if (!entireHealthBar.activeSelf){
            entireHealthBar.SetActive(true);
        }
        
        healthBar.fillAmount = currentHealth / enemyData.maxHealth;

        if (currentHealth <= 0.0f){
            KillEnemy();
        }
    }
    
    public void gainHealth(float value)
    {
        currentHealth += value;

        if (currentHealth > enemyData.maxHealth){
            currentHealth = enemyData.maxHealth;
        }

        healthBar.fillAmount = currentHealth / enemyData.maxHealth;
    }
    
    public void gainHealthPercentage(float percentage)
    {
        currentHealth += enemyData.maxHealth * percentage;

        if (currentHealth > enemyData.maxHealth){
            currentHealth = enemyData.maxHealth;
        }

        healthBar.fillAmount = currentHealth / enemyData.maxHealth;
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
        //Debug.Log("in KillEnemy with " + _manager.activeEnemies);
        --_manager.activeEnemies;
        Destroy(gameObject);
    }

    /*
     * Healer Functionality
     */
    private void healTargets(){
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        enemyData.speed = 0;
        
        foreach (GameObject enemy in enemies){
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);

            if (distanceToEnemy < enemyData.healingRange){
                if (enemyData.healing == HealingValue.FlatValue){
                    enemy.GetComponent<Enemy>().gainHealth(enemyData.healingFlatValue);
                }
                else{
                    enemy.GetComponent<Enemy>().gainHealthPercentage(enemyData.healingPercentage);
                }
            }
        }
        
        enemyData.speed = 1.5f;
    }
    
    
    /*
     * Parses an enemy object and extracts all gameplay data from it.
     * Required return values: maxHealth, goldValue, itemDrop
     * Possible return values: isFlying, isHealer, resistances, vulnerabilities 
     */
    public List<string> parseEnemyDataToString(){
        List<string> enemyDataStrings = new List<string>();

        enemyDataStrings.Add("Max Health:" + enemyData.maxHealth);
        enemyDataStrings.Add("Awards " + enemyData.goldValue + " gold when killed");
        enemyDataStrings.Add("Can award 1 " + Tools.monsterDropToString(enemyData.drops) + " when killed");

        if (enemyData.isFlying){
            enemyDataStrings.Add("[B]Flyer");
        }

        if (enemyData.isHealer){
            if (enemyData.healing == HealingValue.FlatValue){
                enemyDataStrings.Add("[B]Heals " + enemyData.healingFlatValue + " health of nearby enemies every " + enemyData.healingCooldown + " seconds");
            }
            else if(enemyData.healing == HealingValue.Percentage){
                enemyDataStrings.Add("[B]Heals " + enemyData.healingPercentage + "% of nearby enemies health every " + enemyData.healingCooldown + " seconds");
            }
        }

        if (enemyData.hasResistanceToDamageTypes){
            string temp = "[B]Has resistance to ";

            foreach (DamageTypes damageType in enemyData.enemyResistances){
                temp += Tools.DamageTypeToString(damageType) + ", ";
            }

            temp = temp.Substring(0, temp.Length - 2);
            temp += " damage";
            
            enemyDataStrings.Add(temp);
        }
        
        if (enemyData.hasVulnerabilityToDamageTypes){
            string temp = "[B]Is vulnerable to ";

            foreach (DamageTypes damageType in enemyData.enemyVulnerabilities){
                temp += Tools.DamageTypeToString(damageType) + ", ";
            }

            temp = temp.Substring(0, temp.Length - 2);
            temp += " damage";
            
            enemyDataStrings.Add(temp);
        }
        
        return enemyDataStrings;
    }
}