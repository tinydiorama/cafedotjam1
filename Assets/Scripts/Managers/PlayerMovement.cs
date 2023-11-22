using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float runSpeed = 5.0f;
    [SerializeField] private PlayerAttack playerAttack;
    [SerializeField] private TrailRenderer tr;

    private Vector2 moveInput = Vector2.zero;
    private bool canMove = true;
    private bool animLocked = false;
    private Rigidbody2D body;
    private Animator animator;
    private Vector2 _smoothedMovementInput;
    private Vector2 _movementInputSmoothVelocity;

    // dashing
    private float activeMoveSpeed;
    private float dashSpeed = 24f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;
    public float dashCounter;

    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        activeMoveSpeed = runSpeed;
        dashCounter = 0;
    }

    void FixedUpdate()
    {
        if ( GameManager.GetInstance().isPaused )
        {
            moveInput = Vector2.zero;
        } 

        if ( ! animLocked && moveInput != Vector2.zero )
        {
            animator.SetFloat("Horizontal", moveInput.x);
            animator.SetFloat("Vertical", moveInput.y);

            //int count = body.Cast()
        }

        if ( moveInput.x < 0 )
        {
            playerAttack.attackDirection = PlayerAttack.AttackDirection.left;
        } else if ( moveInput.x > 0 )
        {
            playerAttack.attackDirection = PlayerAttack.AttackDirection.right;
        } else if ( moveInput.y < 0 )
        {
            playerAttack.attackDirection = PlayerAttack.AttackDirection.down;
        } else if ( moveInput.y > 0 )
        {
            playerAttack.attackDirection = PlayerAttack.AttackDirection.up;
        }


        _smoothedMovementInput = Vector2.SmoothDamp(
            _smoothedMovementInput,
            moveInput,
            ref _movementInputSmoothVelocity,
            0.1f);
        if ( canMove )
        {
            body.velocity = moveInput * activeMoveSpeed;
        } else
        {
            body.velocity = moveInput * 0;
        }

        updateAnimation();

        if ( dashCounter > 0 )
        {
            dashCounter -= Time.deltaTime;
            if ( dashCounter < 0 )
            {
                dashCounter = 0;
            }
        }
    }

    private void updateAnimation()
    {
        if ( ! animLocked && canMove )
        {
            if ( moveInput != Vector2.zero )
            {
                animator.Play("PlayerMovement");
            }
            else
            {
                animator.Play("PlayerIdle");
            }
        }
    }

    private void OnMove(InputValue movementValue)
    {
        moveInput = movementValue.Get<Vector2>();
    }

    private void OnFire()
    {
        animator.Play("PlayerAttack");
        playerAttack.attack();
        animLocked = true;
        canMove = false;
    }

    public void endAttack()
    {
        Debug.Log("gets here");
        playerAttack.stopAttack();
        animLocked = false;
        canMove = true;
    }

    public void OnInteract()
    {
        if ( dashCounter == 0 )
        {
            Debug.Log("dashing");
            StartCoroutine(dash());
        }
    }

    private IEnumerator dash()
    {
        dashCounter = dashingCooldown;
        activeMoveSpeed = dashSpeed;
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        activeMoveSpeed = runSpeed;
        tr.emitting = false;
    }
}
