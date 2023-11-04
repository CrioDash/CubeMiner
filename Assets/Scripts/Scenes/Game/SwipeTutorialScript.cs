using System;
using System.Collections;
using Data;
using UnityEngine;
using UnityEngine.EventSystems;
using Utilities;

namespace Game
{
    public class SwipeTutorialScript:MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Transform startPoint;
        [SerializeField] private Transform endPoint;
        [SerializeField] private GameObject cursor;

        private CanvasGroup _group;
        
        private void Awake()
        {
            if(PlayerSave.Instance.TutorialCompleted)
                Destroy(gameObject);
            _group = GetComponent<CanvasGroup>();
        }

        private void OnEnable()
        {
            EventBus.Subscribe(EventBus.EventType.GAME_START, ShowTutorial);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe(EventBus.EventType.GAME_START, ShowTutorial);
        }

        private void ShowTutorial()
        {
            if(PlayerSave.Instance.TutorialCompleted)
                return;
            _group.blocksRaycasts = true;
            _group.alpha = 1;
            PauseScript.SetPause();
            StartCoroutine(MoveCursorRoutine());
        }

        private IEnumerator MoveCursorRoutine()
        {
            float t = 0;
            while (true)
            {
                t = 0;
                while (t<1)
                {
                    cursor.transform.position = Vector3.Lerp(startPoint.position, endPoint.position, t);
                    t += Time.fixedUnscaledDeltaTime;
                    yield return null;
                }
                t = 0;
                while (t<1)
                {
                    cursor.transform.position = Vector3.Lerp(endPoint.position, startPoint.position, t);
                    t += Time.fixedUnscaledDeltaTime;
                    yield return null;
                }
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            StopCoroutine(MoveCursorRoutine());
            _group.alpha = 0;
            _group.blocksRaycasts = false;
            PauseScript.SetPause();
        }
    }
}