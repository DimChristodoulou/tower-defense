using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneManager : MonoBehaviour{
    private GameObject _uiCanvas, _loadingScreen, _loadingBar;

    private void Awake(){
        _uiCanvas = GameObject.Find("Canvas");
        _loadingScreen = GameObject.Find("LoadingScreen");
        _loadingBar = GameObject.Find("LoadingBar");
    }

    private void Start(){
        _loadingScreen.SetActive(false);
    }

    public void NewGame(){
        _uiCanvas.SetActive(false);
        _loadingScreen.SetActive(true);
        StartCoroutine(SceneAsynchronousLoad("LevelSelect", _loadingBar));
    }

    public void ExitGame(){
        Application.Quit();
    }

    /**
     * This function is called whenever we want to load a new scene and display a loading bar.
     * Loading is done asynchronously
     * @param string scene - the name of the scene we wish to load
     * @param GameObject loadingBar - the loading bar that will display the loading progress
     */
    public static IEnumerator SceneAsynchronousLoad (string scene, GameObject loadingBar)
    {
        loadingBar.GetComponent<Image>().fillAmount = 0.1f;
        Debug.Log(11);
        yield return new WaitForSeconds(0.5f);
        Debug.Log(11);
        loadingBar.GetComponent<Image>().fillAmount = 0.65f;
        yield return new WaitForSeconds(0.5f);
        Debug.Log(11);
        
        AsyncOperation ao = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(scene);

        while (! ao.isDone)
        {
            // [0, 0.9] > [0, 1]
            float progress = Mathf.Clamp01(ao.progress / 0.9f);
            
            Debug.Log("Loading progress: " + ao.isDone);
            loadingBar.GetComponent<Image>().fillAmount = progress;

            yield return new WaitForSeconds(1f);
        }
        
        yield break;
    }

}
