using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MikeMushroomAI : MonoBehaviour {

    [Header("Attack Settinigs")]
    public GameObject mushroomAttack;
    public float cooldownMin;
    public float cooldownMax;
    public float attackTime;

    private SpriteRenderer sr;
    private Rigidbody2D rb2d;
    private Animator anim;
    private bool playerInReach;
    private bool preparingAttack;
    private bool recentlyDamaged;
    private Transform player;

    void OnEnable () {
        sr = GetComponent<SpriteRenderer>();
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == Tags.Player.ToString())
        {
            playerInReach = true;
            player = collision.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == Tags.Player.ToString())
        {
            playerInReach = false;
        }
    }

    void Update () {
        if (anim.GetBool("Damaged"))
        {
            StopAllCoroutines();
            preparingAttack = false;
            anim.SetBool("Attacking", false);
            recentlyDamaged = true;
            return;
        }

        if (playerInReach && player != null)
        {
            if (!preparingAttack)
            {
                StartCoroutine(PrepareAttack());
            }
        }
	}

    private IEnumerator PrepareAttack()
    {
        preparingAttack = true;

        float time = 0;

        if (recentlyDamaged)
            time = Random.Range(0, cooldownMin);
        else
            time = Random.Range(cooldownMin, cooldownMax);

        yield return new WaitForSecondsRealtime(time);

        recentlyDamaged = false;

        StartCoroutine(Attack());
    }

    private IEnumerator Attack()
    {
        anim.SetBool("Attacking", true);

        GameObject cloud = Instantiate(mushroomAttack, transform.parent);
        cloud.transform.position = player.position;
        EnemyHurtbox[] hurtboxes = cloud.GetComponentsInChildren<EnemyHurtbox>();
        
        foreach(EnemyHurtbox hb in hurtboxes)
        {
            hb.health = GetComponent<EnemyHealth>();
        }

        yield return new WaitForSecondsRealtime(attackTime);
        
        anim.SetBool("Attacking", false);
        preparingAttack = false;
    }
}
