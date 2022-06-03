using Elympics;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MovementController : ElympicsMonoBehaviour
{
	[Header("Parameters:")]
	[SerializeField] private float movementSpeed = 0.0f;
	[SerializeField] private float acceleration = 0.0f;
	[SerializeField] private float jumpForce = 0.0f;

	private new Rigidbody rigidbody = null;

	private bool IsGrounded => Physics.Raycast(transform.position + new Vector3(0, 0.05f, 0), Vector3.down, 0.1f);

	private void Awake()
	{
		rigidbody = GetComponent<Rigidbody>();
	}

	public void ProcessMovement(float forwardMovementValue, float rightMovementValue, bool jump)
	{
		Vector3 movementDirection = new Vector3(forwardMovementValue, 0, rightMovementValue);

		ApplyMovement(movementDirection);

		if (jump && IsGrounded)
			ApplyJump();
	}

	private void ApplyJump()
	{
		rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
	}

	private void ApplyMovement(Vector3 movementDirection)
	{
		Vector3 defaultVelocity = movementDirection * movementSpeed;
		Vector3 fixedVelocity = Vector3.MoveTowards(rigidbody.velocity, defaultVelocity, Elympics.TickDuration * acceleration);

		rigidbody.velocity = new Vector3(fixedVelocity.x, rigidbody.velocity.y, fixedVelocity.z);
	}
}