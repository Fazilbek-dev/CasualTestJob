using UnityEngine;
using UnityEngine.UI;

public class DynamicColor : MonoBehaviour
{
    [SerializeField] private float minValue = 0f;
    [SerializeField] private float maxValue = 100f;
    [SerializeField] private float currentValue = 50f;
    [SerializeField] private float colorChangeSpeed = 2f;

    [SerializeField] private Image imageComponent;

    void Start()
    {

        // Устанавливаем начальный цвет
        UpdateColor();
    }

    void Update()
    {
        UpdateColor();

        // Тестовое изменение значения (можно убрать после проверки)
        // currentValue = Mathf.PingPong(Time.time * 20f, maxValue);
    }

    private void UpdateColor()
    {
        if (imageComponent == null) return;

        // Нормализуем значение от 0 до 1
        float t = Mathf.InverseLerp(minValue, maxValue, currentValue);

        // Вычисляем цвет
        float red = 1f;
        float green = 0f;
        float blue = 0f;

        if (t <= 0.5f)
        {
            green = t * 2f; // Увеличиваем синий от 0 до 1
        }
        else
        {
            green = 1f;
            red = 1f - ((t - 0.5f) * 2f); // Уменьшаем красный от 1 до 0
        }

        // Создаем целевой цвет
        Color targetColor = new Color(red, green, blue);

        // Плавно изменяем цвет
        Color currentColor = imageComponent.color;
        imageComponent.color = Color.Lerp(currentColor, targetColor, Time.deltaTime * colorChangeSpeed);

        // Выводим отладочную информацию
        Debug.Log($"Value: {currentValue}, Normalized: {t}, Target Color: {targetColor}, Current Color: {imageComponent.color}");
    }

    public void SetValue(float newValue)
    {
        currentValue = Mathf.Clamp(newValue, minValue, maxValue);
    }

    // Для проверки в редакторе
    void OnValidate()
    {
        currentValue = Mathf.Clamp(currentValue, minValue, maxValue);
        if (Application.isPlaying && imageComponent != null)
        {
            UpdateColor();
        }
    }
}