using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : StateMachineBehaviour
{
    public bool resetter;
    private int loopcount = 3;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        int loop = animator.GetInteger("LoopCount");
        if (loop <= 0)
        {
            animator.SetInteger("IdleIndex", Random.Range(0, 10));
            if (resetter)
            {
                animator.SetInteger("LoopCount", loopcount);
                animator.SetInteger("IdleIndex", 0);
            }
            return;
        }
        loop--;
        animator.SetInteger("LoopCount", loop);
    }
}
