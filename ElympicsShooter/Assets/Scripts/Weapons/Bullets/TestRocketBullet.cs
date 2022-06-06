using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;

[RequireComponent(typeof(Rigidbody))]
public class TestRocketBullet : ElympicsMonoBehaviour
{
	[SerializeField] private float damage = 10.0f;
	[SerializeField] private float speed = 5.0f;
	[SerializeField] private float timeToSelfDestroy = 5.0f;

	public float TimeToSelfDestroy => timeToSelfDestroy;

	public void Initialize()
	{
		InitializeBulletSpeed();

		if (Elympics.IsServer)
			StartCoroutine(DeathTimer());
	}

	private IEnumerator DeathTimer()
	{
		yield return new WaitForSeconds(5.0f);

		ElympicsDestroy(this.gameObject);
	}

	private void InitializeBulletSpeed()
	{
		var rigidbody = GetComponent<Rigidbody>();
		rigidbody.velocity = this.transform.forward * speed;
	}
}
