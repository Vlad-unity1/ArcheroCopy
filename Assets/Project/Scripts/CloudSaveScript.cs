using System.Collections.Generic;
using UnityEngine;
using Unity.Services.CloudSave;
using UnityEngine.UI;
using Unity.Services.Core;
using Unity.Services.CloudSave.Models;

public class CloudSaveScript : MonoBehaviour // пришлось оставить как есть
{
    public Text status;
    public InputField inpf;

    public async void Start()
    {
        await UnityServices.InitializeAsync();
    }

    public async void SaveData()
    {
        var data = new Dictionary<string, object> { { "firstData", inpf.text } };
        await CloudSaveService.Instance.Data.Player.SaveAsync(data);
    }

    public async void LoadData()
    {
        var keys = new HashSet<string> { "firstData" };
        var serverData = await CloudSaveService.Instance.Data.Player.LoadAsync(keys);

        if (serverData.TryGetValue("firstData", out var item))
        {
            inpf.text = item.Value.GetAsString();
        }
        else
        {
            Debug.Log("Key not found!!");
        }
    }

    public async void DeleteKey()
    {
        await CloudSaveService.Instance.Data.Player.DeleteAsync("firstData");
    }

    public async void RetriveAllKeys()
    {
        List<ItemKey> allKeys = await CloudSaveService.Instance.Data.Player.ListAllKeysAsync();

        foreach (var itemKey in allKeys)
        {
            Debug.Log(itemKey.Key);
        }
    }
}
