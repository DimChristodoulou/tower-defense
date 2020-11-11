using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Enemies;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

[Serializable]
public class PlayerProgress{

    [SerializeField] public List<int> UnlockedUpgrades = new List<int>();
    [SerializeField] public List<int> UnlockedLevels;
    
    [SerializeField] public List<StringLiterals.singularItemDrops> playerDropKeys = new List<StringLiterals.singularItemDrops>();
    [SerializeField] public List<int> playerDropCounts = new List<int>();

    public void saveProgress(){
        foreach (var upgrade in GlobalPlayerProgress.UnlockedUpgrades){
            UnlockedUpgrades.Add(upgrade.id);
        }

        foreach (var drop in GlobalPlayerProgress.playerDrops){
            if (playerDropKeys.Contains(drop.Key)){
                int index = playerDropKeys.IndexOf(drop.Key);
                playerDropCounts[index] += drop.Value;
            }
            else{
                playerDropKeys.Add(drop.Key);
                playerDropCounts.Add(drop.Value);
            }

        }
        
        string json = JsonUtility.ToJson(this);
        System.IO.File.WriteAllText(Application.persistentDataPath + "/unlocks.json", json);
        Debug.Log(json);
        Debug.Log(Application.persistentDataPath);
    }
    
    public PlayerProgress loadProgress(){
        if (File.Exists(Application.persistentDataPath + "/unlocks.json"))
        {
            string json = File.ReadAllText(Application.persistentDataPath + "/unlocks.json");
            Debug.Log(json);
            PlayerProgress playerProgress = JsonUtility.FromJson<PlayerProgress>(json);

            for (int i = 0; i < playerProgress.playerDropKeys.Count; i++){
                if (GlobalPlayerProgress.playerDrops.ContainsKey(playerProgress.playerDropKeys[i])){
                    GlobalPlayerProgress.playerDrops[playerProgress.playerDropKeys[i]] = playerProgress.playerDropCounts[i];
                }
                else{
                    GlobalPlayerProgress.playerDrops.Add(playerProgress.playerDropKeys[i], playerProgress.playerDropCounts[i]);
                }
            }
            
            return playerProgress;
        }

        return createNewProgress();
    }

    private PlayerProgress createNewProgress(){
        return new PlayerProgress();
    }
}
