#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

public static class RefreshCrashAudio
{
    [MenuItem("NeonRush/Refresh & Test Crash Audio (تحديث وربط صوت الكراش)")]
    public static void Refresh()
    {
        // 1. إعادة استيراد ملف الصوت داخل يونيتي
        AssetDatabase.ImportAsset("Assets/Audio/crash.wav", ImportAssetOptions.ForceUpdate);

        AudioClip crashClip = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Audio/crash.wav");
        if (crashClip == null)
        {
            EditorUtility.DisplayDialog("خطأ", "لم يتم العثور على Assets/Audio/crash.wav", "تمام");
            return;
        }

        // 2. البحث عن AudioManager والتأكد من ضبط الـ Sources والـ Clips
        GameObject audioMgrGO = GameObject.Find("AudioManager");
        if (audioMgrGO == null)
        {
            audioMgrGO = new GameObject("AudioManager");
            Undo.RegisterCreatedObjectUndo(audioMgrGO, "Create AudioManager");
        }

        AudioManager audioMgr = audioMgrGO.GetComponent<AudioManager>();
        if (audioMgr == null) audioMgr = audioMgrGO.AddComponent<AudioManager>();

        AudioSource[] sources = audioMgrGO.GetComponents<AudioSource>();
        AudioSource engineSrc = null;
        AudioSource sfxSrc = null;

        if (sources.Length >= 2)
        {
            engineSrc = sources[0];
            sfxSrc = sources[1];
        }
        else
        {
            if (sources.Length == 1) engineSrc = sources[0];
            else engineSrc = audioMgrGO.AddComponent<AudioSource>();

            sfxSrc = audioMgrGO.AddComponent<AudioSource>();
        }

        engineSrc.loop = true;
        engineSrc.playOnAwake = false;
        engineSrc.volume = 0.5f;

        sfxSrc.loop = false;
        sfxSrc.playOnAwake = false;
        sfxSrc.volume = 1.0f;

        SerializedObject so = new SerializedObject(audioMgr);
        so.FindProperty("engineSource").objectReferenceValue = engineSrc;
        so.FindProperty("sfxSource").objectReferenceValue = sfxSrc;
        so.FindProperty("crashClip").objectReferenceValue = crashClip;

        // ربط باقي الملفات لو موجودة
        AudioClip engineClip = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Car Engine Sound - i6 German Free/Assets/Audio/i6_german_free/high_on.wav");
        if (engineClip == null) engineClip = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Audio/engine_loop.wav");
        if (engineClip != null) so.FindProperty("engineClip").objectReferenceValue = engineClip;

        AudioClip startClip = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Car Engine Sound - i6 German Free/Assets/Audio/i6_german_free/startup.wav");
        if (startClip != null) so.FindProperty("startupClip").objectReferenceValue = startClip;

        AudioClip clickClip = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Audio/button_click.wav");
        if (clickClip != null) so.FindProperty("clickClip").objectReferenceValue = clickClip;

        so.ApplyModifiedProperties();
        EditorUtility.SetDirty(audioMgr);
        EditorUtility.SetDirty(audioMgrGO);

        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());

        // تجربة تشغيل الصوت فوراً في المحرر ليتأكد المستخدم من سماعه
        sfxSrc.PlayOneShot(crashClip);

        EditorUtility.DisplayDialog("NeonRush Audio ✓",
            "تم تحديث وتركيب صوت الكراش الجديد بنجاح!\n\n" +
            "• تم ضبط الـ AudioSource ومستوى الصوت.\n" +
            "• تم تشغيل الصوت التجريبي الآن في يونيتي.\n\n" +
            "اضغط Play وجرب تخبط في أي عائق!", "تمام!");

        Debug.Log("<color=green><b>[NeonRush]</b> تم استيراد صوت الاصطدام الجديد بنجاح والتأكد من ربطه!</color>");
    }
}
#endif
