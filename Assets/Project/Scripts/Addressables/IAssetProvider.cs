using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.Addressables
{
    public interface IAssetProvider
    {
        Task<GameObject> LoadPlayerPrefabAsync();
        Task<GameObject> LoadPanelPrefabAsync();
        Task<Button> LoadRewardAdsbAsync();
    }
}