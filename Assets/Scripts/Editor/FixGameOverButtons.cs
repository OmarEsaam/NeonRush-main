#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;

public static class FixGameOverButtons
{
    [MenuItem("NeonRush/Fix Game Over Buttons (BACK + EXIT)")]
    public static void Fix()
    {
        Canvas canvas = Object.FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            EditorUtility.DisplayDialog("خطأ", "مفيش Canvas في المشهد. تأكد من فتح GameScene.", "تمام");
            return;
        }

        GameObject canvasGO = canvas.gameObject;

        // Find GameOverPanel
        Transform panelTr = canvasGO.transform.Find("GameOverPanel");
        if (panelTr == null)
        {
            EditorUtility.DisplayDialog("خطأ", "مفيش GameOverPanel. شغّل Setup Complete UI الأول.", "تمام");
            return;
        }
        GameObject panelGO = panelTr.gameObject;

        // ── 1. TRY AGAIN button — ارفعه شوية علشان يتسع للزرارين تحته ──
        Transform restartTr = panelGO.transform.Find("RestartButton");
        if (restartTr != null)
        {
            RectTransform rt = restartTr.GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(0.5f, 0.38f);
            rt.anchorMax = new Vector2(0.5f, 0.38f);
            rt.sizeDelta = new Vector2(260f, 60f);
        }

        // ── 2. BACK button (يرجع للـ Start Menu) ──
        GameObject backBtnGO = GetOrCreate(panelGO, "GameOverBackButton");
        RectTransform backRect = GetOrAddRect(backBtnGO);
        backRect.anchorMin = new Vector2(0.5f, 0.26f);
        backRect.anchorMax = new Vector2(0.5f, 0.26f);
        backRect.pivot = new Vector2(0.5f, 0.5f);
        backRect.anchoredPosition = Vector2.zero;
        backRect.sizeDelta = new Vector2(260f, 55f);

        Image backImg = backBtnGO.GetComponent<Image>();
        if (backImg == null) backImg = backBtnGO.AddComponent<Image>();
        backImg.color = new Color(0.1f, 0.4f, 0.9f, 1f); // أزرق نيون

        Button backBtn = backBtnGO.GetComponent<Button>();
        if (backBtn == null) backBtn = backBtnGO.AddComponent<Button>();

        GameObject backTextGO = GetOrCreate(backBtnGO, "Text (TMP)");
        RectTransform btr = GetOrAddRect(backTextGO);
        btr.anchorMin = Vector2.zero; btr.anchorMax = Vector2.one; btr.sizeDelta = Vector2.zero;
        TextMeshProUGUI backTMP = GetOrAddTMP(backTextGO);
        backTMP.text = "← BACK";
        backTMP.fontSize = 22;
        backTMP.fontStyle = FontStyles.Bold;
        backTMP.alignment = TextAlignmentOptions.Center;
        backTMP.color = Color.white;

        // ── 3. EXIT button (يغلق اللعبة) ──
        // إزالة أي GameOverExitButton قديم وإعادة إنشاؤه صح
        Transform oldExitTr = panelGO.transform.Find("GameOverExitButton");
        if (oldExitTr != null)
        {
            Undo.DestroyObjectImmediate(oldExitTr.gameObject);
        }

        GameObject exitBtnGO = new GameObject("GameOverExitButton");
        exitBtnGO.transform.SetParent(panelGO.transform, false);
        Undo.RegisterCreatedObjectUndo(exitBtnGO, "Create GameOverExitButton");

        RectTransform exitRect = exitBtnGO.AddComponent<RectTransform>();
        exitRect.anchorMin = new Vector2(0.5f, 0.15f);
        exitRect.anchorMax = new Vector2(0.5f, 0.15f);
        exitRect.pivot = new Vector2(0.5f, 0.5f);
        exitRect.anchoredPosition = Vector2.zero;
        exitRect.sizeDelta = new Vector2(260f, 55f);

        Image exitImg = exitBtnGO.AddComponent<Image>();
        exitImg.color = new Color(0.75f, 0.1f, 0.1f, 1f); // أحمر داكن

        Button exitBtn = exitBtnGO.AddComponent<Button>();

