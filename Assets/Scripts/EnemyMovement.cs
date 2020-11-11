// Decompiled with JetBrains decompiler
// Type: EnemyMovement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A28AF8C8-695A-49DE-887A-AA1AA02D690F
// Assembly location: E:\Tower_Defense_Builds\14-10-2020\Tower Defense_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour{
    private List<GameObject> waypoints;
    private GameObject CurrentWaypoint;
    private int CurrentWaypointIndex;
    private GameManager _gameManager;
    private Enemy _thisEnemy;

    private void Start(){
        _thisEnemy = this.gameObject.GetComponent<Enemy>();
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        GameObject gameObject = GameObject.Find("Waypoints");
        waypoints = new List<GameObject>();

        foreach (Transform transform in gameObject.transform){
            if (transform != null && transform.gameObject != null)
                waypoints.Add(transform.gameObject);
        }

        checkPath();
    }

    private void checkPath() => CurrentWaypoint = waypoints[CurrentWaypointIndex];

    private void Update(){
        if (transform.position != CurrentWaypoint.transform.position)
            transform.position = Vector3.MoveTowards(transform.position, CurrentWaypoint.transform.position, _thisEnemy.enemyData.speed * Time.deltaTime);
        else if (CurrentWaypointIndex < waypoints.Count - 1){
            ++CurrentWaypointIndex;
            checkPath();
        }
        else{
            _gameManager.LoseLife();
            _gameManager.activeEnemies--;
            Destroy(gameObject);
        }
    }
}