using Project.Scripts.WeaponModel;
using Project.Scripts.Weapons;
using System.Collections.Generic;
using Project.Scripts.HealthInfo;
using UnityEngine;
using Project.Scripts.Enemy;

namespace Project.Scripts.Enemies
{
    public class EnemyFactory
    {
        private readonly WeaponFactory _weaponFactory;
        private readonly SceneData _sceneData;
        private readonly List<EnemyModel> _enemies = new();

        public EnemyFactory(WeaponFactory weaponFactory, SceneData sceneData)
        {
            _weaponFactory = weaponFactory;
            _sceneData = sceneData;
        }

        public EnemyModel[] CreateEnemies(EnemySpawnData[] enemySpawnData)
        {
            EnemyModel[] enemies = new EnemyModel[enemySpawnData.Length];
            _enemies.Clear();

            for (int i = 0; i < enemySpawnData.Length; i++)
            {
                var data = enemySpawnData[i];

                Transform spawnPoint = _sceneData.SpawnPoints[i];
                EnemyView enemyObject = Object.Instantiate(data.Config.PrefabEnemy, spawnPoint.position, Quaternion.identity);
                enemyObject.transform.position = spawnPoint.position;
                Transform[] stoneCannonSpawnPoints = enemyObject.WeaponTransform;

                Weapon<StoneCannonConfig> enemyWeapon = _weaponFactory.CreateEnemyWeapon(stoneCannonSpawnPoints);
                data.Config.StartingWeaponConfig = enemyWeapon;
                Health enemyHealth = new(data.Config.MaxHealth, enemyObject.gameObject);
                EnemyModel enemy;

                if (data.Config is EnemyStoneConfig stoneConfig)
                {
                    enemy = new StoneEnemy(stoneConfig, _weaponFactory, enemyWeapon, enemyHealth);
                }
                else
                {
                    enemy = new EnemyModel(data.Config, enemyWeapon, enemyHealth, data.Config.EXP);
                }

                enemies[i] = enemy;
                _enemies.Add(enemy);

                enemyObject.Initialize(enemy, enemyObject.WeaponTransform, enemyHealth);
            }

            return enemies;
        }

        public List<EnemyModel> GetAllEnemies()
        {
            return _enemies;
        }
    }
}