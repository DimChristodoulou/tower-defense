// Decompiled with JetBrains decompiler
// Type: GameManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A28AF8C8-695A-49DE-887A-AA1AA02D690F
// Assembly location: E:\Tower_Defense_Builds\14-10-2020\Tower Defense_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour{
    public List<TowerData> TowerInformation;
    public int gold, life = 10, _selectedTowerIndex = -1, activeEnemies = 0;
    private const int GOLD_PER_SECOND = 2;
    public TextMeshProUGUI lifeText, goldText;
    private Dictionary<EnemyScriptableObject, int> _killedEnemies = new Dictionary<EnemyScriptableObject, int>();
    private bool _isTowerSelected = false, _won = false;
    private PlayerProgress playerProgress;
    
    [SerializeField] private GameObject _tutorialPanelIncomingEnemies, _tutorialPanelBuildMenu, 
        _tutorialPanelEnemyDetails, _tutorialPanelStartEnemyWave, _killedEnemiesPanel, _lossMenu, _winMenu;

    private IList<UpgradeScriptableObject> allUpgrades = new List<UpgradeScriptableObject>();

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

    private void Awake(){
        _tutorialPanelBuildMenu.SetActive(false);
        _tutorialPanelIncomingEnemies.SetActive(false);
        _tutorialPanelEnemyDetails.SetActive(false);
        _tutorialPanelStartEnemyWave.SetActive(false);
        _killedEnemiesPanel.SetActive(false);
        _lossMenu.SetActive(false);
        _winMenu.SetActive(false);
        
        //Load all upgrades to have them available in order to check which are unlocked
        Addressables.LoadAssetsAsync<UpgradeScriptableObject>("Upgrades", null).Completed +=
            delegate(AsyncOperationHandle<IList<UpgradeScriptableObject>> handle) { allUpgrades = handle.Result; };
        
        activeEnemies = 0;
    }

    private void Start(){
        lifeText.text = life.ToString();

        //Load the player progress in the GlobalPlayerProgress class
        playerProgress = new PlayerProgress();
        playerProgress = playerProgress.loadProgress();

        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Level_0" && !GlobalPlayerProgress.passedTutorial){
            Time.timeScale = 0f;
            GlobalPlayerProgress.passedTutorial = true;
            StartCoroutine(playTutorial());
        }
        
        InvokeRepeating("AddGoldPerSecond", 0.0f, 2f);
        Time.timeScale = 1f;
    }

    private void Update(){
        //DEBUG: REMOVE LATER
        if (Input.GetKeyDown(KeyCode.Z)){
            LoseLife();
        }
        
        if (gameObject.GetComponent<WaveManager>().CurrentWave == gameObject.GetComponent<WaveManager>().waveManager.waves.Count 
            && gameObject.GetComponent<GameManager>().life > 0 && activeEnemies <= 0 && !_won){
            
            _won = true;
            _winMenu.SetActive(true);
            Time.timeScale = 0.0f;
            _killedEnemiesPanel.SetActive(true);
            updateKilledEnemiesPanel(_killedEnemiesPanel);

            // If player won, we need to update his wallet with the new drops and unlock the next level
            foreach (var killedEnemy in _killedEnemies){
                if (GlobalPlayerProgress.playerDrops.ContainsKey(killedEnemy.Key.drops)){
                    GlobalPlayerProgress.playerDrops[killedEnemy.Key.drops] += killedEnemy.Value;
                }
                else{
                    GlobalPlayerProgress.playerDrops.Add(killedEnemy.Key.drops, killedEnemy.Value);
                }
            }

            string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            int levelId = Int32.Parse(sceneName.Substring(sceneName.Length-1, 1));

            if(!GlobalPlayerProgress.UnlockedLevels.Contains(levelId)){
                GlobalPlayerProgress.UnlockedLevels.Add(levelId);
            }
            
            playerProgress.saveProgress();
        }
    }

    private void LateUpdate(){
        if (gameObject.GetComponent<Settings>().ButtonToRebind != null || Time.timeScale == 0.0)
            return;

        if (Input.GetKeyDown(gameObject.GetComponent<Settings>().keyBindings["buildArrowTowerBinding"]) && TowerInformation[0].cost < gold){
            SelectTower(TowerData.ARROW_TOWER);
        }
        else if (Input.GetKeyDown(gameObject.GetComponent<Settings>().keyBindings["buildArcaneTowerBinding"]) && TowerInformation[1].cost < gold){
            SelectTower(TowerData.ARCANE_TOWER);
        }
        else if (Input.GetKeyDown(gameObject.GetComponent<Settings>().keyBindings["buildSupportTowerBinding"]) && TowerInformation[2].cost < gold){
            SelectTower(TowerData.SUPPORT_TOWER);
        }
        else if (Input.GetKeyDown(gameObject.GetComponent<Settings>().keyBindings["buildEarthquakeTowerBinding"]) && TowerInformation[3].cost < gold){
            SelectTower(TowerData.EARTHQUAKE_TOWER);
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
        gold += GOLD_PER_SECOND;
        goldText.text = gold.ToString();
    }

    public void RemoveGold(int value){
        gold -= value;
        goldText.text = gold.ToString();
    }

    public void AddGold(int value){
        gold += value;
        goldText.text = gold.ToString();
    }

    public void LoseLife(){
        --life;
        lifeText.text = life.ToString();

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
                    dropText.GetComponent<TextMeshProUGUI>().text = Tools.ReturnIntAsSingularOrPlural(killedEnemy.Value, killedEnemy.Key.itemDrop);
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

    private void SelectTower(int index){
        _isTowerSelected = true;
        _selectedTowerIndex = index;
    }


    #region tutorial specific part
    
    public IEnumerator playTutorial(){
        yield return StartCoroutine(startTutorial());
        yield return StartCoroutine(startTutorialPart2());
        yield return StartCoroutine(startTutorialPart3());
        yield return StartCoroutine(startTutorialPart4());
    }
    
    public IEnumerator startTutorial(){
        Time.timeScale = 0;

        if (!_tutorialPanelBuildMenu.activeSelf){
            _tutorialPanelBuildMenu.SetActive(true);

            while (true){
                if (Input.GetMouseButtonDown(0)){
                    _tutorialPanelBuildMenu.SetActive(false);
                    yield break;
                }
                yield return null;
            }
        }
        
    }
    
    public IEnumerator startTutorialPart2(){
        Debug.Log("start");
        
        if (!_tutorialPanelIncomingEnemies.activeSelf){
            Debug.Log("start2");
            _tutorialPanelIncomingEnemies.SetActive(true);

            while (true){
                yield return null;
                
                if (Input.GetMouseButtonDown(0)){
                    Debug.Log("start3");
                    _tutorialPanelIncomingEnemies.SetActive(false);
                    yield break;
                }
            }
        }
    }
    
    public IEnumerator startTutorialPart3(){

        if (!_tutorialPanelEnemyDetails.activeSelf){
            _tutorialPanelEnemyDetails.SetActive(true);

            while (true){
                yield return null;
                
                if (Input.GetMouseButtonDown(0)){
                    _tutorialPanelEnemyDetails.SetActive(false);
                    yield break;
                }
            }
        }
    }
    
    public IEnumerator startTutorialPart4(){

        if (!_tutorialPanelStartEnemyWave.activeSelf){
            _tutorialPanelStartEnemyWave.SetActive(true);

            while (true){
                yield return null;
                
                if (Input.GetMouseButtonDown(0)){
                    _tutorialPanelStartEnemyWave.SetActive(false);
                    Time.timeScale = 1f;
                    yield break;
                }
            }
        }
    }
    
    #endregion
}