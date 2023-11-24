using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour
{
    [SerializeField] private RectTransform HPpanel;
    [SerializeField] private RectTransform defaultHP;
    [SerializeField] private TextMeshProUGUI stat1Text;
    [SerializeField] private TextMeshProUGUI stat2Text;
    [SerializeField] private GameObject helpText;

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
        hpWidth = defaultHP.rect.width;
        float hpPercent = pm.Health / 100f;
        float newWidth = hpPercent * hpWidth;
        // should be in an event listener
        HPpanel.sizeDelta = new Vector2(newWidth, HPpanel.rect.height);
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
