#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public static class UISetupTool
{
    [MenuItem("NeonRush/Setup Complete UI")]
    public static void SetupUI()
    {
        // 1. Check or create EventSystem
        EventSystem es = Object.FindObjectOfType<EventSystem>();
        if (es == null)
        {
            GameObject esGO = new GameObject("EventSystem");
            esGO.AddComponent<EventSystem>();
            esGO.AddComponent<StandaloneInputModule>();
            Undo.RegisterCreatedObjectUndo(esGO, "Create EventSystem");
        }

        // 2. Check or create Canvas
        Canvas canvas = Object.FindObjectOfType<Canvas>();
        GameObject canvasGO;
        if (canvas == null)
        {
            canvasGO = new GameObject("Canvas");
            canvas = canvasGO.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            CanvasScaler scaler = canvasGO.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            scaler.matchWidthOrHeight = 0.5f;
            canvasGO.AddComponent<GraphicRaycaster>();
            Undo.RegisterCreatedObjectUndo(canvasGO, "Create Canvas");
        }
        else
        {
            canvasGO = canvas.gameObject;
            CanvasScaler scaler = canvasGO.GetComponent<CanvasScaler>();
            if (scaler == null) scaler = canvasGO.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            scaler.matchWidthOrHeight = 0.5f;
        }

        // 3. ScoreText
        Transform existingScore = canvasGO.transform.Find("ScoreText");
        GameObject scoreGO;
        if (existingScore != null)
        {
            scoreGO = existingScore.gameObject;
        }
        else
        {
            scoreGO = new GameObject("ScoreText");
            scoreGO.transform.SetParent(canvasGO.transform, false);
            Undo.RegisterCreatedObjectUndo(scoreGO, "Create ScoreText");
        }

        RectTransform scoreRect = scoreGO.GetComponent<RectTransform>();
        if (scoreRect == null) scoreRect = scoreGO.AddComponent<RectTransform>();
        scoreRect.anchorMin = new Vector2(0.5f, 1f);
        scoreRect.anchorMax = new Vector2(0.5f, 1f);
        scoreRect.pivot = new Vector2(0.5f, 1f);
        scoreRect.anchoredPosition = new Vector2(0f, -50f);
        scoreRect.sizeDelta = new Vector2(600f, 80f);

        TextMeshProUGUI scoreTMP = scoreGO.GetComponent<TextMeshProUGUI>();
        if (scoreTMP == null) scoreTMP = scoreGO.AddComponent<TextMeshProUGUI>();
        scoreTMP.text = "SCORE: 0 M";
        scoreTMP.fontSize = 42;
        scoreTMP.alignment = TextAlignmentOptions.Center;
        scoreTMP.color = new Color(0f, 1f, 1f, 1f); // Neon Cyan

        // 4. GameOverPanel
        Transform existingPanel = canvasGO.transform.Find("GameOverPanel");
        GameObject panelGO;
        if (existingPanel != null)
        {
            panelGO = existingPanel.gameObject;
        }
        else
        {
            panelGO = new GameObject("GameOverPanel");
            panelGO.transform.SetParent(canvasGO.transform, false);
            Undo.RegisterCreatedObjectUndo(panelGO, "Create GameOverPanel");
        }

        RectTransform panelRect = panelGO.GetComponent<RectTransform>();
        if (panelRect == null) panelRect = panelGO.AddComponent<RectTransform>();
        panelRect.anchorMin = Vector2.zero;
        panelRect.anchorMax = Vector2.one;
        panelRect.sizeDelta = Vector2.zero;

        Image panelImg = panelGO.GetComponent<Image>();
        if (panelImg == null) panelImg = panelGO.AddComponent<Image>();
        panelImg.color = new Color(0.04f, 0.04f, 0.08f, 0.88f);

        // TitleText inside Panel
        Transform existingTitle = panelGO.transform.Find("TitleText");
        GameObject titleGO;
        if (existingTitle != null)
        {
            titleGO = existingTitle.gameObject;
        }
        else
        {
            titleGO = new GameObject("TitleText");
            titleGO.transform.SetParent(panelGO.transform, false);
            Undo.RegisterCreatedObjectUndo(titleGO, "Create TitleText");
        }

        RectTransform titleRect = titleGO.GetComponent<RectTransform>();
        if (titleRect == null) titleRect = titleGO.AddComponent<RectTransform>();
        titleRect.anchorMin = new Vector2(0.5f, 0.75f);
        titleRect.anchorMax = new Vector2(0.5f, 0.75f);
        titleRect.pivot = new Vector2(0.5f, 0.5f);
        titleRect.anchoredPosition = Vector2.zero;
        titleRect.sizeDelta = new Vector2(800f, 120f);

        TextMeshProUGUI titleTMP = titleGO.GetComponent<TextMeshProUGUI>();
        if (titleTMP == null) titleTMP = titleGO.AddComponent<TextMeshProUGUI>();
        titleTMP.text = "CRASHED!";
        titleTMP.fontSize = 68;
        titleTMP.alignment = TextAlignmentOptions.Center;
        titleTMP.color = new Color(1f, 0.15f, 0.25f, 1f); // Neon Red/Pink

        // FinalScoreText inside Panel
        Transform existingFinal = panelGO.transform.Find("FinalScoreText");
        GameObject finalGO;
        if (existingFinal != null)
        {
            finalGO = existingFinal.gameObject;
        }
        else
        {
            finalGO = new GameObject("FinalScoreText");
            finalGO.transform.SetParent(panelGO.transform, false);
            Undo.RegisterCreatedObjectUndo(finalGO, "Create FinalScoreText");
        }

        RectTransform finalRect = finalGO.GetComponent<RectTransform>();
        if (finalRect == null) finalRect = finalGO.AddComponent<RectTransform>();
        finalRect.anchorMin = new Vector2(0.5f, 0.55f);
        finalRect.anchorMax = new Vector2(0.5f, 0.55f);
        finalRect.pivot = new Vector2(0.5f, 0.5f);
        finalRect.anchoredPosition = Vector2.zero;
        finalRect.sizeDelta = new Vector2(600f, 80f);

        TextMeshProUGUI finalTMP = finalGO.GetComponent<TextMeshProUGUI>();
        if (finalTMP == null) finalTMP = finalGO.AddComponent<TextMeshProUGUI>();
        finalTMP.text = "FINAL DISTANCE: 0 M";
        finalTMP.fontSize = 34;
        finalTMP.alignment = TextAlignmentOptions.Center;
        finalTMP.color = Color.white;

        // RestartButton inside Panel
        Transform existingBtn = panelGO.transform.Find("RestartButton");
        GameObject btnGO;
        if (existingBtn != null)
        {
            btnGO = existingBtn.gameObject;
        }
        else
        {
            btnGO = new GameObject("RestartButton");
            btnGO.transform.SetParent(panelGO.transform, false);
            Undo.RegisterCreatedObjectUndo(btnGO, "Create RestartButton");
        }

        RectTransform btnRect = btnGO.GetComponent<RectTransform>();
        if (btnRect == null) btnRect = btnGO.AddComponent<RectTransform>();
        btnRect.anchorMin = new Vector2(0.5f, 0.35f);
        btnRect.anchorMax = new Vector2(0.5f, 0.35f);
        btnRect.pivot = new Vector2(0.5f, 0.5f);
        btnRect.anchoredPosition = Vector2.zero;
        btnRect.sizeDelta = new Vector2(260f, 65f);

        Image btnImg = btnGO.GetComponent<Image>();
        if (btnImg == null) btnImg = btnGO.AddComponent<Image>();
        btnImg.color = new Color(0.15f, 0.85f, 1f, 1f); // Neon Cyan

        Button btn = btnGO.GetComponent<Button>();
        if (btn == null) btn = btnGO.AddComponent<Button>();

        // Button text
        Transform existingBtnText = btnGO.transform.Find("Text (TMP)");
        GameObject btnTextGO;
        if (existingBtnText != null)
        {
            btnTextGO = existingBtnText.gameObject;
        }
        else
        {
            btnTextGO = new GameObject("Text (TMP)");
            btnTextGO.transform.SetParent(btnGO.transform, false);
        }

        RectTransform btnTextRect = btnTextGO.GetComponent<RectTransform>();
        if (btnTextRect == null) btnTextRect = btnTextGO.AddComponent<RectTransform>();
        btnTextRect.anchorMin = Vector2.zero;
        btnTextRect.anchorMax = Vector2.one;
        btnTextRect.sizeDelta = Vector2.zero;

        TextMeshProUGUI btnTMP = btnTextGO.GetComponent<TextMeshProUGUI>();
        if (btnTMP == null) btnTMP = btnTextGO.AddComponent<TextMeshProUGUI>();
        btnTMP.text = "TRY AGAIN";
        btnTMP.fontSize = 26;
        btnTMP.fontStyle = FontStyles.Bold;
        btnTMP.alignment = TextAlignmentOptions.Center;
        btnTMP.color = Color.black;

        // Hide GameOverPanel by default
        panelGO.SetActive(false);

        // 5. Setup UIManager on GameManager object
        GameManager gm = Object.FindObjectOfType<GameManager>();
        if (gm == null)
        {
            GameObject gmGO = new GameObject("GameManager");
            gm = gmGO.AddComponent<GameManager>();
            Undo.RegisterCreatedObjectUndo(gmGO, "Create GameManager");
        }

        UIManager uiMgr = gm.GetComponent<UIManager>();
        if (uiMgr == null) uiMgr = gm.gameObject.AddComponent<UIManager>();

        SerializedObject so = new SerializedObject(uiMgr);
        so.FindProperty("scoreText").objectReferenceValue = scoreTMP;
        so.FindProperty("gameOverPanel").objectReferenceValue = panelGO;
        so.FindProperty("finalScoreText").objectReferenceValue = finalTMP;
        so.FindProperty("restartButton").objectReferenceValue = btn;
        so.ApplyModifiedProperties();

        EditorUtility.SetDirty(uiMgr);
        EditorUtility.SetDirty(gm.gameObject);

        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());

        EditorUtility.DisplayDialog("NeonRush", "تم بناء وربط واجهة المستخدم بنجاح! اضغط Play للتجربة.", "تمام");
        Debug.Log("<color=cyan><b>[NeonRush]</b> تم إنشاء وتوصيل جميع عناصر الواجهة بنجاح!</color>");
    }
}
#endif
