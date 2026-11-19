using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Tooltip("اسحب مجسم سيارة اللاعب هنا")]
    [SerializeField] private Transform target;

    [Tooltip("إزاحة الكاميرا بالنسبة للسيارة")]
    [SerializeField] private Vector3 offset = new Vector3(0f, 4f, -7f);

    [Tooltip("نعومة تتبع الكاميرا")]
    [SerializeField] private float followSpeed = 10f;

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 targetPosition = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
    }
}