using System.Threading.Tasks;
using UnityEngine;

namespace Project.Scripts.Addressables
{
    public interface IAssetProvider
    {
        Task<GameObject> LoadPlayerPrefabAsync();
        Task<GameObject> LoadPanelPrefabAsync();
        Task<GameObject> LoadPanelRewardAdsbAsync();
    }
}