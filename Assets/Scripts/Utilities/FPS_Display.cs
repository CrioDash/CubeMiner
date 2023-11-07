using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class FPS_Display : MonoBehaviour
{
    private TextMeshProUGUI _text;

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        _text.text = ((int)(1 / Time.smoothDeltaTime)).ToString();
    }
}
