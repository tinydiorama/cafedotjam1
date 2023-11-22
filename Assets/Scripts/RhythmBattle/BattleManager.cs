using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI bpmDisplay;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private Animator monsterAnimator;

    [SerializeField] private GameObject mainBattle;
    [SerializeField] private GameObject monsterVolley;
    [SerializeField] private GameObject playerVolley;

    [SerializeField] private AudioSource soundPlayer;
    [SerializeField] private AudioClip sound;

    public float timeBetweenBeats;
    public float currentSeconds;
    public float currentBeats;
    public int currentBeatInt;
    public float elapsedTime;
    public float lastAttack;
    public Song currentTrack;
    public bool isAttacking;
    public float timeBeforeBattle;
    public float timeBetweenAttacks;

    // current note track handler
    public Notes currentNotes;
    public int currentNoteIndex;
    public float currentNoteLength;
    public float currentBeatforCurrentTrack;
    public float currentTimeforCurrentTrack;
    public float startOfCurrentTrack;
    public float startBeatofCurrentTrack;
    public string turn;
    private GameObject currVolley;

    public int enemyHP;
    public int playerHP;
    public int enemyDamage;
    public int playerDamage;

    private AudioManager am;

    private static BattleManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("more than one battle manager");
        }
        instance = this;
    }

    public static BattleManager GetInstance()
    {
        return instance;
    }

    void Start()
    {
        am = AudioManager.GetInstance();
        currentTrack = am.getCurrentTrack();
        timeBetweenBeats = 60f / currentTrack.bpm;
        elapsedTime = (float)AudioSettings.dspTime;
        playerAnimator.speed = currentTrack.bpm / 60f;
        monsterAnimator.speed = currentTrack.bpm / 60f;

        PlayerManager pm = PlayerManager.GetInstance();
        playerHP = pm.Health;
        playerDamage = pm.playerDamage;
    }

    void Update()
    {
        currentSeconds = (float)(AudioSettings.dspTime - elapsedTime - currentTrack.firstBeatOffset);
        currentBeats = currentSeconds / timeBetweenBeats;
        if ( (int)currentBeats != currentBeatInt )
        {
            soundPlayer.clip = sound;
            soundPlayer.Play();
            bpmDisplay.text = Mathf.Round(currentBeats).ToString();
            currentBeatInt = (int)currentBeats;

            // do some math to see if we're on a beat
            if ( enemyHP > 0 && currentSeconds > timeBeforeBattle && currentSeconds > timeBetweenAttacks)
            {
                if ( isAttacking )
                {
                    currentTimeforCurrentTrack = (float)(AudioSettings.dspTime - elapsedTime) - startOfCurrentTrack;
                    currentBeatforCurrentTrack = currentTimeforCurrentTrack / timeBetweenBeats;
                    if ( currentNoteIndex < (currentNotes.notes.Count - 1) && 
                        currentNotes.notes[currentNoteIndex] + startBeatofCurrentTrack < currentBeats + 1f)
                    {
                        if ( turn == "enemy" )
                        {
                            AudioManager.GetInstance().playSFX(sound);
                            currVolley = Instantiate(monsterVolley, mainBattle.transform);
                            currVolley.GetComponent<Animator>().speed = (currentTrack.bpm / 60f) / 2;
                            currVolley.GetComponent<VolleyBall>().timeToDestroy = (60f / currentTrack.bpm) * 2;
                            currVolley.GetComponent<VolleyBall>().registerDestroy();
                            turn = "player";
                            currentNoteIndex++;
                        } else
                        {
                            // allocate time for the player's turn, but we're not managing it, they are (see VolleyArea.cs)
                            turn = "enemy";
                            currentNoteIndex++;
                        }
                    } else if ( currentNoteIndex >= (currentNotes.notes.Count - 1) )
                    {
                        // last array slot holds the time before the next attack
                        timeBetweenAttacks = currentSeconds + currentNotes.notes[currentNoteIndex];
                        isAttacking = false;
                        Debug.Log("turn over");
                    }
                } else
                {
                    setUpAttack();
                }
            }

        }
    }

    public void volleyBack()
    {
        // successful volley
        currVolley = Instantiate(playerVolley, mainBattle.transform);
        currVolley.GetComponent<Animator>().speed = (currentTrack.bpm / 60f) / 2;
        currVolley.GetComponent<VolleyBall>().timeToDestroy = (60f / currentTrack.bpm) * 2;
        currVolley.GetComponent<VolleyBall>().registerDestroy();
    }

    private void setUpAttack()
    {
        startOfCurrentTrack = currentSeconds;
        startBeatofCurrentTrack = currentBeatInt;
        isAttacking = true;
        currentNoteLength = 0f;
        currentBeatforCurrentTrack = 0f;
        currentNoteIndex = 0;
        turn = "enemy";
        // pick a note pattern
        int potentialNotes = currentTrack.noteCombos.Count;
        int noteCombo = Random.Range(0, potentialNotes);
        currentNotes = currentTrack.noteCombos[noteCombo];
        for ( int i = 0; i < currentNotes.notes.Count; i++)
        {
            currentNoteLength += (currentNotes.notes[i] - currentNoteLength) * 2; 
            // current notes is additive, and then multiply by 2 because the player needs to repeat it
        }
    }
}
