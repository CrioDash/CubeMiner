using System;
using System.Collections;
using Data;
using TMPro;
using UnityEngine;

namespace UI
{
    public class ScorePanelScript:MonoBehaviour
    {

        private int _score;

        private TextMeshProUGUI _text;

        private void Awake()
        {
            _score = Variables.Score;
            _text = GetComponentInChildren<TextMeshProUGUI>();
        }

        private void Start()
        {
            StartCoroutine(UpdateScoreRoutine());
            
        }

        private IEnumerator UpdateScoreRoutine()
        {

            int lastScore = Variables.Score;

            WaitForSeconds wait = new WaitForSeconds(0.2f);
            WaitForSeconds frame = new WaitForSeconds(Time.deltaTime);

            float t = 0;

            string add = "";
            for (int i = 0; i <= (8 - lastScore.ToString().Length); i++)
                add += "0";
            _text.text = add;
            
            while (true)
            {
                t = 0;
                lastScore = Variables.Score;
                
                if (lastScore == _score)
                {
                    yield return frame;
                    continue;
                }

                add = "";
                for (int i = 0; i < (8 - lastScore.ToString().Length); i++)
                    add += "0";
                
                while (t < 1)
                {
                    
                    _text.text = add + ((int)Mathf.Lerp(_score, lastScore, t));
                    yield return frame;
                    t += Time.deltaTime * 8;
                }
                
                _text.text = add + lastScore;

                _score = lastScore;
                
                yield return wait;
            }
        }
        
    }
}