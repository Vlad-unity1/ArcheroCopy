using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Threading.Tasks;
using Project.Scripts.Addressables;
using UnityEngine.UI;

public class AssetProvider : IAssetProvider
{
    private const string PlayerPrefabAddress = "Assets/Project/Prefabs/Player.prefab";
    private const string PanelPrefabAdress = "Assets/Project/Prefabs/GameOverPanel.prefab";
    private const string ButtonADSAdress = "Assets/Project/Prefabs/ButtonADS.prefab";

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

    public async Task<Button> LoadRewardAdsbAsync()
    {
        var handle = Addressables.LoadAssetAsync<GameObject>(ButtonADSAdress);
        await handle.Task;

        var prefab = handle.Result;
        var button = prefab.GetComponent<Button>();

        return button;
    }
}
