using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Background Music")]
    public AudioSource bgMusicSource;

    [Header("Sound Effects")]
    public AudioSource sfxSource;

    public AudioClip wrappedExplosion;

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
        sfxSource.PlayOneShot(wrappedExplosion);
    }
}