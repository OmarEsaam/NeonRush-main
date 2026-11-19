#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;

public static class RealCarAndRoadUpgrade
{
    [MenuItem("NeonRush/Upgrade To Real Car & Road (تركيب العربية والطريق الحقيقي)")]
    public static void UpgradeCarAndRoad()
    {
        // 1. Create or get Materials
        Material cyanMat = GetOrCreateEmissionMaterial("Assets/Materials/M_NeonCyan.mat", new Color(0f, 1f, 1f, 1f), new Color(0f, 2f, 2f, 1f));
        Material pinkMat = GetOrCreateEmissionMaterial("Assets/Materials/M_NeonPink.mat", new Color(1f, 0.1f, 0.6f, 1f), new Color(2f, 0.2f, 1.2f, 1f));
        Material roadMat = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/M_Road.mat");
        if (roadMat == null)
        {
            roadMat = GetOrCreateEmissionMaterial("Assets/Materials/M_RoadAsphalt.mat", new Color(0.08f, 0.08f, 0.1f, 1f), Color.black);
        }

        // 2. Build RoadTile Prefab
        GameObject roadTile = new GameObject("RoadTile");

        // Road Surface (Width: 12, Length: 50)
        GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Cube);
        ground.name = "Ground";
        ground.transform.SetParent(roadTile.transform);
        ground.transform.localPosition = new Vector3(0f, -0.1f, 25f);
        ground.transform.localScale = new Vector3(12f, 0.2f, 50f);
        if (roadMat != null) ground.GetComponent<MeshRenderer>().sharedMaterial = roadMat;

        // Lane Divider Left (separating Lane -3 and Lane 0 at X = -1.5)
        GameObject dividerL = GameObject.CreatePrimitive(PrimitiveType.Cube);
        dividerL.name = "Divider_Left";
        dividerL.transform.SetParent(roadTile.transform);
        dividerL.transform.localPosition = new Vector3(-1.5f, 0.02f, 25f);
        dividerL.transform.localScale = new Vector3(0.18f, 0.05f, 50f);
        Object.DestroyImmediate(dividerL.GetComponent<Collider>());
        if (cyanMat != null) dividerL.GetComponent<MeshRenderer>().sharedMaterial = cyanMat;

        // Lane Divider Right (separating Lane 0 and Lane +3 at X = +1.5)
        GameObject dividerR = GameObject.CreatePrimitive(PrimitiveType.Cube);
        dividerR.name = "Divider_Right";
        dividerR.transform.SetParent(roadTile.transform);
        dividerR.transform.localPosition = new Vector3(1.5f, 0.02f, 25f);
        dividerR.transform.localScale = new Vector3(0.18f, 0.05f, 50f);
        Object.DestroyImmediate(dividerR.GetComponent<Collider>());
        if (cyanMat != null) dividerR.GetComponent<MeshRenderer>().sharedMaterial = cyanMat;

        // Left Guard Rail
        GameObject railL = GameObject.CreatePrimitive(PrimitiveType.Cube);
        railL.name = "GuardRail_Left";
        railL.transform.SetParent(roadTile.transform);
        railL.transform.localPosition = new Vector3(-5.8f, 0.4f, 25f);
        railL.transform.localScale = new Vector3(0.4f, 0.8f, 50f);
        if (pinkMat != null) railL.GetComponent<MeshRenderer>().sharedMaterial = pinkMat;

        // Right Guard Rail
        GameObject railR = GameObject.CreatePrimitive(PrimitiveType.Cube);
        railR.name = "GuardRail_Right";
        railR.transform.SetParent(roadTile.transform);
        railR.transform.localPosition = new Vector3(5.8f, 0.4f, 25f);
        railR.transform.localScale = new Vector3(0.4f, 0.8f, 50f);
        if (pinkMat != null) railR.GetComponent<MeshRenderer>().sharedMaterial = pinkMat;

        // Overhead Cyberpunk Neon Arch
        GameObject arch = new GameObject("NeonArch");
        arch.transform.SetParent(roadTile.transform);

        GameObject postL = GameObject.CreatePrimitive(PrimitiveType.Cube);
        postL.name = "Post_L";
        postL.transform.SetParent(arch.transform);
        postL.transform.localPosition = new Vector3(-5.8f, 2.5f, 25f);
        postL.transform.localScale = new Vector3(0.3f, 5f, 0.3f);
        Object.DestroyImmediate(postL.GetComponent<Collider>());
        if (cyanMat != null) postL.GetComponent<MeshRenderer>().sharedMaterial = cyanMat;

        GameObject postR = GameObject.CreatePrimitive(PrimitiveType.Cube);
        postR.name = "Post_R";
        postR.transform.SetParent(arch.transform);
        postR.transform.localPosition = new Vector3(5.8f, 2.5f, 25f);
        postR.transform.localScale = new Vector3(0.3f, 5f, 0.3f);
        Object.DestroyImmediate(postR.GetComponent<Collider>());
        if (cyanMat != null) postR.GetComponent<MeshRenderer>().sharedMaterial = cyanMat;

