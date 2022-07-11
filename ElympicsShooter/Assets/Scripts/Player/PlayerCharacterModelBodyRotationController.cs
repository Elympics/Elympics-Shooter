using Elympics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterModelBodyRotationController : ElympicsMonoBehaviour
{
	[Header("References:")]
	[SerializeField] private Transform firstPersonViewContainer = null;
	[SerializeField] private Transform rotatingBone = null;
	[SerializeField] private Transform lookAtTarget = null;

	[Header("Parameters:")]
	[SerializeField] private Vector3 rotationCorrection = new Vector3();

	public void LateUpdate()
	{
		rotatingBone.transform.LookAt(lookAtTarget);
		rotatingBone.transform.rotation = Quaternion.Euler(rotatingBone.transform.rotation.eulerAngles + rotationCorrection);
	}
}
