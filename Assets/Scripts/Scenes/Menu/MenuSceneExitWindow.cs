﻿using UnityEngine;
using Utilities;

namespace UI
{
    public class MenuSceneExitWindow: MonoBehaviour
    {
        public static MenuSceneExitWindow Instance;

        [SerializeField]
        private CanvasGroup btnGroup;
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
            gameObject.SetActive(false);
        }

        public void ChangeWindowState()
        {
            PauseScript.SetPause();
            _isVisible = !_isVisible;
            _canvasGroup.alpha = _isVisible ? 1 : 0;
            _canvasGroup.blocksRaycasts = _isVisible;
            btnGroup.interactable = !_isVisible;
            gameObject.SetActive(_isVisible);
        }

        public void ExitGame()
        {
            Application.Quit();
        }
        
    }
}