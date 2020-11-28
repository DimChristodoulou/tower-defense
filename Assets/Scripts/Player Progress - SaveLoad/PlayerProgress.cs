using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Enemies;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Serialization;
using UnityEngine.UI;

[Serializable]
public class PlayerProgress{
    [SerializeField] public List<int> UnlockedUpgrades = new List<int>();
    [SerializeField] public List<int> UnlockedLevels = new List<int>();
    [SerializeField] public List<int> playerDropCounts = new List<int>();

    [SerializeField] public List<StringLiterals.singularItemDrops> playerDropKeys = new List<StringLiterals.singularItemDrops>();
    [SerializeField] public bool passedTutorial;

    public void saveProgress(){
        SaveUnlockedUpgradesInPlayerProgress();
        SavePlayerDropsInPlayerProgress();
        SaveUnlockedLevelsInPlayerProgress();
        passedTutorial = GlobalPlayerProgress.passedTutorial;
        string json = JsonUtility.ToJson(this);
        System.IO.File.WriteAllText(Application.persistentDataPath + "/unlocks.json", json);
        Debug.Log(json);
    }

    #region SaveHelperMethods
    
    /**
     * Saves unlocked levels from the GlobalPlayerProgress class into the PlayerProgress class
     * by checking the GlobalPlayerProgress.UnlockedLevels Dictionary
     */
    private void SaveUnlockedLevelsInPlayerProgress(){
        foreach (int levelId in GlobalPlayerProgress.UnlockedLevels){
            if (!UnlockedLevels.Contains(levelId)){
                UnlockedLevels.Add(levelId);
            }
        }
    }

    /**
     * Saves player drops from the GlobalPlayerProgress class into the PlayerProgress class
     */
    private void SavePlayerDropsInPlayerProgress(){
        foreach (var drop in GlobalPlayerProgress.playerDrops){
            if (!playerDropKeys.Contains(drop.Key)){
                playerDropKeys.Add(drop.Key);
                playerDropCounts.Add(drop.Value);
            }
            else{
                int index = playerDropKeys.IndexOf(drop.Key);
                playerDropCounts[index] = drop.Value;
            }
        }
    }

    /**
     * Saves unlocked upgrades from the GlobalPlayerProgress class into the PlayerProgress class
     */
    private void SaveUnlockedUpgradesInPlayerProgress(){
        foreach (var upgrade in GlobalPlayerProgress.UnlockedUpgrades){
            if (!UnlockedUpgrades.Contains(upgrade.id)){
                UnlockedUpgrades.Add(upgrade.id);
            }
        }
    }

    #endregion

    public PlayerProgress loadProgress(){
        if (File.Exists(Application.persistentDataPath + "/unlocks.json")){
            string json = File.ReadAllText(Application.persistentDataPath + "/unlocks.json");
            Debug.Log(json);
            PlayerProgress playerProgress = JsonUtility.FromJson<PlayerProgress>(json);
            Debug.Log(playerProgress.UnlockedUpgrades.Count);
            LoadPlayerDropsInGlobalProgress(playerProgress);
            LoadUnlockedLevelsInGlobalProgress(playerProgress);
            LoadUnlockedUpgradesInGlobalProgress(playerProgress);
            GlobalPlayerProgress.passedTutorial = playerProgress.passedTutorial;

            return playerProgress;
        }

        return createNewProgress();
    }

    #region LoadHelperMethods

    /**
     * Loads unlocked upgrades from the JSON extracted player progress file into the GlobalPlayerProgress class
     */
    private static void LoadUnlockedUpgradesInGlobalProgress(PlayerProgress playerProgress){
        foreach (int upgradeId in playerProgress.UnlockedUpgrades){
            Addressables.LoadAssetAsync<UpgradeScriptableObject>("Upgrade_" + upgradeId).Completed +=
                delegate(AsyncOperationHandle<UpgradeScriptableObject> handle) { GlobalPlayerProgress.UnlockedUpgrades.Add(handle.Result);};
        }
    }

    /**
     * Loads unlocked levels from the JSON extracted player progress file into the GlobalPlayerProgress class
     * by checking the GlobalPlayerProgress.UnlockedLevels Dictionary
     */
    private static void LoadUnlockedLevelsInGlobalProgress(PlayerProgress playerProgress){
        foreach (int levelId in playerProgress.UnlockedLevels){
            if (!GlobalPlayerProgress.UnlockedLevels.Contains(levelId)){
                GlobalPlayerProgress.UnlockedLevels.Add(levelId);
            }
        }
    }

    /**
     * Loads player drops from the JSON extracted player progress file into the GlobalPlayerProgress class
     */
    private static void LoadPlayerDropsInGlobalProgress(PlayerProgress playerProgress){
        for (int i = 0; i < playerProgress.playerDropKeys.Count; i++){
            if (GlobalPlayerProgress.playerDrops.ContainsKey(playerProgress.playerDropKeys[i])){
                GlobalPlayerProgress.playerDrops[playerProgress.playerDropKeys[i]] = playerProgress.playerDropCounts[i];
            }
            else{
                GlobalPlayerProgress.playerDrops.Add(playerProgress.playerDropKeys[i], playerProgress.playerDropCounts[i]);
            }
        }
    }

    #endregion

    private PlayerProgress createNewProgress(){
        return new PlayerProgress();
    }
}
