using NextLevelLoader;
using Project.Scripts.Enemies;
using Project.Scripts.PlayerModels;
using Project.Scripts.Players;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Project.Scripts
{
    public class GameFlow : IInitializable, ITickable
    {
        private List<EnemyModel> _enemies;
        private readonly EnemyFactory _enemyFactory;
        private readonly PlayerFactory _playerFactory;
        private readonly SpawnPointPlayerScene _spawnPointPlayer;
        private readonly Joystick _joystick;
        private readonly NextLevel _nextLevelController;
        private readonly EnemySpawnData[] _enemySpawnData;
        private readonly Slider _experienceSlider;
        private readonly SceneData _sceneData;
        private readonly IAnalyticsService _analyticsService;
        private PlayerModel _player;

        public GameFlow(EnemyFactory enemyFactory, PlayerFactory playerFactory, SpawnPointPlayerScene spawnPointPlayer, Joystick joystick,
            NextLevel nextLevelController, EnemySpawnData[] enemySpawnDatas, Slider experienceSlider, SceneData sceneData, IAnalyticsService analyticsService)
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
        }

        public void Initialize()
        {
            _nextLevelController.DisablePanels();
            _player = _playerFactory.CreatePlayer(_spawnPointPlayer, 100, _joystick);
            _enemyFactory.CreateEnemies(_enemySpawnData);
            _enemies = _enemyFactory.GetAllEnemies();

            _player.PlayerHealth.OnEntityDeath += RemovePlayer;

            foreach (var enemy in _enemies)
            {
                enemy.EnemyHealth.OnEntityDeath += () => RemoveEnemy(enemy);
            }

            float savedExperience = PlayerPrefs.GetFloat("EXP", 0);
            _player.CurrentExperience = savedExperience;
            UpdateExperienceSlider();
        }

        private void RemoveEnemy(EnemyModel enemy)
        {
            enemy.EnemyHealth.OnEntityDeath -= () => RemoveEnemy(enemy);
            _enemies.Remove(enemy);
            _player.PlayerMovement.AddExperience(enemy.EXP);
            UpdateExperienceSlider();
            _nextLevelController.SaveExperience(_player.CurrentExperience);

            if (_enemies.Count == 0)
            {
                OnAllEnemiesDefeated();
            }
        }

        private void OnAllEnemiesDefeated()
        {
            _nextLevelController.EnableCollider();
        }

        private void RemovePlayer()
        {
            _player.PlayerHealth.OnEntityDeath -= RemovePlayer;
            PlayerPrefs.DeleteKey("EXP");
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

        public void Tick()
        {
            _player?.Move();
            Debug.Log($"{_player.CurrentExperience}");
        }
    }
}