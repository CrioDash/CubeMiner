using System;
using System.Collections;
using Data;
using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Utilities;

namespace Scenes.Menu
{
    public class AdManager:MonoBehaviour
    {
        
        public static AdManager Instance;
        
        private InterstitialAd _interstitialAd;

        private string _adUnitId = "ca-app-pub-2279679565486395/1967623522";

        public static bool isShowing = false;

        private void Start()
        {
            DontDestroyOnLoad(this);
            if (Instance != null && Instance == this)
            {
                Destroy(Instance.gameObject);
            }
            else
            {
                Instance = this;
            }
            MobileAds.Initialize((InitializationStatus initStatus) =>
            {
                LoadInterstitialAd();
            });
            
        }

        private void OnEnable()
        {
            StartCoroutine(SubAd());
        }

        private IEnumerator SubAd()
        {
            yield return new WaitForSeconds(0.5f);
            if(PlayerSave.Instance.NoAds)
                yield break;
            EventBus.Subscribe(EventBus.EventType.GAME_END, ShowAd);
        }

        private IEnumerator UnsubAd()
        {
            yield return new WaitForSeconds(0.5f);
            if(PlayerSave.Instance.NoAds)
                yield break;
            EventBus.Unsubscribe(EventBus.EventType.GAME_END, ShowAd);
        }
        
        private void OnDisable()
        {
            StartCoroutine(UnsubAd());
        }

        public void LoadInterstitialAd()
        {
            if (_interstitialAd != null)
            {
                _interstitialAd.Destroy();
                _interstitialAd = null;
            }
            
            var adRequest = new AdRequest();
            
            InterstitialAd.Load(_adUnitId, adRequest,
                (InterstitialAd ad, LoadAdError error) =>
                {
                    if (error != null || ad == null)
                    {
                        return;
                    }
                    

                    _interstitialAd = ad;
                    _interstitialAd.OnAdFullScreenContentClosed += () => isShowing = false;
                });
            
        }
        
        private void ShowAd()
        {
            if (_interstitialAd != null && _interstitialAd.CanShowAd() && !PlayerSave.Instance.NoAds)
            {
                _interstitialAd.Show();
            }
        }

    }
}