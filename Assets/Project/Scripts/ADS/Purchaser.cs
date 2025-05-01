using UnityEngine;
using UnityEngine.Purchasing;

public class Purchaser : MonoBehaviour
{
    public void OnPurchaseCompleted(Product product)
    {
        switch (product.definition.id)
        {
            case "RemoveAdd":
                RemoveAds();
                break;
        }
    }

    private void RemoveAds()
    {
        PlayerPrefs.SetInt("removeads", 1);
        Debug.Log("Purchase: removeads");
    }
}