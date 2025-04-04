using System.Threading.Tasks;

public interface IAssetProvider
{
    Task<T> LoadAssetAsync<T>(string key) where T : UnityEngine.Object;
}