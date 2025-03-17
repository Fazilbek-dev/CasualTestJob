using ButchersGames;
using UnityEngine;

public class DanceTrigger : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator animator;           // ������ �� Animator ���������
    [SerializeField] private Transform characterModel;    // ������ �� ������ ��������� (�������� ������)
    [SerializeField] private PlayerController playerController; // ������ �� PlayerController

    [Header("Dance Settings")]
    [SerializeField] private float rotationSpeed = 5f;    // �������� �������� � ������

    private Camera mainCamera;                            // ������ �� ������� ������
    private bool isDancing = false;                       // ���� ��������� �����

    private void Start()
    {
        // ������� ������� ������
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("������� ������ �� �������!");
        }

        // ��������� ����������
        if (animator == null)
        {
            animator = GetComponent<Animator>();
            if (animator == null) Debug.LogError("Animator �� ������!");
        }

        if (characterModel == null)
        {
            characterModel = transform.Find("CharacterModel");
            if (characterModel == null) Debug.LogError("CharacterModel �� ������!");
        }

        if (playerController == null)
        {
            playerController = GetComponent<PlayerController>();
            if (playerController == null) Debug.LogError("PlayerController �� ������!");
        }
    }

    private void Update()
    {
        if (isDancing && mainCamera != null && characterModel != null)
        {
            // ������������ ��������� � ������
            Vector3 directionToCamera = mainCamera.transform.position - characterModel.position;
            directionToCamera.y = 0; // ���������� ������, ������� ������ �� Y
            Quaternion targetRotation = Quaternion.LookRotation(directionToCamera);
            characterModel.rotation = Quaternion.Slerp(characterModel.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    // ��������� ������� ��� ������� �����
    public void StartDance()
    {
        if (animator == null || playerController == null) return;

        // ��������� PlayerController
        playerController.enabled = false;

        // ��������� �������� �����
        animator.SetTrigger("Dance"); // ��������������, ��� ���� ������� "Dance"

        // ������������� ���� �����
        isDancing = true;

        Debug.Log("����� �������!");
    }

    // �����������: ������� ��� ��������� �����
    public void StopDance()
    {
        if (animator == null || playerController == null) return;

        // �������� PlayerController �������
        playerController.enabled = true;

        // ������������� �������� ����� (������������ � ������)
        animator.ResetTrigger("Dance");

        // ���������� ���� �����
        isDancing = false;

        Debug.Log("����� ����������!");
    }
}