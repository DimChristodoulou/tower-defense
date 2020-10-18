// Decompiled with JetBrains decompiler
// Type: EnemySpawn
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A28AF8C8-695A-49DE-887A-AA1AA02D690F
// Assembly location: E:\Tower_Defense_Builds\14-10-2020\Tower Defense_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour{
    public List<GameObject> spawnPoints;
    public List<GameObject> waypoints;
    public int activeEnemyCount;
    private WaveManager _waveManager;

    private void Start() => _waveManager = gameObject.GetComponent<WaveManager>();

    private void Update(){ }

    public IEnumerator SpawnEnemy(WaveManager.WaveEnemies enemyWave){
        bool controlVar = true;

        while (controlVar){
            if (activeEnemyCount < enemyWave.waveEnemyCount){
                Vector3 position = spawnPoints[Random.Range(0, spawnPoints.Count)].transform.position;
                Instantiate(enemyWave.enemiesList[Random.Range(0, enemyWave.enemiesList.Count)], new Vector3(position.x, position.y, 0.0f),
                    Quaternion.identity);
                ++activeEnemyCount;
            }
            else if (activeEnemyCount == enemyWave.waveEnemyCount)
                controlVar = false;

            yield return new WaitForSeconds(2f);
        }

        activeEnemyCount = 0;
    }
}