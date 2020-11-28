using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using Sirenix.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public static class GlobalPlayerProgress{
    public static Dictionary<int, List<int>> unlockLevels = new Dictionary<int, List<int>>(){
        {0, new List<int>(){1}},
        {1, new List<int>(){2}},
        {2, new List<int>(){3,4}},
        {3, new List<int>(){5}},
    };
    
    public static List<UpgradeScriptableObject> UnlockedUpgrades = new List<UpgradeScriptableObject>();
    public static List<int> UnlockedLevels = new List<int>();
    public static Dictionary<StringLiterals.singularItemDrops, int> playerDrops = new Dictionary<StringLiterals.singularItemDrops, int>();
    public static bool passedTutorial;
    
    public static List<UpgradeScriptableObject> getUnlockedUpgradesForSpecificTower(TowerData tower){
        List<UpgradeScriptableObject> applicableUpgrades = new List<UpgradeScriptableObject>();

        foreach (var unlockedUpgrade in UnlockedUpgrades){
            Debug.Log("unlocked " + unlockedUpgrade.id);

            if (unlockedUpgrade.specificTowerOrEnemy == UpgradeScriptableObject.UpgradeSelection.SpecificTowers &&
                unlockedUpgrade.affectedTowers.Contains(tower)){
                applicableUpgrades.Add(unlockedUpgrade);
            }
        }

        return applicableUpgrades;
    }

    public static List<UpgradeScriptableObject> getUnlockedUpgradesForSpecificEnemy(EnemyScriptableObject enemy){
        List<UpgradeScriptableObject> applicableUpgrades = new List<UpgradeScriptableObject>();

        foreach (var unlockedUpgrade in UnlockedUpgrades){
            if (unlockedUpgrade.specificTowerOrEnemy == UpgradeScriptableObject.UpgradeSelection.SpecificEnemies &&
                unlockedUpgrade.affectedEnemies.Contains(enemy)){
                applicableUpgrades.Add(unlockedUpgrade);
            }
        }

        return applicableUpgrades;
    }

    public static float CalculateValueAfterUpgrade(float value, UpgradeScriptableObject.StatEffect statEffect){
        switch (statEffect.valueType){
            case UpgradeScriptableObject.NumericalValue.Percentage:
                value += value * statEffect.upgradeValue;
                return value;
            case UpgradeScriptableObject.NumericalValue.FlatValue:
                return value + statEffect.upgradeValue;
            default:
                return value;
        }
    }

    public static int CalculateValueAfterUpgrade(int value, UpgradeScriptableObject.StatEffect statEffect){
        switch (statEffect.valueType){
            case UpgradeScriptableObject.NumericalValue.Percentage:
                value += (int) (value * statEffect.upgradeValue);
                return value;
            case UpgradeScriptableObject.NumericalValue.FlatValue:
                return (int) (value + statEffect.upgradeValue);
            default:
                return value;
        }
    }

    public static void UpdateUpgradePanel(PlayerProgress playerProgress){
        GameObject[] upgradeBtns = GameObject.FindGameObjectsWithTag("Upgrade Button");

        foreach (GameObject upgradeBtn in upgradeBtns){
            UpgradeScriptableObject currentUpgrade = upgradeBtn.GetComponent<UpgradeButton>().upgrade;

            if (!playerProgress.UnlockedUpgrades.IsNullOrEmpty() && playerProgress.UnlockedUpgrades.Contains(currentUpgrade.id)){
                currentUpgrade.isUnlocked = true;
                upgradeBtn.GetComponent<Image>().sprite = currentUpgrade.UpgradeIcon;
            }
        }
    }

    public static void UpdatePlayerWallet(PlayerProgress playerProgress, GameObject wallet){
        int currentDropIndex;

        foreach (Transform child in wallet.transform){
            GameObject.Destroy(child.gameObject);
        }
        
        foreach (StringLiterals.singularItemDrops drop in playerProgress.playerDropKeys){
             currentDropIndex = playerProgress.playerDropKeys.IndexOf(drop);
             GameObject text = new GameObject();
             text.transform.SetParent(wallet.transform);
             text.AddComponent<TextMeshProUGUI>();
             text.GetComponent<TextMeshProUGUI>().text = playerProgress.playerDropCounts[currentDropIndex] + " " + Tools.monsterDropToString(drop);
             text.GetComponent<TextMeshProUGUI>().fontSize = 16f;
        }
    }

    public static List<int> GetUnlockedLevels(PlayerProgress playerProgress){
        List<int> _unlockedLevels = new List<int>();
        
        foreach (int levelId in playerProgress.UnlockedLevels){
            if (!_unlockedLevels.Contains(levelId)){
                _unlockedLevels.Add(levelId);
            }

            if (unlockLevels.ContainsKey(levelId)){
                foreach (int toUnlockLevelId in unlockLevels[levelId]){
                    if (!_unlockedLevels.Contains(toUnlockLevelId)){
                        _unlockedLevels.Add(toUnlockLevelId);
                    }
                }
            }
        }

        return _unlockedLevels;
    }
}