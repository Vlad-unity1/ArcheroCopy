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

        public GameFlow(EnemyFactory enemyFactory, PlayerFactory playerFactory, PlayerSpawnPoint spawnPointPlayer, Joystick joystick,
            NextLevel nextLevelController, EnemySpawnData[] enemySpawnDatas, Slider experienceSlider, SceneData sceneData, IAnalyticsService analyticsService, TextMeshProUGUI textMeshProUGUI)
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
                enemy.EnemyHealth.OnEntityDeath += () => RemoveEnemy(enemy);
            }

            float savedExperience = PlayerPrefs.GetFloat("EXP", 0);
            _player.CurrentExperience = savedExperience;
            _levelCount = PlayerPrefs.GetInt("LEVEL", 0);
            _levelText.text = "Level: " + _levelCount;
            UpdateExperienceSlider();
        }


        private void RemoveEnemy(EnemyModel enemy)
        {
            enemy.EnemyHealth.OnEntityDeath -= () => RemoveEnemy(enemy);
            _enemies.Remove(enemy);
            _player.PlayerMovement.AddExperience(enemy.EXP);
            UpdateExperienceSlider();
            _nextLevelController.SaveExperience(_player.CurrentExperience);
            _killsCount++;

            if (_enemies.Count == 0)
            {
                OnAllEnemiesDefeated();
                _analyticsService.LogEvent("enemy_death", 
                    ("kills_count", _killsCount));
            }
        }

        private void OnAllEnemiesDefeated()
        {
            _nextLevelController.EnableCollider();
            LevelCounting();
        }

        private void RemovePlayer()
        {
            _player.PlayerHealth.OnEntityDeath -= RemovePlayer;
            PlayerPrefs.DeleteKey("EXP");
            PlayerPrefs.DeleteKey("LEVEL");
            OnAllPlayersDefeated();
            LogDeathAndBulletsFired();
        }

        private void LogDeathAndBulletsFired()
        {
            _analyticsService.LogEvent("entity_death",
                ("bullets_fired", _player.CurrentWeapon.BulletsFired));
        }

        private void OnAllPlayersDefeated()
        {
            _nextLevelController.EnablePanel();
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
            PlayerPrefs.SetInt("LEVEL", _levelCount);
            PlayerPrefs.Save();
            _analyticsService.LogEvent("level_passed", ("levels_number", _levelCount));
            _levelText.text = "Level: " + _levelCount;
        }

        public void Tick()
        {
            _player?.Move();
        }
    }
}