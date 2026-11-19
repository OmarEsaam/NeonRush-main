#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;

public static class SetupDifficultyAndControls
{
    [MenuItem("NeonRush/Setup Difficulty Levels & Manual Speed (تركيب المستويات الـ 3 والتحكم بالسرعة)")]
    public static void Setup()
    {
        Canvas canvas = Object.FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            EditorUtility.DisplayDialog("خطأ", "تأكد من فتح GameScene ووجود Canvas.", "تمام");
            return;
        }

        GameObject canvasGO = canvas.gameObject;

        // 1. Update StartMenuPanel
        Transform startPanelTr = canvasGO.transform.Find("StartMenuPanel");
        if (startPanelTr == null)
        {
            EditorUtility.DisplayDialog("تنبيه", "شغّل Setup Audio & Start Menu أولاً لإنشاء شاشة البداية.", "تمام");
            return;
        }

        GameObject startPanel = startPanelTr.gameObject;

        // Adjust StartButton position
        Transform sbTr = startPanel.transform.Find("StartButton");
        if (sbTr != null)
        {
            RectTransform sbRect = sbTr.GetComponent<RectTransform>();
            sbRect.anchorMin = new Vector2(0.5f, 0.48f);
            sbRect.anchorMax = new Vector2(0.5f, 0.48f);
            sbRect.sizeDelta = new Vector2(300f, 65f);
        }

        // Add / Update "SELECT LEVEL" Button in Start Menu
        GameObject diffBtnGO = GetOrCreateChild(startPanel, "SelectDifficultyButton");
        RectTransform diffBtnRect = GetOrAddRect(diffBtnGO);
        diffBtnRect.anchorMin = new Vector2(0.5f, 0.36f);
        diffBtnRect.anchorMax = new Vector2(0.5f, 0.36f);
        diffBtnRect.pivot = new Vector2(0.5f, 0.5f);
        diffBtnRect.anchoredPosition = Vector2.zero;
        diffBtnRect.sizeDelta = new Vector2(300f, 60f);

        Image diffBtnImg = diffBtnGO.GetComponent<Image>();
        if (diffBtnImg == null) diffBtnImg = diffBtnGO.AddComponent<Image>();
        diffBtnImg.color = new Color(1f, 0.75f, 0f, 1f); // Neon Gold/Amber

        Button diffBtn = diffBtnGO.GetComponent<Button>();
        if (diffBtn == null) diffBtn = diffBtnGO.AddComponent<Button>();

        GameObject diffBtnTextGO = GetOrCreateChild(diffBtnGO, "Text (TMP)");
        RectTransform dtRect = GetOrAddRect(diffBtnTextGO);
        dtRect.anchorMin = Vector2.zero;
        dtRect.anchorMax = Vector2.one;
        dtRect.sizeDelta = Vector2.zero;
        TextMeshProUGUI dtTMP = GetOrAddTMP(diffBtnTextGO);
        dtTMP.text = "SELECT LEVEL";
        dtTMP.fontSize = 24;
        dtTMP.fontStyle = FontStyles.Bold;
        dtTMP.alignment = TextAlignmentOptions.Center;
        dtTMP.color = Color.black;

        // Adjust ExitButton position in Start Menu
        Transform ebTr = startPanel.transform.Find("ExitButton");
        if (ebTr != null)
        {
            RectTransform ebRect = ebTr.GetComponent<RectTransform>();
            ebRect.anchorMin = new Vector2(0.5f, 0.24f);
            ebRect.anchorMax = new Vector2(0.5f, 0.24f);
            ebRect.sizeDelta = new Vector2(240f, 50f);
        }

        // Adjust HighScore position in Start Menu
        Transform hsTr = startPanel.transform.Find("StartHighScoreText");
        if (hsTr != null)
        {
            RectTransform hsRect = hsTr.GetComponent<RectTransform>();
            hsRect.anchorMin = new Vector2(0.5f, 0.12f);
            hsRect.anchorMax = new Vector2(0.5f, 0.12f);
            hsRect.sizeDelta = new Vector2(600f, 50f);
        }

        // 2. Create DifficultyPanel
        GameObject diffPanelGO = GetOrCreateChild(canvasGO, "DifficultyPanel");
        RectTransform dpRect = GetOrAddRect(diffPanelGO);
        dpRect.anchorMin = Vector2.zero;
        dpRect.anchorMax = Vector2.one;
        dpRect.sizeDelta = Vector2.zero;

