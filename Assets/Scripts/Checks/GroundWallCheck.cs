using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundWallCheck : MonoBehaviour
{
    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private LayerMask wall;
    private BoxCollider2D bc;
    public bool onGround { get; private set; }
    public bool onWall { get; private set; }
    public float friction { get; private set; }
    public Vector2 contactNormal { get; private set; }

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

    private void OnCollisionExit2D(Collision2D collision)
    {
        onGround = false;
        onWall = false;
        friction = 0.0f;
    }

    public void EvaluateCollision(Collision2D collision)
    {
        for (int i = 0; i < collision.contactCount; i++)
        {
            contactNormal = collision.GetContact(i).normal;
            onWall = Mathf.Abs(contactNormal.x) >= 0.9f;
        }
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
