using System;
using UnityEngine;
using UnityEngine.UI;

namespace Scenes.Shop
{
    public class ExitButtonScript:MonoBehaviour
    {
        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(() => SceneSwitcher.Instance.LoadScene("MenuScene"));
        }
    }
}