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
        
        //BlockStats

        public static Dictionary<BlockType, BlockInfo> BlockInfo = new Dictionary<BlockType, BlockInfo>();

        public enum BlockType
        {
            Dirt = 0,
            Wood = 1,
        }
        
        public static void ResetStats()
        {
            CurrentHealth = 0;
            Score = 0;
            BlocksCut = 0;
        }
    }
}