using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartPickup : MonoBehaviour
{
    [SerializeField] private AudioClip pickupSound;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {

            PlayerManager player = other.gameObject.GetComponent<PlayerManager>();

            if (player != null)
            {
                player.Health += 10;
                AudioManager.GetInstance().playSFX(pickupSound);
                Destroy(gameObject);
            }
        }
    }
}
