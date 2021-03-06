﻿// Decompiled with JetBrains decompiler
// Type: Tower
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A28AF8C8-695A-49DE-887A-AA1AA02D690F
// Assembly location: E:\Tower_Defense_Builds\14-10-2020\Tower Defense_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public struct LevelUpData
{
    public float damage;
    public float firingRate;
    public float range;
    public int levelUpCost;
    public Sprite levelUpSprite;
}

public class Tower : MonoBehaviour{
    public TowerData towerData;
    private int currentLevel;

    [ShowInInspector] private float damage, firingRate, range, fireCountdown;
    private Transform target;
    private Enemy targetEnemy;
    private List<Enemy> targetEnemies;
    private GameManager _manager;

    private GameObject _shockwaveEffectGO;

    private void Start(){
        _manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        currentLevel = towerData.currentLevel;
        damage = towerData.damage;
        firingRate = towerData.firingRate;
        range = towerData.range;

        UpdateStatsBasedOnUnlockedUpgrades();
        StartUpdatingTargets();

        if (towerData.id == TowerData.EARTHQUAKE_TOWER){
            _shockwaveEffectGO = GetComponentInChildren<Shockwave>().gameObject;
            _shockwaveEffectGO.SetActive(false);
        }
    }

    private void UpdateStatsBasedOnUnlockedUpgrades(){
        List<UpgradeScriptableObject> applicableUpgrades = GlobalPlayerProgress.getUnlockedUpgradesForSpecificTower(towerData);

        foreach (var upgrade in applicableUpgrades){
            foreach (UpgradeScriptableObject.StatEffect effect in upgrade.upgradeEffects){
                switch (effect.affects){
                    case UpgradeScriptableObject.UpgradeEffect.Damage:
                        damage = GlobalPlayerProgress.CalculateValueAfterUpgrade(damage, effect);
                        break;
                    case UpgradeScriptableObject.UpgradeEffect.Cost:
                        break;
                    case UpgradeScriptableObject.UpgradeEffect.Range:
                        range = GlobalPlayerProgress.CalculateValueAfterUpgrade(range, effect);
                        break;
                    case UpgradeScriptableObject.UpgradeEffect.FiringRate:
                        firingRate = GlobalPlayerProgress.CalculateValueAfterUpgrade(firingRate, effect);
                        break;
                }
            }
        }
    }

    private void StartUpdatingTargets(){
        if (towerData.isAreaTower){
            InvokeRepeating("UpdateMultipleTargets", 0.0f, 0.5f);
        }
        else{
            InvokeRepeating("UpdateTarget", 0.0f, 0.5f);
        }
    }

    private void OnDrawGizmos() => Gizmos.DrawSphere(transform.position, range);

    private void Update(){
        if (target == null && (targetEnemies == null || targetEnemies.Count == 0)){
            switch (towerData.id){
                case TowerData.ARCANE_TOWER:
                    if (gameObject.GetComponent<LineRenderer>()){
                        gameObject.GetComponent<LineRenderer>().enabled = false;
                    }
                    break;
                case TowerData.EARTHQUAKE_TOWER:
                    if (_shockwaveEffectGO.activeSelf){
                        _shockwaveEffectGO.SetActive(false);
                    }
                    break;
            }
        }
        else{
            switch (towerData.id){
                case TowerData.ARROW_TOWER:
                    if (fireCountdown <= 0f){
                        /*GameObject arrow = Instantiate(Resources.Load<GameObject>("ArrowPrefab"));
                        arrow.transform.position = transform.position;
                        arrow.GetComponent<Arrow>().Target = target;*/
                        hitEnemy(targetEnemy, damage);
                        fireCountdown = 1f / firingRate;
                    }
                    
                    fireCountdown -= Time.deltaTime;
                    break;
                case TowerData.ARCANE_TOWER:
                    LineRenderer component = GetComponent<LineRenderer>();
                    component.enabled = true;
                    component.SetPosition(0, transform.position);
                    component.SetPosition(1, target.position);
                    hitEnemy(targetEnemy, damage * Time.deltaTime, true);
                    break;
                case TowerData.EARTHQUAKE_TOWER:
                    if (fireCountdown <= 0f){
                        _shockwaveEffectGO.SetActive(true);
                        hitEnemies(targetEnemies, damage);
                        fireCountdown = 1f / firingRate;
                    }
                    
                    fireCountdown -= Time.deltaTime;
                    break;
            }
        }
    }

    /**
     * Hits a single enemy
     * @param Enemy enemyToHit - which Enemy will be hit
     * @param float damageToTake - how much damage will the enemy take
     * @param bool isContinuousDamage - true if tower deals damage over a duration (modified by deltaTime)
     */
    private void hitEnemy(Enemy enemyToHit, float damageToTake, bool isContinuousDamage = false){
        damageToTake = ModifyDamage(damageToTake, isContinuousDamage, enemyToHit);

        if (enemyToHit.CurrentHealth > 0){
            enemyToHit.loseHealth(damageToTake);
        }
    }

