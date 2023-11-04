using System;
using UnityEngine;
using UnityEngine.UI;

namespace Scenes.Shop
{
    public class ToggleShopScript:MonoBehaviour
    {

        [SerializeField] private CanvasGroup _group;

        private Toggle _toggle;

        private void Awake()
        {
            _toggle = GetComponent<Toggle>();
        }

        private void Start()
        {
            _toggle.onValueChanged.AddListener(delegate
            {
                _group.alpha = _toggle.isOn ? 1 : 0;
                _group.blocksRaycasts = _toggle.isOn;
                _group.gameObject.SetActive(_toggle.isOn);
            });
        }
    }
}