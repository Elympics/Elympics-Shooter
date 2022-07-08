using Elympics;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MovementController : ElympicsMonoBehaviour
{
	[Header("References:")]
	[SerializeField] private DeathController deathController = null;
	[SerializeField] private GameStateController gameStateController = null;

	[Header("Parameters:")]
	[SerializeField] private float movementSpeed = 0.0f;
	[SerializeField] private float acceleration = 0.0f;
	[SerializeField] private float jumpForce = 0.0f;

	private new Rigidbody rigidbody = null;

	private bool IsGrounded() => Physics.Raycast(transform.position + new Vector3(0, 0.05f, 0), Vector3.down, 0.1f);

	public event Action<Vector3> MovementValuesChanged;
	public event Action PlayerJumped;
	public event Action<bool> IsGroundedStateUpdate;

	private void Awake()
	{
		rigidbody = GetComponent<Rigidbody>();

		gameStateController.CurrentGameState.ValueChanged += ResetVelocityIfMatchEnded;
	}

	private void ResetVelocityIfMatchEnded(int lastGameState, int newGameState)
	{
		if ((GameState)newGameState == GameState.MatchEnded)
		{
			rigidbody.velocity = Vector3.zero;
		}
	}

	public void ProcessMovement(float forwardMovementValue, float rightMovementValue, bool jump)
	{
		if (deathController.IsDead)
			return;

		Vector3 inputVector = new Vector3(forwardMovementValue, 0, rightMovementValue);
		Vector3 movementDirection = inputVector != Vector3.zero ? this.transform.TransformDirection(inputVector.normalized) : Vector3.zero;

		ApplyMovement(movementDirection);

		var isGrounded = IsGrounded();
		IsGroundedStateUpdate.Invoke(isGrounded);

		if (jump && isGrounded)
			ApplyJump();
	}

	private void ApplyJump()
	{
		rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
		PlayerJumped?.Invoke();
	}

	private void ApplyMovement(Vector3 movementDirection)
	{
		Vector3 defaultVelocity = movementDirection * movementSpeed;
		Vector3 fixedVelocity = Vector3.MoveTowards(rigidbody.velocity, defaultVelocity, Elympics.TickDuration * acceleration);

		rigidbody.velocity = new Vector3(fixedVelocity.x, rigidbody.velocity.y, fixedVelocity.z);

		MovementValuesChanged?.Invoke(movementDirection);
	}
}