        Image dpImg = diffPanelGO.GetComponent<Image>();
        if (dpImg == null) dpImg = diffPanelGO.AddComponent<Image>();
        dpImg.color = new Color(0.02f, 0.02f, 0.06f, 0.96f); // Dark background

        // Title: CHOOSE DIFFICULTY
        GameObject dpTitleGO = GetOrCreateChild(diffPanelGO, "Title");
        RectTransform dptRect = GetOrAddRect(dpTitleGO);
        dptRect.anchorMin = new Vector2(0.5f, 0.82f);
        dptRect.anchorMax = new Vector2(0.5f, 0.82f);
        dptRect.pivot = new Vector2(0.5f, 0.5f);
        dptRect.anchoredPosition = Vector2.zero;
        dptRect.sizeDelta = new Vector2(800f, 90f);
        TextMeshProUGUI dptTMP = GetOrAddTMP(dpTitleGO);
        dptTMP.text = "CHOOSE DIFFICULTY";
        dptTMP.fontSize = 62;
        dptTMP.fontStyle = FontStyles.Bold;
        dptTMP.alignment = TextAlignmentOptions.Center;
        dptTMP.color = new Color(0f, 1f, 1f, 1f); // Cyan

        // Subtitle
        GameObject dpSubGO = GetOrCreateChild(diffPanelGO, "Subtitle");
        RectTransform dpsRect = GetOrAddRect(dpSubGO);
        dpsRect.anchorMin = new Vector2(0.5f, 0.74f);
        dpsRect.anchorMax = new Vector2(0.5f, 0.74f);
        dpsRect.pivot = new Vector2(0.5f, 0.5f);
        dpsRect.anchoredPosition = Vector2.zero;
        dpsRect.sizeDelta = new Vector2(600f, 40f);
        TextMeshProUGUI dpsTMP = GetOrAddTMP(dpSubGO);
        dpsTMP.text = "— SELECT SPEED & TRAFFIC INTENSITY —";
        dpsTMP.fontSize = 20;
        dpsTMP.alignment = TextAlignmentOptions.Center;
        dpsTMP.color = new Color(1f, 0.2f, 0.6f, 1f);

        // EASY Button (Green)
        Button easyBtn = CreateDifficultyCard(diffPanelGO, "EasyButton", "EASY  •  MAX 100 KM/H", new Color(0f, 0.9f, 0.45f, 1f), Color.black, 0.62f);
        TextMeshProUGUI easyRecordTMP = CreateRecordText(diffPanelGO, "EasyRecordText", 0.55f, new Color(0f, 0.9f, 0.45f, 1f));

        // MEDIUM Button (Yellow)
        Button medBtn = CreateDifficultyCard(diffPanelGO, "MediumButton", "MEDIUM  •  MAX 200 KM/H", new Color(1f, 0.84f, 0f, 1f), Color.black, 0.45f);
        TextMeshProUGUI medRecordTMP = CreateRecordText(diffPanelGO, "MediumRecordText", 0.38f, new Color(1f, 0.84f, 0f, 1f));

        // HARD Button (Red)
        Button hardBtn = CreateDifficultyCard(diffPanelGO, "HardButton", "HARD  •  MAX 300 KM/H", new Color(1f, 0.1f, 0.25f, 1f), Color.white, 0.28f);
        TextMeshProUGUI hardRecordTMP = CreateRecordText(diffPanelGO, "HardRecordText", 0.21f, new Color(1f, 0.1f, 0.25f, 1f));

        // BACK Button
        GameObject backBtnGO = GetOrCreateChild(diffPanelGO, "BackButton");
        RectTransform bbRect = GetOrAddRect(backBtnGO);
        bbRect.anchorMin = new Vector2(0.5f, 0.08f);
        bbRect.anchorMax = new Vector2(0.5f, 0.08f);
        bbRect.pivot = new Vector2(0.5f, 0.5f);
        bbRect.anchoredPosition = Vector2.zero;
        bbRect.sizeDelta = new Vector2(220f, 50f);
        Image bbImg = backBtnGO.GetComponent<Image>();
        if (bbImg == null) bbImg = backBtnGO.AddComponent<Image>();
        bbImg.color = new Color(0.2f, 0.2f, 0.3f, 1f);
        Button backBtn = backBtnGO.GetComponent<Button>();
        if (backBtn == null) backBtn = backBtnGO.AddComponent<Button>();

