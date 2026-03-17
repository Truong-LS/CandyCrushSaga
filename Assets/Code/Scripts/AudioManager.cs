using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Background Music")]
    public AudioSource bgMusicSource;

    [Header("Sound Effects")]
    public AudioSource sfxSource;
    public AudioClip especialSwap; // swap 4-5 viên thì có tiếng làyyyyy
    public AudioClip wrappedExplosion;
    public AudioClip HoriVertiExplosion; //âm thanh nổ kẹo sọc ngang sọc dọc hen
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
        if (especialSwap != null) sfxSource.PlayOneShot(especialSwap);
    }
    public void PlaySwapEspecialSound()
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