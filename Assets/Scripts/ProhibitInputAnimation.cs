using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProhibitInputAnimation : StateMachineBehaviour
{

    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        animator.GetComponent<Rigidbody2D>().velocity = new Vector2(0, animator.GetComponent<Rigidbody2D>().velocity.y); 
        animator.GetComponent<PreventInput>().InputProhibited = true;
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        animator.GetComponent<PreventInput>().InputProhibited = false;
    }
}