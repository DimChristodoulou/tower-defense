// Decompiled with JetBrains decompiler
// Type: UpgradeScriptableObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A28AF8C8-695A-49DE-887A-AA1AA02D690F
// Assembly location: E:\Tower_Defense_Builds\14-10-2020\Tower Defense_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Upgrade", menuName = "ScriptableObjects/Upgrade", order = 2)]
[Serializable]
public class UpgradeScriptableObject : SerializedScriptableObject
{
    [Title("Basic Values")]
    public int id;
    public string name;
    
    [Title("Description Fields")]
    [TextArea]
    public string description;
    [TextArea]
    public string flavourDescription;

    [PreviewField(50, ObjectFieldAlignment.Right)]
    public Sprite UpgradeIcon;
    
    public bool isUnlocked;

    [Title("Upgrade Cost")] 
    public Dictionary<StringLiterals.singularItemDrops, int> costs = new Dictionary<StringLiterals.singularItemDrops, int>(){
        
    };
}