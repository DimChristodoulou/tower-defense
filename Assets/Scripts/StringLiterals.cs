// Decompiled with JetBrains decompiler
// Type: StringLiterals
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A28AF8C8-695A-49DE-887A-AA1AA02D690F
// Assembly location: E:\Tower_Defense_Builds\14-10-2020\Tower Defense_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

public static class StringLiterals{

    /*
     * Enemy Drops Strings
     */
    public const string WarTurtleDrop = "War Turtle Shell";
    public const string GoblinDrop = "Goblin Ear";
    public const string SpiderDrop = "Spider Leg";
    public const string WaspDrop = "Wasp Sting";
    public const string CultistDrop = "Cultist Mana Essence";
    public const string SkeletonDrop = "Skeleton Bone";
    public const string MinotaurDrop = "Minotaur Horn";
    
    /*
     * Damage Type Strings
     */
    public const string Piercing = "Piercing";
    public const string Slashing = "Slashing";
    public const string Bludgeoning = "Bludgeoning";
    public const string Nature = "Nature";
    public const string Arcane = "Arcane";
    public const string Fire = "Fire";
    public const string Cold = "Cold";
    public const string Electric = "Electric";


    public enum singularItemDrops{
        WarTurtleDrop,
        GoblinDrop,
        SpiderDrop,
        WaspDrop,
        CultistDrop,
        SkeletonDrop,
        MinotaurDrop
    }
    
    public static List<string> drops = new List<string>(){
        "War Turtle Shell",
        "Goblin Ear",
        "Spider Leg",
        "Wasp Sting",
        "Cultist Mana Essence",
        "Skeleton Bone",
        "Minotaur Horn"
    };

    public static string WaveCountdownText = "Time until next wave:";
}