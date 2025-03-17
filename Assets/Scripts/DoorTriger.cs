using ButchersGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTriger : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    [SerializeField] private int _scoreCheck;

    [SerializeField] private GameObject _winPanel;

    [SerializeField] private Transform _rotatePoint1;
    [SerializeField] private Transform _rotatePoint2;

    [SerializeField] private float _startRot1;
    [SerializeField] private float _startRot2;
    [SerializeField] private float _endRot1;
    [SerializeField] private float _endRot2;

    [SerializeField] private bool _isRotate;

    public float duration = 1f; // ������������ �������� � ��������
    private float elapsedTime = 0f;
    private Quaternion startRotation1;
    private Quaternion startRotation2;
    private Quaternion endRotation1;
    private Quaternion endRotation2;

    void Start()
    {
        startRotation1 = transform.rotation;
        endRotation1 = Quaternion.Euler(0, _endRot1, 0);
        startRotation2 = transform.rotation;
        endRotation2 = Quaternion.Euler(0, _endRot2, 0);
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
            _rotatePoint1.rotation = Quaternion.Lerp(startRotation1, endRotation1, t);
            _rotatePoint2.rotation = Quaternion.Lerp(startRotation2, endRotation2, t);

            // ������������� ��������, ����� �������� ����
            if (t >= 1f)
            {
                _isRotate = false;
                _rotatePoint1.rotation = endRotation1;
                _rotatePoint2.rotation = endRotation2;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerController>(out PlayerController controller))
        {
            if (gameManager.totalScore >= _scoreCheck)
            {
                _isRotate = true;
            }
            else
            {
                controller.GetComponent<DanceTrigger>().StartDance();
                _winPanel.SetActive(true);
            }
        }
    }
}
