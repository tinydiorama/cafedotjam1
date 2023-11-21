using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float runSpeed = 5.0f;
    [SerializeField] private PlayerAttack playerAttack;

    private Vector2 moveInput = Vector2.zero;
    private bool canMove = true;
    private bool animLocked = false;
    private Rigidbody2D body;
    private Animator animator;
    private Vector2 _smoothedMovementInput;
    private Vector2 _movementInputSmoothVelocity;


    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if ( GameManager.GetInstance().isPaused )
        {
            moveInput = Vector2.zero;
        } else
        {

            //moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
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
        body.velocity = moveInput * runSpeed;

        updateAnimation();
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
    }

    public void endAttack()
    {
        Debug.Log("gets here");
        playerAttack.stopAttack();
        animLocked = false;
    }
}
