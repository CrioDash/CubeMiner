using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEngine;
using static Data.Variables;

namespace Data
{
    [Serializable]
    public class PlayerSave
    {
        [NonSerialized]
        public static PlayerSave Instance;

        public int RecordScore
        {
            get;
            set;
        }

        public List<ToolType> Tools
        {
            get;
            set;
        }

        public ToolType CurrentTool
        {
            get;
            private set;
        }

        public int Money
        {
            get;
            set;
        }

        public bool TutorialCompleted { set; get; }

        public PlayerSave()
        {
            Tools = new List<ToolType>();
            TutorialCompleted = false;
            RecordScore = 0;
            Money = 0;
            CurrentTool = ToolType.WoodShovel;
            Tools.Add(CurrentTool);
        }
        
    }
}