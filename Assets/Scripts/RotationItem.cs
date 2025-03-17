using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationItem : MonoBehaviour
{
    // —корость вращени€ в градусах в секунду
    [SerializeField]
    private float rotationSpeed = 90f;

    // Ќаправление вращени€ (1 = по часовой, -1 = против часовой)
    [SerializeField]
    private float rotationDirection = 1f;

    void Update()
    {
        // ¬ычисл€ем угол вращени€ за текущий кадр
        float rotationAmount = rotationSpeed * rotationDirection * Time.deltaTime;

        // ѕримен€ем вращение вокруг оси Y
        transform.Rotate(0f, rotationAmount, 0f);
    }
}
