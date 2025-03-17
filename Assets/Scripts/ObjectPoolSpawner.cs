using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class ObjectPoolSpawner : MonoBehaviour
{
    // ������ ������� ��� ����
    [SerializeField]
    private GameObject objectPrefab;

    // ������ ����
    [SerializeField]
    private int poolSize = 10;

    // ����� ���������� ������� � ��������
    private float lifetime = 1f;

    // ��������� � �������� �������
    private Vector3 startScale = new Vector3(0.01f, 0.01f, 0.01f);
    private Vector3 endScale = new Vector3(0.02f, 0.02f, 0.02f);

    // ��� ��������
    private Queue<GameObject> objectPool;

    void Start()
    {
        // �������������� ���
        objectPool = new Queue<GameObject>();

        // ������� ������� ��� ����
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(objectPrefab, transform.position, Quaternion.identity);
            obj.SetActive(false);
            objectPool.Enqueue(obj);
        }

        // ��������� �������� �����
        //SpawnObject();
    }

    public void SpawnObject(Transform t, int score, Color color)
    {
        // ����� ������ �� ����
        GameObject obj = GetObjectFromPool();
        if (obj != null)
        {
            // ���������� ������ � ������������� ��������� ���������
            obj.transform.position = new Vector3(t.position.x, t.position.y + 0.5f, t.position.z);
            obj.GetComponent<TextMeshProUGUI>().color = new Color(color.r, color.g, color.b);
            obj.GetComponent<TextMeshProUGUI>().text = score + "";
            obj.transform.localScale = startScale;
            obj.SetActive(true);

            // ��������� ��������
            StartCoroutine(ScaleAndReturn(obj));
        }
    }

    private GameObject GetObjectFromPool()
    {
        if (objectPool.Count > 0)
        {
            return objectPool.Dequeue();
        }
        else
        {
            // ���� ��� ����, ������� ����� ������
            GameObject obj = Instantiate(objectPrefab, transform.position, Quaternion.identity);
            obj.SetActive(false);
            return obj;
        }
    }

    private System.Collections.IEnumerator ScaleAndReturn(GameObject obj)
    {
        float elapsedTime = 0f;

        while (elapsedTime < lifetime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / lifetime;
            obj.transform.localScale = Vector3.Lerp(startScale, endScale, t);
            yield return null;
        }

        // ������������� �������� �������
        obj.transform.localScale = endScale;

        // ������������ ������ � ���������� � ���
        obj.SetActive(false);
        objectPool.Enqueue(obj);
    }
}