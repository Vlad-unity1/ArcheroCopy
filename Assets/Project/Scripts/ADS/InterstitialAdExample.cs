using UnityEngine;
using UnityEngine.Advertisements;

namespace Project.Scripts.ADS
{
    public class InterstitialAdExample : IUnityAdsLoadListener, IUnityAdsShowListener
    {
        private readonly string _androidAdUnitId = "Interstitial_Android"; // ID рекламного блока для Android
        private readonly string _iOsAdUnitId = "Interstitial_iOS";         // ID рекламного блока для iOS
        private string _adUnitId;

        public void Initialize()
        {
            // Получаем ID рекламного блока в зависимости от платформы:
            _adUnitId = (Application.platform == RuntimePlatform.IPhonePlayer)
                ? _iOsAdUnitId
                : _androidAdUnitId;
        }

        // Загружаем контент рекламы для заданного рекламного блока:
        public void LoadAd()
        {
            // ВАЖНО! Загружать рекламу можно только ПОСЛЕ инициализации (в этом примере инициализация в другом скрипте).
            Debug.Log("Загрузка рекламы: " + _adUnitId);
            Advertisement.Load(_adUnitId, this);
        }

        // Показываем ранее загруженную рекламу:
        public void ShowAd()
        {
            // Если реклама не была загружена, этот метод не сработает
            Debug.Log("Показ рекламы: " + _adUnitId);
            Advertisement.Show(_adUnitId, this);
        }

        // Реализация методов-интерфейсов для отслеживания загрузки/показа рекламы:

        public void OnUnityAdsAdLoaded(string adUnitId)
        {
            // Этот метод вызывается, если реклама успешно загрузилась.
        }

        public void OnUnityAdsFailedToLoad(string _adUnitId, UnityAdsLoadError error, string message)
        {
            Debug.Log($"Ошибка загрузки рекламы: {_adUnitId} - {error.ToString()} - {message}");
            // Здесь можно реализовать повторную попытку загрузки рекламы.
        }

        public void OnUnityAdsShowFailure(string _adUnitId, UnityAdsShowError error, string message)
        {
            Debug.Log($"Ошибка показа рекламы {_adUnitId}: {error.ToString()} - {message}");
            // Здесь можно, например, попробовать загрузить другую рекламу.
        }

        public void OnUnityAdsShowStart(string _adUnitId)
        {
            // Этот метод вызывается, когда реклама начала показываться.
        }

        public void OnUnityAdsShowClick(string _adUnitId)
        {
            // Этот метод вызывается, когда пользователь кликнул по рекламе.
        }

        public void OnUnityAdsShowComplete(string _adUnitId, UnityAdsShowCompletionState showCompletionState)
        {
            LoadAd();
            // Этот метод вызывается, когда показ рекламы завершён.
            // Можно проверить состояние: была ли досмотрена до конца и выдать награду.
        }
    }
}
