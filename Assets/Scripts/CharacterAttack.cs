using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttack : MonoBehaviour {

    public float cooldown;

    private Animator anim;

    private bool attacking;
    private float walkSpeed;
    private PreventInput pi;
    private Rigidbody2D rb2d;
    private CharacterInventory ci;

	void Start () {
        anim = GetComponent<Animator>();
        pi = GetComponent<PreventInput>();
        rb2d = GetComponent<Rigidbody2D>();
        ci = GetComponent<CharacterInventory>();
	}
	
	void Update () {
        if (pi.InputProhibited)
            return;

		if (Globals.GetButtonDown("Attack") && attacking == false && GetComponent<CharacterJump>().IsGrounded && ci.inv.currentMelee.item != null)
        {
            StartCoroutine(Attack());
        }

        anim.SetBool("SwordAttack", attacking);
	}

    private IEnumerator Attack()
    {
        attacking = true;
        pi.InputProhibited = true;
        rb2d.velocity = Vector2.zero;

        yield return new WaitForSecondsRealtime(cooldown);

        attacking = false;
        pi.InputProhibited = false;
    }
}
