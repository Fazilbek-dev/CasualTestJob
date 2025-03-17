using ButchersGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTrigger : MonoBehaviour
{
    [SerializeField] private Transform _rotatePoint;
    [SerializeField] private Transform _nextRoad;

    [SerializeField] private float _startRot;
    [SerializeField] private float _endRot;

    [SerializeField] private bool _isRotate;
    [SerializeField] private bool _isDoor;

    public float duration = 1f; // ������������ �������� � ��������
    private float elapsedTime = 0f;
    private Quaternion startRotation;
    private Quaternion endRotation;

    private PlayerController playerController;

    void Start()
    {
        startRotation = transform.rotation;
        endRotation = Quaternion.Euler(0, _endRot, 0);
    }

    void Update()
    {
        if (elapsedTime < duration && _isRotate)
        {
            // ����������� ��������� �����
            elapsedTime += Time.deltaTime;

            // ��������� �������� �� 0 �� 1
            float t = elapsedTime / duration;

            // ���������� ������� ������������
            _rotatePoint.rotation = Quaternion.Lerp(startRotation, endRotation, t);

            // ������������� ��������, ����� �������� ����
            if (t >= 1f)
            {
                _isRotate = false;
                _rotatePoint.rotation = endRotation;
            }
        }
        else
        {
            if (playerController != null)
            {
                playerController._isRotating = false;
                playerController = null;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<PlayerController>(out PlayerController controller))
        {
            if(!_isDoor)
                controller.transform.parent = _rotatePoint;
            playerController = controller;
            controller._isRotating = true;
            _isRotate = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<PlayerController>(out PlayerController controller))
        {
            controller.transform.parent = _nextRoad;
        }
    }
}
