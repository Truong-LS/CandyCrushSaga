using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Background Music")]
    public AudioSource bgMusicSource;

    [Header("Sound Effects")]
    public AudioSource sfxSource;
    public AudioClip especialSwap;
    public AudioClip wrappedExplosion;
    public AudioClip HoriVertiExplosion;
    public AudioClip swapSound;
    public AudioClip errorSound;
    public AudioClip landingSound;
    public AudioClip popSound;
    public AudioClip createWrappedSound;
    public AudioClip colorBombExplosion;
    public AudioClip createColorBombSound;
    public AudioClip winSound;
    public AudioClip loseSound;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            float music = PlayerPrefs.GetFloat("MusicVolume", 1f);
            float sfx = PlayerPrefs.GetFloat("SFXVolume", 1f);

            SetMusicVolume(music);
            SetSFXVolume(sfx);
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
    public void PlaySwapEspecialSound()
    {
        if (especialSwap != null) sfxSource.PlayOneShot(especialSwap);
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

    public void PlayCreateWrappedSound()
    {
        if (createWrappedSound != null) sfxSource.PlayOneShot(createWrappedSound);
    }
    public void PlayColorBombExplosion()
    {
        if (colorBombExplosion != null) sfxSource.PlayOneShot(colorBombExplosion);
    }

    public void PlayCreateColorBombSound()
    {
        if (createColorBombSound != null) sfxSource.PlayOneShot(createColorBombSound);
    }
    public void PlayWinSound()
    {
        if (winSound != null) sfxSource.PlayOneShot(winSound);
    }

    public void PlayLoseSound()
    {
        if (loseSound != null) sfxSource.PlayOneShot(loseSound);
    }

    public void SetMusicVolume(float value)
    {
        if (bgMusicSource != null)
            bgMusicSource.volume = value;
    }

    public void SetSFXVolume(float value)
    {
        if (sfxSource != null)
            sfxSource.volume = value;
    }
}