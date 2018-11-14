using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipNPC : MonoBehaviour {

    public Transform player;
	
	void Update () {

        float deltaX = player.position.x - transform.position.x;

        if (deltaX > 0) GetComponent<SpriteRenderer>().flipX = false;
        else if (deltaX < 0) GetComponent<SpriteRenderer>().flipX = true;
    }
}
