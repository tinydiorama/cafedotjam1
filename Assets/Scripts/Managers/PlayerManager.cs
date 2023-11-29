using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] RectTransform fader;
    [SerializeField] private CinemachineVirtualCamera vcam;
    [SerializeField] private Vector2 newCoordinates;
    [SerializeField] private RoomManager currentRoom;
    [SerializeField] private GameObject recordStore;
    [SerializeField] private GameObject diedText;
    [SerializeField] private AudioClip diedSound;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private TextAsset deathMessage;

    private void Start()
    {
        fader.gameObject.SetActive(true);
        LeanTween.alpha(fader, 0, 0).setOnComplete(() =>
        {
            fader.gameObject.SetActive(false);
        });
    }

    public int Health
    {
        set
        {
            health = value;
            StartCoroutine(flashRed());
            if (health <= 0)
            {
                Defeated();
            }
        }
        get
        {
            return health;
        }
    }

    [SerializeField] public int health = 50;
    [SerializeField] public int maxHealth = 50;
    public int playerDamage = 10;

    private static PlayerManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("more than one player manager");
        }
        instance = this;
    }
    public IEnumerator flashRed()
    {
        sprite.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sprite.color = Color.white;
    }

    public static PlayerManager GetInstance()
    {
        return instance;
    }

    public void setActiveRoom(RoomManager newRoom)
    {
        currentRoom = newRoom;
    }

    private void Defeated()
    {
        fader.gameObject.SetActive(true);
        diedText.SetActive(true);
        AudioManager.GetInstance().playSFX(diedSound);
        LeanTween.alpha(fader, 1, 0.1f).setOnComplete(() =>
        {
            Vector2 posDelta = newCoordinates - (Vector2)transform.position;
            currentRoom.resetEnemies();
            currentRoom.gameObject.SetActive(false);
            transform.position = newCoordinates;
            recordStore.SetActive(true);
            vcam.OnTargetObjectWarped(transform, posDelta);
            health = 50;
            LeanTween.alpha(fader, 0, 1.5f).setOnComplete(() =>
            {
                fader.gameObject.SetActive(false);
                diedText.SetActive(false);
                DialogueManager.GetInstance().EnterDialogueMode(deathMessage);
            });
        });
    }
}
