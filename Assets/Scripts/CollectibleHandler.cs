using UnityEngine;

public class CollectibleHandler : MonoBehaviour
{
    [Header("Scoring Settings")]
    [SerializeField] private int moneyPoints = 10;    // Очки за подбор денег
    [SerializeField] private int alcoholPoints = -5;  // Очки за подбор алкоголя (отрицательное значение)

    [SerializeField] private GameManager gameManager;                  // Ссылка на GameManager

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        // Находим GameManager в сцене
        gameManager = FindObjectOfType<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("GameManager не найден в сцене! Создайте объект с компонентом GameManager.");
        }
    }

    // Обработка столкновений с собираемыми объектами
    private void OnTriggerEnter(Collider other)
    {
        if (gameManager == null) return; // Проверка на наличие GameManager

        if (other.TryGetComponent<CollectableItem>(out CollectableItem collectableItem))
        {
            // Подбор денег
            gameManager.AddScore(collectableItem._scoreAdd);
            audioSource.clip = collectableItem._clip;
            audioSource.Play();
            Color c = Color.white;
            if(collectableItem._scoreAdd > 0f)
            {
                c = Color.green;
            }
            else
            {
                c = Color.red;
            }
            //gameManager._spawner.SpawnObject(collectableItem.transform, collectableItem._scoreAdd, c);
            Destroy(other.gameObject); // Уничтожаем объект денег
        }
    }

    // Метод для получения текущего количества очков (опционально, для локального использования)
    public int GetScore()
    {
        return gameManager != null ? gameManager.GetScore() : 0;
    }
}