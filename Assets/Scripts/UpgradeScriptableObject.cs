// Decompiled with JetBrains decompiler
// Type: UpgradeScriptableObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A28AF8C8-695A-49DE-887A-AA1AA02D690F
// Assembly location: E:\Tower_Defense_Builds\14-10-2020\Tower Defense_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Upgrade", menuName = "ScriptableObjects/Upgrade", order = 2)]
[Serializable]
public class UpgradeScriptableObject : ScriptableObject
{
    public int id;
    public string name;
    [TextArea]
    public string description;
    [TextArea]
    public string flavourDescription;
    public Sprite UpgradeIcon;
}