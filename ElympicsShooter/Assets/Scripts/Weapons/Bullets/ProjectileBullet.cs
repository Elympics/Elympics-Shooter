using Elympics;
using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ProjectileBullet : ElympicsMonoBehaviour, IUpdatable
{
	[SerializeField] protected float speed = 5.0f;
	[SerializeField] protected float timeToSelfDestroy = 5.0f;

	[SerializeField] private ExplosionArea explosionAreaPrefab = null;

	public float TimeToSelfDestroy => timeToSelfDestroy;

	protected new Rigidbody rigidbody = null;
	protected new Collider collider = null;

	protected ElympicsBool readyToLaunchExplosion = new ElympicsBool(false);
	protected ElympicsBool markedAsReadyToDestroy = new ElympicsBool(false);

	private GameObject owner = null;
	private Coroutine deathTimerCoroutine = null;

	private void Awake()
	{
		rigidbody = GetComponent<Rigidbody>();
		collider = GetComponent<Collider>();
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
		deathTimerCoroutine = null;
		markedAsReadyToDestroy.Value = true;
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
		readyToLaunchExplosion.Value = true;
	}

	public void ElympicsUpdate()
	{
		if (readyToLaunchExplosion)
			LaunchExplosion();
		if (markedAsReadyToDestroy)
			ElympicsDestroy(this.gameObject);
	}

	private void LaunchExplosion()
	{
		var explosionArea = ElympicsInstantiate(explosionAreaPrefab.gameObject.name, ElympicsPlayer.All);
		explosionArea.transform.position = this.transform.position;
		explosionArea.GetComponent<ExplosionArea>().Detonate();

		readyToLaunchExplosion.Value = false;
	}
}