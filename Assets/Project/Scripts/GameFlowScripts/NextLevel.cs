using Project.Scripts.Addressables;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace NextLevelLoader
{
    public class NextLevel
    {
        private Button _reloadButton;
        private GameObject _panel;
        private GameObject _panelAds;
        private readonly Collider _door;
        private readonly IAssetProvider _assetProvider;
        private readonly Canvas _canvas;
        private readonly SceneLoader _sceneLoader;
        private readonly RewardedAds _rewardedAds;

        public RewardedAds RewardedAds => _rewardedAds;
        public GameObject PanelAds => _panelAds;

        public NextLevel(Collider door, GameObject _panelObject, IAssetProvider assetProvider, Canvas canvas, SceneLoader sceneLoader, RewardedAds rewardedAds)
        {
            _door = door;
            _assetProvider = assetProvider;
            _panel = _panelObject;
            _canvas = canvas;
            _sceneLoader = sceneLoader;
            _rewardedAds = rewardedAds;
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

        public async Task EnablePanelAsync()
        {
            _panel = await _assetProvider.LoadPanelPrefabAsync();

            _panel = GameObject.Instantiate(_panel);
            _panel.transform.SetParent(_canvas.transform, false);
            _panel.SetActive(true);

            _reloadButton = _panel.GetComponentInChildren<Button>();
            _reloadButton.onClick.AddListener(OnReloadButtonClicked);
            Time.timeScale = 0;
        }

        private void OnReloadButtonClicked()
        {
            _sceneLoader.ReloadScene();
        }

        public async Task EnablePanelAdsAsync()
        {
            _panelAds = await _assetProvider.LoadPanelRewardAdsbAsync();

            _panelAds = GameObject.Instantiate(_panelAds);
            _panelAds.transform.SetParent(_canvas.transform, false);
            _panelAds.SetActive(true);
            _rewardedAds.LoadAd();

            _reloadButton = _panelAds.GetComponentInChildren<Button>();
            _rewardedAds.SetAdButton(_reloadButton);
            Time.timeScale = 0;
        }
    }
}