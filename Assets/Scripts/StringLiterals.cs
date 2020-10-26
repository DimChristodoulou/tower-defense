// Decompiled with JetBrains decompiler
// Type: StringLiterals
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A28AF8C8-695A-49DE-887A-AA1AA02D690F
// Assembly location: E:\Tower_Defense_Builds\14-10-2020\Tower Defense_Data\Managed\Assembly-CSharp.dll

public static class StringLiterals{
    public const string LifeText = "Lives: ";
    public const string GoldText = "GOLD: ";
    
    
    public const string WarTurtleDrop = "War Turtle Shell";
    public const string GoblinDrop = "Goblin Ear";
    public const string SpiderDrop = "Spider Leg";
    public const string WaspDrop = "Wasp Sting";
    public const string CultistDrop = "Cultist Mana Essence";

    public enum singularItemDrops{
        WarTurtleDrop,
        GoblinDrop,
        SpiderDrop,
        WaspDrop,
        CultistDrop
    }

    public static string monsterDropToString(singularItemDrops drop){
        switch (drop){
            case singularItemDrops.WarTurtleDrop:
                return WarTurtleDrop;
            case singularItemDrops.GoblinDrop:
                return GoblinDrop;
            case singularItemDrops.SpiderDrop:
                return SpiderDrop;
            case singularItemDrops.WaspDrop:
                return WaspDrop;
            case singularItemDrops.CultistDrop:
                return CultistDrop;
            default:
                return null;
        }
    }
}