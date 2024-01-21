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
        private static Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void Start()
        {
            _button.interactable = !PlayerSave.Instance.NoAds;
        }
        
        public static void RemoveAds()
        {
            PlayerSave.Instance.NoAds = true;
            _button.interactable = !PlayerSave.Instance.NoAds;
        }
        
    }
}