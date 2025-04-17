using Project.Scripts;
using Project.Scripts.Addressables;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace NextLevelLoader
{
    public class NextLevel
    {
        private Button _reloadButton;
        private Button _ButtonAds;
        private GameObject _panel;
        private readonly Collider _door;
        private readonly IAssetProvider _assetProvider;
        private readonly Canvas _canvas;
        private readonly SceneLoader _sceneLoader;
        private readonly RewardedAds _rewardedAds;
        private readonly InterstitialAdExample _interstitialAdExample;
        public RewardedAds RewardedAds => _rewardedAds;
        public Button PanelAds => _ButtonAds;

        public NextLevel(Collider door, GameObject _panelObject, IAssetProvider assetProvider, Canvas canvas, SceneLoader sceneLoader, RewardedAds rewardedAds, InterstitialAdExample interstitialAdExample)
        {
            _door = door;
            _assetProvider = assetProvider;
            _panel = _panelObject;
            _canvas = canvas;
            _sceneLoader = sceneLoader;
            _rewardedAds = rewardedAds;
            _interstitialAdExample = interstitialAdExample;
        }

        public void EnableCollider()
        {
            _door.enabled = true;
        }

        public void DisablePanels()
        {
            _door.enabled = false;
            _panel.SetActive(false);
        }

        public async Task EnablePanelAsync(bool showAdButton)
        {
            _panel = await _assetProvider.LoadPanelPrefabAsync();

            _panel = GameObject.Instantiate(_panel);
            _panel.transform.SetParent(_canvas.transform, false);
            _panel.SetActive(true);

            _reloadButton = _panel.GetComponentInChildren<Button>();
            _reloadButton.onClick.AddListener(OnReloadButtonClicked);

            if (showAdButton)
            {
                _ButtonAds = await _assetProvider.LoadRewardAdsbAsync();
                _ButtonAds = GameObject.Instantiate(_ButtonAds, _panel.transform);
                _rewardedAds.LoadAd();
                _rewardedAds.SetAdButton(_ButtonAds);
            }

            Time.timeScale = 0;
        }

        private void OnReloadButtonClicked()
        {
            ShowAds();
            _sceneLoader.ReloadScene();
        }

        private void ShowAds()
        {
            _interstitialAdExample.ShowAd();
        }
    }
}