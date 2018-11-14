using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BryanBirdAI : MonoBehaviour
{

    [Header("Ground Raycast Settings")]
    public LayerMask layerMaskGround;
    public float yOffsetGround;
    public float xCollisionGround;
    public float xOffsetGround;

    [Header("Walk Settings")]
    public float walkSpeed;
    public float minIdleTime;
    public float maxIdleTime;

    [Header("Attack Settings")]
    public LayerMask layerMaskAttack;
    public Vector3 attackRaycast;
    public float cooldown;

    private SpriteRenderer sr;
    private Rigidbody2D rb2d;
    private Animator anim;
    private Direction facing;
    private bool idling;
    private bool attacking;

    private void OnEnable()
    {
        sr = GetComponent<SpriteRenderer>();
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == Tags.Solid.ToString())
        {
            if (anim.GetBool("Damaged") || attacking)
                return;

            StartIdle();
        }
    }

    void Update()
    {
        if (anim.GetBool("Damaged"))
            return;

        if (sr.flipX)
            facing = Direction.Left;
        else
            facing = Direction.Right;

        Debug.DrawLine(new Vector2(transform.position.x - (xCollisionGround / 2f) + xOffsetGround * (int)facing, transform.position.y - yOffsetGround), new Vector2(transform.position.x + (xCollisionGround / 2f) + xOffsetGround * (int)facing, transform.position.y - yOffsetGround));
        RaycastHit2D groundLine = Physics2D.Linecast(new Vector2(transform.position.x - (xCollisionGround / 2f) + xOffsetGround * (int)facing,
            transform.position.y - yOffsetGround),
            new Vector2(transform.position.x + (xCollisionGround / 2f) + xOffsetGround * (int)facing,
            transform.position.y - yOffsetGround), layerMaskGround);

        if (!attacking)
        {
            AttackUpdate();
        }

        anim.SetBool("Attack", attacking);

        if (!idling && !attacking)
        {
            if (groundLine.collider == null)
            {
                StartIdle();
            }
            else
            {
                rb2d.velocity = new Vector2((int)facing * walkSpeed, rb2d.velocity.y);
                anim.SetFloat("Speed", 1);
            }
        }
    }

    private void AttackUpdate()
    {
        Debug.DrawLine(new Vector2(transform.position.x - (attackRaycast.z / 2f) + attackRaycast.x * (int)facing, transform.position.y - attackRaycast.y), new Vector2(transform.position.x + (attackRaycast.z / 2f) + attackRaycast.x * (int)facing, transform.position.y - attackRaycast.y));
        RaycastHit2D attackLine = Physics2D.Linecast(new Vector2(transform.position.x - (attackRaycast.z / 2f) + attackRaycast.x * (int)facing,
            transform.position.y - attackRaycast.y),
            new Vector2(transform.position.x + (attackRaycast.z / 2f) + attackRaycast.x * (int)facing,
            transform.position.y - attackRaycast.y), layerMaskAttack);

        if (attackLine.collider != null)
        {
            StartCoroutine(Attack());
        }
    }

    private void StartIdle()
    {
        rb2d.velocity = new Vector2(0, rb2d.velocity.y);
        anim.SetFloat("Speed", 0);
        StartCoroutine(Idling());
    }

    private IEnumerator Idling()
    {
        idling = true;

        float time = Random.Range(minIdleTime, maxIdleTime);
        yield return new WaitForSecondsRealtime(time / 2f);
        sr.flipX = !sr.flipX;
        yield return new WaitForSecondsRealtime(time / 2f);

        idling = false;
    }

    private IEnumerator Attack()
    {
        attacking = true;
        rb2d.velocity = Vector2.zero;

        yield return new WaitForSecondsRealtime(cooldown);

        attacking = false;
    }
}
