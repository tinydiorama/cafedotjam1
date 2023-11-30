using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField] private RectTransform HPpanel;
    [SerializeField] private RectTransform defaultHP;
    [SerializeField] private TextMeshProUGUI stat1Text;
    [SerializeField] private TextMeshProUGUI stat2Text;
    [SerializeField] private GameObject helpText;


    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite halfHeart;
    [SerializeField] private Sprite emptyHeart;

    [SerializeField] private TextMeshProUGUI noteCount;

    [SerializeField] private GameObject[] heartDisplays;

    private PlayerManager pm;
    public float hpWidth;

    private static HUD instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("more than one player manager");
        }
        instance = this;
    }

    public static HUD GetInstance()
    {
        return instance;
    }

    // Start is called before the first frame update
    void Start()
    {
        pm = PlayerManager.GetInstance();
        updateCurrentStats();
    }

    // Update is called once per frame
    void Update()
    {
        updateHearts();
    }

    public void updateHearts()
    {
        float maxHealth = pm.maxHealth;
        float currentHealth = pm.Health;
        float numHearts = currentHealth / 10;
        foreach( GameObject heartDisplay in heartDisplays)
        {
            Image heartImage = heartDisplay.GetComponent<Image>();
            if ( numHearts == 0.5 ) // display half a heart
            {
                heartImage.sprite = halfHeart;
            } else if ( numHearts >= 1 ) // display a heart
            {
                heartImage.sprite = fullHeart;
            } else // display an empty heart
            {
                heartImage.sprite = emptyHeart;
            }
            numHearts--;
        }
    }

    public void updateNotes()
    {
        noteCount.text = pm.numNotes.ToString();
    }

    public void updateCurrentStats()
    {
        stat1Text.text = AudioManager.GetInstance().getCurrentPlayerAttackString();
        stat2Text.text = AudioManager.GetInstance().getCurrentPlayerDefenseString();
    }

    public void showHelpText()
    {
        helpText.SetActive(true);
    }
    public void hideHelpText()
    {
        helpText.SetActive(false);
    }
}
