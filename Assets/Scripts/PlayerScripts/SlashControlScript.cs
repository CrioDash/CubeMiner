using System;
using System.Collections;
using Data;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Utilities;

namespace PlayerScripts
{
    public class SlashControlScript : MonoBehaviour
    {
        private delegate void CheckOS();

        private CheckOS checkOS;
        private TrailRenderer _trail;
        private BoxCollider2D _circle;
        private Vector3 _lastClickPos = Vector3.zero;

        private SpriteRenderer _instrument;

        private void Awake()
        {
            _instrument = GetComponentInChildren<SpriteRenderer>();
            _trail = GetComponent<TrailRenderer>();
            _circle = GetComponent<BoxCollider2D>();
        }

        private void Start()
        {

            _instrument.sprite = Variables.ToolInfo[PlayerSave.Instance.CurrentTool].Sprite;
            
            if (Application.platform != RuntimePlatform.Android)
                checkOS = WindowsCheck;
            else
                checkOS = AndroidCheck;
            StartCoroutine(InstrumentRotateRoutine());
        }

        private void OnEnable()
        {
            EventBus.Subscribe(EventBus.EventType.GAME_PAUSE, DisableSlasher);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe(EventBus.EventType.GAME_PAUSE, DisableSlasher);
        }

        private void Update()
        {
            if (PauseScript.IsPaused)
                return;
            checkOS();
        }

        private void AndroidCheck()
        {
            if (!Touchscreen.current.primaryTouch.isInProgress)
            {
                
                _trail.emitting = false;
                _circle.enabled = false;
                _instrument.enabled = false;
                _lastClickPos = Vector3.zero;
                return;
            }
            Vector3 pos = Camera.main.ScreenToWorldPoint(Touchscreen.current.primaryTouch.position.ReadValue());

            pos.z = 0;
            transform.position = pos;

            if (_lastClickPos != Vector3.zero && Math.Abs((_lastClickPos - pos).magnitude) > 0.01f)
            {
                _trail.emitting = true;
                _circle.enabled = true;
                _instrument.enabled = true;
            }

            _lastClickPos = pos;
        }

        private void WindowsCheck()
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

            pos.z = 0;
            transform.position = pos;
            
            if (!Mouse.current.leftButton.isPressed)
            {
                _trail.emitting = false;
                _circle.enabled = false;
                _instrument.enabled = false;
                _lastClickPos = Vector3.zero;
                return;
            }

            if (_lastClickPos != Vector3.zero && Math.Abs((_lastClickPos - pos).magnitude) > 0.01f)
            {
                _trail.emitting = true;
                _circle.enabled = true;
                _instrument.enabled = true;
            }
            
            _lastClickPos = pos;
        }

        private IEnumerator InstrumentRotateRoutine()
        {
            float t = 0;
            
            float minAngle = 65;
            float maxAngle = 115;
            
            Vector3 startVec = Vector3.zero;
            Vector3 endVec = Vector3.zero;
            
            startVec.z = minAngle;
            endVec.z = maxAngle;
            while (true)
            {
                t = 0;
                while (t < 1)
                {
                    
                    _instrument.transform.eulerAngles = Vector3.Lerp(startVec, endVec, t);
                    t += Time.fixedDeltaTime * 2;
                    yield return null;
                }

                t = 0;
                while (t <1)
                {
                    _instrument.transform.eulerAngles = Vector3.Lerp(endVec, startVec, t);
                    t += Time.fixedDeltaTime * 2;
                    yield return null;
                }
            }
        }

        public void DisableSlasher()
        {
            _trail.emitting = false;
            _circle.enabled = false;
            _instrument.enabled = false;
        }
    }
}