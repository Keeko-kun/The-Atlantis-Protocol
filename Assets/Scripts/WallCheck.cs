using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCheck : MonoBehaviour {

    public bool isRight;
    public bool isHead;

    private CharacterWalk cw;
    private Rigidbody2D rb2d;

	void Start () {
        cw = GetComponentInParent<CharacterWalk>();
        rb2d = GetComponentInParent<Rigidbody2D>();
	}

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == Tags.Solid.ToString())
        {
            if (col.GetComponent<PlatformEffector2D>())
                return;

            if (isRight)
            {
                cw.WallToRight = true;
            }
            else if (isHead)
            {
                rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
            }
            else
            {
                cw.WallToLeft = true;
            }

        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == Tags.Solid.ToString())
        {
            if (isRight)
            {
                cw.WallToRight = false;
            }
            else if (isHead)
            {
                return;
            }
            else
            {
                cw.WallToLeft = false;
            }
        }
    }
}
