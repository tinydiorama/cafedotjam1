using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD : MonoBehaviour
{
    [SerializeField] private RectTransform HPpanel;
    [SerializeField] private RectTransform defaultHP;

    private PlayerManager pm;
    public float hpWidth;

    // Start is called before the first frame update
    void Start()
    {
        pm = PlayerManager.GetInstance();
    }

    // Update is called once per frame
    void Update()
    {
        hpWidth = defaultHP.rect.width;
        float hpPercent = pm.Health / 100f;
        float newWidth = hpPercent * hpWidth;
        // should be in an event listener
        Debug.Log(newWidth);
        HPpanel.sizeDelta = new Vector2(newWidth, HPpanel.rect.height);
    }
}
