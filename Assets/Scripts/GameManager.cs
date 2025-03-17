using ButchersGames;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public ObjectPoolSpawner _spawner;

    [SerializeField] private GameObject _losePanel;
    [SerializeField] private PlayerController playerController;

    [SerializeField] private GameObject _poor;
    [SerializeField] private GameObject _middle;
    [SerializeField] private GameObject _rich;

    // Статическая переменная для хранения очков
    public int totalScore = 0;

    // Синглтон для доступа к экземпляру GameManager
    private static GameManager instance;

    private void Awake()
    {
        // Убеждаемся, что существует только один GameManager
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Сохраняем объект между сценами
        }
        else
        {
            Destroy(gameObject); // Уничтожаем дубликаты
        }

        // Инициализация очков
        totalScore = 0;
        Debug.Log("GameManager инициализирован! Очки: " + totalScore);
    }

    private void Update()
    {
        if(totalScore < 0f)
        {
            _losePanel.SetActive(true);
            playerController.enabled = false;
        }
        if(totalScore < 100f)
        {
            _middle.SetActive(false);
            _rich.SetActive(false);
            _poor.SetActive(true);
        }
        else if(totalScore >= 100f && totalScore < 200f)
        {
            _middle.SetActive(true);
            _rich.SetActive(false);
            _poor.SetActive(false);
        }
        else
        {
            _rich.SetActive(true);
            _poor.SetActive(false);
            _middle.SetActive(false);
        }
    }

    // Метод для добавления очков
    public void AddScore(int points)
    {
        totalScore += points;
        Debug.Log("Очки обновлены! Текущие очки: " + totalScore);
    }

    // Метод для получения текущих очков
    public int GetScore()
    {
        return totalScore;
    }

    // Метод для сброса очков (если нужно)
    public void ResetScore()
    {
        totalScore = 0;
        Debug.Log("Очки сброшены! Текущие очки: " + totalScore);
    }
}