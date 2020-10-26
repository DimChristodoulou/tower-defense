// Decompiled with JetBrains decompiler
// Type: GameManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A28AF8C8-695A-49DE-887A-AA1AA02D690F
// Assembly location: E:\Tower_Defense_Builds\14-10-2020\Tower Defense_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using Enemies;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour{
    public List<TowerData> TowerInformation;
    public int gold;
    private const int GOLD_PER_SECOND = 2;
    public int life = 10;
    public TextMeshProUGUI lifeText, goldText;
    private GameObject _lossMenu;
    private GameObject _killedEnemiesPanel;
    private Dictionary<EnemyScriptableObject, int> _killedEnemies = new Dictionary<EnemyScriptableObject, int>();
    private int _selectedTowerIndex = -1;
    private bool _isTowerSelected = false;

    public Dictionary<EnemyScriptableObject, int> KilledEnemies
    {
        get => _killedEnemies;
        set => _killedEnemies = value;
    }

    public int SelectedTowerIndex
    {
        get => _selectedTowerIndex;
        set => _selectedTowerIndex = value;
    }

    public bool IsTowerSelected
    {
        get => _isTowerSelected;
        set => _isTowerSelected = value;
    }

    private void Start(){
        _lossMenu = GameObject.Find("LossMenu");
        _killedEnemiesPanel = GameObject.Find("KilledEnemiesPanel");
        _lossMenu.SetActive(false);
        lifeText.text = "Lives: " + life;
        InvokeRepeating("AddGoldPerSecond", 0.0f, 2f);
    }

    private void Update(){
        if (!Input.GetKeyDown(KeyCode.Z))
            return;

        LoseLife();
    }

    private void LateUpdate(){
        if (gameObject.GetComponent<Settings>().ButtonToRebind != null || Time.timeScale == 0.0)
            return;

        if (Input.GetKeyDown(gameObject.GetComponent<Settings>().keyBindings["buildArrowTowerBinding"]) && TowerInformation[0].cost < gold){
            _isTowerSelected = true;
            _selectedTowerIndex = 0;
        }
        else if (Input.GetKeyDown(gameObject.GetComponent<Settings>().keyBindings["buildArcaneTowerBinding"]) && TowerInformation[1].cost < gold){
            _isTowerSelected = true;
            _selectedTowerIndex = 1;
        }
        else if (Input.GetKeyDown(gameObject.GetComponent<Settings>().keyBindings["buildSupportTowerBinding"]) && TowerInformation[2].cost < gold){
            _isTowerSelected = true;
            _selectedTowerIndex = 2;
        }
        else if (Input.GetKeyDown(gameObject.GetComponent<Settings>().keyBindings["buildEarthquakeTowerBinding"]) && TowerInformation[3].cost < gold){
            _isTowerSelected = true;
            _selectedTowerIndex = 3;
        }
        else if (Input.GetKeyDown(gameObject.GetComponent<Settings>().keyBindings["speedx1"])){
            Time.timeScale = 1f;
        }
        else if (Input.GetKeyDown(gameObject.GetComponent<Settings>().keyBindings["speedx2"])){
            Time.timeScale = 2f;
        }
        else if (Input.GetKeyDown(gameObject.GetComponent<Settings>().keyBindings["speedx3"])){
            Time.timeScale = 3f;
        }
    }

    public void AddGoldPerSecond(){
        gold += 2;
        goldText.text = "GOLD: " + gold;
    }

    public void RemoveGold(int value){
        gold -= value;
        goldText.text = "GOLD: " + gold;
    }

    public void AddGold(int value){
        gold += value;
        goldText.text = "GOLD: " + gold;
    }

    public void LoseLife(){
        --life;
        lifeText.text = "Lives: " + life;
        if (life != 0)
            return;

        Time.timeScale = 0.0f;
        _lossMenu.SetActive(true);
        _killedEnemiesPanel.SetActive(true);

        foreach (KeyValuePair<EnemyScriptableObject, int> killedEnemy in _killedEnemies){
            GameObject gameObject = new GameObject();
            gameObject.transform.SetParent(_killedEnemiesPanel.transform);
            TextMeshProUGUI textMeshProUgui = gameObject.AddComponent<TextMeshProUGUI>();
            textMeshProUgui.fontSize = 14f;
            int num;

            if (killedEnemy.Value == 1){
                TextMeshProUGUI component = textMeshProUgui.GetComponent<TextMeshProUGUI>();
                num = killedEnemy.Value;
                string str = num + " " + killedEnemy.Key.itemDrop;
                component.text = str;
            }
            else if (killedEnemy.Value > 1){
                TextMeshProUGUI component = textMeshProUgui.GetComponent<TextMeshProUGUI>();
                num = killedEnemy.Value;
                string str = num + " " + killedEnemy.Key.itemDrop + "s";
                component.text = str;
            }
        }
    }
}