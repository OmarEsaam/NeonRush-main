#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;

public static class SetupMenuAndAudio
{
    [MenuItem("NeonRush/Setup Audio & Start Menu (تركيب الأصوات والقائمة الرئيسية وزر الخروج)")]
    public static void Setup()
    {
        // 1. Setup AudioManager GameObject & AudioClips
        GameObject audioMgrGO = GameObject.Find("AudioManager");
        if (audioMgrGO == null)
        {
            audioMgrGO = new GameObject("AudioManager");
            Undo.RegisterCreatedObjectUndo(audioMgrGO, "Create AudioManager");
        }

        AudioManager audioMgr = audioMgrGO.GetComponent<AudioManager>();
        if (audioMgr == null) audioMgr = audioMgrGO.AddComponent<AudioManager>();

        AudioClip engineClip = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Audio/engine_loop.wav");
        AudioClip crashClip = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Audio/crash.wav");
        AudioClip clickClip = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Audio/button_click.wav");

        SerializedObject audioSO = new SerializedObject(audioMgr);
        if (engineClip != null) audioSO.FindProperty("engineClip").objectReferenceValue = engineClip;
        if (crashClip != null) audioSO.FindProperty("crashClip").objectReferenceValue = crashClip;
        if (clickClip != null) audioSO.FindProperty("clickClip").objectReferenceValue = clickClip;
        audioSO.ApplyModifiedProperties();
        EditorUtility.SetDirty(audioMgrGO);

        // 2. Find Canvas
        Canvas canvas = Object.FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            EditorUtility.DisplayDialog("تنبيه", "تأكد من فتح مشهد GameScene ووجود Canvas.", "تمام");
            return;
        }

        GameObject canvasGO = canvas.gameObject;

        // 3. Create or Update StartMenuPanel
        Transform existingStart = canvasGO.transform.Find("StartMenuPanel");
        GameObject startPanelGO;
        if (existingStart != null)
        {
            startPanelGO = existingStart.gameObject;
        }
        else
        {
            startPanelGO = new GameObject("StartMenuPanel");
            startPanelGO.transform.SetParent(canvasGO.transform, false);
            Undo.RegisterCreatedObjectUndo(startPanelGO, "Create StartMenuPanel");
        }

        RectTransform startRect = startPanelGO.GetComponent<RectTransform>();
        if (startRect == null) startRect = startPanelGO.AddComponent<RectTransform>();
        startRect.anchorMin = Vector2.zero;
        startRect.anchorMax = Vector2.one;
        startRect.sizeDelta = Vector2.zero;

        Image startBg = startPanelGO.GetComponent<Image>();
        if (startBg == null) startBg = startPanelGO.AddComponent<Image>();
        startBg.color = new Color(0.02f, 0.02f, 0.06f, 0.94f); // Dark Cyberpunk Background

        // Title: NEON RUSH
        GameObject titleGO = GetOrCreateChild(startPanelGO, "GameTitle");
        RectTransform titleRect = GetOrAddRect(titleGO);
        titleRect.anchorMin = new Vector2(0.5f, 0.72f);
        titleRect.anchorMax = new Vector2(0.5f, 0.72f);
        titleRect.pivot = new Vector2(0.5f, 0.5f);
        titleRect.anchoredPosition = Vector2.zero;
        titleRect.sizeDelta = new Vector2(800f, 130f);
        TextMeshProUGUI titleTMP = GetOrAddTMP(titleGO);
        titleTMP.text = "NEON RUSH";
        titleTMP.fontSize = 84;
        titleTMP.fontStyle = FontStyles.Bold;
        titleTMP.alignment = TextAlignmentOptions.Center;
        titleTMP.color = new Color(0f, 1f, 1f, 1f); // Bright Cyan

