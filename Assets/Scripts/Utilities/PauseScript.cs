using PlayerScripts;
using UnityEngine;

namespace Utilities
{
    public static class PauseScript
    {
        public static bool IsPaused
        {
            private set;
            get;
        }
        
        public static void SetPause()
        {
            IsPaused = !IsPaused;
            Time.timeScale = IsPaused ? 0 : 1;
            SlashControlScript.Instance.DisableSlasher();
        }
    }
}