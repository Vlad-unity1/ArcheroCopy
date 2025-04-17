using Project.Scripts.Enemies;
using Project.Scripts.HealthInfo;
using Project.Scripts.Player;
using Project.Scripts.PlayerModels;
using Project.Scripts.Weapons;
using UnityEngine;
using System.Threading.Tasks;
using Project.Scripts.Addressables;

namespace Project.Scripts.Players
{
    public class PlayerFactory
    {
        private readonly WeaponFactory _weaponFactory;
        private readonly SceneData _sceneData;
        private readonly IAssetProvider _assetProvider;
        private readonly PlayerPrefsSave _playerPrefsSave;

        public PlayerFactory(WeaponFactory weaponFactory, SceneData sceneData, IAssetProvider assetProvider, PlayerPrefsSave playerPrefsSave)
        {
            _weaponFactory = weaponFactory;
            _sceneData = sceneData;
            _assetProvider = assetProvider;
            _playerPrefsSave = playerPrefsSave;
        }

        public async Task<PlayerModel> CreatePlayerAsync(PlayerSpawnPoint spawnPosition, int initialHealth, Joystick joystick)
        {
            GameObject playerPrefab = await _assetProvider.LoadPlayerPrefabAsync();
            PlayerMovement playerMovement = Object.Instantiate(playerPrefab, spawnPosition.transform.position, Quaternion.identity).GetComponent<PlayerMovement>();

            var playerInput = new PlayerInputHandler(joystick);
            var weapon = _weaponFactory.CreateWeapon(playerMovement.weaponTransformPrefab);
            var health = new Health(initialHealth, playerMovement.gameObject);
            var playerSaveData = _playerPrefsSave.Load();
            var player = new PlayerModel(health, 10, weapon, playerMovement, playerInput.Joystick, playerSaveData.Experience);

            playerMovement.Initialize(player, playerInput, health, _sceneData, playerSaveData.Experience);
            player.SetWeapon(weapon);

            return player;
        }
    }
}
