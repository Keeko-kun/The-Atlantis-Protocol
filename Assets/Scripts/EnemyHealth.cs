using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour {

    public Enemy baseEnemy;
    public float iFrames;
    public bool takeKnockback;
    public Vector2 knockback;
    public float fadeSpeed;
    public GameObject hurtbox;

    private bool invincible;
    private float currentHealth;
    private Animator anim;
    private Rigidbody2D rb2d;
    private SpriteRenderer sr;
    private Direction dir;

    void OnEnable () {
        currentHealth = baseEnemy.maxHealth;
        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
	}

    public void TakeDamage(float damage, Direction dir)
    {
        if (invincible)
            return;

        this.dir = dir;

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            StartCoroutine(KillEnemy());
        }
        else
        {
            StartCoroutine(DoIFrames());
        }
    }

    private IEnumerator KillEnemy()
    {
        invincible = true;
        anim.SetBool("Damaged", invincible);
        hurtbox.SetActive(false);

        float alpha = 1;

        if (takeKnockback)
        {
            rb2d.velocity = new Vector2(knockback.x * (int)dir, knockback.y);
        }

        bool hasSpawnedCoins = false;

        while (sr.color.a > 0)
        {
            alpha -= fadeSpeed * Time.deltaTime;
            alpha = Mathf.Clamp01(alpha);

            sr.color = new Color(sr.color.r, sr.color.b, sr.color.g, alpha);

            if (sr.color.a <= 0.35f && hasSpawnedCoins == false)
            {
                hasSpawnedCoins = true;
                int coins = Random.Range(baseEnemy.goldDropMin, baseEnemy.goldDropMax);

                yield return StartCoroutine(transform.parent.GetComponent<EnemySpawner>().SpawnCoins(transform, coins));
            }

            yield return null;
        }

        Destroy(gameObject);
    }

    private void Update()
    {
        anim.SetBool("Damaged", invincible);
    }

    private IEnumerator DoIFrames()
    {
        invincible = true;

        float timePassed = 0;

        if (takeKnockback)
        {
            rb2d.velocity = new Vector2(knockback.x * (int)dir, knockback.y);
        }

        while (timePassed <= iFrames)
        {
            timePassed += Time.deltaTime;
            
            yield return null;
        }

        invincible = false;

    }

}
