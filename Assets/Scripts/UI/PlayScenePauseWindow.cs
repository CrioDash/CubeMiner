using System;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

namespace UI
{
    public class PlayScenePauseWindow : MonoBehaviour
    {
        public static PlayScenePauseWindow Instance;

        private bool _isVisible = false;
        private CanvasGroup _canvasGroup;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        private void Start()
        {
            Instance = this;
            _canvasGroup.alpha = _isVisible ? 1 : 0;
            _canvasGroup.blocksRaycasts = _isVisible;
        }

        public void ChangeWindowState()
        {
            _isVisible = !_isVisible;
            _canvasGroup.alpha = _isVisible ? 1 : 0;
            _canvasGroup.blocksRaycasts = _isVisible;
            PauseScript.SetPause();
        }
        
        public void ExitScene()
        {
            SceneSwitcher.Instance.LoadScene("MenuScene");
        }

        public void ReloadScene()
        {
            SceneSwitcher.Instance.LoadScene("PlayScene");
        }
        
    }
}