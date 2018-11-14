using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterJump : MonoBehaviour
{

    [Range(0.1f, 25f)]
    public float fallSpeed;
    [Range(0.1f, 25f)]
    public float jumpMultiplier;
    [Range(0.1f, 25f)]
    public float jumpVelocity;

    private Rigidbody2D rb;

    public bool IsGrounded { get; set; }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    Vector2 moveDirection;

    void Update()
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallSpeed - 1) * Time.deltaTime;

        }
        else if (rb.velocity.y > 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (jumpMultiplier - 1) * Time.deltaTime;
        }

        if (GetComponent<PreventInput>().InputProhibited)
            return;

        if (Globals.GetButtonDown("Jump") && Input.GetAxisRaw("Vertical") > -GetComponent<CharacterGround>().verticalDeadzone)
        {
            if (IsGrounded) //And Not Pointing Down With Direction Of Stick
                Jump();
        }
    }

    public void Jump()
    {
        rb.velocity = Vector2.up * jumpVelocity;
    }
}
