using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerThirdPersonAnimatorMovementController : MonoBehaviour
{
	[SerializeField] private MovementController playerMovementController;

	private readonly int movementForwardParameterHash = Animator.StringToHash("MovementForward");
	private readonly int movementRightParameterHash = Animator.StringToHash("MovementRight");

	private Animator thirdPersonAnimator = null;

	private void Awake()
	{
		thirdPersonAnimator = GetComponent<Animator>();

		playerMovementController.MovementValuesChanged += ProcessMovementValues;
	}

	private void ProcessMovementValues(Vector3 movementDirection)
	{
		var localMovementDirection = playerMovementController.transform.InverseTransformDirection(movementDirection) * 2.0f;

		thirdPersonAnimator.SetFloat(movementForwardParameterHash, localMovementDirection.z);
		thirdPersonAnimator.SetFloat(movementRightParameterHash, localMovementDirection.x);
	}
}
