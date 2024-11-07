using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource uiSource;
    [SerializeField] private int sfxPoolSize = 10;
    [SerializeField] private AudioSource sfxPrefab;

    private List<AudioSource> sfxPool;
    private Dictionary<string, AudioClip> audioClips;

    protected override void Awake()
    {
        base.Awake();
        InitializeAudioClips();
        InitializeSFXPool();

        PlayMusic("BGM_1", 0.1f);
    }

    private void InitializeAudioClips()
    {
        audioClips = new Dictionary<string, AudioClip>();
        AudioClip[] loadedClips = Resources.LoadAll<AudioClip>("Audio");
        foreach (AudioClip clip in loadedClips)
        {
            audioClips.Add(clip.name, clip);
        }
    }

    private void InitializeSFXPool()
    {
        sfxPool = new List<AudioSource>();
        for (int i = 0; i < sfxPoolSize; i++)
        {
            AudioSource newSfxSource = Instantiate(sfxPrefab, transform);
            sfxPool.Add(newSfxSource);
        }
    }

    public void PlayMusic(string clipName, float volume = 1f, bool loop = true)
    {
        if (audioClips.TryGetValue(clipName, out AudioClip clip))
        {
            musicSource.clip = clip;
            musicSource.volume = volume;
            musicSource.loop = loop;
            musicSource.Play();
        }
        else
        {
            Debug.LogWarning($"Audio clip '{clipName}' not found.");
        }
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void PlaySFX(string clipName, float volume = 1f, Transform parent = null)
    {
        if (audioClips.TryGetValue(clipName, out AudioClip clip))
        {
            AudioSource availableSource = GetAvailableSFXSource(parent);
            if (availableSource != null)
            {
                availableSource.clip = clip;
                availableSource.volume = volume;
                availableSource.Play();
            }
        }
        else
        {
            Debug.LogWarning($"Audio clip '{clipName}' not found.");
        }
    }

    private AudioSource GetAvailableSFXSource(Transform parent = null)
    {
        foreach (AudioSource source in sfxPool)
        {
            if (!source.isPlaying)
            {
                if(parent != null)
                {
                    source.transform.parent = parent;
                }
                else
                {
                    source.transform.parent = transform;
                }
                
                return source;
            }
        }
        return null;
    }

    public void PlayUIEffect(string clipName, float volume = 1f)
    {
        if (audioClips.TryGetValue(clipName, out AudioClip clip))
        {
            uiSource.PlayOneShot(clip, volume);
        }
        else
        {
            Debug.LogWarning($"Audio clip '{clipName}' not found.");
        }
    }

    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        foreach (AudioSource source in sfxPool)
        {
            source.volume = volume;
        }
    }

    public void SetUIVolume(float volume)
    {
        uiSource.volume = volume;
    }
}
