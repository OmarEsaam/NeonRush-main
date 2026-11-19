#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;

public static class FixSelectLevelButton
{
    [MenuItem("NeonRush/Fix SELECT LEVEL Button (إصلاح زرار اختيار المستوى)")]
    public static void Fix()
    {
        // 1. إيجاد الـ Canvas
        Canvas canvas = Object.FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            EditorUtility.DisplayDialog("خطأ", "مفيش Canvas في المشهد.", "تمام");
            return;
        }

        // 2. إيجاد StartMenuPanel
        Transform startPanelTr = canvas.transform.Find("StartMenuPanel");
        if (startPanelTr == null)
        {
            EditorUtility.DisplayDialog("خطأ", "مفيش StartMenuPanel. شغّل Setup Audio & Start Menu الأول.", "تمام");
            return;
        }

        // 3. إيجاد زرار SelectDifficultyButton
        Transform selectBtnTr = startPanelTr.Find("SelectDifficultyButton");
        if (selectBtnTr == null)
        {
            EditorUtility.DisplayDialog("خطأ", "مفيش SelectDifficultyButton جوا StartMenuPanel.\nشغّل Setup Difficulty Levels & Manual Speed الأول.", "تمام");
            return;
        }

        Button selectBtn = selectBtnTr.GetComponent<Button>();
        if (selectBtn == null) selectBtn = selectBtnTr.gameObject.AddComponent<Button>();

        // 4. إيجاد UIManager وربط الزرار
        GameManager gm = Object.FindObjectOfType<GameManager>();
        if (gm == null)
        {
            EditorUtility.DisplayDialog("خطأ", "مفيش GameManager في المشهد.", "تمام");
            return;
        }

        UIManager uiMgr = gm.GetComponent<UIManager>();
        if (uiMgr == null)
        {
            EditorUtility.DisplayDialog("خطأ", "مفيش UIManager على الـ GameManager.", "تمام");
            return;
        }

        // ربط كل أزرار الـ Start Menu في نفس الوقت علشان متفوتنا حاجة
        SerializedObject so = new SerializedObject(uiMgr);

        // StartMenuPanel نفسه
        so.FindProperty("startMenuPanel").objectReferenceValue = startPanelTr.gameObject;

        // openDifficultyButton = SelectDifficultyButton
        so.FindProperty("openDifficultyButton").objectReferenceValue = selectBtn;

        // StartButton
        Transform startBtnTr = startPanelTr.Find("StartButton");
        if (startBtnTr != null)
        {
            Button startBtn = startBtnTr.GetComponent<Button>();
            if (startBtn != null) so.FindProperty("startButton").objectReferenceValue = startBtn;
        }

        // ExitButton
        Transform exitBtnTr = startPanelTr.Find("ExitButton");
        if (exitBtnTr != null)
        {
            Button exitBtn = exitBtnTr.GetComponent<Button>();
            if (exitBtn != null) so.FindProperty("exitButton").objectReferenceValue = exitBtn;
        }

        // StartHighScoreText
        Transform hsTr = startPanelTr.Find("StartHighScoreText");
        if (hsTr != null)
        {
            TextMeshProUGUI hsTMP = hsTr.GetComponent<TextMeshProUGUI>();
            if (hsTMP != null) so.FindProperty("startHighScoreText").objectReferenceValue = hsTMP;
        }

        // DifficultyPanel وأزراره
        Transform diffPanelTr = canvas.transform.Find("DifficultyPanel");
        if (diffPanelTr != null)
        {
            so.FindProperty("difficultyPanel").objectReferenceValue = diffPanelTr.gameObject;

            Button easyBtn = diffPanelTr.Find("EasyButton")?.GetComponent<Button>();
            Button medBtn  = diffPanelTr.Find("MediumButton")?.GetComponent<Button>();
            Button hardBtn = diffPanelTr.Find("HardButton")?.GetComponent<Button>();
            Button backBtn = diffPanelTr.Find("BackButton")?.GetComponent<Button>();

            if (easyBtn != null) so.FindProperty("easyButton").objectReferenceValue = easyBtn;
            if (medBtn  != null) so.FindProperty("mediumButton").objectReferenceValue = medBtn;
            if (hardBtn != null) so.FindProperty("hardButton").objectReferenceValue = hardBtn;
            if (backBtn != null) so.FindProperty("backDifficultyButton").objectReferenceValue = backBtn;

            TextMeshProUGUI easyRec = diffPanelTr.Find("EasyRecordText")?.GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI medRec  = diffPanelTr.Find("MediumRecordText")?.GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI hardRec = diffPanelTr.Find("HardRecordText")?.GetComponent<TextMeshProUGUI>();

            if (easyRec != null) so.FindProperty("easyRecordText").objectReferenceValue = easyRec;
            if (medRec  != null) so.FindProperty("mediumRecordText").objectReferenceValue = medRec;
            if (hardRec != null) so.FindProperty("hardRecordText").objectReferenceValue = hardRec;
        }

        so.ApplyModifiedProperties();
        EditorUtility.SetDirty(uiMgr);
        EditorUtility.SetDirty(gm.gameObject);

        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());

        EditorUtility.DisplayDialog("NeonRush ✓",
            "تم ربط كل أزرار الـ Start Menu بنجاح!\n\n" +
            "✓ START GAME\n" +
            "✓ SELECT LEVEL\n" +
            "✓ EXIT\n" +
            "✓ أزرار الـ Difficulty Panel\n\n" +
            "اضغط Ctrl+S ثم Play وجرب.", "جامد!");

        Debug.Log("<color=yellow><b>[NeonRush]</b> تم ربط SELECT LEVEL وكل أزرار Start Menu بنجاح!</color>");
    }
}
#endif
