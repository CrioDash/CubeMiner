using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace Utilities
{
    public class LoadingScreen:MonoBehaviour
    {
        private bool IsFade = true;

        private SpriteRenderer _screenSprite;

        private void Awake()
        {
            _screenSprite = GetComponentInChildren<SpriteRenderer>();
        }

        public void Show()
        {
            _screenSprite.color = Color.black;
            StartCoroutine(ShowRoutine());
        }

        public void Fade()
        {
            _screenSprite.color = Color.clear;
            StartCoroutine(FadeRoutine());
        }

        public IEnumerator FadeRoutine()
        {
            if(!PauseScript.IsPaused)
                PauseScript.SetPause();
            float t = 0;
            while (t < 1)
            {
                _screenSprite.color = Color.Lerp(Color.clear, Color.black, t);
                t += Time.unscaledDeltaTime;
                yield return null;
            }
            _screenSprite.color = Color.black;
            IsFade = true;
        }
        
        private IEnumerator ShowRoutine()
        { 
            float t = 0;
            while (t < 1)
            {
                _screenSprite.color = Color.Lerp(Color.black, Color.clear, t);
                t += Time.unscaledDeltaTime;
                yield return null;
            }
            _screenSprite.color = Color.clear;
            IsFade = false;
            if(PauseScript.IsPaused)
                PauseScript.SetPause();
            EventBus.Publish(EventBus.EventType.GAME_START);
        }
        
    }
}