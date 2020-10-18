// Decompiled with JetBrains decompiler
// Type: Enemies.EnemyScriptableObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A28AF8C8-695A-49DE-887A-AA1AA02D690F
// Assembly location: E:\Tower_Defense_Builds\14-10-2020\Tower Defense_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace Enemies{
    [CreateAssetMenu(fileName = "Enemy", menuName = "ScriptableObjects/Enemy", order = 0)]
    public class EnemyScriptableObject : ScriptableObject{
        public int id;
        public string name;
        public float maxHealth;
        public float speed;
        public int goldValue;
        public string itemDrop;
    }
}