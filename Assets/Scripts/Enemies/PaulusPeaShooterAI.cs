using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaulusPeaShooterAI : MonoBehaviour {

    [Header("General")]
    public bool flipped;
    public float shootingCooldownMin;
    public float shootingCooldownMax;
    public float shootTimeMin;
    public float shootTimeMax;

    [Header("Pea Settings")]
    public float peaTimer;
    public GameObject peaPrefab;
    public Transform peaOrigin;

    [Header("Ground Raycast")]
    public LayerMask layerMask;
    public Vector3 raycast;

    [Header("Jump Settings")]
    [Range(0.1f, 25f)]
    public float fallSpeed;
    [Range(0.1f, 25f)]
    public float jumpMultiplier;
    [Range(0.1f, 25f)]
    public float jumpVelocity;

    private SpriteRenderer sr;
    private Rigidbody2D rb2d;
    private Animator anim;
    private Direction facing;
    private bool preparingToShoot;
    private bool recentlyDamaged;

    void OnEnable () {
        sr = GetComponent<SpriteRenderer>();
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        TurnPaulus();
    }
	

	void Update () {
        if (rb2d.velocity.y < 0)
        {
            rb2d.velocity += Vector2.up * Physics2D.gravity.y * (fallSpeed - 1) * Time.deltaTime;

        }
        else if (rb2d.velocity.y > 0)
        {
            rb2d.velocity += Vector2.up * Physics2D.gravity.y * (jumpMultiplier - 1) * Time.deltaTime;
        }

        if (anim.GetBool("Damaged"))
        {
            StopAllCoroutines();
            preparingToShoot = false;
            anim.SetBool("Jump", false);
            anim.SetBool("Attacking", false);
            recentlyDamaged = true;
            return;
        }

        if (!preparingToShoot)
        {
            StartCoroutine(PrepareShoot());
        }

    }

    private IEnumerator PrepareShoot()
    {
        preparingToShoot = true;

        float time = 0;

        if (recentlyDamaged)
            time = 0;
        else
            time = Random.Range(shootingCooldownMin, shootingCooldownMax);

        yield return new WaitForSecondsRealtime(time);

        recentlyDamaged = false;

        StartCoroutine(JumpAndShoot());
    }

    private IEnumerator JumpAndShoot()
    {
        anim.SetBool("Jump", true);
        rb2d.velocity = Vector2.up * jumpVelocity;

        float time = Random.Range(shootTimeMin, shootTimeMax);
        yield return new WaitForSecondsRealtime(time);

        anim.SetBool("Jump", false);
        anim.SetBool("Attacking", true);

        yield return new WaitForSecondsRealtime(peaTimer);

        GameObject pea = Instantiate(peaPrefab, transform.parent);
        pea.transform.position = peaOrigin.position;
        pea.GetComponent<PaulusPeas>().Facing = facing;
        pea.GetComponent<EnemyHurtbox>().health = GetComponent<EnemyHealth>();

        bool grounded = false;

        while (grounded == false)
        {
            yield return null;

            Debug.DrawLine(new Vector2(transform.position.x - (raycast.z / 2f) + raycast.x * (int)facing, transform.position.y - raycast.y), new Vector2(transform.position.x + (raycast.z / 2f) + raycast.x * (int)facing, transform.position.y - raycast.y));
            RaycastHit2D groundLine = Physics2D.Linecast(new Vector2(transform.position.x - (raycast.z / 2f) + raycast.x * (int)facing,
                transform.position.y - raycast.y),
                new Vector2(transform.position.x + (raycast.z / 2f) + raycast.x * (int)facing,
                transform.position.y - raycast.y), layerMask);

            if (groundLine.collider != null)
            {
                grounded = true;
            }
        }

        anim.SetBool("Attacking", false);
        preparingToShoot = false;

    }

    private void TurnPaulus()
    {
        if (flipped)
        {
            sr.flipX = true;
            facing = Direction.Left;
        }
        else
        {
            sr.flipX = false;
            facing = Direction.Right;
        }

    }
}
