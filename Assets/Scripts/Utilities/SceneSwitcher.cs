using System;
using System.Collections;
using Data;
using Input;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;

public class SceneSwitcher : MonoBehaviour
{
    public static SceneSwitcher Instance;

    private bool _isLoading = false;
    private LoadingScreen _loadingScreen;
    private ButtonsController _buttonsController;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        _loadingScreen = GetComponentInChildren<LoadingScreen>();
        _buttonsController = GetComponentInChildren<ButtonsController>();
    }

    void Start()
    {
        DontDestroyOnLoad(this);
        Application.targetFrameRate = Int32.MaxValue;
        Time.fixedDeltaTime = 0.008f;
    }

    public void LoadScene(string name)
    {
        Variables.Score = 0;
        StartCoroutine(LoadSceneRoutine(name));
    }

    private IEnumerator LoadSceneRoutine(string name)
    {
        if(_isLoading)
            yield break;

        _isLoading = true;

        _loadingScreen.Fade();
        
        AsyncOperation sceneLoad = SceneManager.LoadSceneAsync(name, LoadSceneMode.Single);
        

        sceneLoad.allowSceneActivation = false;

        yield return new WaitForSecondsRealtime(0.25f);
        
        while(sceneLoad.progress<0.9f){
            yield return null;
        }

        sceneLoad.allowSceneActivation = true;
        _loadingScreen.Show();
        
        _buttonsController.ResetControls(name);

        _isLoading = false;


    }
    
    
}
