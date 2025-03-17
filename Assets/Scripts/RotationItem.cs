using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationItem : MonoBehaviour
{
    // �������� �������� � �������� � �������
    [SerializeField]
    private float rotationSpeed = 90f;

    // ����������� �������� (1 = �� �������, -1 = ������ �������)
    [SerializeField]
    private float rotationDirection = 1f;

    void Update()
    {
        // ��������� ���� �������� �� ������� ����
        float rotationAmount = rotationSpeed * rotationDirection * Time.deltaTime;

        // ��������� �������� ������ ��� Y
        transform.Rotate(0f, rotationAmount, 0f);
    }
}
