// Decompiled with JetBrains decompiler
// Type: WaveManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A28AF8C8-695A-49DE-887A-AA1AA02D690F
// Assembly location: E:\Tower_Defense_Builds\14-10-2020\Tower Defense_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WaveManager : MonoBehaviour{
    public int numberOfWaves;
    public List<WaveEnemies> enemiesPerWave;
    public TextMeshProUGUI waveNumberText;
    public GameObject incomingEnemiesPanel;
    private int _currentWave;

    private void Start(){
        _currentWave = 0;
        UpdateWaveText();
        StartCoroutine(SpawnWave());
    }

    private void Update(){
        if (_currentWave != numberOfWaves || gameObject.GetComponent<GameManager>().life <= 0 ||
            gameObject.GetComponent<EnemySpawn>().activeEnemyCount != 0)
            return;

        Debug.Log("You win!");
    }

    public void UpdateWaveText() => waveNumberText.text = "Wave:" + _currentWave + "/" + numberOfWaves;

    public void UpdateIncomingEnemiesPanel(){
        foreach (Component component in incomingEnemiesPanel.transform)
            Destroy(component.gameObject);

        foreach (GameObject enemies in enemiesPerWave[_currentWave].enemiesList){
            GameObject gameObject = new GameObject();
            gameObject.transform.SetParent(incomingEnemiesPanel.transform);
            gameObject.transform.position = incomingEnemiesPanel.transform.position;
            Image image = gameObject.AddComponent<Image>();
            image.sprite = enemies.GetComponent<SpriteRenderer>().sprite;
            image.transform.localScale = new Vector3(0.75f, 0.65f);
        }
    }

    public void SpawnWaveImmediately(){
        if (_currentWave >= numberOfWaves)
            return;

        UpdateIncomingEnemiesPanel();
        StartCoroutine(gameObject.GetComponent<EnemySpawn>().SpawnEnemy(enemiesPerWave[_currentWave]));
        ++_currentWave;
        UpdateWaveText();
    }

    private IEnumerator SpawnWave(){
        WaveManager waveManager = this;

        while (true){
            if (waveManager._currentWave < waveManager.numberOfWaves){
                waveManager.UpdateIncomingEnemiesPanel();
                waveManager.StartCoroutine(waveManager.gameObject.GetComponent<EnemySpawn>().SpawnEnemy(waveManager.enemiesPerWave[waveManager._currentWave]));
                ++waveManager._currentWave;
                waveManager.UpdateWaveText();
            }

            yield return new WaitForSeconds(20f);
        }
    }

    [Serializable]
    public struct WaveEnemies{
        public int waveEnemyCount;
        public List<GameObject> enemiesList;
    }
}