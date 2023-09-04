using System.Collections;
using Data;
using TMPro;
using UnityEngine;

namespace UI
{
    public class MoneyPanelScript:MonoBehaviour
    {
        private int _money;

        private TextMeshProUGUI _text;

        private void Awake()
        {
            _money = Variables.Money;
            _text = GetComponentInChildren<TextMeshProUGUI>();
        }

        private void Start()
        {
            StartCoroutine(UpdateMoneyRoutine());
        }

        private IEnumerator UpdateMoneyRoutine()
        {

            int lastScore = Variables.Money;

            WaitForSeconds wait = new WaitForSeconds(0.1f);
            WaitForSeconds frame = new WaitForSeconds(Time.deltaTime);

            float t = 0;

            while (true)
            {
                t = 0;
                lastScore = Variables.Money;
                
                if (lastScore == _money)
                {
                    yield return frame;
                    continue;
                }

                while (t < 1)
                {
                    
                    _text.text = ((int)Mathf.Lerp(_money, lastScore, t)).ToString();
                    yield return frame;
                    t += Time.deltaTime * 8;
                }
                
                _text.text = lastScore.ToString();

                _money = lastScore;
                
                yield return wait;
            }
        }
    }
}