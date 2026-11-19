using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [Header("Forward Movement")]
    [Tooltip("سرعة التحرك للأمام بوحدة لكل ثانية")]
    [SerializeField] private float forwardSpeed = 20f;

    [Header("Lane Settings")]
    [Tooltip("المسافة بين كل مسار والآخر")]
    [SerializeField] private float laneDistance = 3f;
    [Tooltip("سرعة الانتقال الأفقي بين المسارات")]
    [SerializeField] private float laneChangeSpeed = 15f;

    // فهرس المسار الحالي: -1 = يسار، 0 = وسط، 1 = يمين
    private int currentLane = 0;
    private bool isDead = false;

    private void Update()
    {
        if (isDead) return;

        HandleInput();
        MoveCar();
    }

    private void HandleInput()
    {
        // التحرك لليسار بالضغط على A أو السهم الأيسر
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            ShiftLane(-1);
        }
        // التحرك لليمين بالضغط على D أو السهم الأيمن
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            ShiftLane(1);
        }
    }

    private void ShiftLane(int direction)
    {
        // حصر المسار بين -1 و 1 لمنع الخروج عن الطريق
        currentLane = Mathf.Clamp(currentLane + direction, -1, 1);
    }

    private void MoveCar()
    {
        // 1. حساب موضع المسار المستهدف على محور X
        float targetX = currentLane * laneDistance;

        // 2. التحرك بنعومة نحو المسار المستهدف
        float newX = Mathf.MoveTowards(transform.position.x, targetX, laneChangeSpeed * Time.deltaTime);

        // 3. التحرك للأمام باستمرار على محور Z
        float newZ = transform.position.z + (forwardSpeed * Time.deltaTime);

        // 4. تطبيق الموقع الجديد على مجسم السيارة
        transform.position = new Vector3(newX, transform.position.y, newZ);
    }

    // تُستدعى لإيقاف السيارة عند الاصطدام لاحقاً
    public void StopCar()
    {
        isDead = true;
    }
    // رصد التصادم الفيزيائي مع العوائق
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            // إيقاف السيارة فوراً
            StopCar();

            // إبلاغ المدير لحساب السكور والـ Game Over
            if (GameManager.Instance != null)
            {
                GameManager.Instance.TriggerGameOver();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.TriggerGameOver();
            }
        }
    }
}
