// Decompiled with JetBrains decompiler
// Type: Settings
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A28AF8C8-695A-49DE-887A-AA1AA02D690F
// Assembly location: E:\Tower_Defense_Builds\14-10-2020\Tower Defense_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour{
    public Dictionary<string, KeyCode> keyBindings = new Dictionary<string, KeyCode>{
        {
            "buildArrowTowerBinding",
            KeyCode.A
        }, {
            "buildArcaneTowerBinding",
            KeyCode.R
        }, {
            "buildSupportTowerBinding",
            KeyCode.S
        }, {
            "buildEarthquakeTowerBinding",
            KeyCode.E
        }, {
            "speedx1",
            KeyCode.Alpha1
        }, {
            "speedx2",
            KeyCode.Alpha2
        }, {
            "speedx3",
            KeyCode.Alpha3
        }
    };
    
    public List<KeyCode> forbiddenBindings = new List<KeyCode>(){
        KeyCode.Escape,
        KeyCode.Tab,
        KeyCode.LeftShift,
        KeyCode.RightShift,
        KeyCode.LeftAlt,
        KeyCode.RightAlt,
        KeyCode.LeftWindows,
        KeyCode.RightWindows,
        KeyCode.CapsLock,
        KeyCode.LeftControl,
        KeyCode.RightControl,
        KeyCode.AltGr,
        KeyCode.KeypadEnter,
        KeyCode.Insert,
        KeyCode.Delete,
        KeyCode.Home,
        KeyCode.PageDown,
        KeyCode.PageUp,
        KeyCode.End,
        KeyCode.Numlock,
        KeyCode.Print,
        KeyCode.ScrollLock,
        KeyCode.Pause
    };

    private GameObject _settingsWindow;
    private GameObject _pauseMenu;
    private GameManager _manager;
    public bool isPaused;
    private string _buttonToRebind;
    private Text _buttonLabel;

    private void Start(){
        _settingsWindow = GameObject.Find("Settings");
        _settingsWindow.SetActive(false);
        _pauseMenu = GameObject.Find("PauseMenu");
        _pauseMenu.SetActive(false);
    }

    private void Update(){
        if (Input.GetKeyDown(KeyCode.Escape)){
            if (isPaused)
                UnpauseGame();
            else
                PauseGame();
        }

        if (_buttonToRebind == null || !Input.anyKeyDown)
            return;

        foreach (KeyCode key in Enum.GetValues(typeof(KeyCode))){
            if (Input.GetKeyDown(key)){
                if (keyBindings.ContainsValue(key)){
                    Debug.LogWarning("Button is already a keybind!");
                }
                else if (forbiddenBindings.Contains(key)){
                    Debug.LogWarning("Button is a forbidden keybind!");
                }
                else{
                    keyBindings[_buttonToRebind] = key;
                    _buttonLabel.text = key.ToString();
                    _buttonToRebind = null;
                    break;
                }
            }
        }
    }

    public string ButtonToRebind
    {
        get => _buttonToRebind;
        set => _buttonToRebind = value;
    }

    public void showSettingsMenu(){
        _settingsWindow.SetActive(true);
        _pauseMenu.SetActive(false);
    }

    private void PauseGame(){
        isPaused = true;
        _pauseMenu.SetActive(true);
        Time.timeScale = 0.0f;
    }

    private void UnpauseGame(){
        isPaused = false;
        _pauseMenu.SetActive(false);
        _settingsWindow.SetActive(false);
        Time.timeScale = 1f;
    }

    public void StartRebindFor(GameObject buttonGO){
        _buttonToRebind = buttonGO.name;
        _buttonLabel = buttonGO.GetComponentInChildren<Text>();
    }

    public void ResumeButton(){
        UnpauseGame();
    }
}