using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public int damage = 5;
    public bool isAttacking;

    public enum AttackDirection
    {
        left, right, up, down
    }

    public AttackDirection attackDirection;

    private Vector2 rightAttackOffset;
    [SerializeField] private Collider2D hitCollider;

    private void Start()
    {
        rightAttackOffset = transform.position;
        isAttacking = false;
    }

    private void attackRight()
    {
        transform.localPosition = new Vector3(0.5f, 0);
        StartCoroutine(enableCollider());
    }

    private void attackLeft()
    {
        transform.localPosition = new Vector3(-0.5f, 0);
        StartCoroutine(enableCollider());
    }

    private void attackUp()
    {
        transform.localPosition = new Vector3(0, 1);
        StartCoroutine(enableCollider());
    }

    private void attackDown()
    {
        transform.localPosition = new Vector3(0, -1);
        StartCoroutine(enableCollider());
    }

    IEnumerator enableCollider()
    {
        yield return new WaitForSeconds(0.15f);
        hitCollider.enabled = true;

    }

    public void attack()
    {
        isAttacking = true;
        switch(attackDirection)
        {
            case AttackDirection.left:
                attackLeft();
                break;
            case AttackDirection.right:
                attackRight();
                break;
            case AttackDirection.up:
                attackUp();
                break;
            case AttackDirection.down:
                attackDown();
                break;

        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("player attack");
        // deal damage
        if ( other.gameObject.tag == "Enemy" && isAttacking )
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();

            if ( enemy != null )
            {
                Debug.Log("attacking enemy for damage " + damage + " plus " + AudioManager.GetInstance().getCurrentPlayerAttackChange());
                enemy.Health -= damage + AudioManager.GetInstance().getCurrentPlayerAttackChange();
                isAttacking = false;
            }
        }
    }

    public void stopAttack()
    {
        hitCollider.enabled = false;
    }
}
