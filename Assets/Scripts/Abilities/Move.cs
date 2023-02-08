using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Controller))]
public class Move : MonoBehaviour
{
    [SerializeField, Range(0f, 100f)] private float maxSpeed = 8f;
    [SerializeField, Range(0f, 100f)] private float maxAcceleration = 35f;
    [SerializeField, Range(0f, 100f)] private float maxAirAcceleration = 20f;

    private float maxSpeedChange;
    private float acceleration;
    private bool gCheck;

    private Vector2 direction;
    private Vector2 velocity;
    private Vector2 desiredVelocity;

    private SpriteRenderer sprite;
    private Rigidbody2D rb;
    private GroundCheck ground;
    private Controller control;
    private Animator animator;
    
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        ground = GetComponent<GroundCheck>();
        control = GetComponent<Controller>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        direction.x = control.input.RetrieveMoveInput();
        desiredVelocity = new Vector2(direction.x, 0f) * Mathf.Max(maxSpeed - ground.friction, 0f);
        Flip();
    }

    private void FixedUpdate()
    {
        gCheck = ground.onGround;
        velocity = rb.velocity;
        if (gCheck)
        {
            acceleration = maxAcceleration;
        }
        else
        {
            acceleration = maxAirAcceleration;
        }
        maxSpeedChange = acceleration * Time.deltaTime;
        velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
        rb.velocity = velocity;
        animator.SetFloat("Speed", Mathf.Abs(velocity.x));
    }

    private void Flip()
    {
        if (velocity.x < -0.01f)
        {
            sprite.flipX = true;
        }
        if (velocity.x > 0.01f)
        {
            sprite.flipX = false;
        }
    }
}
