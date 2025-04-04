using Project.Scripts.Enemies;
using Project.Scripts.HealthInfo;
using Project.Scripts.Player;
using Project.Scripts.PlayerModels;
using Project.Scripts.Weapons;
using UnityEngine;

namespace Project.Scripts.Players
{
    public class PlayerFactory
    {
        private readonly WeaponFactory _weaponFactory;
        private readonly SceneData _sceneData;

        public PlayerFactory(WeaponFactory weaponFactory, SceneData sceneData)
        {
            _weaponFactory = weaponFactory;
            _sceneData = sceneData;
        }

        public PlayerModel CreatePlayer(PlayerSpawnPoint spawnPosition, int initialHealth, Joystick joystick)
        {
            PlayerMovement playerMovement = Object.Instantiate(_sceneData.PrefabPlayer, spawnPosition.transform.position, Quaternion.identity);
            var playerInput = new PlayerInputHandler(joystick);

            var weapon = _weaponFactory.CreateWeapon(playerMovement.weaponTransformPrefab);
            var health = new Health(initialHealth, playerMovement.gameObject);
            float savedExp = PlayerPrefs.GetFloat("EXP", 0);
            var player = new PlayerModel(health, 10, weapon, playerMovement, playerInput.Joystick, savedExp);

            playerMovement.Initialize(player, playerInput, health, _sceneData, savedExp);
            player.SetWeapon(weapon);
            
            return player;
        }
    }
}