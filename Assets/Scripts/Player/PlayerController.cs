using UnityEngine;

namespace ButchersGames
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Player Movement Settings")]
        [SerializeField] private float forwardSpeed = 5f;     // Скорость движения вперёд корневого объекта

        [Header("Character Movement Settings")]
        [SerializeField] private Transform characterModel;    // Ссылка на модель персонажа (дочерний объект)
        [SerializeField] private float sideSpeed = 3f;        // Скорость движения влево/вправо модели
        [SerializeField] private float maxSideOffset = 5f;    // Максимальное смещение влево/вправо модели
        [SerializeField] private float touchSensitivity = 0.01f; // Чувствительность движения пальца
        [SerializeField] private float holdSideSpeed = 2f;    // Скорость движения модели при удержании пальца

        [Header("Character Rotation Settings")]
        [SerializeField] private float maxRotationAngle = 45f; // Максимальный угол поворота модели (градусы)
        [SerializeField] private float rotationSpeed = 5f;    // Скорость поворота модели
        [SerializeField] private float maxTouchDeltaForRotation = 4f; // Максимальное смещение пальца для полного поворота
        [SerializeField] private float touchMoveThreshold = 1f; // Порог движения пальца (в пикселях) для определения неподвижности

        private Vector3 initialCharacterPosition; // Начальная локальная позиция модели
        private float touchStartPosX;             // Начальная позиция касания по X для отсчёта поворота
        private float lastTouchPosX;              // Последняя позиция касания для отсчёта движения
        private bool isTouching = false;          // Флаг касания
        private Quaternion targetRotation;        // Целевой поворот для модели
        private float currentXOffset;             // Текущее смещение модели по X относительно начальной позиции

        public bool _isRotating;

        private void Start()
        {
            // Запоминаем начальную локальную позицию и поворот модели
            if (characterModel != null)
            {
                initialCharacterPosition = characterModel.localPosition;
                currentXOffset = 0f; // Изначально смещение равно 0
                targetRotation = characterModel.localRotation;
            }
            else
            {
                Debug.LogError("Character Model не назначен в инспекторе!");
            }
        }

        private void Update()
        {
            if(!_isRotating)
            // Автоматическое движение вперёд корневого объекта
            transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);

            // Обработка ввода
            HandleTouchInput();

            // Ограничение движения модели по сторонам
            ClampSideMovement();

            // Плавный поворот модели персонажа
            if (characterModel != null)
            {
                characterModel.localRotation = Quaternion.Lerp(characterModel.localRotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }

        private void HandleTouchInput()
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        // Начало касания
                        touchStartPosX = touch.position.x;
                        lastTouchPosX = touch.position.x; // Запоминаем начальную позицию касания для движения
                        isTouching = true;
                        break;

                    case TouchPhase.Moved:
                    case TouchPhase.Stationary:
                        // Движение или удержание пальца
                        if (isTouching && characterModel != null)
                        {
                            // Движение модели влево/вправо относительно последнего положения пальца
                            float touchDeltaX = (touch.position.x - lastTouchPosX) * touchSensitivity;
                            currentXOffset += touchDeltaX * sideSpeed; // Обновляем текущее смещение
                            Vector3 newPosition = characterModel.localPosition;
                            newPosition.x = initialCharacterPosition.x + currentXOffset;
                            characterModel.localPosition = newPosition;

                            // Проверяем, движется ли палец
                            float touchMoveDistance = touch.deltaPosition.magnitude;
                            if (touchMoveDistance < touchMoveThreshold)
                            {
                                // Если палец неподвижен, возвращаем поворот к 0 и обновляем начальную позицию касания
                                targetRotation = Quaternion.identity;
                                touchStartPosX = touch.position.x;
                            }
                            else
                            {
                                // Вычисляем угол поворота на основе направления движения пальца
                                float rotationDeltaX = touch.position.x - lastTouchPosX; // Используем разницу с последней позицией
                                float rotationFactor = Mathf.Clamp(rotationDeltaX / maxTouchDeltaForRotation, -1f, 1f); // Нормализуем от -1 до 1
                                float targetAngle = rotationFactor * maxRotationAngle;
                                targetRotation = Quaternion.Euler(0f, targetAngle, 0f);
                            }

                            // Дополнительное движение модели при удержании пальца
                            float touchPositionX = touch.position.x;
                            float screenCenter = Screen.width / 2f;
                            if (touchPositionX < screenCenter - 50f) // Удержание в левой части экрана
                            {
                                MoveCharacterSideways(-holdSideSpeed);
                            }
                            else if (touchPositionX > screenCenter + 50f) // Удержание в правой части экрана
                            {
                                MoveCharacterSideways(holdSideSpeed);
                            }

                            // Обновляем последнюю позицию касания
                            lastTouchPosX = touch.position.x;
                        }
                        break;

                    case TouchPhase.Ended:
                        // Конец касания
                        isTouching = false;
                        targetRotation = Quaternion.identity; // Возвращаем поворот модели в исходное положение (0 градусов)
                        break;
                }
            }
        }

        private void MoveCharacterSideways(float speed)
        {
            // Движение модели влево или вправо при удержании
            if (characterModel != null)
            {
                currentXOffset += speed * Time.deltaTime; // Обновляем текущее смещение
                Vector3 newPosition = characterModel.localPosition;
                newPosition.x = initialCharacterPosition.x + currentXOffset;
                characterModel.localPosition = newPosition;
            }
        }

        private void ClampSideMovement()
        {
            // Ограничиваем движение модели по сторонам
            if (characterModel != null)
            {
                Vector3 currentPosition = characterModel.localPosition;
                currentPosition.x = Mathf.Clamp(currentPosition.x, initialCharacterPosition.x - maxSideOffset, initialCharacterPosition.x + maxSideOffset);
                characterModel.localPosition = currentPosition;
                // Обновляем currentXOffset, чтобы оно соответствовало фактической позиции
                currentXOffset = currentPosition.x - initialCharacterPosition.x;
            }
        }

        // Для визуализации в редакторе
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (characterModel != null)
            {
                Gizmos.color = Color.green;
                Vector3 leftLimit = characterModel.TransformPoint(new Vector3(initialCharacterPosition.x - maxSideOffset, initialCharacterPosition.y, initialCharacterPosition.z));
                Vector3 rightLimit = characterModel.TransformPoint(new Vector3(initialCharacterPosition.x + maxSideOffset, initialCharacterPosition.y, initialCharacterPosition.z));
                Gizmos.DrawSphere(leftLimit, 0.2f);
                Gizmos.DrawSphere(rightLimit, 0.2f);
            }
        }
#endif
    }
}