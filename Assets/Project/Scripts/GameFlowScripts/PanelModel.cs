using Project.Scripts;
using Project.Scripts.Addressables;
using System.Threading.Tasks;
using UnityEngine;

namespace NextLevelLoader
{
    public class PanelModel
    {
        private readonly Collider _door;
        private readonly IAssetProvider _assetProvider;
        private readonly Canvas _canvas;
        private readonly SceneLoader _sceneLoader;
        private readonly RewardedAds _rewardedAds;
        private readonly InterstitialAdExample _interstitialAdExample;
        private PanelView _currentPanelView;

        public PanelModel(Collider door, IAssetProvider assetProvider, Canvas canvas, SceneLoader sceneLoader, RewardedAds rewardedAds, InterstitialAdExample interstitialAdExample, PanelView currentPanelView)
        {
            _door = door;
            _assetProvider = assetProvider;
            _canvas = canvas;
            _sceneLoader = sceneLoader;
            _rewardedAds = rewardedAds;
            _interstitialAdExample = interstitialAdExample;
            _currentPanelView = currentPanelView;
        }

        public void ReloadScene()
        {
            ShowInterstitialAd();
            _sceneLoader.ReloadScene();
        }

        public void ShowRewardedAd()
        {
            _rewardedAds.LoadAd();
            _rewardedAds.ShowAd();
        }

        public async Task CreatePanelAsync()
        {
            _currentPanelView = await _assetProvider.LoadPanelPrefabAsync();
            _currentPanelView.transform.SetParent(_canvas.transform, false);

            _currentPanelView.ReloadGameClicked += ReloadScene;
            _currentPanelView.RewardedAdsClicked += ShowRewardedAd;

            _currentPanelView.gameObject.SetActive(true);
            Time.timeScale = 0;
        }

        public void EnableCollider()
        {
            _door.enabled = true;
        }

        public void DisablePanels()
        {
            _door.enabled = false;
            _currentPanelView.gameObject.SetActive(false);
        }

        private void ShowInterstitialAd()
        {
            _interstitialAdExample.ShowAd();
        }
    }
}