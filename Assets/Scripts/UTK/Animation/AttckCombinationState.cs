using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UTK.Animation;


public class AttckCombinationState : StateMachineBehaviour
{
    // 콤보 입력 시간을 수동 컨트롤할 것인지 여부 [일단 보이지 않게 감춰 둡니다.]
    [HideInInspector]
    [Tooltip("콤보 입력 시간을 수동 컨트롤할 것인지 여부. True면 Wait Time For Input 값 만큼 다음 콤비네이션 어택을 위한 입력을 기다립니다. False인 경우에는 모션 다음 Transition 이동 시작 전까지만 입력을 기다립니다.")]
    public bool isManuallyInputTimeControl = true;
    [HideIf("isMotionSync", true)]
    [Tooltip("입력된 시간 (second) 동안만 다음 콤보 입력을 기다립니다.")]
    public float waitTimeForInput = 0.5f;
    [HideIf("isMotionSync", true)]
    [Tooltip("자연 스러운 모션 연결을 현재 모션이 유지되는 최소한의 시간 (second). normalized 된 시간으로 이전에 다음 콤비네이션 어택이 입력이 되도 최소한 입력된 시간 이후에 이동된다.")]
    public float exitTime = 0.75f;

    float absolutedExitTime = 0f;
    float currentWaitTime = 0f;
    int currentAttackStack = 0;

    enum UpdateMode
    {
        Ready,
        Update,
        Finish,
    }
    UpdateMode updateMode = UpdateMode.Ready;

    const int INVALID_ATTACK_STACK = -1;
    static int ANIMATION_ATTACK_IDLE_ID = Animator.StringToHash("UTK_TemplateCharacter01_attackidle");


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!isManuallyInputTimeControl)
        {
            // 모션 싱크시에는 바로 어택 스택을 증가시킨다.
            int attackStack = animator.GetInteger(AnimatorParameterIDs.AttackStack);
            animator.SetInteger(AnimatorParameterIDs.AttackStack, attackStack + 1);
        }
        else
        {
            currentAttackStack = animator.GetInteger(AnimatorParameterIDs.AttackStack);
            animator.SetInteger(AnimatorParameterIDs.AttackStack, INVALID_ATTACK_STACK);
            currentWaitTime = 0f;
            absolutedExitTime = exitTime * stateInfo.length;
            updateMode = UpdateMode.Ready;
        }
        // animator.ResetTrigger(AnimatorParameterIDs.AttackStack);

        base.OnStateEnter(animator, stateInfo, layerIndex);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!isManuallyInputTimeControl)
            return;

        if (animator.IsInTransition(layerIndex))
        {
            var nextStateInfo = animator.GetNextAnimatorStateInfo(layerIndex);
            if (nextStateInfo.shortNameHash == ANIMATION_ATTACK_IDLE_ID)
            {
                updateMode = UpdateMode.Finish;
                return;
            }
        }

        currentWaitTime += Time.deltaTime;

        switch (updateMode)
        {
            case UpdateMode.Ready:
                {
                    if (currentWaitTime >= absolutedExitTime)
                    {
                        // 매뉴얼하게 입력 제한시간을 입력하는 경우에는 exitTime 이후에 어택 스택을 증가시킨다.
                        animator.SetInteger(AnimatorParameterIDs.AttackStack, currentAttackStack + 1);
                        updateMode = UpdateMode.Update;
                    }
                }
                break;

            case UpdateMode.Update:
                {
                    if (currentWaitTime > waitTimeForInput)
                    {
                        animator.SetInteger(AnimatorParameterIDs.AttackStack, INVALID_ATTACK_STACK);
                        updateMode = UpdateMode.Finish;
                    }
                }
                break;

            case UpdateMode.Finish:
            default:
                break;
        }
    }
}
