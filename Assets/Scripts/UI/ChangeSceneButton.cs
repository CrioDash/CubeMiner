using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeSceneButton : MonoBehaviour
{
    [SerializeField] private string _sceneName;
    
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(()=> SceneSwitcher.Instance.LoadScene(_sceneName));
    }
    
}
