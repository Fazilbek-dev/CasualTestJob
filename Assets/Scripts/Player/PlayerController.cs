using UnityEngine;

namespace ButchersGames
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Player Movement Settings")]
        [SerializeField] private float forwardSpeed = 5f;     // �������� �������� ����� ��������� �������

        [Header("Character Movement Settings")]
        [SerializeField] private Transform characterModel;    // ������ �� ������ ��������� (�������� ������)
        [SerializeField] private float sideSpeed = 3f;        // �������� �������� �����/������ ������
        [SerializeField] private float maxSideOffset = 5f;    // ������������ �������� �����/������ ������
        [SerializeField] private float touchSensitivity = 0.01f; // ���������������� �������� ������
        [SerializeField] private float holdSideSpeed = 2f;    // �������� �������� ������ ��� ��������� ������

        [Header("Character Rotation Settings")]
        [SerializeField] private float maxRotationAngle = 45f; // ������������ ���� �������� ������ (�������)
        [SerializeField] private float rotationSpeed = 5f;    // �������� �������� ������
        [SerializeField] private float maxTouchDeltaForRotation = 4f; // ������������ �������� ������ ��� ������� ��������
        [SerializeField] private float touchMoveThreshold = 1f; // ����� �������� ������ (� ��������) ��� ����������� �������������

        private Vector3 initialCharacterPosition; // ��������� ��������� ������� ������
        private float touchStartPosX;             // ��������� ������� ������� �� X ��� ������� ��������
        private float lastTouchPosX;              // ��������� ������� ������� ��� ������� ��������
        private bool isTouching = false;          // ���� �������
        private Quaternion targetRotation;        // ������� ������� ��� ������
        private float currentXOffset;             // ������� �������� ������ �� X ������������ ��������� �������

        public bool _isRotating;

        private void Start()
        {
            // ���������� ��������� ��������� ������� � ������� ������
            if (characterModel != null)
            {
                initialCharacterPosition = characterModel.localPosition;
                currentXOffset = 0f; // ���������� �������� ����� 0
                targetRotation = characterModel.localRotation;
            }
            else
            {
                Debug.LogError("Character Model �� �������� � ����������!");
            }
        }

        private void Update()
        {
            if(!_isRotating)
            // �������������� �������� ����� ��������� �������
            transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);

            // ��������� �����
            HandleTouchInput();

            // ����������� �������� ������ �� ��������
            ClampSideMovement();

            // ������� ������� ������ ���������
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
                        // ������ �������
                        touchStartPosX = touch.position.x;
                        lastTouchPosX = touch.position.x; // ���������� ��������� ������� ������� ��� ��������
                        isTouching = true;
                        break;

                    case TouchPhase.Moved:
                    case TouchPhase.Stationary:
                        // �������� ��� ��������� ������
                        if (isTouching && characterModel != null)
                        {
                            // �������� ������ �����/������ ������������ ���������� ��������� ������
                            float touchDeltaX = (touch.position.x - lastTouchPosX) * touchSensitivity;
                            currentXOffset += touchDeltaX * sideSpeed; // ��������� ������� ��������
                            Vector3 newPosition = characterModel.localPosition;
                            newPosition.x = initialCharacterPosition.x + currentXOffset;
                            characterModel.localPosition = newPosition;

                            // ���������, �������� �� �����
                            float touchMoveDistance = touch.deltaPosition.magnitude;
                            if (touchMoveDistance < touchMoveThreshold)
                            {
                                // ���� ����� ����������, ���������� ������� � 0 � ��������� ��������� ������� �������
                                targetRotation = Quaternion.identity;
                                touchStartPosX = touch.position.x;
                            }
                            else
                            {
                                // ��������� ���� �������� �� ������ ����������� �������� ������
                                float rotationDeltaX = touch.position.x - lastTouchPosX; // ���������� ������� � ��������� ��������
                                float rotationFactor = Mathf.Clamp(rotationDeltaX / maxTouchDeltaForRotation, -1f, 1f); // ����������� �� -1 �� 1
                                float targetAngle = rotationFactor * maxRotationAngle;
                                targetRotation = Quaternion.Euler(0f, targetAngle, 0f);
                            }

                            // �������������� �������� ������ ��� ��������� ������
                            float touchPositionX = touch.position.x;
                            float screenCenter = Screen.width / 2f;
                            if (touchPositionX < screenCenter - 50f) // ��������� � ����� ����� ������
                            {
                                MoveCharacterSideways(-holdSideSpeed);
                            }
                            else if (touchPositionX > screenCenter + 50f) // ��������� � ������ ����� ������
                            {
                                MoveCharacterSideways(holdSideSpeed);
                            }

                            // ��������� ��������� ������� �������
                            lastTouchPosX = touch.position.x;
                        }
                        break;

                    case TouchPhase.Ended:
                        // ����� �������
                        isTouching = false;
                        targetRotation = Quaternion.identity; // ���������� ������� ������ � �������� ��������� (0 ��������)
                        break;
                }
            }
        }

        private void MoveCharacterSideways(float speed)
        {
            // �������� ������ ����� ��� ������ ��� ���������
            if (characterModel != null)
            {
                currentXOffset += speed * Time.deltaTime; // ��������� ������� ��������
                Vector3 newPosition = characterModel.localPosition;
                newPosition.x = initialCharacterPosition.x + currentXOffset;
                characterModel.localPosition = newPosition;
            }
        }

        private void ClampSideMovement()
        {
            // ������������ �������� ������ �� ��������
            if (characterModel != null)
            {
                Vector3 currentPosition = characterModel.localPosition;
                currentPosition.x = Mathf.Clamp(currentPosition.x, initialCharacterPosition.x - maxSideOffset, initialCharacterPosition.x + maxSideOffset);
                characterModel.localPosition = currentPosition;
                // ��������� currentXOffset, ����� ��� ��������������� ����������� �������
                currentXOffset = currentPosition.x - initialCharacterPosition.x;
            }
        }

        // ��� ������������ � ���������
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