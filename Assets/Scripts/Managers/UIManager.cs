using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Start Menu UI")]
    [SerializeField] private GameObject startMenuPanel;
    [SerializeField] private TextMeshProUGUI startHighScoreText;
    [SerializeField] private Button startButton;
    [SerializeField] private Button openDifficultyButton;
    [SerializeField] private Button exitButton;

    [Header("Difficulty Selection UI")]
    [SerializeField] private GameObject difficultyPanel;
    [SerializeField] private Button easyButton;
    [SerializeField] private Button mediumButton;
    [SerializeField] private Button hardButton;
    [SerializeField] private Button backDifficultyButton;
    [SerializeField] private TextMeshProUGUI easyRecordText;
    [SerializeField] private TextMeshProUGUI mediumRecordText;
    [SerializeField] private TextMeshProUGUI hardRecordText;

    [Header("In-Game UI")]
    [SerializeField] private TextMeshProUGUI scoreText;

    [Header("Game Over UI")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private TextMeshProUGUI bestScoreText;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button gameOverBackButton;   // يرجع للـ Start Menu
    [SerializeField] private Button gameOverExitButton;  // يغلق اللعبة

    public static bool autoStartOnLoad = false;

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
        // إذا كان جاي من زر TRY AGAIN، نبدأ اللعب فوراً بدون إظهار المنيو
        if (autoStartOnLoad)
        {
            autoStartOnLoad = false;

            if (startMenuPanel != null) startMenuPanel.SetActive(false);
            if (difficultyPanel != null) difficultyPanel.SetActive(false);
            if (gameOverPanel != null) gameOverPanel.SetActive(false);

            if (GameManager.Instance != null)
            {
                GameManager.Instance.StartGame();
            }
        }
        else
        {
            // 1. إظهار شاشة البداية وإخفاء باقي اللوحات
            if (startMenuPanel != null) startMenuPanel.SetActive(true);
            if (difficultyPanel != null) difficultyPanel.SetActive(false);
            if (gameOverPanel != null) gameOverPanel.SetActive(false);
        }

        // 2. تحديث السكور القياسي لشاشة البداية
        UpdateHighScoreDisplays();

        // 3. ربط أزرار شاشة البداية
        if (startButton != null)
        {
            startButton.onClick.RemoveAllListeners();
            startButton.onClick.AddListener(OnStartClicked);
        }

        if (openDifficultyButton != null)
        {
            openDifficultyButton.onClick.RemoveAllListeners();
            openDifficultyButton.onClick.AddListener(OpenDifficultyMenu);
        }

        if (exitButton != null)
        {
            exitButton.onClick.RemoveAllListeners();
            exitButton.onClick.AddListener(OnExitClicked);
        }

        // 4. ربط أزرار شاشة الصعوبة
        if (easyButton != null)
        {
            easyButton.onClick.RemoveAllListeners();
            easyButton.onClick.AddListener(() => ChooseDifficulty(GameDifficulty.Easy));
        }

        if (mediumButton != null)
        {
            mediumButton.onClick.RemoveAllListeners();
            mediumButton.onClick.AddListener(() => ChooseDifficulty(GameDifficulty.Medium));
        }

        if (hardButton != null)
        {
            hardButton.onClick.RemoveAllListeners();
            hardButton.onClick.AddListener(() => ChooseDifficulty(GameDifficulty.Hard));
        }

        if (backDifficultyButton != null)
        {
            backDifficultyButton.onClick.RemoveAllListeners();
            backDifficultyButton.onClick.AddListener(BackToStartMenu);
        }

        // 5. ربط أزرار شاشة الخسارة
        if (restartButton != null)
        {
            restartButton.onClick.RemoveAllListeners();
            restartButton.onClick.AddListener(RestartGame);
        }

        if (gameOverExitButton != null)
        {
            gameOverExitButton.onClick.RemoveAllListeners();
            gameOverExitButton.onClick.AddListener(OnExitClicked);
        }

        if (gameOverBackButton != null)
        {
            gameOverBackButton.onClick.RemoveAllListeners();
            gameOverBackButton.onClick.AddListener(BackToStartMenuFromGameOver);
        }
    }

    private void Update()
    {
        // تحديث عداد السكور وسرعة اللعب الحقيقية ومستوى الصعوبة أثناء الجري
        if (GameManager.Instance != null && GameManager.Instance.isGameStarted && !GameManager.Instance.isGameOver)
        {
            if (scoreText != null)
            {
                int scoreInt = Mathf.FloorToInt(GameManager.Instance.score);
                int speedInt = Mathf.FloorToInt(GameManager.Instance.currentSpeed * 3.6f); // تحويل إلى كم/ساعة
                string diffTag = GameManager.Instance.currentDifficulty.ToString().ToUpper();
                string diffColor = (GameManager.Instance.currentDifficulty == GameDifficulty.Easy) ? "#00FF66" :
                                   (GameManager.Instance.currentDifficulty == GameDifficulty.Medium) ? "#FFDD00" : "#FF1744";

                scoreText.text = $"<color={diffColor}>[{diffTag}]</color>  SCORE: {scoreInt} M   <color=#00FFFF>{speedInt} KM/H</color>";
            }
        }
    }

    public void OpenDifficultyMenu()
    {
        if (AudioManager.Instance != null) AudioManager.Instance.PlayClick();
        if (startMenuPanel != null) startMenuPanel.SetActive(false);
        if (difficultyPanel != null) difficultyPanel.SetActive(true);
        UpdateHighScoreDisplays();
    }

    public void BackToStartMenu()
    {
        if (AudioManager.Instance != null) AudioManager.Instance.PlayClick();
        if (difficultyPanel != null) difficultyPanel.SetActive(false);
        if (startMenuPanel != null) startMenuPanel.SetActive(true);
        UpdateHighScoreDisplays();
    }

    // ترجع للـ Start Menu من شاشة Game Over (reload المشهد وإظهار المنيو)
    public void BackToStartMenuFromGameOver()
    {
        if (AudioManager.Instance != null) AudioManager.Instance.PlayClick();
        
        autoStartOnLoad = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ChooseDifficulty(GameDifficulty diff)
    {
        if (AudioManager.Instance != null) AudioManager.Instance.PlayClick();

        if (GameManager.Instance != null)
        {
            GameManager.Instance.SetDifficulty(diff);
        }

        // إخفاء المنيو وبدء السباق مباشرة في هذا المستوى!
        if (difficultyPanel != null) difficultyPanel.SetActive(false);
        if (startMenuPanel != null) startMenuPanel.SetActive(false);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.StartGame();
        }
    }

    private void UpdateHighScoreDisplays()
    {
        if (GameManager.Instance == null) return;

        if (startHighScoreText != null)
        {
            startHighScoreText.text = $"{GameManager.Instance.currentDifficulty.ToString().ToUpper()} RECORD: {Mathf.FloorToInt(GameManager.Instance.highScore)} M";
        }

        if (easyRecordText != null)
        {
            easyRecordText.text = "BEST: " + Mathf.FloorToInt(GameManager.Instance.GetHighScoreFor(GameDifficulty.Easy)) + " M";
        }

        if (mediumRecordText != null)
        {
            mediumRecordText.text = "BEST: " + Mathf.FloorToInt(GameManager.Instance.GetHighScoreFor(GameDifficulty.Medium)) + " M";
        }

        if (hardRecordText != null)
        {
            hardRecordText.text = "BEST: " + Mathf.FloorToInt(GameManager.Instance.GetHighScoreFor(GameDifficulty.Hard)) + " M";
        }
    }

    public void HideStartMenu()
    {
        if (startMenuPanel != null) startMenuPanel.SetActive(false);
        if (difficultyPanel != null) difficultyPanel.SetActive(false);
    }

    public void ShowGameOverUI(float finalScore, float bestScore, bool isNewRecord)
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        if (finalScoreText != null)
        {
            finalScoreText.text = "FINAL DISTANCE: " + Mathf.FloorToInt(finalScore) + " M";
        }

        if (bestScoreText != null)
        {
            string diffName = (GameManager.Instance != null) ? GameManager.Instance.currentDifficulty.ToString().ToUpper() : "";
            if (isNewRecord)
            {
                bestScoreText.text = $"<color=#FFFF00>★ NEW {diffName} RECORD: {Mathf.FloorToInt(bestScore)} M ★</color>";
            }
            else
            {
                bestScoreText.text = $"{diffName} BEST: {Mathf.FloorToInt(bestScore)} M";
            }
        }
    }

    public void OnStartClicked()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.StartGame();
        }
    }

    public void OnExitClicked()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.QuitGame();
        }
    }

    // دالة زر TRY AGAIN (إعادة تشغيل اللعبة فوراً دون المرور بالقائمة الرئيسية)
    public void RestartGame()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayClick();
        }

        autoStartOnLoad = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
