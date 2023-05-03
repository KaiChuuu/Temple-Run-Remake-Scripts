using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public Sound[] musicSounds, sfxSounds, sfxPlayerSounds;
    public AudioSource musicSource, sfxSource, sfxSource2, sfxPlayerSource;

    public Sound currPlayerSound;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayMusic(string name, float volume)
    {
        Sound s = Array.Find(musicSounds, x => x.name == name);

        if(s == null)
        {
            //Error music not found
        }
        else
        {
            musicSource.volume = volume;
            musicSource.clip = s.clip;
            musicSource.Play();
        }
    }

    public void PlaySFX(string name, float pitch, float volume)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == name);

        if(s == null)
        {
            //Error sfx not found
        }
        else
        {
            sfxSource.volume = volume;
            sfxSource.pitch = pitch;
            sfxSource.PlayOneShot(s.clip);
            sfxSource.Play();
        }
    }

    public void PlaySFX2(string name, float pitch, float volume)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == name);

        if (s == null)
        {
            //Error sfx not found
        }
        else
        {
            sfxSource2.volume = volume;
            sfxSource2.pitch = pitch;
            sfxSource2.PlayOneShot(s.clip);
            sfxSource2.Play();
        }
    }

    public void PlayPlayerSFX(string name, float pitch, float volume)
    {
        Sound s = Array.Find(sfxPlayerSounds, x => x.name == name);

        if (s == null)
        {
            //Error sfx not found
        }
        else
        {
            currPlayerSound = s;
            sfxPlayerSource.volume = volume;
            sfxPlayerSource.pitch = pitch;
            sfxPlayerSource.PlayOneShot(s.clip);
            sfxPlayerSource.Play();
        }
    }
}
