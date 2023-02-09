using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller))]
public class Jump : MonoBehaviour
{
    [SerializeField, Range(0f, 10f)] private float jumpHeight = 3f;
    [SerializeField, Range(0, 5)] private int maxAirJumps = 0;
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
    }

    private void FixedUpdate()
    {
        onGround = ground.GetGround();
        velocity = rb.velocity;

        if (onGround)
        {
            jumpPhase = 0;
        }
        if (jumpRequest)
        {
            jumpRequest = false;
            JumpMovement();
        }
        if (rb.velocity.y > 0)
        {
            rb.gravityScale = upwardMovementMultiplier;
            animator.SetBool("isJump", true);
        }
        if (rb.velocity.y < 0)
        {
            rb.gravityScale = downwardMovementMultiplier;
            animator.SetBool("isJump", false);
        }
        if (rb.velocity.y == 0)
        {
            rb.gravityScale = defaultGravityScale;
        }
        rb.velocity = velocity;
    }       

    private void JumpMovement()
    {
        if (onGround || jumpPhase < maxAirJumps)
        {
            ++jumpPhase;
            jumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * jumpHeight);
            if (velocity.y > 0)
            {
                jumpSpeed = Mathf.Max(jumpSpeed - velocity.y, 0f);
            }
            else if (velocity.y < 0)
            {
                jumpSpeed += Mathf.Abs(rb.velocity.y);
            }
            velocity.y = jumpSpeed;
        }
    }
}