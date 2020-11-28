using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Tools
{

    public static string convertNumberToTime(float num){
        return TimeSpan.FromSeconds(num).ToString(@"mm\:ss");
        return Mathf.RoundToInt(num / 60).ToString().PadLeft(2, '0') 
               + ":" 
               + Mathf.RoundToInt(num % 60).ToString().PadLeft(2, '0') ;
    }

    public static string ReturnIntAsSingularOrPlural(int value, string stringToConvert){
        if (value == 1){
            return value + " " + stringToConvert;
        }
        else if (value > 1){
            return value + " " + stringToConvert + "s";
        }
        else{
            return null;
        }
    }

    public static string monsterDropToString(StringLiterals.singularItemDrops drop){
        switch (drop){
            case StringLiterals.singularItemDrops.WarTurtleDrop:
                return StringLiterals.WarTurtleDrop;
            case StringLiterals.singularItemDrops.GoblinDrop:
                return StringLiterals.GoblinDrop;
            case StringLiterals.singularItemDrops.SpiderDrop:
                return StringLiterals.SpiderDrop;
            case StringLiterals.singularItemDrops.WaspDrop:
                return StringLiterals.WaspDrop;
            case StringLiterals.singularItemDrops.CultistDrop:
                return StringLiterals.CultistDrop;
            case StringLiterals.singularItemDrops.SkeletonDrop:
                return StringLiterals.SkeletonDrop;
            case StringLiterals.singularItemDrops.MinotaurDrop:
                return StringLiterals.MinotaurDrop;
            default:
                return null;
        }
    }

    public static string DamageTypeToString(DamageTypes type){
        switch (type){
            case DamageTypes.Piercing:
                return StringLiterals.Piercing;
            case DamageTypes.Slashing:
                return StringLiterals.Slashing;
            case DamageTypes.Bludgeoning:
                return StringLiterals.Bludgeoning;
            case DamageTypes.Nature:
                return StringLiterals.Nature;
            case DamageTypes.Fire:
                return StringLiterals.Fire;
            case DamageTypes.Cold:
                return StringLiterals.Cold;
            case DamageTypes.Arcane:
                return StringLiterals.Arcane;
            case DamageTypes.Electric:
                return StringLiterals.Electric;
            default:
                return null;
        }
    }
}
