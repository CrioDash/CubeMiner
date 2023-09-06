using System;
using System.Collections;
using UnityEngine;

namespace PowerUps
{
    public class TimeSlow: PowerUp
    {
        [SerializeField] private AnimationCurve _animationCurve;
        
        private Coroutine _currentCoroutine; 
        
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
                Time.timeScale = Mathf.Lerp(endScale, startScale, _animationCurve.Evaluate(t/Duration));
                t += Time.unscaledDeltaTime;
                yield return null;
            }
            
        }
    }
}