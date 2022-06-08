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

	private ElympicsGameObject owner = new ElympicsGameObject();
	private Coroutine lifetimeDeathTimerCoroutine = null;

	public void Initialize()
	{
		rigidbodyIsKinematic.ValueChanged += UpdateRigidbodyIsKinematic;
		colliderEnabled.ValueChanged += UpdateColliderEnabled;
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

		if (Elympics.IsServer)
			lifetimeDeathTimerCoroutine = StartCoroutine(SelfDestoryTimer(lifeTime));
	}

	private void ChangeBulletVelocity(Vector3 direction)
	{
		rigidbody.velocity = direction * speed;
	}

	private IEnumerator SelfDestoryTimer(float time)
	{
		yield return new WaitForSeconds(time);

		DestroyProjectile();
	}

	private void DestroyProjectile()
	{
		lifetimeDeathTimerCoroutine = null;
		markedAsReadyToDestroy.Value = true;
	}

	private void OnCollisionEnter(Collision collision)
	{
		//TODO: Dirty fix when object is destroyed
		if (owner.Value == null)
			return;

		if (collision.transform.root.gameObject == owner.Value.gameObject)
			return;

		if (lifetimeDeathTimerCoroutine != null)
			StopCoroutine(lifetimeDeathTimerCoroutine);

		Debug.Log("Yo, dude, Ive been launched coz of this dude: " + collision.gameObject.name);
		DetonateProjectile();

		StartCoroutine(SelfDestoryTimer(timeToDestroyOnExplosion));
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
		bulletMeshRoot.SetActive(false);
		rigidbodyIsKinematic.Value = true;
		rigidbody.useGravity = false;
		colliderEnabled.Value = false;

		explosionArea.Detonate();

		readyToLaunchExplosion.Value = false;
	}
}