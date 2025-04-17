using System;
using NextLevelLoader;
using Project.Scripts.Enemies;
using Project.Scripts.PlayerModels;
using Project.Scripts.Players;
using System.Collections.Generic;
using Project.Scripts.Firebase;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using TMPro;
using System.Threading.Tasks;

namespace Project.Scripts
{
    public class GameFlow : IInitializable, ITickable
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
        private readonly NextLevel _nextLevelController;
        private readonly EnemySpawnData[] _enemySpawnData;
        private readonly Slider _experienceSlider;
        private readonly SceneData _sceneData;
        private readonly IAnalyticsService _analyticsService;
        private readonly TextMeshProUGUI _levelText;
        private readonly PlayerPrefsSave _playerPrefsSaver;

        public GameFlow(EnemyFactory enemyFactory, PlayerFactory playerFactory, PlayerSpawnPoint spawnPointPlayer, Joystick joystick,
            NextLevel nextLevelController, EnemySpawnData[] enemySpawnDatas, Slider experienceSlider, SceneData sceneData, IAnalyticsService analyticsService, TextMeshProUGUI textMeshProUGUI, 
            PlayerPrefsSave playerPrefsSaver)
        {
            _enemyFactory = enemyFactory;
            _playerFactory = playerFactory;
            _spawnPointPlayer = spawnPointPlayer;
            _joystick = joystick;
            _nextLevelController = nextLevelController;
            _enemySpawnData = enemySpawnDatas;
            _experienceSlider = experienceSlider;
            _sceneData = sceneData;
            _analyticsService = analyticsService;
            _levelText = textMeshProUGUI;
            _playerPrefsSaver = playerPrefsSaver;
        }

        public void Initialize()
        {
            _nextLevelController.DisablePanels();
            _ = InitializeAsync();
        }

        private async Task InitializeAsync()
        {
            _player = await _playerFactory.CreatePlayerAsync(_spawnPointPlayer, 100, _joystick);
            _enemyFactory.CreateEnemies(_enemySpawnData);
            _enemies = _enemyFactory.Enemies;

            _player.PlayerHealth.OnEntityDeath += RemovePlayer;

            foreach (var enemy in _enemies)
            {
                enemy.EnemyHealth.OnEntityDeath += GetEnemyDeathHandler(enemy);
            }

            PlayerPrefsLoad();
            UpdateExperienceSlider();
        }

        private Action GetEnemyDeathHandler(EnemyModel enemy)
        {
            return () => RemoveEnemy(enemy);
        }

        private void PlayerPrefsLoad()
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

        private async void ShowRewardedAds()
        {
            await _nextLevelController.EnablePanelAdsAsync();
            _nextLevelController.RewardedAds.OnAdWatched += RevivePlayer;
        }

        private async void RevivePlayer()
        {
            _nextLevelController.RewardedAds.OnAdWatched -= RevivePlayer;

            GameObject.Destroy(_nextLevelController.PanelAds);

            _player = await _playerFactory.CreatePlayerAsync(_spawnPointPlayer, 100, _joystick);
            _player.PlayerHealth.OnEntityDeath += RemovePlayer;

            Time.timeScale = 1;
        }

        private async void OnAllPlayersDefeated()
        {
            await _nextLevelController.EnablePanelAsync();
        }

        private void OnAllEnemiesDefeated()
        {
            _nextLevelController.EnableCollider();
            LevelCounting();
        }

        private void RemovePlayer()
        {
            _player.PlayerHealth.OnEntityDeath -= RemovePlayer;
            if (!_rewardAdsComplete)
            {
                ShowRewardedAds();
                _rewardAdsComplete = true;
            }
            else
            {
                _playerPrefsSaver.Clear();
                LogDeathAndBulletsFired();
                OnAllPlayersDefeated();
            }
        }

        private void LogDeathAndBulletsFired()
        {
            _analyticsService.LogEntityDeath(_player.CurrentWeapon.BulletsFired);
        }

        private void UpdateExperienceSlider()
        {
            float currentExperience = _player.CurrentExperience;
            float maxExperience = _sceneData.MaxExperience;

            _experienceSlider.value = Mathf.Clamp(currentExperience / maxExperience, 0f, 1f);
        }
        
        private void LevelCounting()
        {
            _levelCount++;
            _playerPrefsSaver.Save(_player, _levelCount);
            _analyticsService.LogLevelPassed(_levelCount);
            _levelText.text = "Level: " + _levelCount;
        }

        public void Tick()
        {
            _player?.Move();
        }
    }
}