using Elympics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionArea : ElympicsMonoBehaviour
{
	[Header("Parameters:")]
	[SerializeField] private float explosionDamage = 10.0f;
	[SerializeField] private float explosionRange = 2.0f;

	[Header("References:")]
	[SerializeField] private ParticleSystem explosionPS = null;

	public void Detonate()
	{
		Collider[] objectsInExplosionRange = Physics.OverlapSphere(this.transform.position, explosionRange);

		foreach (Collider objectInExplosionRange in objectsInExplosionRange)
		{
			Debug.Log(objectInExplosionRange.gameObject.name + " got hit!");
		}

		explosionPS.Play();
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(this.transform.position, explosionRange);
	}
}
