using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;
using Object = System.Object;

[CreateAssetMenu(fileName = "Wave Manager", menuName = "ScriptableObjects/Wave Manager", order = 3)]
public class WaveManagerScriptableObject : ScriptableObject
{
    public enum WaveSpawnType {Randomly, WeightedRandom, Specific}
    
    [Serializable]
    public class Wave{
        [TableColumnWidth(150, Resizable = false)]
        [VerticalGroup("Wave Timing Attributes"), LabelWidth(85)]
        public float enemySpawnInterval;
        [VerticalGroup("Wave Timing Attributes"), LabelWidth(85)]
        public float waveSpawn;
        
        [TableColumnWidth(230, Resizable = false)]
        [VerticalGroup("Wave Enemies")]
        public List<GameObject> EnemiesList;

        #region Wave Spawn Type

        [ReadOnly] [FoldoutGroup("How will enemies spawn?")]
        [LabelWidth(80)]
        public WaveSpawnType waveSpawnType;
        
        [Button]
        [FoldoutGroup("How will enemies spawn?")]
        [GUIColor(0.6588f, 0.7294f, 0.6039f)]
        private void Random() => waveSpawnType = WaveSpawnType.Randomly;
        
        [Button]
        [FoldoutGroup("How will enemies spawn?")]
        [GUIColor(0.3789f, 0.7851f, 0.6562f)]
        private void RandomWeighted() => waveSpawnType = WaveSpawnType.WeightedRandom;
        
        [Button]
        [FoldoutGroup("How will enemies spawn?")]
        [GUIColor(0.7929f, 0.4726f, 0.2265f)]
        private void Specific() => waveSpawnType = WaveSpawnType.Specific;

        #endregion

        [Space]
        [ShowIf("@this.waveSpawnType != WaveSpawnType.Randomly")]
        [DetailedInfoBox(  "Show info on options",
                     "-When the wave spawn method is weighted, this takes a value from 0 to 1\n" +
                            "The value shows the probability that the spawned enemy is this enemy\n"+
                            "-When the wave spawn method is specific, this takes a number which shows\n"+
                            "how many enemies of this type will spawn")]
        [ValidateInput("MustBeEqualLengthWithObjectList", "This list should have the same length as the possible enemies list.")]
        public List<int> enemiesWeightOrNumber;
        
        [Space]
        [ShowIf("@this.waveSpawnType != WaveSpawnType.Specific")]
        [TableColumnWidth(150, Resizable = false)]
        public int waveEnemyCount;
        
        public bool MustBeEqualLengthWithObjectList(List<int> list){
            if (list.Count == EnemiesList.Count || waveSpawnType == WaveSpawnType.Randomly)
                return true;
            return false;
        }
        
        public bool MustBeEqualLengthWithIntList(List<GameObject> list){
            if (list.Count == enemiesWeightOrNumber.Count || waveSpawnType == WaveSpawnType.Randomly)
                return true;
            return false;
        }
    }
    
    [FoldoutGroup("Endless Level Configuration")] 
    public bool isEndless;
    [FoldoutGroup("Endless Level Configuration")] [ShowIf("isEndless")] 
    public List<Wave> endlessPossibleEnemies;
    
    [FoldoutGroup("Non-Endless Level Configuration")] [HideIf("isEndless")] 
    [TableList(ShowIndexLabels = true)]
    public List<Wave> waves;
}
