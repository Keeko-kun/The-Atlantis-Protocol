using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkNPC : MonoBehaviour
{
    public DisplayDialouge textBox;

    private Transform player;
    private bool playerStandsInTrigger;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == Tags.Player.ToString())
        {
            player = col.transform;
            playerStandsInTrigger = true;

        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == Tags.Player.ToString())
        {
            player = null;
            playerStandsInTrigger = false;
        }
    }

    void Update()
    {
        if (playerStandsInTrigger && Globals.GetButtonDown("Action"))
        {
            if (player.GetComponent<PreventInput>().InputProhibited)
                return;

            player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            player.GetComponent<PreventInput>().InputProhibited = true;

            textBox.InitiateDialouge(player.GetComponent<PreventInput>());
        }
    }
}
