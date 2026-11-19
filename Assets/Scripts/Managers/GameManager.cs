using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // نمط الـ Singleton عشان نقدر نوصل للمدير من أي اسكريبت بسهولة
    public static GameManager Instance;

    public bool isGameOver { get; private set; } = false;
    public float score { get; private set; } = 0f;

    [Tooltip("اسحب هنا سيارة اللاعب لتتبع المسافة")]
    [SerializeField] private Transform playerTransform;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (playerTransform == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerTransform = player.transform;
            }
        }
    }

    private void Update()
    {
        if (isGameOver) return;

        // حساب السكور بناءً على المسافة المقطوعة للأمام
        if (playerTransform != null)
        {
            score = playerTransform.position.z;
        }
    }

    public void TriggerGameOver()
    {
        if (isGameOver) return;

        isGameOver = true;
        Debug.Log("GAME OVER! Final Score: " + Mathf.FloorToInt(score));

        // إيقاف حركة السيارة
        CarController car = FindObjectOfType<CarController>();
        if (car != null)
        {
            car.StopCar();
        }
    }
}
