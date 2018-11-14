using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterWalk : MonoBehaviour
{

    [Range(0.1f, 25f)]
    public float walkSpeed;
    [Range(0.0f, 1f)]
    public float deadzone;

    private Animator anime;
    private SpriteRenderer sr;
    [HideInInspector]
    public Direction dir;
    private Rigidbody2D rb;

    public bool WallToRight { get; set; }
    public bool WallToLeft { get; set; }

    void Start()
    {
        anime = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (GetComponent<PreventInput>().InputProhibited)
        {
            anime.SetFloat("Speed", 0);
            return;
        }

        float axis = Input.GetAxisRaw("Horizontal");

        dir = DecideDirection(axis);

        if (Mathf.Abs(axis) > deadzone)
        {
            switch (dir)
            {
                case Direction.Right:
                    if (WallToRight)
                    {
                        ResetWalk();
                        return;
                    }
                    break;
                case Direction.Left:
                    if (WallToLeft)
                    {
                        ResetWalk();
                        return;
                    }
                    break;
                case Direction.None:
                    return;
            }


            if (Input.GetAxisRaw("Vertical") < -GetComponent<CharacterGround>().verticalDeadzone && GetComponent<CharacterJump>().IsGrounded)
                dir = Direction.None;

            rb.velocity = new Vector2((int)dir * walkSpeed, rb.velocity.y);

            anime.SetFloat("Speed", Mathf.Abs((int)dir));

        }
        else
        {
            ResetWalk();
        }

    }

    public void ResetWalk()
    {
        rb.velocity = new Vector2(0, rb.velocity.y);
        anime.SetFloat("Speed", 0);
    }

    public Direction DecideDirection(float axis)
    {
        if (axis > deadzone)
        {
            sr.flipX = false;
            return Direction.Right;
        }
        else if (axis < -deadzone)
        {
            sr.flipX = true;
            return Direction.Left;
        }
        else
        {
            return Direction.None;
        }

    }
}

