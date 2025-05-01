using NextLevelLoader;
using Project.Scripts.Addressables;
using Project.Scripts.BulletFactoryEnemy;
using Project.Scripts.BulletModel;
using Project.Scripts.Enemies;
using Project.Scripts.Firebase;
using Project.Scripts.Players;
using Project.Scripts.Weapons;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Project.Scripts.Installers
{
    public class GameplayInstaller : MonoInstaller
    {
        [SerializeField] private PlayerSpawnPoint _spawnPointPlayer;
        [SerializeField] private Joystick _joystick;
        [SerializeField] private BowConfig _bowConfig;
        [SerializeField] private StoneCannonConfig _stoneCannonConfig;
        [SerializeField] private EnemySpawnData[] _enemySpawnData;
        [SerializeField] private SceneData _sceneData;
        [SerializeField] private Slider _slider;
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private Collider _levelCollider;
        [SerializeField] private PanelView _panelGameOver;
        [SerializeField] private Canvas _panelCanvas;
        [SerializeField] private SceneLoader _sceneLoader;

        public override void InstallBindings()
        {
            BindInstances();
            BindServices();
            BindFactories();
            BindConfigs();
            BindUI();
            BindGameLogic();
        }

        private void BindFactories()
        {
            Container.Bind<WeaponFactory>().AsSingle();
            Container.Bind<PlayerFactory>().AsSingle();
            Container.Bind<EnemyFactory>().AsSingle().WithArguments(_sceneData);
            Container.Bind<BulletFactoryEnemies>().AsSingle().WithArguments(_stoneCannonConfig);
            Container.Bind<BulletFactoryPlayer>().AsSingle().WithArguments(_bowConfig);
        }

        private void BindConfigs()
        {
            Container.Bind<BowConfig>().FromInstance(_bowConfig).AsSingle();
            Container.Bind<StoneCannonConfig>().FromInstance(_stoneCannonConfig).AsSingle();
        }

        private void BindInstances()
        {
            Container.BindInstance(_spawnPointPlayer).AsSingle();
            Container.BindInstance(_joystick).AsSingle();
            Container.BindInstance(_enemySpawnData).AsSingle();
            Container.BindInstance(_sceneData).AsSingle();
        }

        private void BindUI()
        {
            Container.Bind<RewardedAds>().AsSingle();
            Container.Bind<AdsInitializer>().AsSingle();
            Container.Bind<InterstitialAdExample>().AsSingle();
            Container.Bind<PanelPresenter>().AsSingle();
            Container.BindInstance(_slider).AsSingle();
            Container.BindInstance(_levelText).AsSingle();
            Container.BindInstance(_panelCanvas);
            Container.BindInstance(_levelCollider);
            Container.BindInstance(_sceneLoader);
            Container.Bind<PanelView>()
               .FromComponentInNewPrefab(_panelGameOver)
               .AsSingle()
               .NonLazy();
            Container.Bind<PanelModel>().AsSingle().WithArguments(_levelCollider, _panelGameOver, _panelCanvas, _sceneLoader);
        }

        private void BindServices()
        {
            Container.Bind<IAssetProvider>().To<AssetProvider>().AsSingle();
            Container.Bind<IAnalyticsService>().To<FirebaseAnalyticsService>().AsSingle();
            Container.Bind<PlayerDataSave>().AsSingle();
            Container.Bind<PlayerPrefsSave>().AsSingle();
            Container.Bind<TimeService>().AsSingle();
        }

        private void BindGameLogic()
        {
            Container.BindInterfacesAndSelfTo<GameFlow>().AsSingle();
        }
    }
}
