// Decompiled with JetBrains decompiler
// Type: UpgradeScriptableObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A28AF8C8-695A-49DE-887A-AA1AA02D690F
// Assembly location: E:\Tower_Defense_Builds\14-10-2020\Tower Defense_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using Enemies;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Upgrade", menuName = "ScriptableObjects/Upgrade", order = 2)]
[Serializable]
public class UpgradeScriptableObject : SerializedScriptableObject
{
    [Serializable]
    public class StatEffect {
    
        #region upgrade_effect_group

        [Button]
        [FoldoutGroup("Which stat(s) does the upgrade affect?")] 
        [GUIColor(0.3f, 0.8f, 0.8f, 1f)]
        private void Damage() => affects = (UpgradeEffect.Damage);

        [Button]
        [FoldoutGroup("Which stat(s) does the upgrade affect?")] 
        [GUIColor(0.8f, 0.3f, 0.8f, 1f)]
        private void Range() => affects = (UpgradeEffect.Range);
    
        [Button]
        [FoldoutGroup("Which stat(s) does the upgrade affect?")] 
        [GUIColor(0.8f, 0.8f, 0.3f, 1f)]
        private void FiringRate() => affects = (UpgradeEffect.FiringRate);
    
        [Button]
        [FoldoutGroup("Which stat(s) does the upgrade affect?")] 
        [GUIColor(0.4f, 0.4f, 0.8f, 1f)]
        private void Cost() => affects = (UpgradeEffect.Cost);
    
        [Button]
        [FoldoutGroup("Which stat(s) does the upgrade affect?")] 
        [GUIColor(0.2f, 0.6f, 0.8f, 1f)]
        private void Other() => affects = (UpgradeEffect.Other);

        [ReadOnly] [FoldoutGroup("Which stat(s) does the upgrade affect?")] 
        public UpgradeEffect affects;

        #endregion
    
        #region upgrade_value

        [HideIf("@this.affects == UpgradeEffect.Other")]
        [FoldoutGroup("How does the upgrade affect the stat(s)?")]
        [EnumToggleButtons]
        public NumericalValue valueType;

        [HideIf("@this.affects == UpgradeEffect.Other")]
        [FoldoutGroup("How does the upgrade affect the stat(s)?")]
        public float upgradeValue;

        #endregion
    
    }
    
    public enum ConditionType {Buff, Debuff}
    public enum UpgradeEffect {Damage, Range, FiringRate, Cost, Other}
    public enum NumericalValue {FlatValue, Percentage}
    public enum UpgradeSelection {SpecificTowers, SpecificEnemies, AllTowers, AllEnemies}

    #region basic_values_group

    [FoldoutGroup("Basic Values")] public int id;
    [FoldoutGroup("Basic Values")] public string name;
    [FoldoutGroup("Basic Values")] [LabelWidth(196)] public bool isUnlocked;
    [FoldoutGroup("Basic Values")] [PreviewField(50, ObjectFieldAlignment.Right)]
    public Sprite UpgradeIcon;

    #endregion

    
    [FoldoutGroup("Description Fields")] [TextArea] public string description;
    [FoldoutGroup("Description Fields")] [TextArea] public string flavourDescription;
    
    
    [FoldoutGroup("Upgrade Cost")]
    public Dictionary<StringLiterals.singularItemDrops, int> costs = new Dictionary<StringLiterals.singularItemDrops, int>(){ };
    
        
    [FoldoutGroup("Prerequisite Upgrades")] public bool hasPrerequisites;
    [ShowIf("hasPrerequisites")] [FoldoutGroup("Prerequisite Upgrades")] public List<UpgradeScriptableObject> prerequisites;
    

    [FoldoutGroup("Mutually Exclusive")] [LabelWidth(250)] public bool isMutuallyExclusiveWithOtherSkills;
    [ShowIf("isMutuallyExclusiveWithOtherSkills")] [FoldoutGroup("Mutually Exclusive")] public List<UpgradeScriptableObject> mutuallyExclusiveSkills;

    #region buff_debuff_group

    [Button]
    [FoldoutGroup("Is Buff Or Debuff")]
    [GUIColor(0, 1, 0)]
    private void Buff() => isBuffOrDebuff = ConditionType.Buff;

    [Button]
    [FoldoutGroup("Is Buff Or Debuff")]
    [GUIColor(1, 0, 0)]
    private void Debuff() => isBuffOrDebuff = ConditionType.Debuff;

    [ReadOnly] [FoldoutGroup("Is Buff Or Debuff")] 
    public ConditionType isBuffOrDebuff;

    #endregion

    [SerializeField]
    public List<StatEffect> upgradeEffects;

    [FoldoutGroup("Does the upgrade affect a specific tower, enemy, all towers or all enemies?")]
    [EnumToggleButtons]
    public UpgradeSelection specificTowerOrEnemy;

    [ShowIf("@this.specificTowerOrEnemy == UpgradeSelection.SpecificTowers")]
    [FoldoutGroup("Which towers does the upgrade affect?")]
    public List<TowerData> affectedTowers;
    
    [ShowIf("@this.specificTowerOrEnemy == UpgradeSelection.SpecificEnemies")]
    [FoldoutGroup("Which enemies does the upgrade affect?")]
    public List<EnemyScriptableObject> affectedEnemies;
}