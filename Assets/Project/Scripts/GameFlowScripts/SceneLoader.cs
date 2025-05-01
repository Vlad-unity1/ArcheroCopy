using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private Button _startGameButton;

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

    public void StartGame()
    {
        SceneManager.LoadScene("StartSceneTest");
    }
}
