using ButchersGames;
using UnityEngine;

public class DanceTrigger : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator animator;           // Ссылка на Animator персонажа
    [SerializeField] private Transform characterModel;    // Ссылка на модель персонажа (дочерний объект)
    [SerializeField] private PlayerController playerController; // Ссылка на PlayerController

    [Header("Dance Settings")]
    [SerializeField] private float rotationSpeed = 5f;    // Скорость поворота к камере

    private Camera mainCamera;                            // Ссылка на главную камеру
    private bool isDancing = false;                       // Флаг состояния танца

    private void Start()
    {
        // Находим главную камеру
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Главная камера не найдена!");
        }

        // Проверяем компоненты
        if (animator == null)
        {
            animator = GetComponent<Animator>();
            if (animator == null) Debug.LogError("Animator не найден!");
        }

        if (characterModel == null)
        {
            characterModel = transform.Find("CharacterModel");
            if (characterModel == null) Debug.LogError("CharacterModel не найден!");
        }

        if (playerController == null)
        {
            playerController = GetComponent<PlayerController>();
            if (playerController == null) Debug.LogError("PlayerController не найден!");
        }
    }

    private void Update()
    {
        if (isDancing && mainCamera != null && characterModel != null)
        {
            // Поворачиваем персонажа к камере
            Vector3 directionToCamera = mainCamera.transform.position - characterModel.position;
            directionToCamera.y = 0; // Игнорируем высоту, поворот только по Y
            Quaternion targetRotation = Quaternion.LookRotation(directionToCamera);
            characterModel.rotation = Quaternion.Slerp(characterModel.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    // Публичная функция для запуска танца
    public void StartDance()
    {
        if (animator == null || playerController == null) return;

        // Отключаем PlayerController
        playerController.enabled = false;

        // Запускаем анимацию танца
        animator.SetTrigger("Dance"); // Предполагается, что есть триггер "Dance"

        // Устанавливаем флаг танца
        isDancing = true;

        Debug.Log("Танец начался!");
    }

    // Опционально: функция для остановки танца
    public void StopDance()
    {
        if (animator == null || playerController == null) return;

        // Включаем PlayerController обратно
        playerController.enabled = true;

        // Останавливаем анимацию танца (возвращаемся к ходьбе)
        animator.ResetTrigger("Dance");

        // Сбрасываем флаг танца
        isDancing = false;

        Debug.Log("Танец остановлен!");
    }
}