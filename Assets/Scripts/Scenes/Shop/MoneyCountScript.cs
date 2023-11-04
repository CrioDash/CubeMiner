using System;
using System.Collections;
using Data;
using TMPro;
using UnityEngine;

namespace Scenes.Shop
{
    public class MoneyCountScript:MonoBehaviour
    {
        private TextMeshProUGUI _text;
        private int MoneyCount { set; get; }

        private void Awake()
        {
            _text = GetComponent<TextMeshProUGUI>();
            MoneyCount = 0;
        }

        private void Start()
        {
            StartCoroutine(CountMoneyRoutine());
        }

        public IEnumerator CountMoneyRoutine()
        {
            float t = 0;
            float tempMoney = 0;
            while (true)
            {
                if(MoneyCount == PlayerSave.Instance.Money)
                {
                    yield return null;
                    continue;
                }

                string add = "";
                t = 0;
                while (t<1)
                {
                    tempMoney = Mathf.RoundToInt(Mathf.Lerp(MoneyCount, PlayerSave.Instance.Money, t));
                    add = "";
                    for (int i = 0; i < 6 - tempMoney.ToString().Length; i++)
                        add += "0";
                    _text.text = add + tempMoney;
                    t += Time.deltaTime*2;
                    yield return new WaitForSeconds(Time.deltaTime);
                }

                MoneyCount = PlayerSave.Instance.Money;
                add = "";
                for (int i = 0; i < 6 - MoneyCount.ToString().Length; i++)
                    add += "0";
                _text.text = add + MoneyCount;
            }
        }

    }
}