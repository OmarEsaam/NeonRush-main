using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private Transform playerTransform;

    private void Start()
    {
        // العثور على سيارة اللاعب عبر التاج المخصص لها
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    private void Update()
    {
        if (playerTransform == null) return;

        // إذا تجاوزت السيارة هذا العائق بمسافة 10 وحدات، يتم تدميره لتفريغ الذاكرة
        if (playerTransform.position.z - transform.position.z > 10f)
        {
            Destroy(gameObject);
        }
    }
}       
