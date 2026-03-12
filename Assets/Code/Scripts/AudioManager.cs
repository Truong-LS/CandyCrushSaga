using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Background Music")]
    public AudioSource bgMusicSource;

    [Header("Sound Effects")]
    public AudioSource sfxSource;

    public AudioClip wrappedExplosion;
    public AudioClip HoriVertiExplosion;
    public AudioClip swapSound;      // Thêm âm thanh vuốt
    public AudioClip errorSound;     // Thêm âm thanh vuốt lỗi
    public AudioClip landingSound;
    public AudioClip popSound;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // giữ khi đổi scene
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayWrappedExplosion()
    {
        
        if (wrappedExplosion != null)
        {
            sfxSource.PlayOneShot(wrappedExplosion);
        }
    }
    public void PlayHoriVertiExplosion()
    {
        if (HoriVertiExplosion != null)
        {
            sfxSource.PlayOneShot(HoriVertiExplosion);
        }
    }

    // --- THÊM 2 HÀM NÀY ---
    public void PlaySwapSound()
    {
        if (swapSound != null) sfxSource.PlayOneShot(swapSound);
    }

    public void PlayErrorSound()
    {
        if (errorSound != null) sfxSource.PlayOneShot(errorSound);
    }
    public void PlayLandingSound()
    {
        if (landingSound != null)
        {
            sfxSource.PlayOneShot(landingSound);
        }
    }
    public void PlayPopSound()
    {
        if (popSound != null) sfxSource.PlayOneShot(popSound);
    }
}