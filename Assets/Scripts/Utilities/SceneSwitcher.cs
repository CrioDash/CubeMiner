using System;
using System.Collections;
using Data;
using Input;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Utilities;

public class SceneSwitcher : MonoBehaviour
{
    private IEnumerator _currentCoroutine;

    public static string lastScene;

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
        Application.targetFrameRate = 60;
       
    }

    public void LoadScene(string name)
    {
        EventBus.Publish(EventBus.EventType.GAME_SAVE);
        StopAllCoroutines();
        if(name == "PlayScene")
            Variables.ResetStats(); 
        StartCoroutine(LoadSceneRoutine(name));
    }

    public void SetCoroutine(IEnumerator coroutineOpen)
    {
        _currentCoroutine = coroutineOpen;
    }
    

    private IEnumerator LoadSceneRoutine(string name)
    {
        if(_isLoading)
            yield break;
        
       

        lastScene = SceneManager.GetActiveScene().name;

        if(_currentCoroutine!=null)
            yield return StartCoroutine(_currentCoroutine);

        _isLoading = true;

       
        
        AsyncOperation sceneLoad = SceneManager.LoadSceneAsync(name, LoadSceneMode.Single);
        
        sceneLoad.allowSceneActivation = false;

        yield return StartCoroutine(_loadingScreen.FadeRoutine());

        while(sceneLoad.progress<0.9f){
            yield return null;
        }

        sceneLoad.allowSceneActivation = true;

        yield return new WaitForSecondsRealtime(0.5f);
        
        _loadingScreen.Show();
        
        _buttonsController.ResetControls(name);

        _isLoading = false;
        

    }
    
    
}