        // Subtitle
        GameObject subGO = GetOrCreateChild(startPanelGO, "Subtitle");
        RectTransform subRect = GetOrAddRect(subGO);
        subRect.anchorMin = new Vector2(0.5f, 0.61f);
        subRect.anchorMax = new Vector2(0.5f, 0.61f);
        subRect.pivot = new Vector2(0.5f, 0.5f);
        subRect.anchoredPosition = Vector2.zero;
        subRect.sizeDelta = new Vector2(600f, 50f);
        TextMeshProUGUI subTMP = GetOrAddTMP(subGO);
        subTMP.text = "— ENDLESS CYBERPUNK RUNNER —";
        subTMP.fontSize = 24;
        subTMP.alignment = TextAlignmentOptions.Center;
        subTMP.color = new Color(1f, 0.15f, 0.6f, 1f); // Neon Pink

        // START GAME Button
        GameObject startBtnGO = GetOrCreateChild(startPanelGO, "StartButton");
        RectTransform startBtnRect = GetOrAddRect(startBtnGO);
        startBtnRect.anchorMin = new Vector2(0.5f, 0.45f);
        startBtnRect.anchorMax = new Vector2(0.5f, 0.45f);
        startBtnRect.pivot = new Vector2(0.5f, 0.5f);
        startBtnRect.anchoredPosition = Vector2.zero;
        startBtnRect.sizeDelta = new Vector2(300f, 75f);
        Image startBtnImg = startBtnGO.GetComponent<Image>();
        if (startBtnImg == null) startBtnImg = startBtnGO.AddComponent<Image>();
        startBtnImg.color = new Color(0f, 0.9f, 1f, 1f); // Cyan
        Button startBtn = startBtnGO.GetComponent<Button>();
        if (startBtn == null) startBtn = startBtnGO.AddComponent<Button>();

        GameObject startBtnTextGO = GetOrCreateChild(startBtnGO, "Text (TMP)");
        RectTransform sbTextRect = GetOrAddRect(startBtnTextGO);
        sbTextRect.anchorMin = Vector2.zero;
        sbTextRect.anchorMax = Vector2.one;
        sbTextRect.sizeDelta = Vector2.zero;
        TextMeshProUGUI sbTMP = GetOrAddTMP(startBtnTextGO);
        sbTMP.text = "START GAME";
        sbTMP.fontSize = 28;
        sbTMP.fontStyle = FontStyles.Bold;
        sbTMP.alignment = TextAlignmentOptions.Center;
        sbTMP.color = Color.black;

        // EXIT Button (in Start Menu)
        GameObject exitBtnGO = GetOrCreateChild(startPanelGO, "ExitButton");
        RectTransform exitBtnRect = GetOrAddRect(exitBtnGO);
        exitBtnRect.anchorMin = new Vector2(0.5f, 0.32f);
        exitBtnRect.anchorMax = new Vector2(0.5f, 0.32f);
        exitBtnRect.pivot = new Vector2(0.5f, 0.5f);
        exitBtnRect.anchoredPosition = Vector2.zero;
        exitBtnRect.sizeDelta = new Vector2(260f, 60f);
        Image exitBtnImg = exitBtnGO.GetComponent<Image>();
        if (exitBtnImg == null) exitBtnImg = exitBtnGO.AddComponent<Image>();
        exitBtnImg.color = new Color(0.85f, 0.15f, 0.2f, 1f); // Red
        Button exitBtn = exitBtnGO.GetComponent<Button>();
        if (exitBtn == null) exitBtn = exitBtnGO.AddComponent<Button>();

        GameObject exitBtnTextGO = GetOrCreateChild(exitBtnGO, "Text (TMP)");
        RectTransform ebTextRect = GetOrAddRect(exitBtnTextGO);
        ebTextRect.anchorMin = Vector2.zero;
        ebTextRect.anchorMax = Vector2.one;
        ebTextRect.sizeDelta = Vector2.zero;
        TextMeshProUGUI ebTMP = GetOrAddTMP(exitBtnTextGO);
        ebTMP.text = "EXIT";
        ebTMP.fontSize = 24;
        ebTMP.fontStyle = FontStyles.Bold;
        ebTMP.alignment = TextAlignmentOptions.Center;
        ebTMP.color = Color.white;

