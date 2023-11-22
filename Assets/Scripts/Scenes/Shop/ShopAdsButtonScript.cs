using System;
using Data;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

namespace Scenes.Shop
{
    public class ShopAdsButtonScript:MonoBehaviour
    {
        
        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void Start()
        {
            _button.interactable = !PlayerSave.Instance.NoAds;
        }

        public void RemoveAds()
        {
            PlayerSave.Instance.NoAds = true;
            _button.interactable = !PlayerSave.Instance.NoAds;
        }
    }
}