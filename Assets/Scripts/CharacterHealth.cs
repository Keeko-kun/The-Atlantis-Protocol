using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHealth : MonoBehaviour
{

    [Header("Health")]
    public HealthBarUI ui;
    public Health health;

    [Header("Knockback")]
    public float iFrames;
    public Vector2 knockback;

    private bool invincible;
    private Direction dir;
    private Rigidbody2D rb2d;
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();

        if (DontDestroy.GetInstance().GetComponent<StoreSceneInfo>().Health != null)
            health = DontDestroy.GetInstance().GetComponent<StoreSceneInfo>().Health;

        UpdateUI();
        UpdateInstance();
    }

    private void UpdateUI()
    {
        ui.UpdateHealth(health.maxHealth, health.currentHealth);
    }

    private void UpdateInstance()
    {
        DontDestroy.GetInstance().GetComponent<StoreSceneInfo>().Health = health;
    }

    public void TakeDamage(float damage, Direction dir)
    {
        if (invincible)
            return;

        this.dir = dir;

        health.currentHealth -= damage;

        UpdateInstance();

        if (health.currentHealth <= 0)
        {
            //KillPlayer();
        }
        else
        {
            UpdateUI();
            StartCoroutine(DoIFrames());
        }
    }

    public void ForceStopIFrames()
    {
        StopAllCoroutines();
        GetComponent<PreventInput>().InputProhibited = true;
        invincible = false;
    }

    private IEnumerator DoIFrames()
    {
        invincible = true;
        float timePassed = 0;
        GetComponent<PreventInput>().InputProhibited = true;

        rb2d.velocity = new Vector2(knockback.x * (int)dir, knockback.y);

        while (timePassed <= iFrames)
        {
            timePassed += Time.deltaTime;

            yield return null;
        }

        invincible = false;
        GetComponent<PreventInput>().InputProhibited = false;

    }
}

[System.Serializable]
public class Health
{
    public float maxHealth;
    public float currentHealth;
}
