using System.Collections.Generic;
using Fruit;
using UI;

namespace Data
{
    public static class Variables
    {
        
        //PlayerStats
        public static int Damage = 1;

        public static int MaxHealth = 5;
     
        public static int CurrentHealth = 0;

        public static int ScoreMultiplier = 1;

        public static int Score = 0;
        public static int Money = 0;

        public static int BlocksCut = 0;
        public static int BlocksFall = 0;

        public static int BestCombo = 0;
        
        //BlockStats

        public static Dictionary<BlockType, BlockInfo> BlockInfo = new Dictionary<BlockType, BlockInfo>();
        
        //ToolStats

        public static Dictionary<ToolType, List<ToolInfo>> ToolInfo = new Dictionary<ToolType, List<ToolInfo>>();
        

        public enum BlockType
        {
            Dirt = 0,
            Sand = 1,
            Snow = 2,
            Gravel = 3,
            Megadirt = 4
        }
        
        public enum ToolType
        {
            Shovel,
            Axe,
            Pickaxe
        }

        public enum PowerType
        {
            TimeSlow = 0,
            ScoreMultiplier = 1, 
            Magnet = 2
        }
        
        public static void ResetStats()
        {
            EndGamePanelScript.Coroutine = null;
            Dynamite.dynamite = null;
            ScoreMultiplier = 1;
            Money = 0;
            BestCombo = 0;
            CurrentHealth = 0;
            Score = 0;
            BlocksCut = 0;
            BlocksFall = 0;
            BlockSpawner.BlocksCut = 0;
        }

        public static void UpdateVariables()
        {
            Damage = ToolInfo[PlayerSave.Instance.CurrentTool][PlayerSave.Instance.ToolLevel[PlayerSave.Instance.CurrentTool]].Damage;
        }
    }
}