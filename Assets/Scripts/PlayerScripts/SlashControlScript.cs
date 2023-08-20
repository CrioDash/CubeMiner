using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace PlayerScripts
{
    public class SlashControlScript : MonoBehaviour
    {
        private delegate void CheckOS();

        private CheckOS checkOS;
        private TrailRenderer _trail;
        private BoxCollider2D _circle;
        private Vector3 _lastClickPos = Vector3.zero;
        
        

        private void Awake()
        {
            _trail = GetComponent<TrailRenderer>();
            _circle = GetComponent<BoxCollider2D>();
        }

        private void Start()
        {
            if (Application.platform != RuntimePlatform.Android)
                checkOS = WindowsCheck;
            else
                checkOS = AndroidCheck;
        }

        private void Update()
        {
            checkOS();
        }

        private void AndroidCheck()
        {
            if (!Touchscreen.current.primaryTouch.isInProgress)
            {
                _trail.emitting = false;
                _circle.enabled = false;
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
            }

            _lastClickPos = pos;
        }

        private void WindowsCheck()
        {
            if (!Mouse.current.leftButton.isPressed)
            {
                _trail.emitting = false;
                _circle.enabled = false;
                _lastClickPos = Vector3.zero;
                return;
            }
            Vector3 pos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

            pos.z = 0;
            transform.position = pos;

            if (_lastClickPos != Vector3.zero && Math.Abs((_lastClickPos - pos).magnitude) > 0.01f)
            {
                _trail.emitting = true;
                _circle.enabled = true;
            }
            
            _lastClickPos = pos;
        }
    }
}