using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;

public class RocketLauncher : Weapon
{
	[SerializeField] private Transform bulletSpawnPoint = null;
	[SerializeField] private ProjectileBullet bulletPrefab = null;
	[SerializeField] private WeaponBulletPooler weaponBulletPooler = null;

	public ProjectileBullet BulletPrefab => bulletPrefab;

	protected override void ProcessBulletSpawn()
	{
		var bullet = weaponBulletPooler.GetBullet();

		bullet.transform.position = bulletSpawnPoint.position;

		bullet.Launch(bulletSpawnPoint.transform.forward);
	}
}
