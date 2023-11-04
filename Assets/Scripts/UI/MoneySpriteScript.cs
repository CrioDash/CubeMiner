using System;
using System.Collections;
using Data;
using UnityEngine;

namespace UI
{
    public class MoneySpriteScript:MonoBehaviour
    {
        [SerializeField] private GameObject moneySpritePrefab;
        [SerializeField] private Transform endPos;

        public static MoneySpriteScript Instance;

        private ParticleSystem _system;
        
        private void Awake()
        {
            Instance = this;
            _system = GetComponentInChildren<ParticleSystem>();
        }

        public void CreateMoney(int reward, Vector3 pos)
        {
            StartCoroutine(CreateMoneyRoutine(reward, pos));
        }

        private IEnumerator CreateMoneyRoutine(int reward, Vector3 pos)
        {
            float t = 0;

            Vector3 startPos = pos;
            Vector3 endPos = this.endPos.transform.position;

            GameObject gm = Instantiate(moneySpritePrefab, pos, Quaternion.identity);
            gm.transform.localScale *= 1.5f;

            while (t < 1)
            {
                gm.transform.position = Vector3.Lerp(startPos,endPos, t);
                t += Time.deltaTime * 2;
                yield return null;
            }

            Destroy(gm);
            
            _system.Play();

            Variables.Money += reward;
            Variables.Money = Mathf.Clamp(Variables.Money, 0, 999999);
        }
    }
}