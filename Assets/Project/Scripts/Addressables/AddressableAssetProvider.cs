using System.Threading.Tasks;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Project.Scripts.Addressables
{
    public class AddressableAssetProvider : IAssetProvider
    {
        public async Task<T> LoadAssetAsync<T>(string key) where T : UnityEngine.Object
        {
            AsyncOperationHandle<T> handle = UnityEngine.AddressableAssets.Addressables.LoadAssetAsync<T>(key);
            await handle.Task;
            return handle.Result;
        }
    }
}