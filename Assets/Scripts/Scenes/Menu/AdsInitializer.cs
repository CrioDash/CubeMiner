using System;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;
using UnityEngine.Purchasing;

namespace Scenes.Menu
{
    public class AdsInitializer : MonoBehaviour
    {

        public static bool IsOpened = false;
        private bool connected = false;

        private void Awake()
        {
            PlayGamesPlatform.DebugLogEnabled = true;
            PlayGamesPlatform.Activate();
        }

        private void Start()
        {
            IronSource.Agent.init("1c95909b5");
            IronSource.Agent.validateIntegration();

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

        private void OnEnable()
        {
            IronSourceEvents.onSdkInitializationCompletedEvent += SdkInitializationCompletedEvent;
            IronSourceInterstitialEvents.onAdReadyEvent += InterstitialOnAdReadyEvent;
            IronSourceInterstitialEvents.onAdLoadFailedEvent += InterstitialOnAdLoadFailed;
            IronSourceInterstitialEvents.onAdOpenedEvent += InterstitialOnAdOpenedEvent;
            IronSourceInterstitialEvents.onAdClickedEvent += InterstitialOnAdClickedEvent;
            IronSourceInterstitialEvents.onAdShowSucceededEvent += InterstitialOnAdShowSucceededEvent;
            IronSourceInterstitialEvents.onAdShowFailedEvent += InterstitialOnAdShowFailedEvent;
            IronSourceInterstitialEvents.onAdClosedEvent += InterstitialOnAdClosedEvent;
        }

        
        
       
        void InterstitialOnAdReadyEvent(IronSourceAdInfo adInfo) {
        }

        void InterstitialOnAdLoadFailed(IronSourceError ironSourceError) {
        }

        void InterstitialOnAdOpenedEvent(IronSourceAdInfo adInfo) {
        }

        void InterstitialOnAdClickedEvent(IronSourceAdInfo adInfo) {
        }

        void InterstitialOnAdShowFailedEvent(IronSourceError ironSourceError, IronSourceAdInfo adInfo) {
        }

        void InterstitialOnAdClosedEvent(IronSourceAdInfo adInfo)
        {
            IsOpened = false;
        }

        void InterstitialOnAdShowSucceededEvent(IronSourceAdInfo adInfo)
        {
            IsOpened = true;
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            IronSource.Agent.onApplicationPause(pauseStatus);
        }
        
        private void SdkInitializationCompletedEvent(){}
        
    }
}