        GameObject bbTextGO = GetOrCreateChild(backBtnGO, "Text (TMP)");
        RectTransform bbtRect = GetOrAddRect(bbTextGO);
        bbtRect.anchorMin = Vector2.zero;
        bbtRect.anchorMax = Vector2.one;
        bbtRect.sizeDelta = Vector2.zero;
        TextMeshProUGUI bbtTMP = GetOrAddTMP(bbTextGO);
        bbtTMP.text = "← BACK";
        bbtTMP.fontSize = 20;
        bbtTMP.fontStyle = FontStyles.Bold;
        bbtTMP.alignment = TextAlignmentOptions.Center;
        bbtTMP.color = Color.white;

        // Hide DifficultyPanel by default
        diffPanelGO.SetActive(false);

        // 3. Wire into UIManager
        GameManager gm = Object.FindObjectOfType<GameManager>();
        if (gm != null)
        {
            UIManager uiMgr = gm.GetComponent<UIManager>();
            if (uiMgr == null) uiMgr = gm.gameObject.AddComponent<UIManager>();

            SerializedObject so = new SerializedObject(uiMgr);
            so.FindProperty("openDifficultyButton").objectReferenceValue = diffBtn;
            so.FindProperty("difficultyPanel").objectReferenceValue = diffPanelGO;
            so.FindProperty("easyButton").objectReferenceValue = easyBtn;
            so.FindProperty("mediumButton").objectReferenceValue = medBtn;
            so.FindProperty("hardButton").objectReferenceValue = hardBtn;
            so.FindProperty("backDifficultyButton").objectReferenceValue = backBtn;
            so.FindProperty("easyRecordText").objectReferenceValue = easyRecordTMP;
            so.FindProperty("mediumRecordText").objectReferenceValue = medRecordTMP;
            so.FindProperty("hardRecordText").objectReferenceValue = hardRecordTMP;

            so.ApplyModifiedProperties();
            EditorUtility.SetDirty(uiMgr);
            EditorUtility.SetDirty(gm.gameObject);
        }

        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());

        EditorUtility.DisplayDialog("NeonRush Levels", "تم تركيب المستويات الثلاثة والتحكم بالسرعة بنجاح!\n\n1. التحكم بالبنزين (W / سهم لأعلى) والفرامل (S / سهم لأسفل).\n2. شاشة اختيار المستوى (EASY 100km/h - MEDIUM 200km/h - HARD 300km/h).\n3. كثافة العوائق وسرعة الصوت تتغير حسب كل مستوى.\n\nاضغط Play وجرب اللعبة!", "جامد جداً!");
        Debug.Log("<color=green><b>[NeonRush]</b> تم تركيب نظام المستويات الثلاثة والتحكم بالسرعة بنجاح!</color>");
    }

    private static Button CreateDifficultyCard(GameObject parent, string name, string text, Color btnColor, Color textColor, float anchorY)
    {
        GameObject btnGO = GetOrCreateChild(parent, name);
        RectTransform rt = GetOrAddRect(btnGO);
        rt.anchorMin = new Vector2(0.5f, anchorY);
        rt.anchorMax = new Vector2(0.5f, anchorY);
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = Vector2.zero;
        rt.sizeDelta = new Vector2(420f, 65f);

        Image img = btnGO.GetComponent<Image>();
        if (img == null) img = btnGO.AddComponent<Image>();
        img.color = btnColor;

        Button btn = btnGO.GetComponent<Button>();
        if (btn == null) btn = btnGO.AddComponent<Button>();

        GameObject textGO = GetOrCreateChild(btnGO, "Text (TMP)");
        RectTransform tr = GetOrAddRect(textGO);
        tr.anchorMin = Vector2.zero;
        tr.anchorMax = Vector2.one;
        tr.sizeDelta = Vector2.zero;

        TextMeshProUGUI tmp = GetOrAddTMP(textGO);
        tmp.text = text;
        tmp.fontSize = 24;
        tmp.fontStyle = FontStyles.Bold;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.color = textColor;

        return btn;
    }

    private static TextMeshProUGUI CreateRecordText(GameObject parent, string name, float anchorY, Color color)
    {
        GameObject textGO = GetOrCreateChild(parent, name);
        RectTransform tr = GetOrAddRect(textGO);
        tr.anchorMin = new Vector2(0.5f, anchorY);
        tr.anchorMax = new Vector2(0.5f, anchorY);
        tr.pivot = new Vector2(0.5f, 0.5f);
        tr.anchoredPosition = Vector2.zero;
        tr.sizeDelta = new Vector2(400f, 35f);

        TextMeshProUGUI tmp = GetOrAddTMP(textGO);
        tmp.text = "BEST: 0 M";
        tmp.fontSize = 18;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.color = color;
        return tmp;
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
