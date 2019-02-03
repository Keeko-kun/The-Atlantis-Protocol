using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHurtbox : MonoBehaviour {

    public CharacterHealth ch;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == Tags.EnemyHitbox.ToString() || collision.tag == Tags.EnemyHurtbox.ToString() || collision.tag == Tags.EnemyProjectile.ToString())
        {
            float damage = -1;

            if (collision.GetComponentInParent<EnemyHealth>())
            {
                if(collision.tag == Tags.EnemyHitbox.ToString())
                    damage = collision.GetComponentInParent<EnemyHealth>().baseEnemy.attackStrength;
                else if (collision.tag == Tags.EnemyHurtbox.ToString())
                    damage = collision.GetComponentInParent<EnemyHealth>().baseEnemy.contactDamage;
            }
            else if (collision.GetComponent<EnemyHurtbox>())
            {
                if (collision.tag == Tags.EnemyHurtbox.ToString())
                {
                    damage = collision.GetComponent<EnemyHurtbox>().health.baseEnemy.contactDamage;
                }
                else if (collision.tag == Tags.EnemyProjectile.ToString())
                {
                    damage = collision.GetComponent<EnemyHurtbox>().health.baseEnemy.attackStrength;
                    Destroy(collision.gameObject);
                }

            }


            if (damage != -1)
            {
                Direction dir = Direction.None;
                float deltaX = collision.transform.position.x - transform.position.x;

                if (deltaX > 0) dir = Direction.Left;
                else if (deltaX < 0) dir = Direction.Right;

                ch.TakeDamage(damage, dir);
            }
        }
        else if (collision.tag == Tags.Coin.ToString())
        {
            if (collision.GetComponent<CoinBehaviour>().Enabled)
            {
                collision.GetComponent<CoinBehaviour>().Enabled = false;
                Destroy(collision.gameObject);
                CharacterInventory ci = GetComponentInParent<CharacterInventory>();
                ci.GiveItem(ci.coin, 1);
            }
        }
    }
}
