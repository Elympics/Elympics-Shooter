using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;

public class RocketLauncher : Weapon
{
	[SerializeField] private Transform bulletSpawnPoint = null;
	[SerializeField] private TestRocketBullet bulletPrefab = null;

	protected override void ProcessBulletSpawn()
	{
		var createdBullet = ElympicsInstantiate(bulletPrefab.gameObject.name, ElympicsPlayer.World);
		if (createdBullet.TryGetComponent(out TestRocketBullet testRocketBullet))
		{
			testRocketBullet.Initialize();
		}
	}
}
