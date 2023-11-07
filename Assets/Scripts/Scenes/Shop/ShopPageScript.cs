using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scenes.Shop
{
    public class ShopPageScript:MonoBehaviour
    {
        [SerializeField] private Material referenceMat;

        private Coroutine _coroutine;
        
        private CanvasGroup _group;
        
        private void Start()
        {
            _group = GetComponent<CanvasGroup>();
        }

        public void ShowPage(bool open)
        {
            if (_coroutine != null) 
                StopCoroutine(_coroutine);
            if (open)
                gameObject.SetActive(true);
            _coroutine = StartCoroutine(open ? ShowPageRoutine() : CloseOptionsRoutine());
        }

        private IEnumerator ShowPageRoutine()
        {
            
            Transform parent = transform.parent;
            Vector3 localPos = transform.localPosition;
            transform.SetParent(null);
            transform.SetParent(parent);
            transform.localPosition = localPos;
            _group.alpha = 1;
            float t = 0;
            while (t< 1)
            {
                _group.alpha = Mathf.Lerp(0, 1, t);
                t += Time.deltaTime*6f;
                yield return null;
            }

            _group.blocksRaycasts = true;
            
        }
        
        private IEnumerator CloseOptionsRoutine()
        {
            float t = 0;
            while (t< 1)
            {
                t += Time.deltaTime*6;
                yield return null;
            }
            _group.alpha = 0;
            _group.blocksRaycasts = false;
            gameObject.SetActive(false);
            
        }
    }
}