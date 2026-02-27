using UnityEngine;
using UnityEngine.UI;

public class SettingUI : MonoBehaviour
{
    public Slider musicSlider;
    public Slider sfxSlider;

    void Start()
    {
        // Lấy dữ liệu đã lưu
        float music = PlayerPrefs.GetFloat("MusicVolume", 1f);
        float sfx = PlayerPrefs.GetFloat("SFXVolume", 1f);

        musicSlider.value = music;
        sfxSlider.value = sfx;

        musicSlider.onValueChanged.AddListener(OnMusicChanged);
        sfxSlider.onValueChanged.AddListener(OnSFXChanged);
    }

    void OnMusicChanged(float value)
    {
        PlayerPrefs.SetFloat("MusicVolume", value);
        Debug.Log("Music: " + value);
    }

    void OnSFXChanged(float value)
    {
        PlayerPrefs.SetFloat("SFXVolume", value);
        Debug.Log("SFX: " + value);
    }

    public void CloseSetting()
    {
        gameObject.SetActive(false);
    }
}