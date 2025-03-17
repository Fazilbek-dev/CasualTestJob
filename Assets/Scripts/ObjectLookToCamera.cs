using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectLookToCamera : MonoBehaviour
{
    public bool useMainCamera = true;        // ������������ ������� ������
    public Camera targetCamera;              // ��� ������� ���������� ������
    public Vector3 lookDirection = Vector3.forward;  // ����������� ������� ������� (��������� ����������)
    public bool smoothRotation = false;      // ������� �������
    public float rotationSpeed = 5f;         // �������� �������� ��������

    private Camera cam;

    void Start()
    {
        // ����������, ����� ������ ������������
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

        // �������� ������ �� ������� � ������
        Vector3 directionToCamera = cam.transform.position - transform.position;

        // ����������� �������� ����������� �������
        Vector3 normalizedLookDirection = lookDirection.normalized;

        // ��������� ������� �������
        Quaternion targetRotation = Quaternion.LookRotation(directionToCamera, Vector3.up);

        // ������� �������������� �������, ����� ��������� ������� �������� �� ������
        Quaternion adjustment = Quaternion.FromToRotation(Vector3.forward, normalizedLookDirection);
        targetRotation = targetRotation * Quaternion.Inverse(adjustment);

        if (smoothRotation)
        {
            // ������� �������
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }
        else
        {
            // ���������� �������
            transform.rotation = targetRotation;
        }
    }

    // ����� ��� ��������� ����������� ������� ����� ���
    public void SetLookDirection(Vector3 newDirection)
    {
        lookDirection = newDirection;
    }
}
