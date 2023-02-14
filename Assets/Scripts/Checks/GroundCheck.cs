using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [SerializeField] private LayerMask jumpableGround;
    private BoxCollider2D bc;
    public bool onGround;
    public float friction;

    private void Start()
    {
        bc = GetComponent<BoxCollider2D>();
    }

    public bool GetGround()
    {
        return onGround;
    }

    public float GetFriction()
    {
        return friction;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        EvaluateCollision(collision);
        RetrieveFriction(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        EvaluateCollision(collision);
        RetrieveFriction(collision);
    }

    private void EvaluateCollision(Collision2D collision)
    {
        onGround = Physics2D.BoxCast(bc.bounds.center, bc.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }

    private void RetrieveFriction(Collision2D collision)
    {
        PhysicsMaterial2D material = collision.rigidbody.sharedMaterial;
        friction = 0; 
        if (material != null)
        {
            friction = material.friction;
        }
    }
}
