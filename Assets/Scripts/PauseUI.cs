using System;
using UnityEngine;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    public AudioSlider musicSlider;
    public AudioSlider soundSlider;
    public Button continueButton;
    public Button restartButton;
    public Button backButton;

    private void Start()
    {
        Continue();
        musicSlider.InitSlider();
        soundSlider.InitSlider();
        continueButton.onClick.AddListener(Continue);
        restartButton.onClick.AddListener(SceneLoadManager.Instance.Reload);
        backButton.onClick.AddListener(SceneLoadManager.Instance.GoToStart);
    }

    private void Continue()
    {
        if (!gameObject.activeSelf) return;
        Time.timeScale = 1f;
        this.gameObject.SetActive(false);
    }

    public void Pause()
    {
        if (gameObject.activeSelf) return;
        Time.timeScale = 0f;
        this.gameObject.SetActive(true);
    }
}

[Serializable]
public class AudioSlider
{
    public Slider slider;
    public AudioType audioType;
    public void InitSlider()
    {
        switch (audioType)
        {
            case AudioType.Mater:
                slider.value = PlayerPrefs.GetFloat("master", 0.5f);
                AudioManager.Instance.SetMasterVolume(slider.value);
                slider.onValueChanged.AddListener((value) =>
                {
                    AudioManager.Instance.SetMasterVolume(value);
                });
                break;
            case AudioType.Music:
                slider.value = PlayerPrefs.GetFloat("music", 0.5f);
                AudioManager.Instance.SetMusicVolume(slider.value);
                slider.onValueChanged.AddListener((value) =>
                {
                    AudioManager.Instance.SetMusicVolume(value);
                });
                break;
            case AudioType.Sound:
                slider.value = PlayerPrefs.GetFloat("sound", 0.5f);
                AudioManager.Instance.SetSfxVolume(slider.value);
                slider.onValueChanged.AddListener((value) =>
                {
                    AudioManager.Instance.SetSfxVolume(value);
                });
                break;
            default:
                return;
        }
        
    }
}

[Serializable]
public enum AudioType
{
    Mater,
    Music,
    Sound
}