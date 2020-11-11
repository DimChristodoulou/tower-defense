// Decompiled with JetBrains decompiler
// Type: Enemies.EnemyScriptableObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A28AF8C8-695A-49DE-887A-AA1AA02D690F
// Assembly location: E:\Tower_Defense_Builds\14-10-2020\Tower Defense_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Enemies{
    public enum HealingValue {FlatValue, Percentage}
    
    [CreateAssetMenu(fileName = "Enemy", menuName = "ScriptableObjects/Enemy", order = 0)]
    public class EnemyScriptableObject : ScriptableObject{
        [Required] public int id;
        [Required] public string name;
        public string itemDrop;
        public StringLiterals.singularItemDrops drops;
        
        [Title("Modifiable Properties")]
        [ValidateInput("CantBeZero", "This field cannot be zero")]
        public float maxHealth;
        public float speed;
        public int goldValue;

        [Title("Conditional Enemy Properties")]
        public bool isFlying;
        public bool isHealer;

        [DetailedInfoBox("A monster with the healer attribute restores a portion of the health of nearby monsters", "" +
                                "Healing Percentage determines how much health per monster will be restored (percentile - mutually exclusive with Healing Percentage)\n" +
                                "\n"+
                                "Healing Value determines how much health per monster will be restored (flat value - mutually exclusive with Healing Percentage)\n" +
                                "\n"+
                                "Healing Cooldown determines how frequently the healing will occur in seconds\n" +
                                "\n"+
                                "Healing Range determines what area around the healer is caught in the healing effect")]
        
        [Title("Healer Parameters")]
        [ShowIf("isHealer")]
        [EnumToggleButtons]
        public HealingValue healing;
        
        [ShowIf("@this.isHealer && this.healing == HealingValue.FlatValue")] public float healingFlatValue;
        [ShowIf("@this.isHealer && this.healing == HealingValue.Percentage")] public float healingPercentage;
        [ShowIf("isHealer")] public float healingCooldown;
        [ShowIf("isHealer")] public float healingRange;


        private bool CantBeZero(float field){
            return Math.Abs(field) > 0f;
        }
    }
}