        // Start Menu High Score Text
        GameObject hsGO = GetOrCreateChild(startPanelGO, "StartHighScoreText");
        RectTransform hsRect = GetOrAddRect(hsGO);
        hsRect.anchorMin = new Vector2(0.5f, 0.20f);
        hsRect.anchorMax = new Vector2(0.5f, 0.20f);
        hsRect.pivot = new Vector2(0.5f, 0.5f);
        hsRect.anchoredPosition = Vector2.zero;
        hsRect.sizeDelta = new Vector2(500f, 60f);
        TextMeshProUGUI hsTMP = GetOrAddTMP(hsGO);
        hsTMP.text = "BEST DISTANCE: 0 M";
        hsTMP.fontSize = 28;
        hsTMP.alignment = TextAlignmentOptions.Center;
        hsTMP.color = new Color(1f, 0.9f, 0.1f, 1f); // Neon Yellow

        // Start Menu active by default
        startPanelGO.SetActive(true);

        // 4. Update GameOverPanel with BestScoreText & ExitButton
        Transform goPanelTr = canvasGO.transform.Find("GameOverPanel");
        GameObject goExitBtnGO = null;
        TextMeshProUGUI bestScoreTMP = null;
        Button restartBtn = null;

        if (goPanelTr != null)
        {
            GameObject goPanel = goPanelTr.gameObject;

            // Best Score Text
            GameObject bsGO = GetOrCreateChild(goPanel, "BestScoreText");
            RectTransform bsRect = GetOrAddRect(bsGO);
            bsRect.anchorMin = new Vector2(0.5f, 0.46f);
            bsRect.anchorMax = new Vector2(0.5f, 0.46f);
            bsRect.pivot = new Vector2(0.5f, 0.5f);
            bsRect.anchoredPosition = Vector2.zero;
            bsRect.sizeDelta = new Vector2(600f, 60f);
            bestScoreTMP = GetOrAddTMP(bsGO);
            bestScoreTMP.text = "BEST: 0 M";
            bestScoreTMP.fontSize = 28;
            bestScoreTMP.alignment = TextAlignmentOptions.Center;
            bestScoreTMP.color = new Color(1f, 0.9f, 0.1f, 1f);

            // Adjust RestartButton position
            Transform rbTr = goPanel.transform.Find("RestartButton");
            if (rbTr != null)
            {
                RectTransform rbRect = rbTr.GetComponent<RectTransform>();
                rbRect.anchorMin = new Vector2(0.5f, 0.34f);
                rbRect.anchorMax = new Vector2(0.5f, 0.34f);
                restartBtn = rbTr.GetComponent<Button>();
            }

            // Add Exit Button to GameOverPanel
            goExitBtnGO = GetOrCreateChild(goPanel, "GameOverExitButton");
            RectTransform goebRect = GetOrAddRect(goExitBtnGO);
            goebRect.anchorMin = new Vector2(0.5f, 0.22f);
            goebRect.anchorMax = new Vector2(0.5f, 0.22f);
            goebRect.pivot = new Vector2(0.5f, 0.5f);
            goebRect.anchoredPosition = Vector2.zero;
            goebRect.sizeDelta = new Vector2(260f, 55f);
            Image goebImg = goExitBtnGO.GetComponent<Image>();
            if (goebImg == null) goebImg = goExitBtnGO.AddComponent<Image>();
            goebImg.color = new Color(0.85f, 0.15f, 0.2f, 1f);
            Button goExitBtn = goExitBtnGO.GetComponent<Button>();
            if (goExitBtn == null) goExitBtn = goExitBtnGO.AddComponent<Button>();

            GameObject goebTextGO = GetOrCreateChild(goExitBtnGO, "Text (TMP)");
            RectTransform goebTextRect = GetOrAddRect(goebTextGO);
            goebTextRect.anchorMin = Vector2.zero;
            goebTextRect.anchorMax = Vector2.one;
            goebTextRect.sizeDelta = Vector2.zero;
            TextMeshProUGUI goebTMP = GetOrAddTMP(goebTextGO);
            goebTMP.text = "EXIT";
            goebTMP.fontSize = 22;
            goebTMP.fontStyle = FontStyles.Bold;
            goebTMP.alignment = TextAlignmentOptions.Center;
            goebTMP.color = Color.white;

            // Keep GameOverPanel hidden by default
            goPanel.SetActive(false);
        }

