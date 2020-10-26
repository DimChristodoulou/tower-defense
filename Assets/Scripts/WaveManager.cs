// Decompiled with JetBrains decompiler
// Type: WaveManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A28AF8C8-695A-49DE-887A-AA1AA02D690F
// Assembly location: E:\Tower_Defense_Builds\14-10-2020\Tower Defense_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class WaveManager : MonoBehaviour{
    
    [Serializable]
    public struct WaveEnemies{
        public int waveEnemyCount;
        [AssetList(Path = "/Enemies")] public List<GameObject> enemiesList;
    }
    
    [Title("Endless Level Configuration")]
    public bool isEndless;
    [ShowIf("isEndless")] public List<WaveEnemies> endlessPossibleEnemies;
    
    [FormerlySerializedAs("enemiesPerWave")]
    [Title("Non-Endless Level Configuration")]
    [HideIf("isEndless")] public List<WaveEnemies> waves;
    
    [Title("Generic Wave Configuration")]
    public TextMeshProUGUI waveNumberText;
    public GameObject incomingEnemiesPanel;
    
    private int _currentWave;

    private void Start(){
        _currentWave = 0;
        UpdateWaveText();
        StartCoroutine(SpawnWave());
    }

    private void Update(){
        if (_currentWave != waves.Count || gameObject.GetComponent<GameManager>().life <= 0 ||
            gameObject.GetComponent<EnemySpawn>().activeEnemyCount != 0)
            return;

        Debug.Log("You win!");
    }

    public void UpdateWaveText(){
        if (isEndless){
            waveNumberText.text = "Wave:" + _currentWave;
        }
        else{
            waveNumberText.text = "Wave:" + _currentWave + "/" + waves.Count;
        }
    }

    public void UpdateIncomingEnemiesPanel(){
        foreach (Component component in incomingEnemiesPanel.transform){
            Destroy(component.gameObject);
        }

        foreach (GameObject enemies in waves[_currentWave].enemiesList){
            GameObject gameObject = new GameObject();
            gameObject.transform.SetParent(incomingEnemiesPanel.transform);
            gameObject.transform.position = incomingEnemiesPanel.transform.position;
            Image image = gameObject.AddComponent<Image>();
            image.sprite = enemies.GetComponent<SpriteRenderer>().sprite;
            image.transform.localScale = new Vector3(0.75f, 0.65f);
        }
    }

    public void SpawnWaveImmediately(){
        if (isEndless){
            StartSpawningEnemies(endlessPossibleEnemies[0]);
        }
        else{
            if (_currentWave < waves.Count){
                StartSpawningEnemies(waves[_currentWave]);
            }
        }
    }

    private IEnumerator SpawnWave(){
        while (true){
            if (isEndless){
                StartSpawningEnemies(endlessPossibleEnemies[0]);
            }
            else{
                if (_currentWave < waves.Count){
                    StartSpawningEnemies(waves[_currentWave]);
                }
            }

            yield return new WaitForSeconds(20f);
        }
    }

    private void StartSpawningEnemies(WaveEnemies possibleEnemies){
        if (!isEndless){
            // We dont have to update the incoming enemies panel when the wave is endless
            UpdateIncomingEnemiesPanel();
        }

        StartCoroutine(gameObject.GetComponent<EnemySpawn>().SpawnEnemy(possibleEnemies, isEndless));
        ++_currentWave;
        UpdateWaveText();
    }
}