using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

public class WaveManagerEditorWindow : OdinMenuEditorWindow{
    private CreateNewWaveManager _createNewWaveManager; 
    
    [MenuItem("My Game/Wave Manager Window")]
    private static void OpenWindow(){
        GetWindow<WaveManagerEditorWindow>().Show();
    }

    protected override void OnDestroy(){
        base.OnDestroy();

        if (_createNewWaveManager != null){
            DestroyImmediate(_createNewWaveManager.wave);
        }
    }

    protected override OdinMenuTree BuildMenuTree(){
        var tree = new OdinMenuTree();
        
        _createNewWaveManager = new CreateNewWaveManager();
        tree.Add("Create New Wave Manager", new CreateNewWaveManager());
        tree.AddAllAssetsAtPath("Wave Manager", "Assets/Wave Managers", typeof(WaveManagerScriptableObject));
        
        return tree;
    }

    protected override void OnBeginDrawEditors(){
        //Gets the reference to the currently selected item
        OdinMenuTreeSelection selection = this.MenuTree.Selection;

        SirenixEditorGUI.BeginHorizontalToolbar();
        {
            GUILayout.FlexibleSpace();

            if (SirenixEditorGUI.ToolbarButton("Delete Current")){
                WaveManagerScriptableObject asset = selection.SelectedValue as WaveManagerScriptableObject;
                string path = AssetDatabase.GetAssetPath(asset);
                AssetDatabase.DeleteAsset(path);
                AssetDatabase.SaveAssets();
            }
        }
        SirenixEditorGUI.EndHorizontalToolbar();
    }

    public class CreateNewWaveManager{
        [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
        public WaveManagerScriptableObject wave;

        public CreateNewWaveManager(){
            wave = ScriptableObject.CreateInstance<WaveManagerScriptableObject>();
            wave.name = "New Wave Manager";
        }

        [Button("Add new Wave Manager")]
        private void CreateNewData(){
            AssetDatabase.CreateAsset(wave, "Assets/Wave Managers/" + wave.name + ".asset");
            AssetDatabase.SaveAssets();

            //Create instance of the scriptable object
            wave = ScriptableObject.CreateInstance<WaveManagerScriptableObject>();
            wave.name = "New Wave Manager";
        }
    }
}