using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ButtonShaderSparkle:MonoBehaviour
    {
        private Image _imageBtn;
        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _imageBtn = GetComponent<Image>();
        }

        private void Update()
        {
            if(_button.interactable)
                _imageBtn.material.SetFloat("_unscaledTime", Time.unscaledTime);
        }
    }
}