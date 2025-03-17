using UnityEngine;
using TMPro;

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText; // ������ �� ��������� ������� UI
    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("GameManager �� ������ ��� ����������� �����!");
        }
        UpdateScoreText();
    }

    private void Update()
    {
        UpdateScoreText(); // ���������� � �������� ������� (����� �������������� ����� �������)
    }

    private void UpdateScoreText()
    {
        if (scoreText != null && gameManager != null)
        {
            scoreText.text = "Score: " + gameManager.GetScore();
        }
    }
}