using System;
using System.Collections;
using Data;
using UnityEngine;

using Utilities;

namespace Scenes.Menu
{
    public class InterstitialAdTest : MonoBehaviour
    {
        private void Start()
        {
            IronSource.Agent.loadInterstitial();
        }

        private void OnEnable()
        {
            EventBus.Subscribe(EventBus.EventType.GAME_END, CallAd);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe(EventBus.EventType.GAME_END, CallAd);
        }

        public void CallAd()
        {
            IronSource.Agent.showInterstitial();
        }
    }
}
