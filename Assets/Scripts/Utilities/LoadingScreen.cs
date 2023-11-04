using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Utilities
{
    public class LoadingScreen:MonoBehaviour
    {
        private bool IsFade = true;

        private CanvasGroup _group;
        private Image _screenSprite;

        private void Awake()
        {
            _screenSprite = GetComponentInChildren<Image>();
            _group = GetComponent<CanvasGroup>();
        }

        public void Show()
        {
            _group.alpha = 1;
            StartCoroutine(ShowRoutine());
        }

        public void Fade()
        {
            _group.alpha = 0;
            _group.blocksRaycasts = true;
            StartCoroutine(FadeRoutine());
        }

        public IEnumerator FadeRoutine()
        {
            _group.blocksRaycasts = true;
            if(!PauseScript.IsPaused)
                PauseScript.SetPause();
            float t = 0;
            while (t < 1)
            {
                _group.alpha = Mathf.Lerp(0, 1, t);
                t += Time.unscaledDeltaTime*2;
                yield return null;
            }

            _group.alpha = 1;
            IsFade = true;
        }
        
        private IEnumerator ShowRoutine()
        { 
            float t = 0;
            while (t < 1)
            {
                _group.alpha = Mathf.Lerp(1, 0, t);
                t += Time.unscaledDeltaTime*2;
                yield return null;
            }
            _group.alpha = 0;
            _group.blocksRaycasts = false;
            IsFade = false;
            if(PauseScript.IsPaused)
                PauseScript.SetPause();
            EventBus.Publish(EventBus.EventType.GAME_START);
        }
        
    }
}