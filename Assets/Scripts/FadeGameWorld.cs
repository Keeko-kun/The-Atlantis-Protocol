using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FadeGameWorld : MonoBehaviour {

    [Range(0, 5)]
    public float speed;
    public bool visible;
    public List<SpriteRenderer> spriteRenderers;
    public List<TextMeshPro> textMeshes;

    private float alpha = 0;
    private float fadeDir;
    private PreventInput player;

    void OnEnable()
    {
        if (visible)
        {
            alpha = 1;
        }

    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == Tags.Player.ToString())
        {
            player = col.GetComponent<PreventInput>();
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == Tags.Player.ToString())
        {
            player = null;
            visible = false;
        }
    }

    void Update()
    {
        if (player)
        {
            if (player.InputProhibited)
                visible = false;
            else
                visible = true;
        }

        if (visible)
        {
            fadeDir = 1;
        }
        else
        {
            fadeDir = -1;
        }

        if (alpha >= 0 || alpha <= 1)
            Fade();
    }

    private void Fade()
    {
        alpha += fadeDir * speed * Time.deltaTime;
        alpha = Mathf.Clamp01(alpha);

        foreach (SpriteRenderer sr in spriteRenderers)
        {
            sr.color = new Color(sr.color.r, sr.color.b, sr.color.g, alpha);
        }

        foreach (TextMeshPro tmp in textMeshes)
        {
            tmp.color = new Color(tmp.color.r, tmp.color.b, tmp.color.g, alpha);
        }
    }
}
