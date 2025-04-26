using UnityEngine;
using UnityEngine.Advertisements;

namespace Project.Scripts.Installers
{
    public class AdsInitializer : IUnityAdsInitializationListener
    {
        private readonly string _androidGameId = "5834596";
       // private readonly string _iOSGameId = "5834597";
        private readonly bool _testMode = true;
        private string _gameId;

        public void InitializeAds()
        {
#if UNITY_IOS
            _gameId = _iOSGameId;
#elif UNITY_ANDROID
            _gameId = _androidGameId;
#elif UNITY_EDITOR
            _gameId = _androidGameId; //Only for testing the functionality in the Editor
#endif
            if (!Advertisement.isInitialized && Advertisement.isSupported)
            {
                Advertisement.Initialize(_gameId, _testMode, this);
            }
        }


        public void OnInitializationComplete()
        {
            Debug.Log("Unity Ads initialization complete.");
        }

        public void OnInitializationFailed(UnityAdsInitializationError error, string message)
        {
            Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
        }
    }
}