using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestOpen : MonoBehaviour {

    [SerializeField]
    private Chest content;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private GameObject coin;

    private bool hasBeenOpened = false;
    private bool playerStandsInTrigger;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag(Tags.Player.ToString()))
        {
            playerStandsInTrigger = true;
        }

    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag(Tags.Player.ToString()))
        {
            playerStandsInTrigger = false;
        }
    }

    void Start () {
        if (animator.GetBool("open"))
        {
            SetOpen();
        }
	}

    void Update()
    {
        if (!hasBeenOpened)
        {
            if (playerStandsInTrigger && Globals.GetButtonDown("Action"))
            {
                StartCoroutine(PerformOpen());
            }
        }
    }

    private void SetOpen()
    {
        hasBeenOpened = true;
        GetComponent<FadeGameWorld>().disabled = true;
    }

    private IEnumerator PerformOpen()
    {
        SetOpen();
        animator.SetBool("open", true);     

        yield return new WaitForSeconds(0.25f);

        switch (content.contents)
        {
            case ChestContents.Money:
                for (int i = 0; i < content.amount; i++)
                {
                    Instantiate(coin, animator.transform.position, animator.transform.rotation, animator.transform);
                    yield return new WaitForSeconds(0.075f);
                }
                break;

            case ChestContents.Enemy:
                break;

            case ChestContents.Item:
                break;

            case ChestContents.Nothing:
                break;
        }
    }

}
