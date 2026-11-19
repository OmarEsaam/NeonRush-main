using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Obstacle Setup")]
    [Tooltip("اسحب هنا الـ Obstacle Prefab")]
    [SerializeField] private GameObject obstaclePrefab;

    [Tooltip("المسافة بين المسارات (يجب أن تطابق CarController)")]
    [SerializeField] private float laneDistance = 3f;

    [Header("Spawn Settings")]
    [Tooltip("مسافة توليد العائق للأمام بالنسبة للسيارة")]
    [SerializeField] private float spawnDistanceAhead = 60f;

    [Tooltip("المسافة بين كل عائق والذي يليه")]
    [SerializeField] private float distanceBetweenObstacles = 25f;

    [Tooltip("عدد العوائق المبدئية عند تشغيل اللعبة")]
    [SerializeField] private int initialObstaclesCount = 8;

    private Transform playerTransform;
    private float nextSpawnZ = 30f; // بداية موضع أول عائق للأمام

    private void Start()
    {
        // العثور على سيارة اللاعب
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }

        // توليد أول دفعة عوائق في بداية المرحلة
        for (int i = 0; i < initialObstaclesCount; i++)
        {
            SpawnObstacle();
        }
    }

    private void Update()
    {
        if (playerTransform == null) return;

        // توليد عائق جديد للأمام كلما اقتربت السيارة
        if (playerTransform.position.z + spawnDistanceAhead > nextSpawnZ)
        {
            SpawnObstacle();
        }
    }

    private void SpawnObstacle()
    {
        // اختيار مسار عشوائي: -1 (يسار)، 0 (وسط)، 1 (يمين)
        int randomLane = Random.Range(-1, 2);
        float spawnX = randomLane * laneDistance;

        Vector3 spawnPosition = new Vector3(spawnX, 0.75f, nextSpawnZ);
        Instantiate(obstaclePrefab, spawnPosition, Quaternion.identity);

        // تجهيز المسافة للعائق التالي
        nextSpawnZ += distanceBetweenObstacles;
    }

    public void SetObstacleDistance(float distance)
    {
        distanceBetweenObstacles = distance;
    }
}
