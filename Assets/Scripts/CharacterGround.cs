using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGround : MonoBehaviour {


    public LayerMask playerMask;
    public float lineEnd;
    public float xCollision;
    public float verticalDeadzone;
    public List<string> platformTags;

    private Rigidbody2D rb;
    private Animator anim;
    private CharacterJump cj;

    void Start () {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        cj = GetComponent<CharacterJump>();
    }

    private void Update()
    {
        Debug.DrawLine(new Vector2(transform.position.x - (xCollision / 2f), transform.position.y - lineEnd), new Vector2(transform.position.x + (xCollision / 2f), transform.position.y - lineEnd));
        RaycastHit2D groundLine = Physics2D.Linecast(new Vector2(transform.position.x - (xCollision / 2f), transform.position.y - lineEnd), new Vector2(transform.position.x + (xCollision / 2f), transform.position.y - lineEnd), playerMask);

        if (groundLine.collider == null)
        {
            SetGroundState(false);
        }
        else
        {
            if (groundLine.collider.tag == Tags.SemiSolid.ToString() && Globals.GetButtonDown("Jump") && Input.GetAxisRaw("Vertical") < -verticalDeadzone)
            {
                groundLine.collider.GetComponent<FallTrough>().ActivateFall();
            }

            if (platformTags.Any(t => t == groundLine.collider.tag) && !groundLine.collider.isTrigger)
            {
                SetGroundState(true);
            }
            else
            {
                SetGroundState(false);
            }
        }
    }

    private void SetGroundState(bool state)
    {
        anim.SetBool("Grounded", state);
        cj.IsGrounded = state;
    }

    private IEnumerator CheckVelocity()
    {
        yield return new WaitForSecondsRealtime(0.05f);
        if (rb.velocity.y == 0)
            SetGroundState(true);
        else
            SetGroundState(false);
    }

}
