using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [Header("Lane Settings")]
    [Tooltip("المسافة بين كل مسار والآخر")]
    [SerializeField] private float laneDistance = 3f;
    [Tooltip("سرعة الانتقال الأفقي بين المسارات")]
    [SerializeField] private float laneChangeSpeed = 16f;

    [Header("Speed Control Settings")]
    [Tooltip("قوة التسارع عند الضغط على زر البنزين (سهم لأعلى / W)")]
    [SerializeField] private float accelerationRate = 24f;
    [Tooltip("قوة الفرامل عند الضغط على زر الفرامل (سهم لأسفل / S)")]
    [SerializeField] private float brakeRate = 38f;

    private float currentSpeed = 20f;
    private int currentLane = 0;
    private bool isDead = false;

    private void Start()
    {
        if (GameManager.Instance != null)
        {
            currentSpeed = GameManager.Instance.minSpeed;
        }
    }

    private void Update()
    {
        if (isDead) return;

        // لا تتحرك قبل الضغط على Start Game في القائمة
        if (GameManager.Instance != null && !GameManager.Instance.isGameStarted) return;

        HandleLaneInput();
        HandleSpeedInput();
        MoveCar();
    }

    private void HandleLaneInput()
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

    private void HandleSpeedInput()
    {
        float minSpd = (GameManager.Instance != null) ? GameManager.Instance.minSpeed : 18f;
        float maxSpd = (GameManager.Instance != null) ? GameManager.Instance.maxSpeed : 55f;

        // الضغط على بنزين (Up Arrow أو W)
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, maxSpd, accelerationRate * Time.deltaTime);
        }
        // الضغط على فرامل (Down Arrow أو S)
        else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, minSpd, brakeRate * Time.deltaTime);
        }
        else
        {
            // عند ترك الزر، سرعة سير انسيابية في الثلث الأول بين الأدنى والأقصى
            float cruiseSpeed = minSpd + (maxSpd - minSpd) * 0.35f;
            currentSpeed = Mathf.MoveTowards(currentSpeed, cruiseSpeed, (accelerationRate * 0.5f) * Time.deltaTime);
        }

        currentSpeed = Mathf.Clamp(currentSpeed, minSpd, maxSpd);

        // تحديث السرعة في GameManager
        if (GameManager.Instance != null)
        {
            GameManager.Instance.currentSpeed = currentSpeed;
        }

        // ضبط نغمة المحرك مع سرعة البنزين
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetEnginePitch(currentSpeed / 22f);
        }
    }

    private void ShiftLane(int direction)
    {
        currentLane = Mathf.Clamp(currentLane + direction, -1, 1);
    }

    private void MoveCar()
    {
        // 1. حساب موضع المسار المستهدف على محور X
        float targetX = currentLane * laneDistance;

        // 2. التحرك بنعومة نحو المسار المستهدف
        float newX = Mathf.MoveTowards(transform.position.x, targetX, laneChangeSpeed * Time.deltaTime);

        // 3. التحرك للأمام باستمرار على محور Z بالسرعة الحالية
        float newZ = transform.position.z + (currentSpeed * Time.deltaTime);

        // 4. تطبيق الموقع الجديد على مجسم السيارة
        transform.position = new Vector3(newX, transform.position.y, newZ);
    }

    public void StopCar()
    {
        isDead = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            StopCar();

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
            StopCar();

            if (GameManager.Instance != null)
            {
                GameManager.Instance.TriggerGameOver();
            }
        }
    }
}
