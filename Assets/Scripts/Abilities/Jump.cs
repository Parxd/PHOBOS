using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(Controller))]
public class Jump : MonoBehaviour
{
    [SerializeField, Range(0f, 10f)] private float jumpHeight = 3f;
    [SerializeField, Range(0, 5)] private int maxJumps = 2;
    [SerializeField, Range(0f, 5f)] private float downwardMovementMultiplier = 3f;
    [SerializeField, Range(0f, 5f)] private float upwardMovementMultiplier = 1.7f;

    private Vector2 velocity;
    private Rigidbody2D rb;
    private GroundCheck ground;
    private Controller control;
    private Animator animator;

    private int jumpPhase;
    private float jumpSpeed;
    private float defaultGravityScale;

    private bool jumpRequest;
    private bool onGround;

    private enum JumpState {singleJump, doubleJump}

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ground = GetComponent<GroundCheck>();
        control = GetComponent<Controller>();
        animator = GetComponent<Animator>();
        defaultGravityScale = 1f;
    }

    void Update()
    {
        jumpRequest |= control.input.RetrieveJumpInput();
        if (rb.velocity.y < 0)
        {
            if (jumpPhase == 1)
            {
                animator.SetBool("isJump", false);
            }
            if (jumpPhase == 2)
            {
                animator.SetBool("isDoubleJump", false);
            }
        }
    }

    private void FixedUpdate()
    {
        onGround = ground.GetGround();
        velocity = rb.velocity;

        if (onGround) // Reset jump counter back to 0 when landing on ground
        {
            jumpPhase = 0;
        }
        if (jumpRequest) // If spacebar is pressed and a jump is request
        {
            jumpRequest = false;
            JumpMovement();
        }
        if (rb.velocity.y > 0) // Upwards movement
        {
            rb.gravityScale = upwardMovementMultiplier;
        }
        if (rb.velocity.y < 0) // Downwards movement
        {
            rb.gravityScale = downwardMovementMultiplier;
        }
        if (rb.velocity.y == 0) // On ground OR no vertical movement
        {
            rb.gravityScale = defaultGravityScale;
        }
        rb.velocity = velocity;
    }
    
    private void JumpMovement()
    {
        if (onGround || jumpPhase < maxJumps) // Check if jump is available
        {
            ++jumpPhase;
            if (jumpPhase == 1)
            {
                animator.SetBool("isJump", true);
            }
            if (jumpPhase == 2)
            {
                animator.SetBool("isJump", false); // In case of double jumping without time to fall
                animator.SetBool("isDoubleJump", true);
            }
            jumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * jumpHeight);
            if (velocity.y > 0)
            {
                jumpSpeed = Mathf.Max(jumpSpeed - velocity.y, 0f);
            }
            else if (velocity.y < 0)
            {
                jumpSpeed += Mathf.Abs(rb.velocity.y);
            }
            velocity.y += jumpSpeed;
        }
    }
}