using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.UI;

public static class GlobalPlayerProgress{
    public static List<List<int>> unlockLevels = new List<List<int>>(){
        new List<int>(){0,1},
        new List<int>(){1,2},
        new List<int>(){2,3,4}
    };
    
    public static List<UpgradeScriptableObject> UnlockedUpgrades = new List<UpgradeScriptableObject>();
    public static List<int> UnlockedLevels = new List<int>();
    public static Dictionary<StringLiterals.singularItemDrops, int> playerDrops = new Dictionary<StringLiterals.singularItemDrops, int>();

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

    public static float CalculateValueAfterUpgrade(float value, UpgradeScriptableObject upgrade){
        switch (upgrade.valueType){
            case UpgradeScriptableObject.NumericalValue.Percentage:
                value += value * upgrade.upgradeValue;
                return value;
            case UpgradeScriptableObject.NumericalValue.FlatValue:
                return value + upgrade.upgradeValue;
            default:
                return value;
        }
    }

    public static int CalculateValueAfterUpgrade(int value, UpgradeScriptableObject upgrade){
        switch (upgrade.valueType){
            case UpgradeScriptableObject.NumericalValue.Percentage:
                value += (int) (value * upgrade.upgradeValue);
                return value;
            case UpgradeScriptableObject.NumericalValue.FlatValue:
                return (int) (value + upgrade.upgradeValue);
            default:
                return value;
        }
    }

    public static void UpdateUpgradePanel(PlayerProgress playerProgress){
        GameObject[] upgradeBtns = GameObject.FindGameObjectsWithTag("Upgrade Button");

        foreach (GameObject upgradeBtn in upgradeBtns){
            UpgradeScriptableObject currentUpgrade = upgradeBtn.GetComponent<UpgradeButton>().upgrade;
            Debug.Log(playerProgress.UnlockedUpgrades.Count);
            Debug.Log(currentUpgrade.id);
            
            if (!playerProgress.UnlockedUpgrades.IsNullOrEmpty() && playerProgress.UnlockedUpgrades.Contains(currentUpgrade.id)){
                currentUpgrade.isUnlocked = true;
                upgradeBtn.GetComponent<Image>().sprite = currentUpgrade.UpgradeIcon;
            }
        }
    }
}