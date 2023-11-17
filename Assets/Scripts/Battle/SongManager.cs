using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SongManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI bpmDisplay;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private Animator monsterAnimator;
    public float bpm;
    public float timeBetweenBeats;
    public float currentSeconds;
    public float currentBeats;
    public float elapsedTime;
    public AudioSource track;

    void Start()
    {
        track = GetComponent<AudioSource>();
        timeBetweenBeats = 60f / bpm;
        elapsedTime = (float)AudioSettings.dspTime;
        playerAnimator.speed = bpm / 60f;
        monsterAnimator.speed = bpm / 60f;
        track.Play();
    }

    void Update()
    {
        currentSeconds = (float)(AudioSettings.dspTime - elapsedTime);
        currentBeats = currentSeconds / timeBetweenBeats;

        bpmDisplay.text = Mathf.Round(currentBeats).ToString();
    }
}
