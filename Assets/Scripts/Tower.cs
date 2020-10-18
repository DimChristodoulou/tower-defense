// Decompiled with JetBrains decompiler
// Type: Tower
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A28AF8C8-695A-49DE-887A-AA1AA02D690F
// Assembly location: E:\Tower_Defense_Builds\14-10-2020\Tower Defense_Data\Managed\Assembly-CSharp.dll

using System;
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
    private float damage;
    private float firingRate;
    private float range;
    private float fireCountdown;
    private Transform target;
    private Enemy targetEnemy;
    private GameManager _manager;

    private void Start(){
        _manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        currentLevel = towerData.level;
        damage = towerData.damage;
        firingRate = towerData.firingRate;
        range = towerData.range;
        InvokeRepeating("UpdateTarget", 0.0f, 0.5f);
    }

    private void OnDrawGizmos() => Gizmos.DrawSphere(transform.position, range);

    private void Update(){
        if (target == null){
            if (!(bool) GetComponent<LineRenderer>())
                return;

            GetComponent<LineRenderer>().enabled = false;
        }
        else if (towerData.id == 0){
            if (fireCountdown <= 0.0){
                hitEnemy(targetEnemy, damage);
                fireCountdown = 1f / firingRate;
            }

            fireCountdown -= Time.deltaTime;
        }
        else{
            if (towerData.id != 1)
                return;

            LineRenderer component = GetComponent<LineRenderer>();
            component.enabled = true;
            component.SetPosition(0, transform.position);
            component.SetPosition(1, target.position);
            hitEnemy(targetEnemy, damage * Time.deltaTime, true);
        }
    }

    private void hitEnemy(Enemy enemyToHit, float damageToTake, bool isContinuousDamage = false){
        foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("Support Tower")){
            if (Vector2.Distance(transform.position, gameObject.transform.position) <= (double) gameObject.GetComponent<Tower>().range){
                if (isContinuousDamage)
                    damageToTake += damage * Time.deltaTime * gameObject.GetComponent<Tower>().damage;
                else
                    damageToTake += damage * gameObject.GetComponent<Tower>().damage;
            }
        }

        enemyToHit.loseHealth(damageToTake);
    }

    private void UpdateTarget(){
        GameObject[] gameObjectsWithTag = GameObject.FindGameObjectsWithTag("Enemy");
        float num1 = float.PositiveInfinity;
        GameObject gameObject1 = null;

        foreach (GameObject gameObject2 in gameObjectsWithTag){
            float num2 = Vector3.Distance(transform.position, gameObject2.transform.position);

            if (num2 < (double) num1){
                num1 = num2;
                gameObject1 = gameObject2;
            }
        }

        if (gameObject1 != null && num1 <= (double) range){
            target = gameObject1.transform;
            targetEnemy = gameObject1.GetComponent<Enemy>();
        }
        else
            target = null;
    }

    public void OnMouseOver(){
        if (currentLevel >= towerData.numberOfLevelups || towerData.towerLevelUps[currentLevel].levelUpCost >= _manager.gold)
            return;

        Cursor.SetCursor((Texture2D) Resources.Load("upgradeLevelCursor"), Vector2.zero, CursorMode.Auto);
    }

    public void OnMouseExit() => Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);

    public void OnMouseDown(){
        if (currentLevel >= towerData.numberOfLevelups || towerData.towerLevelUps[currentLevel].levelUpCost >= _manager.gold)
            return;

        _manager.RemoveGold(towerData.towerLevelUps[currentLevel].levelUpCost);
        damage = towerData.towerLevelUps[currentLevel].damage;
        range = towerData.towerLevelUps[currentLevel].range;
        firingRate = towerData.towerLevelUps[currentLevel].firingRate;
        if (towerData.towerLevelUps[currentLevel].levelUpSprite)
            gameObject.GetComponent<SpriteRenderer>().sprite = towerData.towerLevelUps[currentLevel].levelUpSprite;
        ++currentLevel;
    }
}