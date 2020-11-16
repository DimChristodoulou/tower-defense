// Decompiled with JetBrains decompiler
// Type: TowerData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A28AF8C8-695A-49DE-887A-AA1AA02D690F
// Assembly location: E:\Tower_Defense_Builds\14-10-2020\Tower Defense_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public enum DamageTypes{
    Piercing,
    Slashing,
    Bludgeoning,
    Nature,
    Fire,
    Electric,
    Cold,
    Arcane
}

[CreateAssetMenu(fileName = "Tower", menuName = "ScriptableObjects/TowerData", order = 1)]
[Serializable]
public class TowerData : ScriptableObject
{
    public const int ARROW_TOWER = 0;
    public const int ARCANE_TOWER = 1;
    public const int SUPPORT_TOWER = 2;
    public const int EARTHQUAKE_TOWER = 3;
    
    [Title("Core Values")]
    public int id;
    public string pathToResource;
    
    [Title("Modifiable Values")]
    public int cost;
    public float damage;
    public float firingRate;
    public float range;
    public List<DamageTypes> towerDamageTypes;

    [Title("Conditional Tower Properties")]
    public bool isAreaTower;
    public bool isSupportTower;
    public bool canHitFlyingEnemies;
    
    [Title("Level-Up Related Values")]
    public int currentLevel;
    [SerializeField]
    public List<LevelUpData> towerLevelUps;
    
}