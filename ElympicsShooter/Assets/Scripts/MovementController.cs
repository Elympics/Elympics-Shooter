using Elympics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MovementController : ElympicsMonoBehaviour
{
	[Header("Parameters:")]
	[SerializeField] private float movementSpeed = 0.0f;
	[SerializeField] private float acceleration = 0.0f;

	private new Rigidbody rigidbody = null;

	private void Awake()
	{
		rigidbody = GetComponent<Rigidbody>();
	}

	public void ProcessMovement(float forwardMovementValue, float rightMovementValue)
	{
		Vector3 movementDirection = new Vector3(forwardMovementValue, 0, rightMovementValue);

		ApplyMovement(movementDirection);
	}

	private void ApplyMovement(Vector3 movementDirection)
	{
		Vector3 defaultVelocity = movementDirection * movementSpeed;
		Vector3 fixedVelocity = Vector3.MoveTowards(rigidbody.velocity, defaultVelocity, Elympics.TickDuration * acceleration);

		rigidbody.velocity = new Vector3(fixedVelocity.x, rigidbody.velocity.y, fixedVelocity.z);

		Debug.Log(rigidbody.velocity);
	}
}
