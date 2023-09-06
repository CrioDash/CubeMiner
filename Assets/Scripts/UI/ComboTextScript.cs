using System;
using System.Collections;
using Data;
using PlayerScripts;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace UI
{
    public class ComboTextScript:MonoBehaviour
    {
        [SerializeField] private AnimationCurve animationCurve;
        
        [SerializeField] private GameObject textPrefab;
        [SerializeField] private Transform spawnPos;
        [SerializeField] private float timeToWait;

        private static SlashControlScript _slash;
        
        public static int comboCount = 0;

        private void Start()
        {
            _slash = FindObjectOfType<SlashControlScript>();
            StartCoroutine(ComboCountRoutine());
        }

        private IEnumerator ComboCountRoutine()
        {
            WaitUntil wait = new WaitUntil(() => comboCount != 0);
            WaitForSeconds waitSec = new WaitForSeconds(timeToWait);
            while (true)
            {
                comboCount = 0;
                yield return wait;
                yield return waitSec;

                if (Variables.BestCombo < comboCount)
                    Variables.BestCombo = comboCount;

                if (comboCount < 3)
                {
                    yield return null;
                    continue;
                }
                
                TextMeshProUGUI text = Instantiate(textPrefab, spawnPos).GetComponent<TextMeshProUGUI>();
                text.transform.position = spawnPos.position;
                text.text = "Combo X" + comboCount + "!";

                switch (comboCount)
                {
                    case 3:
                        text.color = Color.yellow;
                        break;
                    case 4:
                        text.color = Color.cyan;
                        break;
                    case >=5:
                        text.color = Color.magenta;
                        break;
                }

                Variables.Score += comboCount * 100;

                StartCoroutine(TextGrowRoutine(text.transform));
                StartCoroutine(TextRotateRoutine(text.transform));
                StartCoroutine(TextFadeRoutine(text));
               
                
                yield return null;
            }
        }

        private IEnumerator TextGrowRoutine(Transform obj)
        {
            float t = 0;

            while (t < 1)
            {
                obj.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, t);
                t += Time.unscaledDeltaTime * 4;
                yield return null;
            }
            obj.transform.localScale = Vector3.one;
        }

        private IEnumerator TextRotateRoutine(Transform obj)
        {
            float t = 0;

            Vector3 eulerAngle = obj.transform.eulerAngles;
            
            float startRotation = obj.transform.eulerAngles.z - 15;
            float endRotation = obj.transform.eulerAngles.z + 15;

            while (true)
            {
                t = 0;

                while (t<1)
                {
                    if (obj == null)
                        yield break;
                    eulerAngle.z = Mathf.Lerp(startRotation, endRotation, animationCurve.Evaluate(t));
                    obj.transform.eulerAngles = eulerAngle;
                    t += Time.unscaledDeltaTime*2;
                    yield return null;
                }

                t = 0;

                while (t<1)
                {
                    if (obj == null)
                        yield break;
                    eulerAngle.z = Mathf.Lerp(endRotation, startRotation, animationCurve.Evaluate(t));
                    obj.transform.eulerAngles = eulerAngle;
                    t += Time.unscaledDeltaTime*2;
                    yield return null;
                }
            }
        }

        private IEnumerator TextFadeRoutine(TextMeshProUGUI txt)
        {
            float t = 0;
            Color startClr = txt.color;
            Color endClr = txt.color;
            endClr.a = 0;
            while (t<1)
            {
                txt.color = Color.Lerp(startClr, endClr, t);
                t += Time.unscaledDeltaTime/2;
                yield return null;
            }
            Destroy(txt.gameObject);
        }
        
    }
}