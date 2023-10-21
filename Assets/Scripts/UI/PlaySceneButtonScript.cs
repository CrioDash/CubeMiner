using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaySceneButtonScript : MonoBehaviour
{
    [SerializeField] private AnimationCurve animationCurve;
    [SerializeField] private GameObject canvasContainer;
    [SerializeField] private Transform startPos;
    [SerializeField] private Transform endPos;
    [SerializeField] private string sceneName;
    
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(() =>
            SceneSwitcher.Instance.SetCoroutine(MenuPlayCoroutine()));
        _button.onClick.AddListener(() => SceneSwitcher.Instance.LoadScene(sceneName)); 
    }

    private void Start()
    {
        if (SceneSwitcher.lastScene == "PlayScene")
            StartCoroutine(MenuStartCoroutine());
    }

    private IEnumerator MenuPlayCoroutine()
    {
        float t = 0;
        while (t < 1)
        {
            canvasContainer.transform.position = Vector3.Lerp(startPos.position, endPos.position, animationCurve.Evaluate(t));
            t += Time.deltaTime*1.5f;
            yield return null;
        }
    }

    private IEnumerator MenuStartCoroutine()
    {
        float t = 0;
        while (t < 1)
        {
            canvasContainer.transform.position = Vector3.Lerp(endPos.position, startPos.position, animationCurve.Evaluate(t));
            t += Time.deltaTime*2;
            yield return null;
        }
    }

}
