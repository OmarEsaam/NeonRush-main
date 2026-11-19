using System.Collections.Generic;
using UnityEngine;

public class RoadSpawner : MonoBehaviour
{
    [Header("Road Prefab")]
    [Tooltip("اسحب هنا مجسم قطعة الطريق (RoadTile Prefab)")]
    [SerializeField] private GameObject roadTilePrefab;

    [Header("Settings")]
    [Tooltip("طول قطعة الطريق الواحدة")]
    [SerializeField] private float tileLength = 50f;

    [Tooltip("عدد القطع الأولية للطريق")]
    [SerializeField] private int initialTilesCount = 6;

    private Transform playerTransform;
    private float nextSpawnZ = 0f;
    private List<GameObject> activeTiles = new List<GameObject>();

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }

        // توليد أول دفعة من قطع الطريق
        for (int i = 0; i < initialTilesCount; i++)
        {
            SpawnTile();
        }
    }

    private void Update()
    {
        if (playerTransform == null) return;

        // توليد قطعة جديدة للأمام كلما اقتربت السيارة من نهاية الطريق المتاح
        if (playerTransform.position.z + (tileLength * 3) > nextSpawnZ)
        {
            SpawnTile();
            RemoveOldestTile();
        }
    }

    private void SpawnTile()
    {
        if (roadTilePrefab == null) return;

        Vector3 spawnPosition = new Vector3(0f, 0f, nextSpawnZ);
        GameObject tile = Instantiate(roadTilePrefab, spawnPosition, Quaternion.identity);
        tile.transform.SetParent(transform);
        activeTiles.Add(tile);

        nextSpawnZ += tileLength;
    }

    private void RemoveOldestTile()
    {
        if (activeTiles.Count > initialTilesCount)
        {
            GameObject oldTile = activeTiles[0];
            activeTiles.RemoveAt(0);
            if (oldTile != null)
            {
                Destroy(oldTile);
            }
        }
    }
}
