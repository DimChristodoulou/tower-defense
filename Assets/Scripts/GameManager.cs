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
    private GameObject _lossMenu, _winMenu;
    private GameObject _killedEnemiesPanel;
    private Dictionary<EnemyScriptableObject, int> _killedEnemies = new Dictionary<EnemyScriptableObject, int>();
    private int _selectedTowerIndex = -1;
    private bool _isTowerSelected = false;
    private bool _won = false;
    private PlayerProgress playerProgress;

    public int activeEnemies = 0;

    public List<UpgradeScriptableObject> allUpgrades;

    /*
     * Access Methods
     */
    #region access methods
    
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
    #endregion
    /*
     * End Access Methods
     */
    
    private void Start(){
        Time.timeScale = 1f;
        
        _lossMenu = GameObject.Find("LossMenu");
        _winMenu = GameObject.Find("WinMenu");
        _killedEnemiesPanel = GameObject.Find("KilledEnemiesPanel");
        _killedEnemiesPanel.SetActive(false);
        _lossMenu.SetActive(false);
        _winMenu.SetActive(false);
        lifeText.text = "Lives: " + life;
        InvokeRepeating("AddGoldPerSecond", 0.0f, 2f);
        
        //Load all upgrades into the Global Player Progress class
        playerProgress = new PlayerProgress();
        playerProgress = playerProgress.loadProgress();
        
        foreach (UpgradeScriptableObject upgrade in allUpgrades){
            
            foreach (int unlockedUpgrade in playerProgress.UnlockedUpgrades){
                if (upgrade.id == unlockedUpgrade){
                    GlobalPlayerProgress.UnlockedUpgrades.Add(upgrade);
                }
            }
        }
    }

    private void Update(){
        //DEBUG: REMOVE LATER
        if (Input.GetKeyDown(KeyCode.Z)){
            LoseLife();
        }
        
        if (gameObject.GetComponent<WaveManager>().CurrentWave == gameObject.GetComponent<WaveManager>().waveManager.waves.Count 
            && gameObject.GetComponent<GameManager>().life > 0 
            && activeEnemies == 0 && !_won){
            
            _won = true;
            _winMenu.SetActive(true);
            Time.timeScale = 0.0f;
            _killedEnemiesPanel.SetActive(true);
            updateKilledEnemiesPanel(_killedEnemiesPanel);

            foreach (var drops in GlobalPlayerProgress.playerDrops){
                Debug.Log(drops.Key);
                Debug.Log(drops.Value);
            }
            
            // If player won, we need to update his wallet with the new drops and unlock the next level
            foreach (var killedEnemy in _killedEnemies){
                Debug.Log(killedEnemy.Key.drops);
                if (GlobalPlayerProgress.playerDrops.ContainsKey(killedEnemy.Key.drops)){
                    GlobalPlayerProgress.playerDrops[killedEnemy.Key.drops] += killedEnemy.Value;
                }
                else{
                    GlobalPlayerProgress.playerDrops.Add(killedEnemy.Key.drops, killedEnemy.Value);
                }
            }
            playerProgress.saveProgress();
        }
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

        if (life == 0){
            Time.timeScale = 0.0f;
            _lossMenu.SetActive(true);
            _killedEnemiesPanel.SetActive(true);

            updateKilledEnemiesPanel(_killedEnemiesPanel);
        }
    }

    private void updateKilledEnemiesPanel(GameObject _killedEnemiesPanel){
        List<GameObject> dropTexts = new List<GameObject>();

        foreach (var drop in StringLiterals.drops){
            dropTexts.Add(GameObject.Find(drop + " Text"));
            dropTexts[dropTexts.Count-1].SetActive(false);
        }
        
        foreach (KeyValuePair<EnemyScriptableObject, int> killedEnemy in _killedEnemies){
            foreach (GameObject dropText in dropTexts){
                if (dropText.name == killedEnemy.Key.itemDrop + " Text"){
                    dropText.SetActive(true);
                    int num;
            
                    if (killedEnemy.Value == 1){
                        num = killedEnemy.Value;
                        string str = num + " " + killedEnemy.Key.itemDrop;
                        dropText.GetComponent<TextMeshProUGUI>().text = str;
                    }
                    else if (killedEnemy.Value > 1){
                        num = killedEnemy.Value;
                        string str = num + " " + killedEnemy.Key.itemDrop + "s";
                        dropText.GetComponent<TextMeshProUGUI>().text = str;
                    }
                }
            }
        }
    }

    public void GoToLevelSelect(){
        UnityEngine.SceneManagement.SceneManager.LoadScene("LevelSelect");
    }
    
    public void QuitGame(){
        Application.Quit();
    }

    public void RestartLevel(){
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}