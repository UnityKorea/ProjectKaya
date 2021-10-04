using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.InputSystem;
using UTK.Animation;

[RequireComponent(typeof(Animator))]
//[RequireComponent(typeof(Rigidbody))]
public class PlayerAttackControll : MonoBehaviour
{
    #region Public Serialize
    [Header("Effect")] 
    [SerializeField] private VisualEffect vfxSkill1;
    [SerializeField] private VisualEffect vfxSkill2;
    [SerializeField] private VisualEffect vfxSkill3;
    [SerializeField] private VisualEffect vfxSkill4;
    #endregion

    #region Privates
    // private Rigidbody _rigidbody;
    private Animator _animator;
    #endregion

    #region AnimationIDs

    #endregion
    
    private void Start()
    {
        _animator = GetComponent<Animator>();
    }
    
    private void OnAttack(InputValue value)
    {
        if (_animator)
        {
            _animator.SetTrigger(AnimatorParameterIDs.Attack);
            _animator.SetInteger(AnimatorParameterIDs.AttackStack,_animator.GetInteger(AnimatorParameterIDs.AttackStack)+1);
        }
    }

    private void OnSkill1()
    {
        if(_animator)
            _animator.SetTrigger(AnimatorParameterIDs.Skill1);
        
        if (vfxSkill1 == null) return;
        vfxSkill1.gameObject.SetActive(true);
        vfxSkill1.Play();
    }
    
    private void OnSkill2()
    {
        if(_animator)
            _animator.SetTrigger(AnimatorParameterIDs.Skill2);
        
        if (vfxSkill2 == null) return;
        vfxSkill2.gameObject.SetActive(true);
        vfxSkill2.Play();
    }
    
    private void OnSkill3()
    {
        if(_animator)
            _animator.SetTrigger(AnimatorParameterIDs.Skill3);
        
        if (vfxSkill3 == null) return;
        vfxSkill3.gameObject.SetActive(true);
        vfxSkill3.Play();
    }

    private void OnSkill4()
    {
        if(_animator)
            _animator.SetTrigger(AnimatorParameterIDs.Skill4);
        
        if (vfxSkill4 == null) return;
        vfxSkill4.gameObject.SetActive(true);
        vfxSkill4.Play();
    }
}
