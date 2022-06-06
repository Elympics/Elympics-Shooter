using Elympics;
using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ProjectileBullet : ElympicsMonoBehaviour, IPoolerObject
{
	[SerializeField] protected float damage = 10.0f;
	[SerializeField] protected float speed = 5.0f;
	[SerializeField] protected float timeToSelfDestroy = 5.0f;

	public float TimeToSelfDestroy => timeToSelfDestroy;

	protected new Rigidbody rigidbody = null;
	protected new Collider collider = null;

	private WeaponBulletPooler assignedPooler = null;
	private Coroutine deathTimerCoroutine = null;

	private void Awake()
	{
		rigidbody = GetComponent<Rigidbody>();
		collider = GetComponent<Collider>();
	}

	public void SetPooler(WeaponBulletPooler assignedPooler)
	{
		this.assignedPooler = assignedPooler;
	}

	public void Launch(Vector3 direction)
	{
		ChangeBulletVelocity(direction);

		if (Elympics.IsServer)
			deathTimerCoroutine = StartCoroutine(DeathTimer());
	}

	private void ChangeBulletVelocity(Vector3 direction)
	{
		rigidbody.velocity = direction * speed;
	}

	private IEnumerator DeathTimer()
	{
		yield return new WaitForSeconds(timeToSelfDestroy);

		assignedPooler.ReturnBullet(this);
		deathTimerCoroutine = null;
	}

	public virtual void OnTaken()
	{
		rigidbody.isKinematic = false;
		rigidbody.useGravity = true;

		if (collider)
			collider.enabled = true;
	}

	public virtual void OnReturned()
	{
		rigidbody.isKinematic = true;
		rigidbody.useGravity = false;

		if (collider)
			collider.enabled = false;
	}
}