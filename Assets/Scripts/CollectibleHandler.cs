using UnityEngine;

public class CollectibleHandler : MonoBehaviour
{
    [Header("Scoring Settings")]
    [SerializeField] private int moneyPoints = 10;    // ���� �� ������ �����
    [SerializeField] private int alcoholPoints = -5;  // ���� �� ������ �������� (������������� ��������)

    [SerializeField] private GameManager gameManager;                  // ������ �� GameManager

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        // ������� GameManager � �����
        gameManager = FindObjectOfType<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("GameManager �� ������ � �����! �������� ������ � ����������� GameManager.");
        }
    }

    // ��������� ������������ � ����������� ���������
    private void OnTriggerEnter(Collider other)
    {
        if (gameManager == null) return; // �������� �� ������� GameManager

        if (other.TryGetComponent<CollectableItem>(out CollectableItem collectableItem))
        {
            // ������ �����
            gameManager.AddScore(collectableItem._scoreAdd);
            audioSource.clip = collectableItem._clip;
            audioSource.Play();
            Color c = Color.white;
            if(collectableItem._scoreAdd > 0f)
            {
                c = Color.green;
            }
            else
            {
                c = Color.red;
            }
            //gameManager._spawner.SpawnObject(collectableItem.transform, collectableItem._scoreAdd, c);
            Destroy(other.gameObject); // ���������� ������ �����
        }
    }

    // ����� ��� ��������� �������� ���������� ����� (�����������, ��� ���������� �������������)
    public int GetScore()
    {
        return gameManager != null ? gameManager.GetScore() : 0;
    }
}