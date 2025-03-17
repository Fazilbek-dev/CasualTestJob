using ButchersGames;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public ObjectPoolSpawner _spawner;

    [SerializeField] private GameObject _losePanel;
    [SerializeField] private PlayerController playerController;

    [SerializeField] private GameObject _poor;
    [SerializeField] private GameObject _middle;
    [SerializeField] private GameObject _rich;

    // ����������� ���������� ��� �������� �����
    public int totalScore = 0;

    // �������� ��� ������� � ���������� GameManager
    private static GameManager instance;

    private void Awake()
    {
        // ����������, ��� ���������� ������ ���� GameManager
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // ��������� ������ ����� �������
        }
        else
        {
            Destroy(gameObject); // ���������� ���������
        }

        // ������������� �����
        totalScore = 0;
        Debug.Log("GameManager ���������������! ����: " + totalScore);
    }

    private void Update()
    {
        if(totalScore < 0f)
        {
            _losePanel.SetActive(true);
            playerController.enabled = false;
        }
        if(totalScore < 100f)
        {
            _middle.SetActive(false);
            _rich.SetActive(false);
            _poor.SetActive(true);
        }
        else if(totalScore >= 100f && totalScore < 200f)
        {
            _middle.SetActive(true);
            _rich.SetActive(false);
            _poor.SetActive(false);
        }
        else
        {
            _rich.SetActive(true);
            _poor.SetActive(false);
            _middle.SetActive(false);
        }
    }

    // ����� ��� ���������� �����
    public void AddScore(int points)
    {
        totalScore += points;
        Debug.Log("���� ���������! ������� ����: " + totalScore);
    }

    // ����� ��� ��������� ������� �����
    public int GetScore()
    {
        return totalScore;
    }

    // ����� ��� ������ ����� (���� �����)
    public void ResetScore()
    {
        totalScore = 0;
        Debug.Log("���� ��������! ������� ����: " + totalScore);
    }
}