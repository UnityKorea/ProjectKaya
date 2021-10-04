using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UTK.Animation;

public class AttackIdleState : StateMachineBehaviour
{
    const float INFINITE_WAIT_TIME = -1f;
    [Tooltip("attack idle 에서 idle로 전환되는 시간(sec)을 입력합니다. 기본값은 3초입니다. -1을 입력하면 attackidle 상태로 무한 대기하게 됩니다.")]
    public float waitAttackEndTime = 3f;
    float currentWaitTime = 0f;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        int currentStack = animator.GetInteger(AnimatorParameterIDs.AttackStack);
        if (currentStack != 0)
        {
            // current_stack이 0이 아니면 공격 모션에도 돌아온 상태로 간주합니다. 예외가 발생하게 되면 그에 따로 수정이 필요합니다.
            currentWaitTime = 0f;

            // 연타하다 보면 입력을 중지했음에도 마지막에 한 타가 더 나가는 문제로 attack 트리거를 리셋하는 코드를 추가하였습니다.
            animator.ResetTrigger(AnimatorParameterIDs.Attack);
            // 콤비네이션 스택을 초기화합니다.
            animator.SetInteger(AnimatorParameterIDs.AttackStack, 0);
        }

        base.OnStateEnter(animator, stateInfo, layerIndex);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (waitAttackEndTime != INFINITE_WAIT_TIME)
        {
            currentWaitTime += Time.deltaTime;
            if (currentWaitTime >= waitAttackEndTime)
            {
                // 일정 시간(waitAttackEndTime) 대기 후 어택 아이들에서 일반 아이들로 넘어간다.
                animator.ResetTrigger(AnimatorParameterIDs.AttackIdle);
                animator.SetTrigger(AnimatorParameterIDs.Idle);

                currentWaitTime = 0f;
            }
        }

        base.OnStateUpdate(animator, stateInfo, layerIndex);
    }
}
