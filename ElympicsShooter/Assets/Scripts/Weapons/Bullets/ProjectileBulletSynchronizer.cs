using Elympics;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBulletSynchronizer : ElympicsMonoBehaviour, IInitializable
{
	[Header("References:")]
	[SerializeField] private new Rigidbody rigidbody = null;
	[SerializeField] private new Collider collider = null;

	//public ElympicsBool RigidbodyUseGravity = new ElympicsBool(false);
	public ElympicsBool RigidbodyIsKinematic = new ElympicsBool(false);
	public ElympicsBool ColliderEnabled = new ElympicsBool(false);

	public void Initialize()
	{
		RigidbodyIsKinematic.Value = rigidbody.isKinematic;
		RigidbodyIsKinematic.ValueChanged += UpdateRigidbodyIsKinematic;

		ColliderEnabled.Value = collider.enabled;
		ColliderEnabled.ValueChanged += UpdateColliderEnabled;
	}

	private void UpdateColliderEnabled(bool lastValue, bool newValue)
	{
		collider.enabled = newValue;
	}

	private void UpdateRigidbodyIsKinematic(bool lastValue, bool newValue)
	{
		rigidbody.isKinematic = newValue;
	}
}
