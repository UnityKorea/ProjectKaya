using UnityEngine;
using UnityEngine.InputSystem;
using UTK.Animation;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerJumpControll))]

public class PlayerController : MonoBehaviour
{
	[Header("Player")]
	[Tooltip("Move speed of the character in m/s")]
	public float MoveSpeed = 2.0f;
	[Tooltip("Sprint speed of the character in m/s")]
	public float SprintSpeed = 5.335f;
	[Tooltip("How fast the character turns to face movement direction")]
	[Range(0.0f, 0.3f)]
	public float RotationSmoothTime = 0.12f;
	[Tooltip("Acceleration and deceleration")]
	public float SpeedChangeRate = 10.0f;
	
	public float CurrentSpeed;

	public bool AlwaysRun;
	
    #region Privates
    private Rigidbody _rigidbody;
    private Animator _animator;
    private Camera _camera;
    private CharacterController _controller;

    
    private float _animationBlend;
    private float _targetRotation = 0.0f;
    private float _rotationVelocity;


    private Vector2 _move;

    private PlayerJumpControll _jumpControll;
    #endregion

    private bool _sprint;
    
    private void Start()
    {
        _camera = Camera.main;
        
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        _controller = GetComponent<CharacterController>();
        _jumpControll = GetComponent<PlayerJumpControll>();
    }

    private void Update()
    {
	    Move();
    }


    private void Move()
		{
			// set target speed based on move speed, sprint speed and if sprint is pressed
			var targetSpeed = (_sprint || AlwaysRun) ? SprintSpeed : MoveSpeed;

			// a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

			// note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
			// if there is no input, set the target speed to 0
			if (_move == Vector2.zero) targetSpeed = 0.0f;

			// a reference to the players current horizontal velocity
			float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;
			float speedOffset = 0.1f;

			// accelerate or decelerate to target speed
			if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
			{
				// creates curved result rather than a linear one giving a more organic speed change
				// note T in Lerp is clamped, so we don't need to clamp our speed
				CurrentSpeed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed, Time.deltaTime * SpeedChangeRate);

				// round speed to 3 decimal places
				CurrentSpeed = Mathf.Round(CurrentSpeed * 1000f) / 1000f;
			}
			else
			{
				CurrentSpeed = targetSpeed;
			}
			_animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);

			// normalise input direction
			Vector3 inputDirection = new Vector3(_move.x, 0.0f, _move.y).normalized;

			// note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
			// if there is a move input rotate player when the player is moving
			if (_move != Vector2.zero)
			{
				_targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + _camera.transform.eulerAngles.y;
				float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity, RotationSmoothTime);

				// rotate to face input direction relative to camera position
				transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
			}


			Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;
			// move the player
			_controller.Move(targetDirection.normalized * (CurrentSpeed * Time.deltaTime) + new Vector3(0.0f, _jumpControll.VerticalVelocity, 0.0f) * Time.deltaTime);

			// update animator if using character
			if (_animator)
			{
				_animator.SetFloat(AnimatorParameterIDs.Speed, CurrentSpeed);
			}
		}
    private void OnMovement(InputValue value)
    {
        _move = value.Get<Vector2>();
    }

    private void OnSprint(InputValue value)
    {
        _sprint = value.isPressed;
    }

    private void Dash()
    {
	    Debug.Log("Dash!");
    }
}
