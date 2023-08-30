using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    public class GetToolInfoScript:MonoBehaviour
    {
        private void Start()
        {
            Variables.ToolInfo = new Dictionary<Variables.ToolType, ToolInfo>();
            ToolInfo[] info = Resources.LoadAll<ToolInfo>("Tools/");
            for(int i=0;i<info.Length;i++)
            {
                Variables.ToolInfo.Add(info[i].Type, info[i]);
            }
        }
    }
}