using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameDifficulty
{
    Easy,
    Medium,
    Hard
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool isGameStarted { get; private set; } = false;
    public bool isGameOver { get; private set; } = false;
    public float score { get; private set; } = 0f;
    public float highScore { get; private set; } = 0f;

    [Header("Difficulty Configuration")]
    public GameDifficulty currentDifficulty = GameDifficulty.Medium;

    public float minSpeed { get; private set; } = 25f;
    public float maxSpeed { get; private set; } = 55f;
    public float currentSpeed = 25f;

    [Header("Player Reference")]
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
            return;
        }

        ApplyDifficultySettings(currentDifficulty);
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
        if (!isGameStarted || isGameOver) return;

        // حساب السكور بالمسافة
        if (playerTransform != null)
        {
            score = playerTransform.position.z;
        }
    }

    public void SetDifficulty(GameDifficulty diff)
    {
        currentDifficulty = diff;
        ApplyDifficultySettings(diff);
    }

    private void ApplyDifficultySettings(GameDifficulty diff)
    {
        float obstacleDist = 25f;

        switch (diff)
        {
            case GameDifficulty.Easy:
                minSpeed = 16f; // ~60 km/h
                maxSpeed = 28f; // ~100 km/h
                obstacleDist = 38f;
                break;

            case GameDifficulty.Medium:
                minSpeed = 24f; // ~85 km/h
                maxSpeed = 56f; // ~200 km/h
                obstacleDist = 25f;
                break;

            case GameDifficulty.Hard:
                minSpeed = 35f; // ~125 km/h
                maxSpeed = 84f; // ~300 km/h
                obstacleDist = 16f;
                break;
        }

        currentSpeed = minSpeed;

        // تحديث أعلى سكور لهذا المستوى
        highScore = PlayerPrefs.GetFloat("NeonRush_HighScore_" + diff.ToString(), 0f);

        // إبلاغ مولد العوائق بالمسافة الجديدة
        ObstacleSpawner spawner = FindObjectOfType<ObstacleSpawner>();
        if (spawner != null)
        {
            spawner.SetObstacleDistance(obstacleDist);
        }
    }

    public float GetHighScoreFor(GameDifficulty diff)
    {
        return PlayerPrefs.GetFloat("NeonRush_HighScore_" + diff.ToString(), 0f);
    }

    public void StartGame()
    {
        if (isGameStarted) return;

        isGameStarted = true;

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayClick();
            AudioManager.Instance.StartEngine();
        }

        if (UIManager.Instance != null)
        {
            UIManager.Instance.HideStartMenu();
        }
    }

    public void QuitGame()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayClick();
        }

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void TriggerGameOver()
    {
        if (isGameOver) return;

        isGameOver = true;

        // 1. إيقاف حركة السيارة
        CarController car = FindObjectOfType<CarController>();
        if (car != null)
        {
            car.StopCar();
        }

        // 2. إيقاف صوت المحرك وتشغيل صوت الاصطدام
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StopEngine();
            AudioManager.Instance.PlayCrash();
        }

        // 3. اهتزاز الكاميرا
        CameraFollow cam = FindObjectOfType<CameraFollow>();
        if (cam != null)
        {
            cam.TriggerShake(0.55f, 0.55f);
        }

        // 4. فحص وحفظ أعلى سكور لهذا المستوى المحدد
        bool isNewRecord = false;
        string key = "NeonRush_HighScore_" + currentDifficulty.ToString();
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetFloat(key, highScore);
            PlayerPrefs.Save();
            isNewRecord = true;
        }

        // 5. إظهار شاشة الخسارة مع النتائج
        if (UIManager.Instance != null)
        {
            UIManager.Instance.ShowGameOverUI(score, highScore, isNewRecord);
        }
    }
}
