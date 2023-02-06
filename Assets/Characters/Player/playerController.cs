using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed = 1f;
    public float jumpVelocity = 1f;
    bool jumpRequest = false;
    public float fallMultiplier = 2f;
    private BoxCollider2D bc;
    bool isGrounded;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            jumpRequest = true;
        }
    }

    
    void FixedUpdate()
    {
        if (!isGrounded)
        {
            return; 
        }

        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        
        Vector2 direction = new Vector2(x,y);

        walk(direction);

        if (jumpRequest)
        {
            rb.AddForce(Vector2.up * jumpVelocity, ForceMode2D.Impulse);
            jumpRequest = false;
        }

        if (rb.velocity.y<0)
        {
            rb.gravityScale = fallMultiplier;
        } else
        {
            rb.gravityScale = 1f;
        }

    }
     
    private void walk(Vector2 direction)
    {
        rb.velocity = new Vector2(direction.x * speed, rb.velocity.y);
    }

    private void OncollisionEnter2D(Collider2D collision)
    {
        isGrounded = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }
}
