using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
using Firebase;
using Firebase.Extensions;

namespace Project.Scripts.Firebase
{
    public class FirebaseInstaller : MonoInstaller
    {
        private string _gameSceneName = "StartSceneTest";

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
                    Debug.Log("Firebase инициализирован.");
                    SceneManager.LoadScene(_gameSceneName);
                }
                else
                {
                    Debug.LogError($"Ошибка инициализации Firebase: {task.Result}");
                }
            });
        }
    }
}
