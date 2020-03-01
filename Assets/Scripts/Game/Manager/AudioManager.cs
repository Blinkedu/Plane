using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    private float musicVolume = 0.5f;
    public float MusicVolume { get { return musicVolume; } }
    private float soundVolume = 0.5f;
    public float SoundVolume { get { return soundVolume; } }

    private AudioSource musicSource;
    private List<AudioSource> soundSources = new List<AudioSource>();

    private void Awake()
    {
        Instance = this;
        InitSource();
    }

    private void InitSource()
    {
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true;
        musicSource.playOnAwake = false;
        musicSource.volume = MusicVolume;

        for (int i = 0; i < 10; i++)
        {
            CreateSoundSource();
        }
    }

    public void PlayMusic(string musicName)
    {
        AudioClip clip = LoadAudioClip(musicName);
        if(clip == null)
        {
            Debug.LogError("PlayMusic Error: AudioClip is null! path=" + musicName);
        }
        else
        {
            if (clip == musicSource.clip) return;
            musicSource.clip = clip;
            musicSource.Play();
        }
    }

    public void PlaySound(string soundName)
    {
        AudioClip clip = LoadAudioClip(soundName);
        if (clip == null)
        {
            Debug.LogError("PlayMusic Error: AudioClip is null! path=" + soundName);
        }
        else
        {
            var source = GetUsableSoundSource();
            source.clip = clip;
            source.Play();
        }
    }

    public void SetMusicVolume(float volume)
    {
        this.musicVolume = volume;
        musicSource.volume = volume;
    }

    public void SetSoundVolume(float volume)
    {
        this.soundVolume = volume;
        foreach (var item in soundSources)
        {
            item.volume = volume;
        }
    }

    private AudioSource GetUsableSoundSource()
    {
        for (int i = 0; i < soundSources.Count; i++)
        {
            if(soundSources[i].isPlaying == false)
            {
                return soundSources[i];
            }
        }
        return CreateSoundSource();
    }

    private AudioSource CreateSoundSource()
    {
        AudioSource s = gameObject.AddComponent<AudioSource>();
        s.loop = false;
        s.playOnAwake = false;
        s.volume = MusicVolume;
        soundSources.Add(s);
        return s;
    }

    private AudioClip LoadAudioClip(string name)
    {
        return ResManager.Instance.Load<AudioClip>(name);
    }
}
