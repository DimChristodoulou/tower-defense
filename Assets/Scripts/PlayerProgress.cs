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
    
    [SerializeField] public List<StringLiterals.singularItemDrops> playerDropKeys = new List<StringLiterals.singularItemDrops>();
    [SerializeField] public List<int> playerDropCounts = new List<int>();

    [SerializeField] public bool passedTutorial;

    public void saveProgress(){
        foreach (var upgrade in GlobalPlayerProgress.UnlockedUpgrades){
            if (!UnlockedUpgrades.Contains(upgrade.id)){
                UnlockedUpgrades.Add(upgrade.id);
            }
        }

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

        foreach (int levelId in GlobalPlayerProgress.UnlockedLevels){
            if (!UnlockedLevels.Contains(levelId)){
                UnlockedLevels.Add(levelId);
            }
        }
        
        string json = JsonUtility.ToJson(this);
        System.IO.File.WriteAllText(Application.persistentDataPath + "/unlocks.json", json);
        Debug.Log(json);
    }
    
    public PlayerProgress loadProgress(){
        if (File.Exists(Application.persistentDataPath + "/unlocks.json"))
        {
            string json = File.ReadAllText(Application.persistentDataPath + "/unlocks.json");
            PlayerProgress playerProgress = JsonUtility.FromJson<PlayerProgress>(json);

            for (int i = 0; i < playerProgress.playerDropKeys.Count; i++){
                if (GlobalPlayerProgress.playerDrops.ContainsKey(playerProgress.playerDropKeys[i])){
                    GlobalPlayerProgress.playerDrops[playerProgress.playerDropKeys[i]] = playerProgress.playerDropCounts[i];
                }
                else{
                    GlobalPlayerProgress.playerDrops.Add(playerProgress.playerDropKeys[i], playerProgress.playerDropCounts[i]);
                }
            }

            foreach (int levelId in playerProgress.UnlockedLevels){
                if (!GlobalPlayerProgress.UnlockedLevels.Contains(levelId)){
                    GlobalPlayerProgress.UnlockedLevels.Add(levelId);
                }
            }

            foreach (int upgradeId in playerProgress.UnlockedUpgrades){
                Addressables.LoadAssetAsync<UpgradeScriptableObject>("Upgrade_"+upgradeId).Completed +=
                    delegate(AsyncOperationHandle<UpgradeScriptableObject> handle)
                    {
                        GlobalPlayerProgress.UnlockedUpgrades.Add(handle.Result);
                    };
            }
            
            return playerProgress;
        }

        return createNewProgress();
    }

    private PlayerProgress createNewProgress(){
        return new PlayerProgress();
    }
}
