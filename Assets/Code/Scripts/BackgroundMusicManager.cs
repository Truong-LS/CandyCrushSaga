using UnityEngine;
using UnityEngine.SceneManagement;

public class BackgroundMusicManager : MonoBehaviour
{
    private static BackgroundMusicManager instance;
    private AudioSource audioSource;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            audioSource = GetComponent<AudioSource>();

            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "InGame_1")
        {
            audioSource.Stop(); // dừng nhạc khi vào game
        }
        else if (scene.name == "InGame_2")
        {
            audioSource.Stop(); // dừng nhạc khi vào game
        }
        else if (!audioSource.isPlaying && (scene.name == "Map" || scene.name == "MainMenu"))
        {
            audioSource.Play(); // bật lại nếu quay về menu/map
        }
    }
}