using System;
using UnityEngine;
using UnityEngine.UI;

namespace Scenes.Shop
{
    public class ToggleShopScript:MonoBehaviour
    {

        [SerializeField] private ShopPageScript _pageScript;

        private Toggle _toggle;

        private void Awake()
        {
            _toggle = GetComponent<Toggle>();
        }

        private void Start()
        {
            _toggle.onValueChanged.AddListener(delegate
            {
               _pageScript.ShowPage(_toggle.isOn);
               _toggle.interactable = !_toggle.isOn;
            });
        }
    }
}