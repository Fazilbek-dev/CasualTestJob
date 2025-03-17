using UnityEngine;

public class WalkAnimationController : MonoBehaviour
{
    [Header("Animation Settings")]
    [SerializeField] private Animator animator;           // Ссылка на компонент Animator
    [SerializeField] private float transitionSpeed = 0.1f; // Скорость плавного перехода между значениями

    // Границы для анимаций
    private const float SAD_WALK_THRESHOLD = 0f;          // Грустная ходьба (около 0)
    private const float NORMAL_WALK_THRESHOLD = 150f;     // Обычная ходьба (около 150)
    private const float HAPPY_WALK_THRESHOLD = 300f;      // Счастливая ходьба (около 300)

    private GameManager gameManager;                      // Ссылка на GameManager для получения очков
    private float currentAnimationValue;                  // Текущее значение для анимации

    private void Start()
    {
        // Находим GameManager
        gameManager = FindObjectOfType<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("GameManager не найден!");
        }

        // Находим Animator
        if (animator == null)
        {
            animator = GetComponent<Animator>();
            if (animator == null)
            {
                Debug.LogError("Animator не найден на объекте!");
            }
        }

        // Инициализируем текущее значение анимации
        currentAnimationValue = gameManager != null ? gameManager.GetScore() : 0f;
        UpdateAnimation();
    }

    private void Update()
    {
        if (gameManager == null || animator == null) return;

        // Получаем текущее количество очков
        float targetValue = gameManager.GetScore();

        // Плавно интерполируем текущее значение к целевому
        currentAnimationValue = Mathf.Lerp(currentAnimationValue, targetValue, transitionSpeed * Time.deltaTime);

        // Обновляем анимацию
        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        // Устанавливаем параметр анимации в Animator
        animator.SetFloat("WalkSpeed", currentAnimationValue);
    }
}