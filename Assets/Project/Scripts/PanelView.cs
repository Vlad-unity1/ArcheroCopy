using Project.Scripts;
using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PanelView : MonoBehaviour
{
    [SerializeField] private Button _reloadGameButton;
    [SerializeField] private Button _rewardedAdsButton;

    public event Action ReloadGameClicked;
    public event Action RewardedAdsClicked;

    public void Awake()
    {
        _reloadGameButton.onClick.AddListener(OnReloadGameClicked);
        _rewardedAdsButton.onClick.AddListener(OnRewardedAdsClicked);
    }

    private void OnDestroy()
    {
        _reloadGameButton.onClick.RemoveListener(OnReloadGameClicked);
        _rewardedAdsButton.onClick.RemoveListener(OnRewardedAdsClicked);
    }

    private void OnReloadGameClicked()
    {
        ReloadGameClicked?.Invoke();
    }

    private void OnRewardedAdsClicked()
    {
        RewardedAdsClicked?.Invoke();
    }
}