        GameObject exitTextGO = new GameObject("Text (TMP)");
        exitTextGO.transform.SetParent(exitBtnGO.transform, false);
        RectTransform etr = exitTextGO.AddComponent<RectTransform>();
        etr.anchorMin = Vector2.zero; etr.anchorMax = Vector2.one; etr.sizeDelta = Vector2.zero;
        TextMeshProUGUI exitTMP = exitTextGO.AddComponent<TextMeshProUGUI>();
        exitTMP.text = "EXIT";
        exitTMP.fontSize = 22;
        exitTMP.fontStyle = FontStyles.Bold;
        exitTMP.alignment = TextAlignmentOptions.Center;
        exitTMP.color = Color.white;

        // ── 4. تأكد إن الـ BestScoreText موجود ──
        Transform bestTr = panelGO.transform.Find("BestScoreText");
        TextMeshProUGUI bestTMP = null;
        if (bestTr != null) bestTMP = bestTr.GetComponent<TextMeshProUGUI>();
        if (bestTMP == null)
        {
            GameObject bestGO = new GameObject("BestScoreText");
            bestGO.transform.SetParent(panelGO.transform, false);
            RectTransform bsRect = bestGO.AddComponent<RectTransform>();
            bsRect.anchorMin = new Vector2(0.5f, 0.47f);
            bsRect.anchorMax = new Vector2(0.5f, 0.47f);
            bsRect.pivot = new Vector2(0.5f, 0.5f);
            bsRect.anchoredPosition = Vector2.zero;
            bsRect.sizeDelta = new Vector2(600f, 50f);
            bestTMP = bestGO.AddComponent<TextMeshProUGUI>();
            bestTMP.text = "EASY BEST: 0 M";
            bestTMP.fontSize = 26;
            bestTMP.alignment = TextAlignmentOptions.Center;
            bestTMP.color = new Color(1f, 0.9f, 0f, 1f); // ذهبي
        }

        // ── 5. ربط كل الأزرار بالـ UIManager ──
        GameManager gm = Object.FindObjectOfType<GameManager>();
        if (gm != null)
        {
            UIManager uiMgr = gm.GetComponent<UIManager>();
            if (uiMgr == null) uiMgr = gm.gameObject.AddComponent<UIManager>();

            SerializedObject so = new SerializedObject(uiMgr);

            // GameOver panel
            so.FindProperty("gameOverPanel").objectReferenceValue = panelGO;

            // BestScoreText
            var bestProp = so.FindProperty("bestScoreText");
            if (bestProp != null) bestProp.objectReferenceValue = bestTMP;

            // Restart
            if (restartTr != null)
            {
                Button restartBtn = restartTr.GetComponent<Button>();
                so.FindProperty("restartButton").objectReferenceValue = restartBtn;
            }

            // Back button → gameOverBackButton
            var backProp = so.FindProperty("gameOverBackButton");
            if (backProp != null) backProp.objectReferenceValue = backBtn;

            // Exit button
            so.FindProperty("gameOverExitButton").objectReferenceValue = exitBtn;

            so.ApplyModifiedProperties();
            EditorUtility.SetDirty(uiMgr);
            EditorUtility.SetDirty(gm.gameObject);
        }

        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());

        EditorUtility.DisplayDialog("NeonRush ✓",
            "تم إصلاح أزرار Game Over!\n\n" +
            "▶ TRY AGAIN  → يعيد اللعبة\n" +
            "← BACK       → يرجع للـ Start Menu\n" +
            "EXIT         → يغلق اللعبة\n\n" +
            "اضغط Ctrl+S ثم Play للتجربة.", "جامد!");

        Debug.Log("<color=cyan><b>[NeonRush]</b> تم إضافة زرار BACK وإصلاح EXIT في شاشة Game Over!</color>");
    }

    private static GameObject GetOrCreate(GameObject parent, string name)
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
        return r != null ? r : go.AddComponent<RectTransform>();
    }

    private static TextMeshProUGUI GetOrAddTMP(GameObject go)
    {
        TextMeshProUGUI t = go.GetComponent<TextMeshProUGUI>();
        return t != null ? t : go.AddComponent<TextMeshProUGUI>();
    }
}
#endif
