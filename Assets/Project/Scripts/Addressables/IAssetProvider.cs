using System.Threading.Tasks;

namespace Project.Scripts.Addressables
{
    public interface IAssetProvider
    {
        Task<T> LoadAssetAsync<T>(string key) where T : UnityEngine.Object;
    }
}