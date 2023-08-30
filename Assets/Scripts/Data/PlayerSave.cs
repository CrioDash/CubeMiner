using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
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
            private set;
        }
        
        public ToolType Tool
        {
            get;
            private set;
        }

        public bool TutorialCompleted { set; get; }

        public PlayerSave()
        {
            TutorialCompleted = false;
            RecordScore = 0;
            Tool = ToolType.WoodShovel;
        }
        
    }
}