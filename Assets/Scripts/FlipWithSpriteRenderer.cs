using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipWithSpriteRenderer : MonoBehaviour {

    public SpriteRenderer spriteRenderer;

    private Direction facing;
    private float posX;

	void OnEnable () {
        posX = transform.localPosition.x;
	}
	
	void Update () {
        if (spriteRenderer.flipX)
            facing = Direction.Left;
        else
            facing = Direction.Right;

        transform.localScale = new Vector3((int)facing, 1, 1);
        transform.localPosition = new Vector3(posX * (int)facing,
            transform.localPosition.y,
            transform.localPosition.z);
	}
}
