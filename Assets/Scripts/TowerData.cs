// Decompiled with JetBrains decompiler
// Type: TowerData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A28AF8C8-695A-49DE-887A-AA1AA02D690F
// Assembly location: E:\Tower_Defense_Builds\14-10-2020\Tower Defense_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Tower", menuName = "ScriptableObjects/TowerData", order = 1)]
[Serializable]
public class TowerData : ScriptableObject
{
    public const int ARROW_TOWER = 0;
    public const int ARCANE_TOWER = 1;
    public int id;
    public int cost;
    public int numberOfLevelups;
    public int level;
    public float damage;
    public float firingRate;
    public float range;
    public string pathToResource;
    public bool isAreaTower;
    public bool isSupportTower;
    [SerializeField]
    public List<LevelUpData> towerLevelUps;
}