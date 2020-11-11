// Decompiled with JetBrains decompiler
// Type: EnemySpawn
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A28AF8C8-695A-49DE-887A-AA1AA02D690F
// Assembly location: E:\Tower_Defense_Builds\14-10-2020\Tower Defense_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;

public class EnemySpawn : MonoBehaviour{
    public List<GameObject> spawnPoints;
    public List<GameObject> waypoints;
    private WaveManager _waveManager;
    private GameManager _gameManager;

    private void Start(){
        _waveManager = gameObject.GetComponent<WaveManager>();
        _gameManager = gameObject.GetComponent<GameManager>();
    }

    private void Update(){ }

    public IEnumerator SpawnRandomEnemy(WaveManagerScriptableObject.Wave wave, bool isEndless = false){
        int activeEnemyCount = 0;
        bool controlVar = true;

        if (!isEndless){
            while (controlVar){
                if (activeEnemyCount < wave.waveEnemyCount){
                    Vector3 position = spawnPoints[Random.Range(0, spawnPoints.Count)].transform.position;

                    //IF TIMER IS 20 AND WAVE HAS 10 ENEMIES WITH 2 SECOND SPAWN INTERVAL, SMTH LIKE A TIMING BUG OCCURS
                    Instantiate(wave.EnemiesList[Random.Range(0, wave.EnemiesList.Count)], 
                                new Vector3(position.x, position.y, 0.0f), 
                                        Quaternion.identity);

                    ++_gameManager.activeEnemies;
                    ++activeEnemyCount;
                }
                else if (activeEnemyCount == wave.waveEnemyCount){
                    controlVar = false;
                }

                yield return new WaitForSeconds(wave.enemySpawnInterval);
            }
        }
        else{
            while (controlVar){
                int sizeOfWave = Random.Range(0, 100);
                if (activeEnemyCount < sizeOfWave){
                    Vector3 position = spawnPoints[Random.Range(0, spawnPoints.Count)].transform.position;
                    Instantiate(wave.EnemiesList[Random.Range(0, wave.EnemiesList.Count)], new Vector3(position.x, position.y, 0.0f),
                        Quaternion.identity);
                    ++activeEnemyCount;
                }
                else if (activeEnemyCount == sizeOfWave)
                    controlVar = false;

                yield return new WaitForSeconds(2f);
            }
        }
        
    }

    public IEnumerator SpawnSpecificEnemies(WaveManagerScriptableObject.Wave wave){
        int activeEnemyCount = 0;
        bool controlVar = true;
        
        Vector3 position = spawnPoints[Random.Range(0, spawnPoints.Count)].transform.position;
        
        for (int i = 0; i < wave.EnemiesList.Count; i++){
            for (int j = 0; j < wave.enemiesWeightOrNumber[i]; j++){
                Instantiate(wave.EnemiesList[i], 
                    new Vector3(position.x, position.y, 0.0f), 
                    Quaternion.identity);
                
                ++_gameManager.activeEnemies;
                ++activeEnemyCount;
                
                yield return new WaitForSeconds(wave.enemySpawnInterval);
            }
        }
    }
}