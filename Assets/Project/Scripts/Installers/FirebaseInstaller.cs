using Firebase;
using Firebase.Extensions;
using UnityEngine.SceneManagement;
using Zenject;

namespace Project.Scripts.Firebase
{
    public class FirebaseInstaller : MonoInstaller
    {
        private readonly string _gameSceneName = "MenuScene";

        public override void InstallBindings()
        {
            Container.Bind<IAnalyticsService>().To<FirebaseAnalyticsService>().AsSingle();
            InitializeFirebase();
        }

        private void InitializeFirebase()
        {
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
            {
                if (task.Result == DependencyStatus.Available)
                {
                    SceneManager.LoadScene(_gameSceneName);
                }
            });
        }
    }
}
