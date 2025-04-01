using UnityEngine;
using UnityEngine.SceneManagement;

namespace NextLevelLoader
{
    public class NextLevel : MonoBehaviour
    {
        [SerializeField] private Collider _door;
        [SerializeField] private GameObject _panel;

        public void EnableCollider()
        {
            _door.enabled = true;
        }

        public void DisablePanels()
        {
            _door.enabled = false;
            _panel.SetActive(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            ReloadScene();
        }

        public void ReloadScene()
        {
            string currentSceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(currentSceneName);
            Time.timeScale = 1.0f;
        }

        public void EnablePanel()
        {
            _panel.SetActive(true);
            Time.timeScale = 0;
        }

        public void SaveExperience(float currentExperience)
        {
            PlayerPrefs.SetFloat("EXP", currentExperience);
            PlayerPrefs.Save();
        }
    }
}