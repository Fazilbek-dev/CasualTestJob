using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class ObjectPoolSpawner : MonoBehaviour
{
    // Префаб объекта для пула
    [SerializeField]
    private GameObject objectPrefab;

    // Размер пула
    [SerializeField]
    private int poolSize = 10;

    // Время активности объекта в секундах
    private float lifetime = 1f;

    // Начальный и конечный масштаб
    private Vector3 startScale = new Vector3(0.01f, 0.01f, 0.01f);
    private Vector3 endScale = new Vector3(0.02f, 0.02f, 0.02f);

    // Пул объектов
    private Queue<GameObject> objectPool;

    void Start()
    {
        // Инициализируем пул
        objectPool = new Queue<GameObject>();

        // Создаем объекты для пула
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(objectPrefab, transform.position, Quaternion.identity);
            obj.SetActive(false);
            objectPool.Enqueue(obj);
        }

        // Запускаем тестовый спавн
        //SpawnObject();
    }

    public void SpawnObject(Transform t, int score, Color color)
    {
        // Берем объект из пула
        GameObject obj = GetObjectFromPool();
        if (obj != null)
        {
            // Активируем объект и устанавливаем начальные параметры
            obj.transform.position = new Vector3(t.position.x, t.position.y + 0.5f, t.position.z);
            obj.GetComponent<TextMeshProUGUI>().color = new Color(color.r, color.g, color.b);
            obj.GetComponent<TextMeshProUGUI>().text = score + "";
            obj.transform.localScale = startScale;
            obj.SetActive(true);

            // Запускаем анимацию
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
            // Если пул пуст, создаем новый объект
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

        // Устанавливаем конечный масштаб
        obj.transform.localScale = endScale;

        // Деактивируем объект и возвращаем в пул
        obj.SetActive(false);
        objectPool.Enqueue(obj);
    }
}