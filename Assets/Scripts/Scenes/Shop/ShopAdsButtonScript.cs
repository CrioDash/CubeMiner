using System;
using Data;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Purchasing;
using UnityEngine.UI;

namespace Scenes.Shop
{
    public class ShopAdsButtonScript:MonoBehaviour
    {

        private Button _button;
        private CodelessIAPButton _iapButton;

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