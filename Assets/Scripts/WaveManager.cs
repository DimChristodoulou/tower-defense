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
    private TextMeshProUGUI _nextWaveTimerText;
    
    private int _currentWave;
    private float _currentWaveTimer;
    Coroutine _lastRoutine = null;

    public int CurrentWave
    {
        get => _currentWave;
        set => _currentWave = value;
    }

    private void Start(){
        _nextWaveTimerText = GameObject.Find("NextWaveTimer").GetComponent<TextMeshProUGUI>();
        _currentWave = 0;
        UpdateWaveText();
        _lastRoutine = StartCoroutine(SpawnWave());
    }

    private void Update(){
        if (!waveManager.isEndless && _currentWaveTimer > 0){
            _currentWaveTimer -= Time.deltaTime;
            _nextWaveTimerText.text = StringLiterals.WaveCountdownText + Tools.convertNumberToTime(_currentWaveTimer);
        }
    }

    /*
     * Updates the wave text in the WaveNumber game object
     */
    public void UpdateWaveText(){
        if (waveManager.isEndless){
            waveNumberText.text = "Wave:" + _currentWave;
        }
        else{
            waveNumberText.text = "Wave:" + _currentWave + "/" + waveManager.waves.Count;
        }
    }

    /**
     * Updates the Gameobject for the incoming enemies - adds on click events on the enemy sprites to also display the enemy information
     */
    public void UpdateIncomingEnemiesPanel(){
        foreach (Component component in incomingEnemiesPanel.transform){
            Destroy(component.gameObject);
        }

        foreach (GameObject enemy in waveManager.waves[_currentWave].EnemiesList){
            GameObject container = Instantiate(Resources.Load<GameObject>("EnemyInfoContainer"));
            container.transform.SetParent(incomingEnemiesPanel.transform);
            container.transform.position = incomingEnemiesPanel.transform.position;
            
            Image image = container.GetComponentInChildren<Image>();
            image.sprite = enemy.GetComponent<SpriteRenderer>().sprite;
            //image.transform.localScale = new Vector3(0.5f, 0.5f);
            container.AddComponent<EnemyInformationPanel>();
            container.GetComponent<EnemyInformationPanel>().enemy = enemy.GetComponent<Enemy>();

            if (waveManager.waves[_currentWave].waveSpawnType == WaveManagerScriptableObject.WaveSpawnType.Specific){
                container.GetComponentInChildren<TextMeshProUGUI>().text = waveManager.waves[_currentWave]
                    .enemiesWeightOrNumber[waveManager.waves[_currentWave].EnemiesList.IndexOf(enemy)].ToString();
                //container.GetComponentInChildren<TextMeshProUGUI>().fontSize = 30;
                container.GetComponentInChildren<TextMeshProUGUI>().alignment = TextAlignmentOptions.Center;
            }
            else{
                container.GetComponentInChildren<TextMeshProUGUI>().enabled = false;
            }
        }
    }

    /**
     * Invoked when player clicks the spawn next wave immediately button
     */
    public void SpawnWaveImmediately(){
        if (waveManager.isEndless){
            StartSpawningEnemies(waveManager.endlessPossibleEnemies[0]);
        }
        else{
            if (_currentWave < waveManager.waves.Count){
                UpdateNextWaveTimerText(_currentWave);
                StopCoroutine(_lastRoutine);
                StartSpawningEnemies(waveManager.waves[_currentWave]);
                _lastRoutine = StartCoroutine(SpawnWave());
                UpdateIncomingEnemiesPanel();
            }
        }
    }

    private IEnumerator SpawnWave(){
        int internalCurrentWave;
        
        while (true){
            internalCurrentWave = _currentWave;

            if (!waveManager.isEndless){
                // We dont have to update the incoming enemies panel when the wave is endless
                UpdateIncomingEnemiesPanel();
            }
            
            UpdateNextWaveTimerText(internalCurrentWave);
            
            yield return new WaitForSeconds(waveManager.waves[internalCurrentWave].waveSpawn);
            if (waveManager.isEndless){
                StartSpawningEnemies(waveManager.endlessPossibleEnemies[0]);
            }
            else{
                if (_currentWave < waveManager.waves.Count){
                    Debug.Log("Start Spawn of wave "+_currentWave);
                    StartSpawningEnemies(waveManager.waves[internalCurrentWave]);
                }
            }
            
            //yield return new WaitForSeconds(waveManager.waves[internalCurrentWave].waveSpawn);
        }
    }

    private void UpdateNextWaveTimerText(int currentWave){
        _currentWaveTimer = waveManager.waves[currentWave].waveSpawn;
    }

    private void StartSpawningEnemies(WaveManagerScriptableObject.Wave currentWave){
        switch (currentWave.waveSpawnType){
            case WaveManagerScriptableObject.WaveSpawnType.Randomly:
                StartCoroutine(gameObject.GetComponent<EnemySpawn>().SpawnRandomEnemy(currentWave, waveManager.isEndless));
                break;
            case WaveManagerScriptableObject.WaveSpawnType.WeightedRandom:
                StartCoroutine(gameObject.GetComponent<EnemySpawn>().SpawnWeightedRandomEnemy(currentWave));
                break;
            case WaveManagerScriptableObject.WaveSpawnType.Specific:
                StartCoroutine(gameObject.GetComponent<EnemySpawn>().SpawnSpecificEnemies(currentWave));
                break;
        }
        
        ++_currentWave;
        UpdateWaveText();
    }
}