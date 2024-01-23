using System.Collections.Generic;
using GoogleMobileAds.Ump.Api;
using UnityEngine;

namespace Scenes.Menu
{
    public class GDPRScript : MonoBehaviour
    {
        ConsentForm _consentForm;
        
        void Start()
        {
            var debugSettings = new ConsentDebugSettings
            {
                DebugGeography = DebugGeography.EEA
            };
            
            ConsentRequestParameters request = new ConsentRequestParameters
            {
                TagForUnderAgeOfConsent = false,
                ConsentDebugSettings = debugSettings,
            };
            
            ConsentInformation.Update(request, OnConsentInfoUpdated);
        }

        void OnConsentInfoUpdated(FormError error)
        {
            if (error != null) return;

            if (ConsentInformation.IsConsentFormAvailable())
            {
                LoadConsentForm();
            }
        }

        void LoadConsentForm()
        {
            ConsentForm.Load(OnLoadConsentForm);
        }

        void OnLoadConsentForm(ConsentForm consentForm, FormError error)
        {
            if (error != null) return;

            _consentForm = consentForm;

            if (ConsentInformation.ConsentStatus == ConsentStatus.Required)
            {
                _consentForm.Show(OnShowForm);
            }
        }


        void OnShowForm(FormError error)
        {
            if (error != null) return;

            LoadConsentForm();
        }
    }

}