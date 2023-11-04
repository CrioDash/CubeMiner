using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Data
{
    public class GetToolInfoScript:MonoBehaviour
    {
        private void Start()
        {
            Variables.ToolInfo = new Dictionary<Variables.ToolType, List<ToolInfo>>();
            
            List<List<ToolInfo>> toolInfo = new List<List<ToolInfo>>();
            
            toolInfo.Add(Resources.LoadAll<ToolInfo>("Tools/Shovels/").ToList().OrderBy(o => o.Damage).ToList());

            Variables.ToolInfo.Add(Variables.ToolType.Shovel, new List<ToolInfo>());
            
            
            for(int i = 0; i < toolInfo.Count; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Variables.ToolInfo[(Variables.ToolType)i].Add(toolInfo[i][j]);
                }
            }
        }
    }
}