using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

public class AudioManager : PersistentSingleton<AudioManager>
{
    [SerializeField] private AudioMixer audioMixer;

    [SerializeField, Tooltip("音乐列表")] private List<Audio> musicList;
    [SerializeField, Tooltip("音效列表")] private List<Audio> soundList;
        
    //改变音调
    const float pitchMin = 0.9f;
    const float pitchMax = 1.1f;

    #region 音量设置
    //AudioMixer音量设置
    public void SetMasterVolume(float value)
    {
        PlayerPrefs.SetFloat("master",value);
        var tmp = value * 40 - 40;
        audioMixer.SetFloat("vMaster", tmp);
    }
    public void SetMusicVolume(float value)
    {
        PlayerPrefs.SetFloat("music",value);
        var tmp = value * 40 - 40;
        audioMixer.SetFloat("vMusic", tmp);
    }
    public void SetSfxVolume(float value)
    {
        PlayerPrefs.SetFloat("sound",value);
        var tmp = value * 40 - 40;
        audioMixer.SetFloat("vSound", tmp);
    }
    public void UpdateVolume()
    {
        SetMasterVolume(PlayerPrefs.GetFloat("master", 0.5f));
        SetMusicVolume(PlayerPrefs.GetFloat("music", 0.5f));
        SetSfxVolume(PlayerPrefs.GetFloat("sound", 0.5f));
    }
    #endregion

    #region 音效管理
    public void PlaySound(string mName)
    {
        var audioSource = Find(mName, soundList);
        if (audioSource == null) { return; }
        audioSource.pitch = 1;
        audioSource.Play();
    }
    // 改变音调，主要用于重复播放的音效
    public void PlayRandomSound(string mName)
    {
        var audioSource = Find(mName, soundList);
        if (audioSource == null) { return; }
        audioSource.pitch =  Random.Range(pitchMin , pitchMax );
        audioSource.Play();
    }
    #endregion

    #region 音乐管理
    /// <summary>
    /// 循环播放音乐
    /// </summary>
    /// <param name="mName"></param>
    public void PlayMusic(string mName)//循环
    {
        var audioSource = Find(mName, musicList);
        if (audioSource == null) { return; }
        audioSource.loop = true;
        audioSource.Play();
    }
    public void PlayMusicOne(string mName)//不循环
    {
        var audioSource = Find(mName, musicList);
        if (audioSource == null) { return; }
        audioSource.loop = false;
        audioSource.Play();
    }

    /// <summary>
    /// 停止播放音乐
    /// </summary>
    public void PauseMusic()
    {
        foreach (var music in musicList)
        {
            music.audioSource.Pause();
        }
    }
    public void StopMusic()
    {
        foreach (var music in musicList)
        {
            music.audioSource.Stop();
        }
    }
    public void ContinueMusic()
    {
        foreach (var music in musicList)
        {
            music.audioSource.Play();
        }
    }

    public void StopSound()
    {
        foreach (var sound in soundList)
        {
            sound.audioSource.Stop();
        }
    }
    #endregion

    private AudioSource Find(string name, List<Audio> audioList)
    {
        foreach (var audio in audioList)
        {
            if (audio.name == name)
            {
                return audio.audioSource;
            }
        }
        Debug.LogWarning("AudioManager:找不到对应音频");
        return null;
    }
}

[Serializable]
class Audio
{
    public string name;
    public AudioSource audioSource;
}