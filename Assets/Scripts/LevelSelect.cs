using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct PossibleEnemiesInLevel{
    public List<GameObject> possibleEnemies;
}

public class LevelSelect : MonoBehaviour{

    public List<PossibleEnemiesInLevel> possibleEnemiesPerLevel;
    public List<string> levelDescriptions = new List<string>(new string[]{"Testing Description"});
    public List<string> levelNames = new List<string>(new string[]{"Testing ame"});
    
    [SerializeField] private GameObject _levelPanel;
    [SerializeField] private GameObject _levelPanelLevelPreviewField;
    [SerializeField] private GameObject _levelPanelLevelDescriptionField;
    [SerializeField] private GameObject _levelPanelLevelNameField;
    [SerializeField] private GameObject _levelPanelLevelPossibleEnemiesPanel;
    [SerializeField] private GameObject _upgradesPanel;

    [SerializeField] private Sprite _unlockedLevelBtnSprite;

    private int areaIndex, levelIndex;
    
    // Start is called before the first frame update
    void Start(){
        _upgradesPanel = GameObject.Find("GlobalUpgrades");
        
        //Load all upgrades into the Global Player Progress class
        PlayerProgress playerProgress = new PlayerProgress();
        playerProgress = playerProgress.loadProgress();
        
        GlobalPlayerProgress.UpdateUpgradePanel(playerProgress);
        _upgradesPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void showPanelForLevel(string levelId){
        areaIndex = Int32.Parse(levelId.Split('_')[0]);
        levelIndex = Int32.Parse(levelId.Split('_')[1]);
        
        _levelPanel.SetActive(true);
        _levelPanelLevelPreviewField.GetComponent<Image>().sprite = Resources.Load<Sprite>("LevelMaps/Level_" + levelId);
        _levelPanelLevelNameField.GetComponent<Text>().text = levelNames[levelIndex];
        _levelPanelLevelDescriptionField.GetComponent<TextMeshProUGUI>().text = levelDescriptions[levelIndex];
        _updatePossibleEnemiesPanel(_levelPanelLevelPossibleEnemiesPanel);
        
        _levelPanel.GetComponentInChildren<Button>().onClick.AddListener((() => selectLevel(levelId)));
    }

    public void closeLevelPanel(){
        _levelPanel.SetActive(false);
    }

    private void _updatePossibleEnemiesPanel(GameObject possibleEnemiesPanel){
        foreach (Component component in possibleEnemiesPanel.transform)
            Destroy(component.gameObject);

        foreach (GameObject enemies in possibleEnemiesPerLevel[levelIndex].possibleEnemies){
            GameObject gameObject = new GameObject();
            gameObject.transform.SetParent(possibleEnemiesPanel.transform);
            gameObject.transform.position = possibleEnemiesPanel.transform.position;
            Image image = gameObject.AddComponent<Image>();
            image.sprite = enemies.GetComponent<SpriteRenderer>().sprite;
            image.transform.localScale = new Vector3(0.75f, 0.65f);
        }
    }

    public void selectLevel(string levelId){
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
