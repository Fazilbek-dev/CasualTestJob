using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectLookToCamera : MonoBehaviour
{
    public bool useMainCamera = true;        // Использовать главную камеру
    public Camera targetCamera;              // Или указать конкретную камеру
    public Vector3 lookDirection = Vector3.forward;  // Направление взгляда объекта (локальные координаты)
    public bool smoothRotation = false;      // Плавный поворот
    public float rotationSpeed = 5f;         // Скорость плавного поворота

    private Camera cam;

    void Start()
    {
        // Определяем, какую камеру использовать
        if (useMainCamera)
        {
            cam = Camera.main;
        }
        else if (targetCamera != null)
        {
            cam = targetCamera;
        }
        else
        {
            Debug.LogWarning("No camera assigned and useMainCamera is false!");
            enabled = false;
            return;
        }
    }

    void Update()
    {
        if (cam == null) return;

        // Получаем вектор от объекта к камере
        Vector3 directionToCamera = cam.transform.position - transform.position;

        // Нормализуем заданное направление взгляда
        Vector3 normalizedLookDirection = lookDirection.normalized;

        // Вычисляем целевую ротацию
        Quaternion targetRotation = Quaternion.LookRotation(directionToCamera, Vector3.up);

        // Создаем корректирующую ротацию, чтобы выбранная сторона смотрела на камеру
        Quaternion adjustment = Quaternion.FromToRotation(Vector3.forward, normalizedLookDirection);
        targetRotation = targetRotation * Quaternion.Inverse(adjustment);

        if (smoothRotation)
        {
            // Плавный поворот
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }
        else
        {
            // Мгновенный поворот
            transform.rotation = targetRotation;
        }
    }

    // Метод для установки направления взгляда через код
    public void SetLookDirection(Vector3 newDirection)
    {
        lookDirection = newDirection;
    }
}
