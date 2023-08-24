using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    public class BlockInfoScript:MonoBehaviour
    {
        private void Start()
        {
            Variables.BlockInfo = new Dictionary<Variables.BlockType, BlockInfo>();
            BlockInfo[] info = Resources.LoadAll<BlockInfo>("");
            for(int i=0;i<info.Length;i++)
            {
                Variables.BlockInfo.Add((Variables.BlockType)i, info[i]);
            }
        }
    }
}