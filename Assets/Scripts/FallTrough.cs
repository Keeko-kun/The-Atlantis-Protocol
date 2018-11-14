using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FallTrough : MonoBehaviour {

    private TilemapCollider2D col;

	void Start () {
        col = GetComponent<TilemapCollider2D>();
	}

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == Tags.Player.ToString())
        {
            col.isTrigger = false;
        }
    }

    public void ActivateFall()
    {
        col.isTrigger = true;
    }
}
