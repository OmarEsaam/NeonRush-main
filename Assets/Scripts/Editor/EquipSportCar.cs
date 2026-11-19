#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;

public static class EquipSportCar
{
    [MenuItem("NeonRush/Equip Yellow SportCar (تركيب العربية الصفراء الفخمة)")]
    public static void EquipCar()
    {
        // 1. Convert SportCar Materials to URP Lit
        Shader urpLit = Shader.Find("Universal Render Pipeline/Lit");
        if (urpLit != null)
        {
            string[] matGuids = AssetDatabase.FindAssets("t:Material", new[] { "Assets/SportCar" });
            foreach (string guid in matGuids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                Material mat = AssetDatabase.LoadAssetAtPath<Material>(path);
                if (mat != null && mat.shader != urpLit)
                {
                    Color origCol = mat.HasProperty("_Color") ? mat.GetColor("_Color") : Color.white;
                    Texture mainTex = mat.HasProperty("_MainTex") ? mat.GetTexture("_MainTex") : null;
                    Texture bumpMap = mat.HasProperty("_BumpMap") ? mat.GetTexture("_BumpMap") : null;

                    mat.shader = urpLit;
                    mat.SetColor("_BaseColor", origCol);
                    if (mainTex != null) mat.SetTexture("_BaseMap", mainTex);
                    if (bumpMap != null) mat.SetTexture("_BumpMap", bumpMap);

                    EditorUtility.SetDirty(mat);
                }
            }
        }

        // Give Body1 a rich Cyberpunk Yellow
        Material bodyMat = AssetDatabase.LoadAssetAtPath<Material>("Assets/SportCar/Models/SportCar_1/Materials/Body1.mat");
        if (bodyMat != null)
        {
            bodyMat.SetColor("_BaseColor", new Color(1f, 0.82f, 0.05f, 1f)); // Vibrant Sport Yellow
            bodyMat.SetFloat("_Smoothness", 0.92f); // Glossy car paint
            bodyMat.SetFloat("_Metallic", 0.65f);   // Metallic body
            EditorUtility.SetDirty(bodyMat);
        }

        // Enable glowing Taillights (Neon Red)
        Material rearLightMat = AssetDatabase.LoadAssetAtPath<Material>("Assets/SportCar/Models/SportCar_1/Materials/rearlight1.mat");
        if (rearLightMat != null)
        {
            rearLightMat.EnableKeyword("_EMISSION");
            rearLightMat.SetColor("_EmissionColor", new Color(3f, 0.1f, 0.1f, 1f));
            rearLightMat.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;
            EditorUtility.SetDirty(rearLightMat);
        }

        Material backLightMat = AssetDatabase.LoadAssetAtPath<Material>("Assets/SportCar/Models/SportCar_1/Materials/Back_Light.mat");
        if (backLightMat != null)
        {
            backLightMat.EnableKeyword("_EMISSION");
            backLightMat.SetColor("_EmissionColor", new Color(3.5f, 0.15f, 0.05f, 1f));
            backLightMat.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;
            EditorUtility.SetDirty(backLightMat);
        }

        // Enable glowing Headlights
        Material frontLightMat = AssetDatabase.LoadAssetAtPath<Material>("Assets/SportCar/Models/SportCar_1/Materials/frontlight1.mat");
        if (frontLightMat != null)
        {
            frontLightMat.EnableKeyword("_EMISSION");
            frontLightMat.SetColor("_EmissionColor", new Color(2f, 2.2f, 2.5f, 1f));
            frontLightMat.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;
            EditorUtility.SetDirty(frontLightMat);
        }

        AssetDatabase.SaveAssets();

        // 2. Find PlayerCar in Scene
        GameObject playerCar = GameObject.FindGameObjectWithTag("Player");
        if (playerCar == null) playerCar = GameObject.Find("PlayerCar");

        if (playerCar == null)
        {
            EditorUtility.DisplayDialog("تنبيه", "لم يتم العثور على مجسم PlayerCar في المشهد الحالي. تأكد من فتح GameScene.", "تمام");
            return;
        }

        // Disable primitive visual
        MeshRenderer mr = playerCar.GetComponent<MeshRenderer>();
        if (mr != null) mr.enabled = false;

        // Remove old visual child if exists
        Transform oldVisual = playerCar.transform.Find("CarVisual");
        if (oldVisual != null) Object.DestroyImmediate(oldVisual.gameObject);

        // 3. Load & Instantiate SportCar Prefab
        GameObject sportCarPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/SportCar/Prefabs/SportCar_1.prefab");
        if (sportCarPrefab != null)
        {
            GameObject carVisual = (GameObject)PrefabUtility.InstantiatePrefab(sportCarPrefab, playerCar.transform);
            carVisual.name = "CarVisual";
            carVisual.transform.localPosition = new Vector3(0f, 0f, 0f);
            carVisual.transform.localRotation = Quaternion.identity;
            carVisual.transform.localScale = Vector3.one * 0.55f;

            // Remove any colliders, audio listeners or rigidbodies from imported prefab
            foreach (Collider c in carVisual.GetComponentsInChildren<Collider>())
            {
                Object.DestroyImmediate(c);
            }
            foreach (Rigidbody rb in carVisual.GetComponentsInChildren<Rigidbody>())
            {
                Object.DestroyImmediate(rb);
            }

            // Adjust BoxCollider on PlayerCar to fit the sports car
            BoxCollider col = playerCar.GetComponent<BoxCollider>();
            if (col != null)
            {
                col.center = new Vector3(0f, 0.35f, 0f);
                col.size = new Vector3(0.95f, 0.65f, 1.85f);
            }

            EditorUtility.SetDirty(playerCar);
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
                UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());

            EditorUtility.DisplayDialog("NeonRush", "تم تركيب وضبط العربية الصفراء الفخمة (Sport Car) بنجاح!\n\n- تم تحويل الخامات لـ URP Lit\n- تم تفعيل الإضاءة النيون للفوانيس الخلفية والأمامية\n- تم ضبط المقاس والـ Collider بدقة", "يا جمالو!");
            Debug.Log("<color=yellow><b>[NeonRush]</b> تم تركيب العربية الصفراء بنجاح!</color>");
        }
        else
        {
            EditorUtility.DisplayDialog("خطأ", "لم يتم العثور على SportCar_1.prefab في مسار Assets/SportCar/Prefabs", "تمام");
        }
    }
}
#endif
