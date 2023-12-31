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
        yield return new WaitForSeconds(0f);
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
        // deal damage
        if ( other.gameObject.tag == "Enemy" && isAttacking )
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();

            if ( enemy != null )
            {

                Vector3 direction = (other.transform.position - transform.position).normalized;
                Rigidbody2D otherBody = other.GetComponent<Rigidbody2D>();
                //Debug.Log("attacking enemy for damage " + damage + " plus " + AudioManager.GetInstance().getCurrentPlayerAttackChange());
                enemy.Health -= damage + AudioManager.GetInstance().getCurrentPlayerAttackChange();
                isAttacking = false;

                if ( otherBody != null && ! enemy.noKnockback )
                {
                    StartCoroutine(knockback(otherBody.GetComponent<EnemyAI>(), otherBody, direction));
                }
            }
        }
    }

    private IEnumerator knockback(EnemyAI enemyAI, Rigidbody2D otherBody, Vector3 direction)
    {
        if ( otherBody != null )
        {
            enemyAI.enemyKnockback = true;
            otherBody.velocity = new Vector2(0, 0);
            otherBody.velocity = direction * 10f;
            yield return new WaitForSeconds(0.3f);
            if ( otherBody != null )
            {
                otherBody.velocity = new Vector2(0, 0);
            }
            enemyAI.enemyKnockback = false;
        }
    }

    public void stopAttack()
    {
        hitCollider.enabled = false;
    }
}
