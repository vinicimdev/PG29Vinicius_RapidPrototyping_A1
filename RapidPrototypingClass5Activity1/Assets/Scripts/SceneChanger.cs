using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class SceneChanger : MonoBehaviour
{
    [Header("Scene Names")]
    [SerializeField] private string prototype1SceneName = "Prototype1";
    [SerializeField] private string prototype2SceneName = "Prototype2";
    [SerializeField] private string prototype3SceneName = "Prototype3";

    void Update()
    {
        if (Keyboard.current == null)
            return;

        // Press 1 to load Prototype 1
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            LoadScene(prototype1SceneName);
        }

        // Press 2 to load Prototype 2
        if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            LoadScene(prototype2SceneName);
        }

        // Press 3 to load Prototype 3
        if (Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            LoadScene(prototype3SceneName);
        }
    }

    void LoadScene(string sceneName)
    {
        // Reset time scale in case it was paused
        Time.timeScale = 1f;

        // Load the scene
        SceneManager.LoadScene(sceneName);
    }
}