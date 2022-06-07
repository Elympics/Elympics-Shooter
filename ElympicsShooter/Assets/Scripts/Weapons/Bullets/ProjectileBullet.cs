using Elympics;
using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ProjectileBullet : ElympicsMonoBehaviour, IPoolerObject, IUpdatable
{
	[Header("Parameters:")]
	[SerializeField] protected float speed = 5.0f;
	[SerializeField] protected float timeToSelfDestroy = 5.0f;

	[Header("References:")]
	[SerializeField] private ExplosionArea explosionAreaPrefab = null;
	[SerializeField] private ProjectileBulletSynchronizer projectileBulletSynchronizer = null;

	public float TimeToSelfDestroy => timeToSelfDestroy;

	public ElympicsInt assignedPlayerId = new ElympicsInt(-1);

	protected new Rigidbody rigidbody = null;
	protected new Collider collider = null;

	protected ElympicsGameObject explosionAreaInstance = new ElympicsGameObject();
	private ElympicsBool DetonateBulletRPC = new ElympicsBool(false);

	private WeaponBulletPooler assignedPooler = null;
	private GameObject owner = null;
	private Coroutine deathTimerCoroutine = null;

	private bool explosionAreaCreated = false;

	private void Awake()
	{
		rigidbody = GetComponent<Rigidbody>();
		collider = GetComponent<Collider>();

		DetonateBulletRPC.ValueChanged += DetonateProjectile;
	}

	public void SetPooler(WeaponBulletPooler assignedPooler)
	{
		this.assignedPooler = assignedPooler;
	}

	public void SetOwner(GameObject owner)
	{
		this.owner = owner;
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

		DetonateBulletRPC.Value = true;
		DestroyProjectile();
	}

	private void DetonateProjectile(bool lastValue, bool newValue)
	{
		if (newValue)
		{
			explosionAreaInstance.Value.transform.position = this.transform.position;
			explosionAreaInstance.Value.GetComponent<ExplosionArea>().Detonate();

			DetonateBulletRPC.Value = false;
		}
	}

	public virtual void OnTaken()
	{
		projectileBulletSynchronizer.RigidbodyIsKinematic.Value = false;
		rigidbody.useGravity = true;

		projectileBulletSynchronizer.ColliderEnabled.Value = true;
	}

	public virtual void OnReturned()
	{
		projectileBulletSynchronizer.RigidbodyIsKinematic.Value = true;
		rigidbody.useGravity = false;

		projectileBulletSynchronizer.ColliderEnabled.Value = false;
	}

	public void ElympicsUpdate()
	{
		if (Elympics.IsServer && assignedPlayerId.Value != -1 && !explosionAreaCreated)
		{
			var explosionAreaInstance = ElympicsInstantiate(explosionAreaPrefab.gameObject.name, ElympicsPlayer.FromIndex(assignedPlayerId.Value));

			explosionAreaInstance.transform.position = this.transform.position;
			this.explosionAreaInstance.Value = explosionAreaInstance.GetComponent<ElympicsBehaviour>();

			explosionAreaCreated = true;
		}
	}
}