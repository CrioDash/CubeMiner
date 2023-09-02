using System;
using System.Collections;
using Data;
using PlayerScripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ComboTextScript:MonoBehaviour
    {
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
                text.text = comboCount + "X:Combo";

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

                StartCoroutine(TextFade(text));
                
                yield return null;
            }
        }

        private IEnumerator TextFade(TextMeshProUGUI txt)
        {
            float t = 0;
            Color startClr = txt.color;
            Color endClr = txt.color;
            endClr.a = 0;
            while (t<1)
            {
                txt.color = Color.Lerp(startClr, endClr, t);
                t += Time.fixedDeltaTime;
                yield return null;
            }
            Destroy(txt.gameObject);
        }
        
    }
}