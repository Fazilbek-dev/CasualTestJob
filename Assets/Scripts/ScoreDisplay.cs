using UnityEngine;
using TMPro;

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText; // Ссылка на текстовый элемент UI
    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("GameManager не найден для отображения очков!");
        }
        UpdateScoreText();
    }

    private void Update()
    {
        UpdateScoreText(); // Обновление в реальном времени (можно оптимизировать через события)
    }

    private void UpdateScoreText()
    {
        if (scoreText != null && gameManager != null)
        {
            scoreText.text = "Score: " + gameManager.GetScore();
        }
    }
}