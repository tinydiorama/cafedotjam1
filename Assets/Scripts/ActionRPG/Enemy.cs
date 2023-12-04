using Ink.Parsed;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Vector2 defaultCoords;
    [SerializeField] private GameObject heartDrop;
    [Range(0, 9)]
    [SerializeField] private int chanceOfHeartDrop;
    [Range(0, 9)]
    [SerializeField] private int chanceOfNoteDrop;

    public bool noKnockback;

    public int Health
    {
        set
        {
            health = value;
            StartCoroutine(flashRed());
            if ( health <= 0 )
            {
                Defeated();
            }
        }
        get
        {
            return health;
        }
    }

    [SerializeField] private int health = 10;
    public int damage = 5;

    public void resetPosition()
    {
        transform.localPosition = defaultCoords;
    }

    public virtual void Defeated()
    {
        // spawn random drops
        int randomDrop = Random.Range(0, 9);
        if ( randomDrop <= chanceOfHeartDrop)
        {
            GameObject obj = Instantiate(heartDrop, transform.parent);
            obj.transform.localPosition = transform.localPosition;
        }
        Destroy(gameObject);
    }

    public IEnumerator flashRed()
    {
        sprite.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sprite.color = Color.white;
    }

}
