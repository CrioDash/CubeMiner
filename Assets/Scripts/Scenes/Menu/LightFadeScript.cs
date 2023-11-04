using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Menu
{
    public class LightFadeScript:MonoBehaviour
    {

        private Light2D _light;

        private void Awake()
        {
            _light = GetComponent<Light2D>();
        }

        private void Start()
        {
            StartCoroutine(LightFadeRoutine());
        }

        private IEnumerator LightFadeRoutine()
        {
            float startFalloff = _light.falloffIntensity;
            float endFalloff = startFalloff - 0.1f;
            
            float t = 0;
            while (true)
            {
                t = 0;
                while (t < 1)
                {
                    _light.falloffIntensity = Mathf.Lerp(startFalloff, endFalloff, t);
                    t += Time.deltaTime;
                    yield return null;
                }

                t = 0;
                while (t<1)
                {
                    _light.falloffIntensity = Mathf.Lerp(endFalloff, startFalloff, t);
                    t += Time.deltaTime;
                    yield return null;
                }
            }
        }
    }
}