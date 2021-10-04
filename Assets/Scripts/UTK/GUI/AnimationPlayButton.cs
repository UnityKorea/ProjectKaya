using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UTK.Animation;

public class AnimationPlayButton : MonoBehaviour
{
    public enum AnimationType
    {
        Idle,
        AttackIdle,
        AttackRun,
        AttackDash,
        Run,
        Walk,
        Jump,
        Knockback,
        Dodge,
        Hitted,
    }

    public AnimationType animationType;
    public Animator animator;
    
    // Start is called before the first frame update
    private void Start()
    {
        var button = GetComponent<Button>();
        if (null != button)
        {
            button.onClick.AddListener(() =>
            {
                StartCoroutine(AnimationPlayCo());        
            });
        }
    }

    protected virtual IEnumerator AnimationPlayCo()
    {
        if(animator == null) yield break;
        yield return null;
        switch (animationType)
        {
            case AnimationType.Idle:
                animator.SetTrigger("Idle");
                break;
            case AnimationType.AttackIdle:
                animator.SetTrigger("AttackidleDemo");
                break;
            case AnimationType.AttackRun:
                animator.SetTrigger("battlerun");
                break;
            case AnimationType.AttackDash:
                animator.SetTrigger("dash");
                break;
            case AnimationType.Run:
                animator.SetTrigger("run");
                break;
            case AnimationType.Walk:
                animator.SetTrigger("Walk");
                break;
            case AnimationType.Jump:
                animator.SetTrigger("Jump");
                break;
            case AnimationType.Knockback:
                animator.SetTrigger("Downstart");
                break;
            case AnimationType.Dodge:
                animator.SetTrigger("dodge");
                break;
            case AnimationType.Hitted:
                animator.SetTrigger("Damage");
                break;
            default: break;
        }
    }
}
