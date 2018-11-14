using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcicleAttack : MonoBehaviour {

    public float speed;
    public bool flipped;

    private Direction direction = Direction.Right;
    private Vector2 speedVector;
    private Animator anim;

	void Start () {
        speedVector = new Vector2(speed, 0);
        anim = GetComponent<Animator>();
	}

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == Tags.Solid.ToString())
        {
            anim.SetBool("OnHit", true);
            Destroy(gameObject, anim.GetCurrentAnimatorStateInfo(0).length);
        }
    }

    void Update () {
        if (flipped)
        {
            direction = Direction.Left;
            GetComponent<SpriteRenderer>().flipX = true;
        }

        if (!anim.GetBool("OnHit"))
            transform.Translate((int)direction * speedVector * Time.deltaTime);
	}
}
