using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UTK.Animation;

[RequireComponent(typeof(Animator))]
// [RequireComponent(typeof(Rigidbody))]
// [RequireComponent(typeof(InputActions_Player))]
public class PlayerJumpControll : MonoBehaviour
{
    #region Public Serializable
    [Header("Player Grounded")]
    [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
    public bool grounded = true;
    [Tooltip("Useful for rough ground")]
    public float groundedOffset = -0.14f;
    [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
    public float groundedRadius = 0.28f;
    [Tooltip("What layers the character uses as ground")]
    public LayerMask groundLayers;
    [Space(10)]
    [Tooltip("The height the player can jump")]
    public float jumpHeight = 1.2f;
    [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
    public float gravity = -15.0f;
    [Space(10)]
    [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
    public float jumpTimeout = 0.50f;
    [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
    public float fallTimeout = 0.15f;
    #endregion

    #region Privates
    // timeout deltatime
    private float _jumpTimeoutDelta;
    private float _fallTimeoutDelta;
    private float _verticalVelocity;
    private const float TerminalVelocity = 53.0f;
    
    // private Rigidbody _rigidbody;
    private Animator _animator;
    // private PlayerInput _input;

    private bool _jump;
    #endregion
    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        JumpAndGravity();
        GroundedCheck();
    }

    private void GroundedCheck()
    {
        // set sphere position, with offset
        var spherePosition = transform.position - Vector3.down * groundedOffset;
        grounded = Physics.CheckSphere(spherePosition, groundedRadius, groundLayers, QueryTriggerInteraction.Ignore);
        
        // update animator if using character
        if(!_animator) return;
        _animator.SetBool(AnimatorParameterIDs.Grounded, grounded);
    }
    
    private void JumpAndGravity()
    {
        if (grounded)
        {
            // reset the fall timeout timer
            _fallTimeoutDelta = fallTimeout;

            // update animator if using character
            if (_animator)
            {
                _animator.SetBool(AnimatorParameterIDs.Jump, false);
                _animator.SetBool(AnimatorParameterIDs.FreeFall, false);
            }

            // stop our velocity dropping infinitely when grounded
            if (_verticalVelocity < 0.0f)
            {
                _verticalVelocity = -2f;
            }

            // Jump
            if (_jump && _jumpTimeoutDelta <= 0.0f)
            {
                // the square root of H * -2 * G = how much velocity needed to reach desired height
                _verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
                // update animator if using character
                _animator.SetBool(AnimatorParameterIDs.Jump, true);
            }

            // jump timeout
            if (_jumpTimeoutDelta >= 0.0f)
            {
                _jumpTimeoutDelta -= Time.deltaTime;
            }
        }
        else
        {
            // reset the jump timeout timer
            _jumpTimeoutDelta = jumpTimeout;

            // fall timeout
            if (_fallTimeoutDelta >= 0.0f)
            {
                _fallTimeoutDelta -= Time.deltaTime;
            }
            else
            {
                // update animator if using character
                if (_animator)
                {
                    _animator.SetBool(AnimatorParameterIDs.FreeFall, true);
                }
            }

            // if we are not grounded, do not jump
            _jump = false;
        }

        // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
        if (_verticalVelocity < TerminalVelocity)
        {
            _verticalVelocity += gravity * Time.deltaTime;
        }
    }

    public float VerticalVelocity    {get { return _verticalVelocity; }}


    private void OnJump(InputValue value)
    {
        Debug.Log("OnJump Pressed");
        _jump = value.isPressed;
    }
}
