using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Tooltip("اسحب مجسم سيارة اللاعب هنا")]
    [SerializeField] private Transform target;

    [Tooltip("إزاحة الكاميرا بالنسبة للسيارة")]
    [SerializeField] private Vector3 offset = new Vector3(0f, 4.5f, -9.5f);

    [Tooltip("نعومة تتبع الكاميرا")]
    [SerializeField] private float followSpeed = 10f;

    private float shakeDuration = 0f;
    private float shakeMagnitude = 0.4f;

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 targetPosition = target.position + offset;

        // تطبيق اهتزاز الكاميرا لو كان نشطاً
        if (shakeDuration > 0f)
        {
            Vector3 shakeOffset = Random.insideUnitSphere * shakeMagnitude;
            shakeOffset.z = 0f; // عدم تحريك الكاميرا للأمام أو الخلف
            targetPosition += shakeOffset;
            shakeDuration -= Time.deltaTime;
        }

        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
    }

    // تُستدعى لإحداث اهتزاز قوي عند الاصطدام
    public void TriggerShake(float duration = 0.45f, float magnitude = 0.45f)
    {
        shakeDuration = duration;
        shakeMagnitude = magnitude;
    }
}
