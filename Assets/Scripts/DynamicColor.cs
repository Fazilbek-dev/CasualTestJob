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

        // ������������� ��������� ����
        UpdateColor();
    }

    void Update()
    {
        UpdateColor();

        // �������� ��������� �������� (����� ������ ����� ��������)
        // currentValue = Mathf.PingPong(Time.time * 20f, maxValue);
    }

    private void UpdateColor()
    {
        if (imageComponent == null) return;

        // ����������� �������� �� 0 �� 1
        float t = Mathf.InverseLerp(minValue, maxValue, currentValue);

        // ��������� ����
        float red = 1f;
        float green = 0f;
        float blue = 0f;

        if (t <= 0.5f)
        {
            green = t * 2f; // ����������� ����� �� 0 �� 1
        }
        else
        {
            green = 1f;
            red = 1f - ((t - 0.5f) * 2f); // ��������� ������� �� 1 �� 0
        }

        // ������� ������� ����
        Color targetColor = new Color(red, green, blue);

        // ������ �������� ����
        Color currentColor = imageComponent.color;
        imageComponent.color = Color.Lerp(currentColor, targetColor, Time.deltaTime * colorChangeSpeed);

        // ������� ���������� ����������
        Debug.Log($"Value: {currentValue}, Normalized: {t}, Target Color: {targetColor}, Current Color: {imageComponent.color}");
    }

    public void SetValue(float newValue)
    {
        currentValue = Mathf.Clamp(newValue, minValue, maxValue);
    }

    // ��� �������� � ���������
    void OnValidate()
    {
        currentValue = Mathf.Clamp(currentValue, minValue, maxValue);
        if (Application.isPlaying && imageComponent != null)
        {
            UpdateColor();
        }
    }
}