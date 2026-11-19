#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

public static class EquipGermanEngineAudio
{
    [MenuItem("NeonRush/Equip German i6 Engine Sound (تركيب صوت المحرك الألماني)")]
    public static void EquipAudio()
    {
        // 1. Find or create AudioManager
        GameObject audioMgrGO = GameObject.Find("AudioManager");
        if (audioMgrGO == null)
        {
            audioMgrGO = new GameObject("AudioManager");
            Undo.RegisterCreatedObjectUndo(audioMgrGO, "Create AudioManager");
        }

        AudioManager audioMgr = audioMgrGO.GetComponent<AudioManager>();
        if (audioMgr == null) audioMgr = audioMgrGO.AddComponent<AudioManager>();

        // 2. Load German i6 Clips
        string baseAudioPath = "Assets/Car Engine Sound - i6 German Free/Assets/Audio/i6_german_free/";
        AudioClip engineClip = AssetDatabase.LoadAssetAtPath<AudioClip>(baseAudioPath + "high_on.wav");
        if (engineClip == null) engineClip = AssetDatabase.LoadAssetAtPath<AudioClip>(baseAudioPath + "med_on.wav");

        AudioClip startupClip = AssetDatabase.LoadAssetAtPath<AudioClip>(baseAudioPath + "startup.wav");
        AudioClip crashClip = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Audio/crash.wav");
        AudioClip clickClip = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Audio/button_click.wav");

        // 3. Serialize Properties
        SerializedObject so = new SerializedObject(audioMgr);
        if (engineClip != null) so.FindProperty("engineClip").objectReferenceValue = engineClip;
        if (startupClip != null) so.FindProperty("startupClip").objectReferenceValue = startupClip;
        if (crashClip != null) so.FindProperty("crashClip").objectReferenceValue = crashClip;
        if (clickClip != null) so.FindProperty("clickClip").objectReferenceValue = clickClip;
        so.ApplyModifiedProperties();

        EditorUtility.SetDirty(audioMgrGO);
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());

        EditorUtility.DisplayDialog("NeonRush Audio", "تم تركيب صوت محرك السباق الألماني i6 بنجاح!\n\n- صوت تدوير المحرك (Startup)\n- صوت المحرك القوي عند الانطلاق (High RPM Engine Loop)\n- التسارع التلقائي للنغمة مع زيادة السرعة\n\nاضغط Play واستمتع بالصوت!", "عاش!");
        Debug.Log("<color=green><b>[NeonRush]</b> تم تركيب صوت المحرك الألماني الحقيقي بنجاح!</color>");
    }
}
#endif
