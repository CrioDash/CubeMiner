using System;
using System.Collections;
using Data;
using Fruit;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

namespace Game
{
    public class ClickTutorialScript : MonoBehaviour
    {
        [SerializeField] private GameObject _textPanel;
        [SerializeField] private Sprite[] _sprites;
        [SerializeField] private Image _cursorImage;
        
        private CanvasGroup _group;

        public static bool isShown = false;
        
        private void OnEnable()
        {
            EventBus.Subscribe(EventBus.EventType.SPAWN_DYNAMITE, () => StartCoroutine(ShowTutorialRoutine()));
            EventBus.Subscribe(EventBus.EventType.GAME_PAUSE, EndTutorial);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe(EventBus.EventType.SPAWN_DYNAMITE, () => StartCoroutine(ShowTutorialRoutine()));
            EventBus.Unsubscribe(EventBus.EventType.GAME_PAUSE, EndTutorial);
        }

        private void Awake()
        {
            _group = GetComponent<CanvasGroup>();
        }

        private void EndTutorial()
        {
            if(!PlayerSave.Instance.TutorialCompleted)
                return;
            _group.alpha = 0;
            StopCoroutine(ShowTutorialRoutine());
        }

        private IEnumerator ShowTutorialRoutine()
        {
            yield return new WaitForSeconds(1.75f);
            if(PlayerSave.Instance.TutorialCompleted)
                yield break;
            PauseScript.SetPause();

            isShown = true;
            
            transform.position = FindObjectOfType<Dynamite>().transform.position;
            _group.alpha = 1;

            float t = 0;
            while (PauseScript.IsPaused)
            {
                t = 0;
                while (t<1)
                {
                    _cursorImage.sprite = _sprites[1];
                    _textPanel.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one*1.2f, t);
                    t += Time.fixedUnscaledDeltaTime;
                    yield return null;
                }
                t = 0;
                while (t<1)
                {
                    _cursorImage.sprite = _sprites[0];
                    _textPanel.transform.localScale = Vector3.Lerp(Vector3.one*1.2f, Vector3.one, t);
                    t += Time.fixedUnscaledDeltaTime;
                    yield return null;
                }
            }

        }
    }
}