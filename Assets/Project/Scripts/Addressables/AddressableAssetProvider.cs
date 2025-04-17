using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Threading.Tasks;
using Project.Scripts.Addressables;

public class AssetProvider : IAssetProvider
{
    private const string PlayerPrefabAddress = "Assets/Project/Prefabs/Player.prefab";
    private const string PanelPrefabAdress = "Assets/Project/Prefabs/GameOverPanel.prefab";
    private const string PanelAdsPrefabAdress = "Assets/Project/Prefabs/PanelAds.prefab";

    public async Task<GameObject> LoadPlayerPrefabAsync()
    {
        var handle = Addressables.LoadAssetAsync<GameObject>(PlayerPrefabAddress);
        await handle.Task;
        return handle.Result;
    }

    public async Task<GameObject> LoadPanelPrefabAsync()
    {
        var handle = Addressables.LoadAssetAsync<GameObject>(PanelPrefabAdress);
        await handle.Task;
        return handle.Result;
    }

    public async Task<GameObject> LoadPanelRewardAdsbAsync()
    {
        var handle = Addressables.LoadAssetAsync<GameObject>(PanelAdsPrefabAdress);
        await handle.Task;
        return handle.Result;
    }
}
