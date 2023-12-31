using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float runSpeed = 5.0f;
    [SerializeField] private PlayerAttack playerAttack;
    [SerializeField] private TrailRenderer tr;
    [SerializeField] private AudioClip attackSound;

    private Vector2 moveInput = Vector2.zero;
    private bool canMove = true;
    private bool animLocked = false;
    private Rigidbody2D body;
    private Animator animator;
    private Vector2 _smoothedMovementInput;
    private Vector2 _movementInputSmoothVelocity;
    private GameManager gm;
    private float attackingCounter;
    private float attackingCooldown = 0.3f;

    // dashing
    private float activeMoveSpeed;
    private float dashSpeed = 15f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 0.5f;
    public float dashCounter;

    // dialogue
    public bool dialogueInRange;

    // change playlist
    // Door
    private bool doorInRange;
    private DoorManager doorTrigger;
    private BossDoor bossDoor;

    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        activeMoveSpeed = runSpeed;
        dashCounter = 0;
        gm = GameManager.GetInstance();
    }

    private void Update()
    {
        if ( HUD.GetInstance() != null )
        {
            if (dialogueInRange == true || doorInRange == true || gm.isConfrontBoss )
            {
                HUD.GetInstance().hideHelpText();
            }
            else
            {
                HUD.GetInstance().showHelpText();
            }
        }
    }

    void FixedUpdate()
    {
        if ( gm.isPaused )
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

        if ( attackingCounter > 0 )
        {
            attackingCounter -= Time.deltaTime;
            if ( attackingCounter < 0 )
            {
                attackingCounter = 0;
            }
        }

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

    public void setAlbumFaceUp()
    {
        animator.SetFloat("Horizontal", 0);
        animator.SetFloat("Vertical", 1);
        animator.Play("PlayerIdle");
    }

    private void OnMove(InputValue movementValue)
    {
        moveInput = movementValue.Get<Vector2>();
    }

    public void OnFire()
    {
        if ( gm.isGameStarted && ! gm.isPaused )
        {
            if ( attackingCounter == 0 )
            {
                attackingCounter = attackingCooldown;
                animator.Play("PlayerAttack");
                playerAttack.attack();
                AudioManager.GetInstance().playSFX(attackSound);
                animLocked = true;
                canMove = false;
            }
        }
    }

    public void endAttack()
    {
        playerAttack.stopAttack();
        animLocked = false;
        canMove = true;
        playerAttack.isAttacking = false;
    }

    public void OnDash()
    {
        if (gm.isGameStarted && !gm.isPaused)
        {
            if (dashCounter == 0)
            {
                StartCoroutine(dash());
            }
        }
    }

    private IEnumerator dash()
    {
        animator.Play("PlayerDash");
        dashCounter = dashingCooldown;
        activeMoveSpeed = dashSpeed;
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        activeMoveSpeed = runSpeed;
        tr.emitting = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Door")
        {
            doorInRange = true;
            doorTrigger = collision.gameObject.GetComponent<DoorManager>();
            if ( doorTrigger == null ) // must be the bossDoor
            {
                bossDoor = collision.gameObject.GetComponent<BossDoor>();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Door")
        {
            doorInRange = false;
            doorTrigger = null;
            bossDoor = null;
        }
    }

    public void OnInteract()
    {
        if ( ! gm.isPaused )
        {
            if (!dialogueInRange && doorInRange)
            {
                if (doorTrigger != null) // must be the bossDoor
                {
                    doorTrigger.goToCoordinates();
                } 
            }
            else if (!doorInRange && gm.isGameStarted && !dialogueInRange)// change playlist
            {
                if (!gm.isConfrontBoss )
                {
                    AudioManager.GetInstance().ChangeSong();
                    HUD.GetInstance().updateCurrentStats();
                }
            }
        }
    }
}
