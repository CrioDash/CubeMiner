using System.Collections.Generic;
using UI;

namespace Data
{
    public static class Variables
    {
        
        //PlayerStats
        public static int Damage = 1;

        public static int MaxHealth = 5;
     
        public static int CurrentHealth = 0;

        public static int Score = 0;

        public static int BlocksCut = 0;
        public static int BlocksFall = 0;

        public static int BestCombo = 0;
        
        //BlockStats

        public static Dictionary<BlockType, BlockInfo> BlockInfo = new Dictionary<BlockType, BlockInfo>();
        public static Dictionary<ToolType, ToolInfo> ToolInfo = new Dictionary<ToolType, ToolInfo>();

        public enum BlockType
        {
            Dirt = 0,
            Wood = 1,
            Snow = 2,
            Gold = 3
        }
        
        public enum ToolType
        {
            WoodShovel,
            StoneShovel,
            IronShovel,
            DiamondShovel,
            WoodAxe,
            StoneAxe,
            IronAxe,
            DiamondAxe,
            WoodPickaxe,
            StonePickaxe,
            IronPickaxe,
            DiamondPickaxe,
        }
        
        public static void ResetStats()
        {
            BestCombo = 0;
            CurrentHealth = 0;
            Score = 0;
            BlocksCut = 0;
            BlocksFall = 0;
            BlockSpawner.BlocksCut = 0;
        }

        public static void UpdateVariables(PlayerSave save)
        {
            Damage = ToolInfo[PlayerSave.Instance.CurrentTool].Damage;
        }
    }
}