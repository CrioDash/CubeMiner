
using System.Collections;
using UnityEngine;


namespace Utilities
{
    public class LoadingScreen:MonoBehaviour
    {

        private CanvasGroup _group;

        private void Awake()
        {
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
            if(PauseScript.IsPaused)
                PauseScript.SetPause();
            EventBus.Publish(EventBus.EventType.GAME_START);
        }
        
    }
}