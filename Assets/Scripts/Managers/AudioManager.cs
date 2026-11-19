using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource engineSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip engineClip;
    [SerializeField] private AudioClip startupClip;
    [SerializeField] private AudioClip crashClip;
    [SerializeField] private AudioClip clickClip;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // إعداد AudioSources تلقائياً لو مش مربوطين
        if (engineSource == null)
        {
            engineSource = gameObject.AddComponent<AudioSource>();
            engineSource.loop = true;
            engineSource.playOnAwake = false;
            engineSource.volume = 0.45f;
        }

        if (sfxSource == null)
        {
            sfxSource = gameObject.AddComponent<AudioSource>();
            sfxSource.loop = false;
            sfxSource.playOnAwake = false;
            sfxSource.volume = 0.85f;
        }
    }

    public void StartEngine()
    {
        if (sfxSource != null && startupClip != null)
        {
            sfxSource.PlayOneShot(startupClip);
        }

        if (engineSource != null && engineClip != null)
        {
            engineSource.clip = engineClip;
            engineSource.pitch = 0.9f;
            engineSource.Play();
        }
    }

    public void StopEngine()
    {
        if (engineSource != null && engineSource.isPlaying)
        {
            engineSource.Stop();
        }
    }

    public void SetEnginePitch(float speedRatio)
    {
        if (engineSource != null && engineSource.isPlaying)
        {
            // نغمة الصوت تتصاعد بنعومة مع تسارع السيارة
            engineSource.pitch = Mathf.Clamp(0.85f + (speedRatio - 1f) * 0.45f, 0.85f, 1.65f);
        }
    }

    public void PlayCrash()
    {
        if (sfxSource != null && crashClip != null)
        {
            sfxSource.PlayOneShot(crashClip);
        }
    }

    public void PlayClick()
    {
        if (sfxSource != null && clickClip != null)
        {
            sfxSource.PlayOneShot(clickClip);
        }
    }
}
