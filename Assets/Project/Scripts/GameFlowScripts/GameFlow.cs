using NextLevelLoader;
using Project.Scripts.Enemies;
using Project.Scripts.Firebase;
using Project.Scripts.Installers;
using Project.Scripts.PlayerModels;
using Project.Scripts.Players;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Project.Scripts
{
    public class GameFlow : IInitializable, ITickable, IDisposable
    {
        private int _killsCount;
        private int _levelCount;
        private bool _rewardAdsComplete = false;

        private List<EnemyModel> _enemies;
        private PlayerModel _player;

        private readonly EnemyFactory _enemyFactory;
        private readonly PlayerFactory _playerFactory;
        private readonly PlayerSpawnPoint _spawnPointPlayer;
        private readonly Joystick _joystick;
        private readonly PanelModel _panelModel;
        private readonly PanelPresenter _panelPresenter;
        private readonly EnemySpawnData[] _enemySpawnData;
        private readonly Slider _experienceSlider;
        private readonly SceneData _sceneData;
        private readonly IAnalyticsService _analyticsService;
        private readonly TextMeshProUGUI _levelText;
        private readonly PlayerPrefsSave _playerPrefsSaver;
        private readonly AdsInitializer _adsInitializer;
        private readonly InterstitialAdExample _interstitialAdExample;
        private readonly RewardedAds _rewardedAds;
        private readonly TimeService _timeService;

        public GameFlow(
            EnemyFactory enemyFactory,
            PlayerFactory playerFactory,
            PlayerSpawnPoint spawnPointPlayer,
            Joystick joystick,
            PanelModel panelModel,
            PanelPresenter panelPresenter,
            EnemySpawnData[] enemySpawnData,
            Slider experienceSlider,
            SceneData sceneData,
            IAnalyticsService analyticsService,
            TextMeshProUGUI textMeshProUGUI,
            PlayerPrefsSave playerPrefsSaver,
            AdsInitializer adsInitializer,
            InterstitialAdExample interstitialAdExample, RewardedAds rewardedAds, TimeService timeService
            )
        {
            _enemyFactory = enemyFactory;
            _playerFactory = playerFactory;
            _spawnPointPlayer = spawnPointPlayer;
            _joystick = joystick;
            _panelModel = panelModel;
            _panelPresenter = panelPresenter;
            _enemySpawnData = enemySpawnData;
            _experienceSlider = experienceSlider;
            _sceneData = sceneData;
            _analyticsService = analyticsService;
            _levelText = textMeshProUGUI;
            _playerPrefsSaver = playerPrefsSaver;
            _adsInitializer = adsInitializer;
            _interstitialAdExample = interstitialAdExample;
            _rewardedAds = rewardedAds;
            _timeService = timeService;
        }

        public void Initialize()
        {
            _panelModel.DisablePanels();
            _ = InitializeAsync();
            _adsInitializer.InitializeAds();
            _interstitialAdExample.Initialize();
            _panelPresenter.OnRewardedAdClicked += RevivePlayer;
            _rewardedAds.OnAdWatched += RevivePlayer;
            _rewardAdsComplete = false;
    }

        private async Task InitializeAsync()
        {
            _player = await _playerFactory.CreatePlayerAsync(_spawnPointPlayer, 100, _joystick);
            _enemyFactory.CreateEnemies(_enemySpawnData);
            _enemies = _enemyFactory.Enemies;

            _player.PlayerHealth.OnEntityDeath += RemovePlayer;

            SetupEnemies();
            LoadPlayerPrefs();
            UpdateExperienceSlider();
        }

        private Action GetEnemyDeathHandler(EnemyModel enemy)
        {
            return () => RemoveEnemy(enemy);
        }

        private void LoadPlayerPrefs()
        {
            var savedData = _playerPrefsSaver.Load();
            _player.CurrentExperience = savedData.Experience;
            _levelCount = savedData.Level;
            _levelText.text = "Level: " + _levelCount;
        }


        private void RemoveEnemy(EnemyModel enemy)
        {
            enemy.EnemyHealth.OnEntityDeath -= GetEnemyDeathHandler(enemy);
            _enemies.Remove(enemy);
            _player.PlayerMovement.AddExperience(enemy.EXP);
            UpdateExperienceSlider();
            _playerPrefsSaver.Save(_player, _player.CurrentExperience);
            _killsCount++;

            if (_enemies.Count == 0)
            {
                OnAllEnemiesDefeated();
                _analyticsService.LogEnemyDeath(_killsCount);
            }
        }

        private void OnAllEnemiesDefeated()
        {
            _panelModel.EnableCollider();
            LevelUp();
        }

        private void RemovePlayer()
        {
            _player.PlayerHealth.OnEntityDeath -= RemovePlayer;
            _timeService.Pause();

            if (!_rewardAdsComplete)
            {
                _ = _panelModel.CreatePanelAsync();
            }
            else
            {
                _ = _panelModel.CreatePanelAsync();
                _playerPrefsSaver.Clear();
                LoadPlayerPrefs();
                UpdateExperienceSlider();
                LogDeathAnalytics();
            }
        }

        private async void RevivePlayer()
        {
            _panelPresenter.OnRewardedAdClicked -= RevivePlayer;
            _rewardAdsComplete = true;

            _panelModel.DisablePanels();
            _player = await _playerFactory.CreatePlayerAsync(_spawnPointPlayer, 100, _joystick);
            _player.PlayerHealth.OnEntityDeath += RemovePlayer;
            _timeService.SetPlayerModel(_player);
            _timeService.Continue();
        }

        private void UpdateExperienceSlider()
        {
            float current = _player.CurrentExperience;
            float max = _sceneData.MaxExperience;
            _experienceSlider.value = Mathf.Clamp(current / max, 0f, 1f);
        }

        private void LevelUp()
        {
            _levelCount++;
            _playerPrefsSaver.Save(_player, _levelCount);
            _analyticsService.LogLevelPassed(_levelCount);
            _levelText.text = "Level: " + _levelCount;
        }

        private void LogDeathAnalytics()
        {
            _analyticsService.LogEntityDeath(_player.CurrentWeapon.BulletsFired);
        }

        private void SetupEnemies()
        {
            foreach (var enemy in _enemies)
            {
                enemy.EnemyHealth.OnEntityDeath += GetEnemyDeathHandler(enemy);
                _timeService.SetPEnemyModel(enemy);
            }
        }

        public void Tick()
        {
            _player?.Move();
        }

        public void Dispose()
        {
            _panelPresenter.OnRewardedAdClicked -= RevivePlayer;
            _rewardedAds.OnAdWatched -= RevivePlayer;
        }
    }
}
