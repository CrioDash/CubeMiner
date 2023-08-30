using System;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

namespace UI
{
    public class PauseButtonScript:MonoBehaviour
    {

        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void OnEnable()
        {
            EventBus.Subscribe(EventBus.EventType.GAME_END, Fade);
            EventBus.Subscribe(EventBus.EventType.GAME_PAUSE, Fade);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe(EventBus.EventType.GAME_END, Fade);
            EventBus.Unsubscribe(EventBus.EventType.GAME_PAUSE, Fade);
        }

        void Fade()
        {
            _button.interactable = !PauseScript.IsPaused;
        }
    }
}