    private void UpdateTarget(){
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;
        
        foreach (GameObject enemy in enemies){
            // If the enemy is flying and the tower can hit flying enemies
            if (enemy.GetComponent<Enemy>().enemyData.isFlying){
                if (towerData.canHitFlyingEnemies){
                    float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);

                    if (distanceToEnemy < shortestDistance){
                        shortestDistance = distanceToEnemy;
                        nearestEnemy = enemy;
                    }
                }
            }
            else{
                float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                
                if (distanceToEnemy < shortestDistance){
                    shortestDistance = distanceToEnemy;
                    nearestEnemy = enemy;
                }
            }
        }
        
        if (nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
            targetEnemy = nearestEnemy.GetComponent<Enemy>();
        } else
        {
            target = null;
        }
    }

    private void UpdateMultipleTargets(){
        targetEnemies = new List<Enemy>();
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float shortestDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies){
            //Earthquake towers cant target flying enemies - Possible TODO in case we want them to
            if (!enemy.GetComponent<Enemy>().enemyData.isFlying){
                float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);

                if (distanceToEnemy < towerData.range){
                    targetEnemies.Add(enemy.GetComponent<Enemy>());
                }
            }
        }
    }
    
    /**
     * Hits a list of enemies
     * @param Enemy enemiesToHit - which Enemies will be hit
     * @param float damageToTake - how much damage will the enemies take
     * @param bool isContinuousDamage - true if tower deals damage over a duration (modified by deltaTime)
     */
    private void hitEnemies(List<Enemy> enemiesToHit, float damageToTake, bool isContinuousDamage = false){
        
        foreach (Enemy enemyToHit in enemiesToHit){
            damageToTake = ModifyDamage(damageToTake, isContinuousDamage, enemyToHit);

            // Possibly need that check in case multiple towers hit same enemy and "kill him" multiple times
            if (enemyToHit.CurrentHealth > 0){
                enemyToHit.loseHealth(damageToTake);
            }
        }
    }

    /**
     * Fired when mouse cursor is over the Tower object
     * Modifies the game cursor to a plus sign, indicating that the tower can be levelled up
     */
    public void OnMouseOver(){
        if (currentLevel < towerData.towerLevelUps.Count && towerData.towerLevelUps[currentLevel].levelUpCost <= _manager.gold){
            Cursor.SetCursor((Texture2D) Resources.Load("upgradeLevelCursor"), Vector2.zero, CursorMode.Auto);
        }
    }

    /**
     * Fired when mouse cursor exits the Tower object
     * Reverts the cursor back to its original Texture
     */
    public void OnMouseExit() => Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);

    /**
     * Fired when mouse cursor clicks the Tower object
     * Levels up the tower, if the player has enough gold
     */
    public void OnMouseDown(){
        if (currentLevel < towerData.towerLevelUps.Count && towerData.towerLevelUps[currentLevel].levelUpCost <= _manager.gold){

            _manager.RemoveGold(towerData.towerLevelUps[currentLevel].levelUpCost);
            damage = towerData.towerLevelUps[currentLevel].damage;
            range = towerData.towerLevelUps[currentLevel].range;
            firingRate = towerData.towerLevelUps[currentLevel].firingRate;

            if (towerData.towerLevelUps[currentLevel].levelUpSprite){
                gameObject.GetComponent<SpriteRenderer>().sprite = towerData.towerLevelUps[currentLevel].levelUpSprite;
            }

            ++currentLevel;
        }
    }
    
    /*
     * Damage modifying functions
     */
    private float ModifyDamage(float damageToTake, bool isContinuousDamage, Enemy enemyToHit){
        foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("Support Tower")){
            if (Vector2.Distance(transform.position, gameObject.transform.position) <= gameObject.GetComponent<Tower>().range){
                if (isContinuousDamage)
                    damageToTake += damage * Time.deltaTime * gameObject.GetComponent<Tower>().damage;
                else
                    damageToTake += damage * gameObject.GetComponent<Tower>().damage;
            }
        }
        
        //Take into account resistances/vulnerabilities
        foreach (DamageTypes damageType in towerData.towerDamageTypes){
            if (enemyToHit.enemyData.hasResistanceToDamageTypes){
                if (enemyToHit.enemyData.enemyResistances.Contains(damageType)){
                    //Resistance to a damage type reduces incoming damage by 50%
                    damageToTake -= damageToTake / 2;
                }
            }

            if (enemyToHit.enemyData.hasVulnerabilityToDamageTypes){
                if (enemyToHit.enemyData.enemyResistances.Contains(damageType)){
                    //Resistance to a damage type increases incoming damage by 50%
                    damageToTake += damageToTake / 2;
                }
            }
        }

        return damageToTake;
    }
}