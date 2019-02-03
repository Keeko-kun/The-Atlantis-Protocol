using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinBehaviour : MonoBehaviour
{

    [Range(0, 10)]
    public float maxVelocity;
    public float lifeTime;
    public float flickerSpeed;

    private Rigidbody2D rb2d;
    private SpriteRenderer sr;

    public bool Enabled { get; set; }

    void OnEnable()
    {
        Enabled = true;
        rb2d = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        StartCoroutine(SetVelocity());
    }

    private IEnumerator SetVelocity()
    {
        float x = Random.Range(-maxVelocity, maxVelocity);
        yield return null;
        float y = Random.Range(0.5f, maxVelocity);

        rb2d.velocity = new Vector2(x, y);

        StartCoroutine(DespawnCoins());
    }

    private IEnumerator DespawnCoins()
    {
        float flickerTime = 0;
        float totalTime = 0;
        float startFlicker = lifeTime - (lifeTime * 0.25f);

        while (totalTime < lifeTime)
        {          
            if (totalTime >= startFlicker)
            {
                if (flickerTime >= flickerSpeed)
                {
                    flickerTime = 0;
                    sr.enabled = !sr.enabled;
                }
                else
                {
                    flickerTime += Time.deltaTime;
                }
            }

            totalTime += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }

}
