using GoogleMobileAds.Api;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;


namespace Scenes.Menu
{
    public class PlayScoreInitializer : MonoBehaviour
    {
        
        private bool connected = false;

        private void Awake()
        {
            PlayGamesPlatform.DebugLogEnabled = true;
            PlayGamesPlatform.Activate();
            MobileAds.Initialize((InitializationStatus initStatus) =>
            {
           
            });
        }

        private void Start()
        {

            PlayGamesPlatform.Instance.Authenticate(ProcessAuthentification);
        }

        private void ProcessAuthentification(SignInStatus status)
        {
            connected = status == SignInStatus.Success;
        }
        
        public void ShowLeaderboard()
        {
            if(!connected)
            {
                PlayGamesPlatform.Activate();
                PlayGamesPlatform.Instance.Authenticate(ProcessAuthentification);
            }
            else
                Social.ShowLeaderboardUI();
        }
        
    }
}