        GameObject topBeam = GameObject.CreatePrimitive(PrimitiveType.Cube);
        topBeam.name = "TopBeam";
        topBeam.transform.SetParent(arch.transform);
        topBeam.transform.localPosition = new Vector3(0f, 5f, 25f);
        topBeam.transform.localScale = new Vector3(12f, 0.3f, 0.3f);
        Object.DestroyImmediate(topBeam.GetComponent<Collider>());
        if (cyanMat != null) topBeam.GetComponent<MeshRenderer>().sharedMaterial = cyanMat;

        // Save as Prefab
        if (!Directory.Exists("Assets/Prefabs"))
        {
            Directory.CreateDirectory("Assets/Prefabs");
        }
        string prefabPath = "Assets/Prefabs/RoadTile.prefab";
        GameObject savedPrefab = PrefabUtility.SaveAsPrefabAsset(roadTile, prefabPath);
        Object.DestroyImmediate(roadTile);

        // 3. Setup RoadSpawner in Scene
        GameObject oldRoad = GameObject.Find("Road");
        if (oldRoad != null)
        {
            Undo.DestroyObjectImmediate(oldRoad);
        }

        RoadSpawner spawner = Object.FindObjectOfType<RoadSpawner>();
        GameObject spawnerGO;
        if (spawner == null)
        {
            spawnerGO = new GameObject("RoadSpawner");
            spawner = spawnerGO.AddComponent<RoadSpawner>();
            Undo.RegisterCreatedObjectUndo(spawnerGO, "Create RoadSpawner");
        }
        else
        {
            spawnerGO = spawner.gameObject;
        }

        SerializedObject spawnerSO = new SerializedObject(spawner);
        spawnerSO.FindProperty("roadTilePrefab").objectReferenceValue = savedPrefab;
        spawnerSO.FindProperty("tileLength").floatValue = 50f;
        spawnerSO.FindProperty("initialTilesCount").intValue = 6;
        spawnerSO.ApplyModifiedProperties();

        // 4. Upgrade PlayerCar with real 3D Model
        GameObject playerCar = GameObject.FindGameObjectWithTag("Player");
        if (playerCar == null) playerCar = GameObject.Find("PlayerCar");

        if (playerCar != null)
        {
            // Remove primitive visual
            MeshRenderer mr = playerCar.GetComponent<MeshRenderer>();
            if (mr != null) mr.enabled = false;

            // Remove old visual child if exists
            Transform oldVisual = playerCar.transform.Find("CarVisual");
            if (oldVisual != null) Object.DestroyImmediate(oldVisual.gameObject);

            // Load 3D model
            GameObject carModelAsset = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Models/rally-car.obj");
            if (carModelAsset != null)
            {
                GameObject carVisual = (GameObject)PrefabUtility.InstantiatePrefab(carModelAsset, playerCar.transform);
                carVisual.name = "CarVisual";
                carVisual.transform.localPosition = new Vector3(0f, 0.45f, 0f);
                carVisual.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
                carVisual.transform.localScale = Vector3.one * 0.85f;

                // Remove any colliders that came with model
                foreach (Collider c in carVisual.GetComponentsInChildren<Collider>())
                {
                    Object.DestroyImmediate(c);
                }
            }

            // Adjust BoxCollider on playerCar to match car dimensions
            BoxCollider col = playerCar.GetComponent<BoxCollider>();
            if (col != null)
            {
                col.center = new Vector3(0f, 0.65f, 0f);
                col.size = new Vector3(1.7f, 1.1f, 3.6f);
            }

            EditorUtility.SetDirty(playerCar);
        }

        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());

        EditorUtility.DisplayDialog("NeonRush Upgrade", "تم ترقية اللعبة بنجاح!\n\n1. تم تركيب سيارة 3D حقيقية (Rally Sports Car) بدلاً من المكعب.\n2. تم بناء طريق نيون لا نهائي حقيقي (Real Infinite Highway) بحواجز مضيئة ومسارات واضحة.\n\nاضغط Play للتجربة!", "يا سلام!");
        Debug.Log("<color=green><b>[NeonRush]</b> تم تركيب مجسم السيارة والطريق اللانهائي بنجاح!</color>");
    }

    private static Material GetOrCreateEmissionMaterial(string path, Color baseColor, Color emissionColor)
    {
        Material mat = AssetDatabase.LoadAssetAtPath<Material>(path);
        if (mat == null)
        {
            Shader shader = Shader.Find("Universal Render Pipeline/Lit");
            if (shader == null) shader = Shader.Find("Standard");
            mat = new Material(shader);
            mat.SetColor("_BaseColor", baseColor);
            mat.EnableKeyword("_EMISSION");
            mat.SetColor("_EmissionColor", emissionColor);
            mat.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;

            string dir = Path.GetDirectoryName(path);
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            AssetDatabase.CreateAsset(mat, path);
        }
        return mat;
    }
}
#endif
