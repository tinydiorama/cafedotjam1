using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float runSpeed = 5.0f;

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

            moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }

        if ( ! animLocked && moveInput != Vector2.zero )
        {
            animator.SetFloat("Horizontal", moveInput.x);
            animator.SetFloat("Vertical", moveInput.y);
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
}
