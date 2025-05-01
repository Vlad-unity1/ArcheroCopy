using System.Threading.Tasks;
using Project.Scripts.PanelSettings;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.Addressables
{
    public class AssetProvider : IAssetProvider
    {
        private const string PlayerPrefabAddress = "Assets/Project/Prefabs/Player.prefab";
        private const string PanelPrefabAddress = "Assets/Project/Prefabs/GameOverPanel.prefab";
        private const string ButtonADSAddress = "Assets/Project/Prefabs/ButtonADS.prefab";

        public async Task<GameObject> LoadPlayerPrefabAsync()
        {
            var handle = UnityEngine.AddressableAssets.Addressables.LoadAssetAsync<GameObject>(PlayerPrefabAddress);
            await handle.Task;
            return handle.Result;
        }

        public async Task<PanelView> LoadPanelPrefabAsync()
        {
            var handle = UnityEngine.AddressableAssets.Addressables.InstantiateAsync(PanelPrefabAddress);
            await handle.Task;

            var panelGO = handle.Result;
            var panelView = panelGO.GetComponent<PanelView>();

            return panelView;
        }

        public async Task<Button> LoadRewardAdsbAsync()
        {
            var handle = UnityEngine.AddressableAssets.Addressables.LoadAssetAsync<GameObject>(ButtonADSAddress);
            await handle.Task;

            var prefab = handle.Result;
            var button = prefab.GetComponent<Button>();

            return button;
        }
    }
}
