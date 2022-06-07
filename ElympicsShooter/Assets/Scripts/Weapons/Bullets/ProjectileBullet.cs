using Elympics;
using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ProjectileBullet : ElympicsMonoBehaviour, IPoolerObject
{
	[SerializeField] protected float speed = 5.0f;
	[SerializeField] protected float timeToSelfDestroy = 5.0f;

	[SerializeField] private ExplosionArea explosionAreaPrefab = null;

	public float TimeToSelfDestroy => timeToSelfDestroy;

	protected new Rigidbody rigidbody = null;
	protected new Collider collider = null;

	protected ElympicsGameObject explosionAreaInstance = new ElympicsGameObject(null);

	private WeaponBulletPooler assignedPooler = null;
	private GameObject owner = null;
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

	public void SetOwner(GameObject owner)
	{
		this.owner = owner;
	}

	public void Initialize()
	{
		var explosionAreaInstance = ElympicsInstantiate(explosionAreaPrefab.gameObject.name, ElympicsPlayer.All);

		explosionAreaInstance.transform.position = this.transform.position;
		this.explosionAreaInstance.Value = explosionAreaInstance.GetComponent<ElympicsBehaviour>();
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

		DestroyProjectile();
	}

	private void DestroyProjectile()
	{
		assignedPooler.ReturnBullet(this);
		deathTimerCoroutine = null;
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.transform.root.gameObject == owner)
			return;

		DetonateProjectile();
		DestroyProjectile();
	}

	private void DetonateProjectile()
	{
		explosionAreaInstance.Value.transform.position = this.transform.position;
		explosionAreaInstance.Value.GetComponent<ExplosionArea>().Detonate();
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