using UnityEngine;

public class WalkAnimationController : MonoBehaviour
{
    [Header("Animation Settings")]
    [SerializeField] private Animator animator;           // ������ �� ��������� Animator
    [SerializeField] private float transitionSpeed = 0.1f; // �������� �������� �������� ����� ����������

    // ������� ��� ��������
    private const float SAD_WALK_THRESHOLD = 0f;          // �������� ������ (����� 0)
    private const float NORMAL_WALK_THRESHOLD = 150f;     // ������� ������ (����� 150)
    private const float HAPPY_WALK_THRESHOLD = 300f;      // ���������� ������ (����� 300)

    private GameManager gameManager;                      // ������ �� GameManager ��� ��������� �����
    private float currentAnimationValue;                  // ������� �������� ��� ��������

    private void Start()
    {
        // ������� GameManager
        gameManager = FindObjectOfType<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("GameManager �� ������!");
        }

        // ������� Animator
        if (animator == null)
        {
            animator = GetComponent<Animator>();
            if (animator == null)
            {
                Debug.LogError("Animator �� ������ �� �������!");
            }
        }

        // �������������� ������� �������� ��������
        currentAnimationValue = gameManager != null ? gameManager.GetScore() : 0f;
        UpdateAnimation();
    }

    private void Update()
    {
        if (gameManager == null || animator == null) return;

        // �������� ������� ���������� �����
        float targetValue = gameManager.GetScore();

        // ������ ������������� ������� �������� � ��������
        currentAnimationValue = Mathf.Lerp(currentAnimationValue, targetValue, transitionSpeed * Time.deltaTime);

        // ��������� ��������
        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        // ������������� �������� �������� � Animator
        animator.SetFloat("WalkSpeed", currentAnimationValue);
    }
}