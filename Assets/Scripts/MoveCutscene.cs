using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCutscene : MonoBehaviour {

    public Animator animator;
    [Range(0, 10)]
    public float speed;

    public IEnumerator Move(int distance)
    {
        Vector3 target = new Vector2(transform.position.x + distance * 0.5f, transform.position.y);

        animator.SetFloat("Speed", 1);

        while (transform.position != target)
        {
            animator.SetFloat("Speed", 1);
            transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
            yield return null;
        }

        animator.SetFloat("Speed", 0);

        yield return null;
    }
}
