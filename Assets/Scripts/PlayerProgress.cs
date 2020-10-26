using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.Serialization;

public static class GlobalPlayerProgress{
    public static List<int> UnlockedUpgrades = new List<int>();
    public static List<int> UnlockedLevels = new List<int>();
    public static Dictionary<StringLiterals.singularItemDrops, int> playerDrops = new Dictionary<StringLiterals.singularItemDrops, int>();
}

[Serializable]
public class PlayerProgress{

    [SerializeField] public List<int> UnlockedUpgrades = new List<int>();
    [SerializeField] public List<int> UnlockedLevels = new List<int>();
    [SerializeField] public Dictionary<StringLiterals.singularItemDrops, int> playerDrops = new Dictionary<StringLiterals.singularItemDrops, int>();

    public void saveProgress(){
        UnlockedUpgrades = GlobalPlayerProgress.UnlockedUpgrades;
        string json = JsonUtility.ToJson(this);
        Debug.Log(json);
    }
}