        // 5. Wire into UIManager on GameManager
        GameManager gm = Object.FindObjectOfType<GameManager>();
        if (gm != null)
        {
            UIManager uiMgr = gm.GetComponent<UIManager>();
            if (uiMgr == null) uiMgr = gm.gameObject.AddComponent<UIManager>();

            SerializedObject so = new SerializedObject(uiMgr);
            so.FindProperty("startMenuPanel").objectReferenceValue = startPanelGO;
            so.FindProperty("startHighScoreText").objectReferenceValue = hsTMP;
            so.FindProperty("startButton").objectReferenceValue = startBtn;
            so.FindProperty("exitButton").objectReferenceValue = exitBtn;

            Transform scoreTr = canvasGO.transform.Find("ScoreText");
            if (scoreTr != null) so.FindProperty("scoreText").objectReferenceValue = scoreTr.GetComponent<TextMeshProUGUI>();

            if (goPanelTr != null) so.FindProperty("gameOverPanel").objectReferenceValue = goPanelTr.gameObject;

            Transform finalTr = (goPanelTr != null) ? goPanelTr.Find("FinalScoreText") : null;
            if (finalTr != null) so.FindProperty("finalScoreText").objectReferenceValue = finalTr.GetComponent<TextMeshProUGUI>();

            if (bestScoreTMP != null) so.FindProperty("bestScoreText").objectReferenceValue = bestScoreTMP;
            if (restartBtn != null) so.FindProperty("restartButton").objectReferenceValue = restartBtn;
            if (goExitBtnGO != null) so.FindProperty("gameOverExitButton").objectReferenceValue = goExitBtnGO.GetComponent<Button>();

            so.ApplyModifiedProperties();
            EditorUtility.SetDirty(uiMgr);
            EditorUtility.SetDirty(gm.gameObject);
        }

        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());

        EditorUtility.DisplayDialog("NeonRush", "تم تركيب الأصوات والقائمة الرئيسية وزر الخروج بنجاح!\n\n1. شاشة بداية أنيقة (Start Menu) مع أعلى سكور وزر EXIT.\n2. صوت المحرك يتسارع مع السرعة، وصوت صدمة قوي عند الخسارة.\n3. اهتزاز الكاميرا وحفظ أعلى سكور تلقائياً.\n\nاضغط Play وجرب اللعبة!", "ممتاز جداً!");
        Debug.Log("<color=green><b>[NeonRush]</b> تم تجهيز القائمة الرئيسية ونظام الصوتيات وزر الخروج بنجاح!</color>");
    }

    private static GameObject GetOrCreateChild(GameObject parent, string name)
    {
        Transform t = parent.transform.Find(name);
        if (t != null) return t.gameObject;
        GameObject go = new GameObject(name);
        go.transform.SetParent(parent.transform, false);
        return go;
    }

    private static RectTransform GetOrAddRect(GameObject go)
    {
        RectTransform r = go.GetComponent<RectTransform>();
        if (r == null) r = go.AddComponent<RectTransform>();
        return r;
    }

    private static TextMeshProUGUI GetOrAddTMP(GameObject go)
    {
        TextMeshProUGUI t = go.GetComponent<TextMeshProUGUI>();
        if (t == null) t = go.AddComponent<TextMeshProUGUI>();
        return t;
    }
}
#endif
