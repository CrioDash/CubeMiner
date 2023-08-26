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
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe(EventBus.EventType.GAME_END, Fade);
        }

        void Fade()
        {
            _button.interactable = false;
        }
    }
}