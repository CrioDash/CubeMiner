using System;
using System.Collections;
using Data;
using TMPro;
using UnityEngine;

namespace UI
{
    public class BlockPanelScript:MonoBehaviour
    {

        private TMP_InputField _text;

        private void Awake()
        {
            _text = GetComponentInChildren<TMP_InputField>();
        }
        

        private void Update()
        {
            _text.text = BlockSpawner.BlocksCut + "/" + BlockSpawner.currentBlockGoal;
        }
    }
}