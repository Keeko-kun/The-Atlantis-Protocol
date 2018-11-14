using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHurtbox : MonoBehaviour {

    public EnemyHealth health;

    private BoxCollider2D col;

    private void OnEnable()
    {
        col = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (gameObject.tag == Tags.EnemyProjectile.ToString())
            return;

        float damage = -1;

        switch (collision.tag)
        {
            case "Melee":
                damage = collision.GetComponentInParent<CharacterInventory>().inv.currentMelee.item.baseStrength;
                break;
            case "Special":
                damage = collision.GetComponentInParent<CharacterInventory>().inv.currentSpecial.item.baseStrength;
                break;
            case "Item":
                damage = collision.GetComponentInParent<CharacterInventory>().inv.currentSpecial.item.baseStrength;
                break;
        }

        if (damage != -1)
        {
            Direction dir = Direction.None;
            float deltaX = collision.transform.position.x - transform.position.x;

            if (deltaX > 0) dir = Direction.Left;
            else if (deltaX < 0) dir = Direction.Right;

            health.TakeDamage(damage, dir);
        }
    }

    private void Update()
    {
        if (gameObject.tag == Tags.EnemyProjectile.ToString())
            return;

        if (GetComponentInParent<Animator>().GetBool("Damaged"))
        {
            col.enabled = false;
        }
        else
        {
            col.enabled = true;
        }
    }
}
