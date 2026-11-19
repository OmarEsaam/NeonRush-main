#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

public static class FixCarAndCamera
{
    [MenuItem("NeonRush/Fix Car Size & Camera (ضبط حجم العربية والكاميرا بضغطة زر)")]
    public static void ApplyFix()
    {
        // 1. Scale down CarVisual
        GameObject playerCar = GameObject.FindGameObjectWithTag("Player");
        if (playerCar == null) playerCar = GameObject.Find("PlayerCar");

        if (playerCar != null)
        {
            Transform carVisual = playerCar.transform.Find("CarVisual");
            if (carVisual != null)
            {
                carVisual.localScale = Vector3.one * 0.55f;
                carVisual.localPosition = new Vector3(0f, 0f, 0f);
            }

            BoxCollider col = playerCar.GetComponent<BoxCollider>();
            if (col != null)
            {
                col.center = new Vector3(0f, 0.35f, 0f);
                col.size = new Vector3(0.95f, 0.65f, 1.85f);
            }

            EditorUtility.SetDirty(playerCar);
        }

        // 2. Adjust Main Camera
        Camera cam = Camera.main;
        if (cam == null)
        {
            GameObject camGO = GameObject.Find("Main Camera");
            if (camGO != null) cam = camGO.GetComponent<Camera>();
        }

        if (cam != null)
        {
            cam.fieldOfView = 65f;
            cam.transform.rotation = Quaternion.Euler(18f, 0f, 0f);

            CameraFollow cf = cam.GetComponent<CameraFollow>();
            if (cf != null)
            {
                SerializedObject so = new SerializedObject(cf);
                SerializedProperty offsetProp = so.FindProperty("offset");
                if (offsetProp != null)
                {
                    offsetProp.vector3Value = new Vector3(0f, 4.5f, -9.5f);
                }
                SerializedProperty speedProp = so.FindProperty("followSpeed");
                if (speedProp != null)
                {
                    speedProp.floatValue = 10f;
                }
                so.ApplyModifiedProperties();
                EditorUtility.SetDirty(cf);
            }

            // Also set initial camera position so it doesn't snap abruptly
            if (playerCar != null)
            {
                cam.transform.position = playerCar.transform.position + new Vector3(0f, 4.5f, -9.5f);
            }

            EditorUtility.SetDirty(cam.gameObject);
        }

        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());

        EditorUtility.DisplayDialog("NeonRush", "تم تصغير العربية وضبط زاوية وبعد الكاميرا!\n\nScale العربية: 0.28\nالكاميرا: Y=5.5, Z=-14\n\nاضغط Play وجرب الرؤية دلوقتي.", "تمام!");
        Debug.Log("<color=green><b>[NeonRush]</b> تم ضبط حجم العربية وزاوية الكاميرا بنجاح!</color>");
    }
}
#endif
