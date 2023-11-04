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

        public Dictionary<ToolType, int> ToolLevel = new Dictionary<ToolType, int>();

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

        public Dictionary<PowerType, int> powerupLevels = new Dictionary<PowerType, int>();

        public bool TutorialCompleted { set; get; }

        public PlayerSave()
        {
            powerupLevels.Add(PowerType.TimeSlow, 0);
            powerupLevels.Add(PowerType.ScoreMultiplier, 0);
            powerupLevels.Add(PowerType.Magnet, 0);
            TutorialCompleted = false;
            RecordScore = 0;
            Money = 50000;
            CurrentTool = ToolType.Shovel;
            ToolLevel.Add(CurrentTool, 0);
        }
        
    }
}