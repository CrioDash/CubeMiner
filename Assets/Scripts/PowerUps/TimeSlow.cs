using System;
using System.Collections;
using Data;
using UnityEngine;
using Utilities;

namespace PowerUps
{
    public class TimeSlow: PowerUp
    {
        [SerializeField] private AnimationCurve _animationCurve;
        
        private Coroutine _currentCoroutine; 
        
        private void Awake()
        {
            Duration += PlayerSave.Instance.powerupLevels[Type] * 0.5f;
        }
        
        public override void UsePowerUp()
        {
            if(_currentCoroutine!=null)
                StopCoroutine(_currentCoroutine);
            _currentCoroutine = StartCoroutine(SlowTimeRoutine());
            
        }

        public override void RemovePowerUp()
        {
            StopCoroutine(_currentCoroutine);
            Time.timeScale = 1f;
        }

        private IEnumerator SlowTimeRoutine()
        {
            float t = 0;
            float startScale = 1;
            float endScale = 0.35f;
            
            while (t < Duration)
            {
                while (PauseScript.IsPaused)
                {
                    yield return null;
                }
                Time.timeScale = Mathf.Lerp(endScale, startScale, _animationCurve.Evaluate(t/Duration));
                t += Time.unscaledDeltaTime;
                yield return null;
            }
            
        }
    }
}