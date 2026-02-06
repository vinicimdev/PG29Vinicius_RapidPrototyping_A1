using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game Stats")]
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;
    private int score = 0;

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private Slider healthSlider;
    //[SerializeField] private GameObject gameOverPanel;

    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        currentHealth = maxHealth;
        score = 0;
        UpdateUI();

        //if (gameOverPanel != null)
        //{
        //    gameOverPanel.SetActive(false);
        //}
    }

    public void AddScore(int points)
    {
        score += points;
        UpdateUI();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(0, currentHealth);
        UpdateUI();

        if (currentHealth <= 0)
        {
            GameOver();
        }
    }

    void UpdateUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score.ToString();
        }

        if (healthText != null)
        {
            healthText.text = "Health: " + currentHealth.ToString();
        }

        if (healthSlider != null)
        {
            healthSlider.value = (float)currentHealth / maxHealth;
        }
    }

    void GameOver()
    {
        Time.timeScale = 0f;

        //if (gameOverPanel != null)
        //{
        //    gameOverPanel.SetActive(true);
        //    TextMeshProUGUI finalScoreText = gameOverPanel.GetComponentInChildren<TextMeshProUGUI>();
        //    if (finalScoreText != null)
        //    {
        //        finalScoreText.text = "Game Over!\nFinal Score: " + score;
        //    }
        //}
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}