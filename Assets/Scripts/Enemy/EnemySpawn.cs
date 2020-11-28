// Decompiled with JetBrains decompiler
// Type: EnemySpawn
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A28AF8C8-695A-49DE-887A-AA1AA02D690F
// Assembly location: E:\Tower_Defense_Builds\14-10-2020\Tower Defense_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using Enemies;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawn : MonoBehaviour{
    public System.Collections.Generic.List<GameObject> spawnPoints;
    public System.Collections.Generic.List<GameObject> waypoints;
    private WaveManager _waveManager;
    private GameManager _gameManager;

    private void Awake(){
        _waveManager = gameObject.GetComponent<WaveManager>();
        _gameManager = gameObject.GetComponent<GameManager>();
    }

    private void Start(){

    }

    private void Update(){ }

    public IEnumerator SpawnRandomEnemy(WaveManagerScriptableObject.Wave wave, bool isEndless = false){
        int activeEnemyCount = 0;
        bool controlVar = true;

        if (!isEndless){
            while (controlVar){
                if (activeEnemyCount < wave.waveEnemyCount){
                    int t = Random.Range(0, spawnPoints.Count);
                    Vector3 position = new Vector3(spawnPoints[t].transform.position.x, spawnPoints[t].transform.position.y, 0);
                    
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

        for (int i = 0; i < wave.EnemiesList.Count; i++){
            for (int j = 0; j < wave.enemiesWeightOrNumber[i]; j++){
                int t = Random.Range(0, spawnPoints.Count);
                Vector3 position = new Vector3(spawnPoints[t].transform.position.x, spawnPoints[t].transform.position.y, 0);
                
                Instantiate(wave.EnemiesList[i], 
                    new Vector3(position.x, position.y, 0.0f), 
                    Quaternion.identity);
                
                _gameManager.activeEnemies++;
                ++activeEnemyCount;
                
                yield return new WaitForSeconds(wave.enemySpawnInterval);
            }
        }
    }
    
    public IEnumerator SpawnWeightedRandomEnemy(WaveManagerScriptableObject.Wave wave){
        int activeEnemyCount = 0;
        bool controlVar = true;
        
        while (controlVar){
            if (activeEnemyCount < wave.waveEnemyCount){
                Vector3 position = spawnPoints[Random.Range(0, spawnPoints.Count)].transform.position;
                
                Instantiate(wave.EnemiesList[GetRandomWeightedIndex(wave.enemiesWeightOrNumber)], 
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
    
    //Could re-check this to implement a better way, this isn't bad but returning the last
    //index when the previous did not fulfill the generator feels "clunky"
    private int GetRandomWeightedIndex(System.Collections.Generic.List<int> weights)
    {
        // Get the total sum of all the weights.
        float weightSum = 0f;
        for (int i = 0; i < weights.Count; ++i){
            weightSum += weights[i];
        }
     
        // Step through all the possibilities, one by one, checking to see if each one is selected.
        int index = 0;
        int lastIndex = weights.Count - 1;
        while (index < lastIndex){
            // Do a probability check with a likelihood of weights[index] / weightSum.
            if (Random.Range(0, weightSum) < weights[index]){
                return index;
            }
     
            // Remove the last item from the sum of total untested weights and try again.
            weightSum -= weights[index++];
        }
     
        // No other item was selected, so return very last index.
        return index;
    }

}