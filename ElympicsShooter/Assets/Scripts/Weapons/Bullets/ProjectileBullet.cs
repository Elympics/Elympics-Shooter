using Elympics;
using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ProjectileBullet : ElympicsMonoBehaviour, IUpdatable, IInitializable
{
	[Header("Parameters:")]
	[SerializeField] protected float speed = 5.0f;
	[SerializeField] protected float lifeTime = 5.0f;
	[SerializeField] protected float timeToDestroyOnExplosion = 1.0f;

	[Header("References:")]
	[SerializeField] private ExplosionArea explosionArea = null;
	[SerializeField] private GameObject bulletMeshRoot = null;
	[SerializeField] protected new Rigidbody rigidbody = null;
	[SerializeField] protected new Collider collider = null;

	public float LifeTime => lifeTime;

	protected ElympicsBool readyToLaunchExplosion = new ElympicsBool(false);
	protected ElympicsBool markedAsReadyToDestroy = new ElympicsBool(false);

	protected ElympicsBool rigidbodyIsKinematic = new ElympicsBool(true);
	protected ElympicsBool colliderEnabled = new ElympicsBool(false);
	protected ElympicsBool bulletExploded = new ElympicsBool(false);

	private ElympicsGameObject owner = new ElympicsGameObject();
	private ElympicsFloat deathTimer = new ElympicsFloat(0.0f);

	public void Initialize()
	{
		rigidbodyIsKinematic.ValueChanged += UpdateRigidbodyIsKinematic;
		colliderEnabled.ValueChanged += UpdateColliderEnabled;

		markedAsReadyToDestroy.ValueChanged += DestroyProjectile;
		readyToLaunchExplosion.ValueChanged += DetonateProjectile;
	}

	public void SetApplyingDamageCallback(Action weaponAppliedDamage)
	{
		explosionArea.SetApplyingDamageCallback(weaponAppliedDamage);
	}

	private void UpdateColliderEnabled(bool lastValue, bool newValue)
	{
		collider.enabled = newValue;
	}

	private void UpdateRigidbodyIsKinematic(bool lastValue, bool newValue)
	{
		rigidbody.isKinematic = newValue;
	}

	public void SetOwner(ElympicsBehaviour owner)
	{
		this.owner.Value = owner;
	}

	public void Launch(Vector3 direction)
	{
		rigidbody.useGravity = true;
		rigidbodyIsKinematic.Value = false;
		colliderEnabled.Value = true;

		ChangeBulletVelocity(direction);
	}

	private void ChangeBulletVelocity(Vector3 direction)
	{
		rigidbody.velocity = direction * speed;
	}

	private void DestroyProjectile(bool lastValue, bool newValue)
	{
		if (newValue)
			ElympicsDestroy(this.gameObject);
	}

	private void OnCollisionEnter(Collision collision)
	{
		//TODO: Dirty fix when object is destroyed
		if (owner.Value == null)
			return;

		if (collision.transform.root.gameObject == owner.Value.gameObject)
			return;

		readyToLaunchExplosion.Value = true;
	}

	private void DetonateProjectile(bool lastValue, bool newValue)
	{
		if (newValue)
			LaunchExplosion();
	}

	public void ElympicsUpdate()
	{
		deathTimer.Value += Elympics.TickDuration;

		if ((!bulletExploded && deathTimer >= lifeTime)
			|| (bulletExploded && deathTimer >= timeToDestroyOnExplosion))
		{
			markedAsReadyToDestroy.Value = true;
		}
	}

	private void LaunchExplosion()
	{
		bulletMeshRoot.SetActive(false);
		rigidbodyIsKinematic.Value = true;
		rigidbody.useGravity = false;
		colliderEnabled.Value = false;

		explosionArea.Detonate();

		bulletExploded.Value = true;
		deathTimer.Value = 0.0f;
	}
}