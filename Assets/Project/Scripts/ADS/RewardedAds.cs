﻿using System;
using UnityEngine;
using UnityEngine.Advertisements;

namespace Project.Scripts.ADS
{
    public class RewardedAds : IUnityAdsLoadListener, IUnityAdsShowListener
    {
        public event Action OnAdWatched;
        private readonly string _androidAdUnitId = "Rewarded_Android";
        private readonly string _iOSAdUnitId = "Rewarded_iOS";
        private string _adUnitId = null; // This will remain null for unsupported platforms


        // Call this public method when you want to get an ad ready to show.
        public void LoadAd()
        {
            // IMPORTANT! Only load content AFTER initialization (in this example, initialization is handled in a different script).
            if (PlayerPrefs.GetInt("removeads", 0) == 1)
            {
                Debug.Log("Ads are disabled, not loading ad.");
                OnAdWatched?.Invoke();
                return; 
            }

            _adUnitId = _iOSAdUnitId;
            _adUnitId = _androidAdUnitId;
            Debug.Log("Loading Ad: " + _adUnitId);
            Advertisement.Load(_adUnitId, this);
        }

        // If the ad successfully loads, add a listener to the button and enable it:
        public void OnUnityAdsAdLoaded(string adUnitId)
        {
            Debug.Log("Ad Loaded: " + adUnitId);

            if (adUnitId.Equals(_adUnitId))
            {
                // Configure the button to call the ShowAd() method when clicked:

                // Enable the button for users to click:

            }
        }

        // Implement a method to execute when the user clicks the button:
        public void ShowAd()
        {
            if (PlayerPrefs.GetInt("removeads", 0) == 1)
            {
                Debug.Log("Ads are disabled, not showing ad.");
                return;
            }

            Advertisement.Show(_adUnitId, this);
        }

        // Implement the Show Listener's OnUnityAdsShowComplete callback method to determine if the user gets a reward:
        public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
        {
            if (adUnitId.Equals(_adUnitId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
            {
                Debug.Log("Unity Ads Rewarded Ad Completed");
                OnAdWatched?.Invoke();
                // Grant a reward.
                OnDestroy();
            }
        }

        // Implement Load and Show Listener error callbacks:
        public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
        {
            Debug.Log($"Error loading Ad Unit {adUnitId}: {error.ToString()} - {message}");
            // Use the error details to determine whether to try to load another ad.
        }

        public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
        {
            Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
            // Use the error details to determine whether to try to load another ad.
        }

        public void OnUnityAdsShowStart(string adUnitId) { }
        public void OnUnityAdsShowClick(string adUnitId) { }

        void OnDestroy()
        {
            // Clean up the button listeners:

        }
    }
}