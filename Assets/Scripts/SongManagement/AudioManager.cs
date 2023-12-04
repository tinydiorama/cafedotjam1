using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource _audiosource;
    [SerializeField] private AudioSource _sfxsource;
    [SerializeField] private AudioClip titleSong;
    [SerializeField] private List<Song> songs;
    [SerializeField] private Song bossSong;
    [SerializeField] private int songPlaying;
    [SerializeField] private float transitionTime = 1f;

    private static AudioManager instance;
    private bool playlistStarted;
    private bool bossMusicPlaying;

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

    private void Update()
    {
        if ( !_audiosource.isPlaying && playlistStarted && ! GameManager.GetInstance().isConfrontBoss )
        {
            ChangeSong();
            HUD.GetInstance().updateCurrentStats();
        }
    }

    public void FadeOutMusic()
    {
        StartCoroutine(Fade());
    }

    public void playBossMusic()
    {
        bossMusicPlaying = true;
        _audiosource.clip = bossSong.track;
        _audiosource.loop = true;
        _audiosource.volume = 1;
        _audiosource.Play();
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
        playlistStarted = true;
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
        if ( bossMusicPlaying )
        {
            return bossSong;
        }
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
        if (bossMusicPlaying)
        {
            return bossSong.damageOffset;
        }
        return songs[songPlaying].damageOffset;
    }

    public int getCurrentPlayerDefenseChange()
    {
        if (bossMusicPlaying)
        {
            return bossSong.defenseOffset;
        }
        return songs[songPlaying].defenseOffset;
    }

    public string getCurrentPlayerAttackString()
    {
        if (bossMusicPlaying)
        {
            return bossSong.stat1;
        }
        return songs[songPlaying].stat1;
    }

    public string getCurrentPlayerDefenseString()
    {
        if (bossMusicPlaying)
        {
            return bossSong.stat2;
        }
        return songs[songPlaying].stat2;
    }
}
