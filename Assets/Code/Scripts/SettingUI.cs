using UnityEngine;
using UnityEngine.UI;

public class SettingUI : MonoBehaviour
{
    public Slider musicSlider;
    public Slider sfxSlider;

    private AudioSource bgMusicSource;

    void Start()
    {
        // 🔍 Tìm Background Music
        GameObject bgObj = GameObject.Find("BackgroundMusic");

        if (bgObj != null)
        {
            bgMusicSource = bgObj.GetComponent<AudioSource>();
        }
        else
        {
            Debug.LogWarning("Không tìm thấy BackgroundMusic!");
        }

        // 🔥 Load dữ liệu đã lưu
        float music = PlayerPrefs.GetFloat("MusicVolume", 1f);
        float sfx = PlayerPrefs.GetFloat("SFXVolume", 1f);

        musicSlider.value = music;
        sfxSlider.value = sfx;

        // 🔥 APPLY NGAY (cái em đang thiếu)
        if (bgMusicSource != null)
            bgMusicSource.volume = music;

        if (AudioManager.instance != null)
            AudioManager.instance.SetSFXVolume(sfx);

        // 🎧 Lắng nghe thay đổi
        musicSlider.onValueChanged.AddListener(OnMusicChanged);
        sfxSlider.onValueChanged.AddListener(OnSFXChanged);
    }

    void OnMusicChanged(float value)
    {
        PlayerPrefs.SetFloat("MusicVolume", value);

        if (bgMusicSource != null)
            bgMusicSource.volume = value;
    }

    void OnSFXChanged(float value)
    {
        PlayerPrefs.SetFloat("SFXVolume", value);

        if (AudioManager.instance != null)
            AudioManager.instance.SetSFXVolume(value);
    }

    public void CloseSetting()
    {
        gameObject.SetActive(false);
    }
}