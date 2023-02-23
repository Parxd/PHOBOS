using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller))] // PlayerController input
public class Dash : MonoBehaviour
{
    [SerializeField, Range(0f, 100f)] private float maxSpeed = 8f;

    private SpriteRenderer sprite;
    private Rigidbody2D rb;
    private GroundWallCheck ground;
    private Controller control;
    private Animator animator;
    private TrailRenderer tr;

    private bool onGround;

    [SerializeField] private float dashVelocity = 24f;
    [SerializeField] private float dashTime = 0.2f;
    [SerializeField] private float dashCoolDown = 1f;
    private Vector2 dashDir;
    bool canDash = true;
    bool isDash;
    bool dashInput;


    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        ground = GetComponent<GroundWallCheck>();
        control = GetComponent<Controller>();
        animator = GetComponent<Animator>();
        tr = GetComponent<TrailRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        onGround = ground.GetGround();
        dashInput = control.input.RetrieveDashInput();

        if (dashInput && canDash) // if player dashes
        {
            isDash = true;
            canDash = false;
            tr.emitting = true; // trail rendering is true once player dashes
            
            dashDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")); // direction of player input

            if (dashDir==Vector2.zero) // if there is no directional input
            {
                dashDir = new Vector2(transform.localScale.x, 0);
            }

            StartCoroutine(stopDash());

        }

        animator.SetBool("isDash", isDash);

        if (isDash) // actual dash
        {
            rb.velocity = dashDir.normalized * dashVelocity;
            return;
        }
         
        if (onGround) // if player is on ground, player can dash again
        {
            canDash = true;
        }
    }

    private IEnumerator stopDash()
    {
        yield return new WaitForSeconds(dashTime);
        tr.emitting = false;
        isDash = false;
    }
}
