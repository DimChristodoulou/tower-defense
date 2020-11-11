// Decompiled with JetBrains decompiler
// Type: WaveManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A28AF8C8-695A-49DE-887A-AA1AA02D690F
// Assembly location: E:\Tower_Defense_Builds\14-10-2020\Tower Defense_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class WaveManager : MonoBehaviour{

    public WaveManagerScriptableObject waveManager;
    
    [Title("Generic Wave Configuration")]
    public TextMeshProUGUI waveNumberText;
    public GameObject incomingEnemiesPanel;
    
    private int _currentWave;

    public int CurrentWave
    {
        get => _currentWave;
        set => _currentWave = value;
    }

    private void Start(){
        _currentWave = 0;
        UpdateWaveText();
        StartCoroutine(SpawnWave());
    }

    private void Update(){
        
    }

    public void UpdateWaveText(){
        if (waveManager.isEndless){
            waveNumberText.text = "Wave:" + _currentWave;
        }
        else{
            waveNumberText.text = "Wave:" + _currentWave + "/" + waveManager.waves.Count;
        }
    }

    public void UpdateIncomingEnemiesPanel(){
        foreach (Component component in incomingEnemiesPanel.transform){
            Destroy(component.gameObject);
        }

        foreach (GameObject enemies in waveManager.waves[_currentWave].EnemiesList){
            GameObject gameObject = new GameObject();
            gameObject.transform.SetParent(incomingEnemiesPanel.transform);
            gameObject.transform.position = incomingEnemiesPanel.transform.position;
            Image image = gameObject.AddComponent<Image>();
            image.sprite = enemies.GetComponent<SpriteRenderer>().sprite;
            image.transform.localScale = new Vector3(0.75f, 0.65f);
        }
    }

    public void SpawnWaveImmediately(){
        if (waveManager.isEndless){
            StartSpawningEnemies(waveManager.endlessPossibleEnemies[0]);
        }
        else{
            if (_currentWave < waveManager.waves.Count){
                StartSpawningEnemies(waveManager.waves[_currentWave]);
            }
        }
    }

    private IEnumerator SpawnWave(){
        while (true){
            int internalCurrentWave = _currentWave;
            if (waveManager.isEndless){
                StartSpawningEnemies(waveManager.endlessPossibleEnemies[0]);
            }
            else{
                if (_currentWave < waveManager.waves.Count){
                    Debug.Log("Start Spawn of wave "+_currentWave);
                    StartSpawningEnemies(waveManager.waves[internalCurrentWave]);
                }
            }
            
            yield return new WaitForSeconds(waveManager.waves[internalCurrentWave].waveSpawn);
        }
    }

    private void StartSpawningEnemies(WaveManagerScriptableObject.Wave currentWave){
        if (!waveManager.isEndless){
            // We dont have to update the incoming enemies panel when the wave is endless
            UpdateIncomingEnemiesPanel();
        }
        
        switch (currentWave.waveSpawnType){
            case WaveManagerScriptableObject.WaveSpawnType.Randomly:
                StartCoroutine(gameObject.GetComponent<EnemySpawn>().SpawnRandomEnemy(currentWave, waveManager.isEndless));
                break;
            case WaveManagerScriptableObject.WaveSpawnType.WeightedRandom:
                break;
            case WaveManagerScriptableObject.WaveSpawnType.Specific:
                StartCoroutine(gameObject.GetComponent<EnemySpawn>().SpawnSpecificEnemies(currentWave));
                break;
        }
        
        ++_currentWave;
        UpdateWaveText();
    }
}