using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Cutscene))]
public class ActivateCutscene : MonoBehaviour {

    public List<int> eventsTrue;
    public List<int> eventsFalse;

    private Cutscene cutscene;
    private Transform player;
    private bool running = false;

    private void Start()
    {
        cutscene = GetComponent<Cutscene>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == Tags.Player.ToString())
        {
            player = collision.transform;
            StartCoroutine(RunCutscene());
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == Tags.Player.ToString())
        {
            player = collision.transform;
            StartCoroutine(RunCutscene());
        }
    }

    public IEnumerator RunCutscene()
    {
        if (player.GetComponent<PreventInput>().InputProhibited)
            yield break;

        if (CanActivate() == false)
            yield break;

        if (running)
            yield break;

        running = true;

        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        player.GetComponent<PreventInput>().InputProhibited = true;

        yield return StartCoroutine(cutscene.InitiateCutscene(player.GetComponent<PreventInput>()));

        player.GetComponent<PreventInput>().InputProhibited = false;
        running = false;
    }

    public bool CanActivate()
    {
        foreach (int id in eventsTrue)
        {
            if (!Globals.events[id])
            {
                return false;
            }
        }

        foreach (int id in eventsFalse)
        {
            if (Globals.events[id])
            {
                return false;
            }
        }

        return true;
    }

    public void SetPlayer(Transform player)
    {
        this.player = player;
    }
}
