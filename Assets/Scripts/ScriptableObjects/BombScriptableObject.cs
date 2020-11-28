using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Bomb", menuName = "ScriptableObjects/BombScriptableObject", order = 1)]
[Serializable]
public class BombScriptableObject : ScriptableObject
{
    
    public const int STANDARD_BOMB = 0;

    [Title("Core Values")]
    public int id;
    public string pathToResource;
    
    [Title("Modifiable Values")]
    public int cost;
    public float damage;
    public float range;
    public List<DamageTypes> BombDamageTypes;

    [Title("Conditional Bomb Properties")]
    public bool canHitFlyingEnemies;
    
}
