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
		DetectTargetsInExplosionRange();

		explosionPS.Play();
	}

	private void DetectTargetsInExplosionRange()
	{
		Collider[] objectsInExplosionRange = Physics.OverlapSphere(this.transform.position, explosionRange);

		foreach (Collider objectInExplosionRange in objectsInExplosionRange)
		{
			TryToApplyDamageToTarget(objectInExplosionRange.transform.root.gameObject);
		}
	}

	private void TryToApplyDamageToTarget(GameObject objectInExplosionRange)
	{
		if (objectInExplosionRange.TryGetComponent<StatsController>(out StatsController targetStatsController))
		{
			//TODO: Add damage modification depending on distance from explosion center
			targetStatsController.ChangeHealth(-explosionDamage);
		}
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(this.transform.position, explosionRange);
	}
}
