using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSaveTrigger : MonoBehaviour {

    public ActivateCutscene cutscene;

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

            cutscene.SetPlayer(player);
            StartCoroutine(cutscene.RunCutscene());
        }
    }
}
