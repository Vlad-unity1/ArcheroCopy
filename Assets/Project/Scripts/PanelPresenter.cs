using NextLevelLoader;
using System;
using UnityEngine;

public class PanelPresenter
{
    private readonly PanelView _view;
    private readonly PanelModel _nextLevel;

    public event Action OnRewardedAdClicked;

    public PanelPresenter(PanelView view, PanelModel nextLevel)
    {
        _view = view;
        _nextLevel = nextLevel;

        _view.ReloadGameClicked += OnReloadClicked;
        _view.RewardedAdsClicked += OnRewardedAdClickedInternal;
    }

    private void OnReloadClicked()
    {
        _nextLevel.ReloadScene();
        _nextLevel.DisablePanels();
    }

    public void OnRewardedAdClickedInternal()
    {
        _nextLevel.ShowRewardedAd();
        _nextLevel.DisablePanels();
        OnRewardedAdClicked?.Invoke();
    }

    public void Dispose()
    {
        _view.ReloadGameClicked -= OnReloadClicked;
        _view.RewardedAdsClicked -= OnRewardedAdClicked;
    }
}