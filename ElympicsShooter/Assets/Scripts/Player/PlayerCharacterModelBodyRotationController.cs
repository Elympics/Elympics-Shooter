using Elympics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterModelBodyRotationController : ElympicsMonoBehaviour
{
	[Header("References:")]
	[SerializeField] private Transform rotatingBone = null;
	[SerializeField] private Transform lookAtTarget = null;

	public void LateUpdate()
	{
		rotatingBone.transform.LookAt(lookAtTarget);
	}
}
