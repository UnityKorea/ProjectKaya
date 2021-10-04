using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkDelayStateMachineBehaviour : StateMachineBehaviour
{
    private float time;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        time = Time.time;
        animator.SetBool("IsMoving", true);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Time.time - time > 3f)
        {
            animator.SetBool("IsMoving", false);
        }
    }
}
