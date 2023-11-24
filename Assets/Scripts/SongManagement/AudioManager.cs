using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource _audiosource;
    [SerializeField] private AudioSource _sfxsource;
    [SerializeField] private AudioClip titleSong;
    [SerializeField] private List<Song> songs;
    [SerializeField] private int songPlaying;
    [SerializeField] private float transitionTime = 1f;

    private static AudioManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("more than one audio manager");
        }
        instance = this;
        songPlaying = 0;
        playTitleSong();
    }

    public static AudioManager GetInstance()
    {
        return instance;
    }

    public void FadeOutMusic()
    {
        StartCoroutine(Fade());
    }

    private IEnumerator Fade()
    {
        if (_audiosource.isPlaying)
        {
            for (float i = 0; i < transitionTime; i += Time.deltaTime)
            {
                _audiosource.volume = (1 - (i / transitionTime));
                yield return null;
            }

            _audiosource.Stop();
        }
    }

    public void playTitleSong()
    {
        _audiosource.clip = titleSong;
        _audiosource.Play();
        _audiosource.loop = true;
    }

    public void StartPlaylist()
    {
        _audiosource.volume = 1;
        songPlaying = 0;
        _audiosource.loop = false;
        _audiosource.clip = songs[songPlaying].track;
        _audiosource.Play();
    }
    public void ChangeSong()
    {
        _audiosource.volume = 1;
        songPlaying++;
        if (songPlaying >= songs.Count)
        {
            songPlaying = 0;
        }
        _audiosource.clip = songs[songPlaying].track;
        _audiosource.Play();
    }

    public Song getCurrentTrack()
    {
        return songs[songPlaying];
    }

    public void playSFXLoop(AudioClip clip)
    {
        _sfxsource.loop = true;
        _sfxsource.clip = clip;
        _sfxsource.Play();
    }

    public void playSFX(AudioClip clip)
    {
        _sfxsource.loop = false;
        _sfxsource.PlayOneShot(clip);
    }
    public void playSFX(AudioClip clip, float volume)
    {
        _sfxsource.loop = false;
        _sfxsource.PlayOneShot(clip, volume);
    }

    public int getCurrentPlayerAttackChange()
    {
        return songs[songPlaying].damageOffset;
    }

    public int getCurrentPlayerDefenseChange()
    {
        return songs[songPlaying].defenseOffset;
    }

    public string getCurrentPlayerAttackString()
    {
        return songs[songPlaying].stat1;
    }

    public string getCurrentPlayerDefenseString()
    {
        return songs[songPlaying].stat2;
    }
}
