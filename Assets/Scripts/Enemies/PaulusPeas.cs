using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaulusPeas : MonoBehaviour {

    public float lifeTime;
    public float speed;

    private float totalTime;
    private Rigidbody2D rb2d;

    public Direction Facing { get; set; }

	void OnEnable () {
        rb2d = GetComponent<Rigidbody2D>();
        totalTime = 0;
	}

    void Update () {
        if (Facing == Direction.Left)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }

        rb2d.velocity = new Vector2((int)Facing * speed, rb2d.velocity.y);

        totalTime += Time.deltaTime;
        if (totalTime >= lifeTime)
        {
            Destroy(gameObject);
        }
    }
}
