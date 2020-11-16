using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour{
    
    public List<string> levelDescriptions = new List<string>(new string[]{"Testing Description"});
    public List<string> levelNames = new List<string>(new string[]{"Testing ame"});
    
    [SerializeField] private GameObject _levelPanel;
    [SerializeField] private GameObject _levelPanelLevelPreviewField;
    [SerializeField] private GameObject _levelPanelLevelDescriptionField;
    [SerializeField] private GameObject _levelPanelLevelNameField;
    [SerializeField] private GameObject _levelPanelLevelPossibleEnemiesPanel;
    [SerializeField] private GameObject _upgradesPanel;
    [SerializeField] private GameObject _upgradesPanelPlayerWallet;

    [SerializeField] private Sprite _unlockedLevelBtnSprite;

    private int _areaIndex, _levelIndex;
    private List<int> unlockedLevels;
    
    // Start is called before the first frame update
    void Start(){
        _upgradesPanel = GameObject.Find("GlobalUpgrades");
        _upgradesPanelPlayerWallet = GameObject.Find("PlayerWallet");
        
        //Load all upgrades into the Global Player Progress class
        PlayerProgress playerProgress = new PlayerProgress();
        playerProgress = playerProgress.loadProgress();
        Debug.Log("Finished loading");
        GlobalPlayerProgress.UpdateUpgradePanel(playerProgress);
        GlobalPlayerProgress.UpdatePlayerWallet(playerProgress, _upgradesPanelPlayerWallet);
        _upgradesPanel.SetActive(false);

        unlockedLevels = GlobalPlayerProgress.GetUnlockedLevels(playerProgress);

        if (unlockedLevels.Count == 0){
            unlockedLevels.Add(0);
        }

        foreach (int levelId in unlockedLevels){
            GameObject levelBtn = GameObject.Find("Level_" + levelId + "_Btn");
            levelBtn.GetComponent<Button>().onClick.AddListener(() => showPanelForLevel(levelId));

            Addressables.LoadAssetAsync<Sprite>("Assets/Emerald Treasure/images/ui_seekbar_tick.png").Completed +=
                delegate(AsyncOperationHandle<Sprite> handle) { levelBtn.GetComponent<Image>().sprite = handle.Result; };
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void showPanelForLevel(int levelId){

        _levelPanel.SetActive(true);
        _levelPanelLevelPreviewField.GetComponent<Image>().sprite = Resources.Load<Sprite>("LevelMaps/Level_" + levelId);
        _levelPanelLevelNameField.GetComponent<Text>().text = levelNames[levelId];
        _levelPanelLevelDescriptionField.GetComponent<TextMeshProUGUI>().text = levelDescriptions[levelId];
        _updatePossibleEnemiesPanel(_levelPanelLevelPossibleEnemiesPanel, levelId);
        
        _levelPanel.GetComponentInChildren<Button>().onClick.AddListener((() => selectLevel(levelId)));
    }

    public void closeLevelPanel(){
        _levelPanel.SetActive(false);
    }

    private void _updatePossibleEnemiesPanel(GameObject possibleEnemiesPanel, int levelId){
        foreach (Component component in possibleEnemiesPanel.transform)
            Destroy(component.gameObject);

        Addressables.LoadAssetAsync<WaveManagerScriptableObject>("Assets/Wave Managers/Level_" + levelId + " Wave Manager.asset").Completed +=
            delegate(AsyncOperationHandle<WaveManagerScriptableObject> handle)
        {
            List<GameObject> allWaveEnemies = new List<GameObject>();
            foreach (WaveManagerScriptableObject.Wave wave in handle.Result.waves){
                foreach (GameObject enemy in wave.EnemiesList){
                    if (!allWaveEnemies.Contains(enemy)){
                        allWaveEnemies.Add(enemy);
                    }
                }
            }

            foreach (var enemy in allWaveEnemies){
                GameObject gameObject = new GameObject();
                gameObject.transform.SetParent(possibleEnemiesPanel.transform);
                gameObject.transform.position = possibleEnemiesPanel.transform.position;
                Image image = gameObject.AddComponent<Image>();
                image.sprite = enemy.GetComponent<SpriteRenderer>().sprite;
                image.transform.localScale = new Vector3(0.75f, 0.65f);
            }
        };
    }

    public void selectLevel(int levelId){
        //Scene manager load
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level_"+levelId);
    }

    public void toggleUpgradePanel(){
        if (_upgradesPanel.activeSelf){
            _upgradesPanel.SetActive(false);
        }
        else{
            _upgradesPanel.SetActive(true);
        }
    }
}
