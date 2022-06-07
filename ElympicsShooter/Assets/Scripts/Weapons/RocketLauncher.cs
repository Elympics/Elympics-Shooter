using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;

public class RocketLauncher : Weapon
{
	[SerializeField] private Transform bulletSpawnPoint = null;
	[SerializeField] private ProjectileBullet bulletPrefab = null;

	public ProjectileBullet BulletPrefab => bulletPrefab;

	private ElympicsBool createBullet = new ElympicsBool(false);

	protected override void ProcessBulletSpawn()
	{
		createBullet.Value = true;
	}

	public override void ElympicsUpdate()
	{
		base.ElympicsUpdate();

		if (createBullet)
		{
			var bullet = ElympicsInstantiate(bulletPrefab.gameObject.name, ElympicsPlayer.All);
			bullet.GetComponent<ProjectileBullet>().SetOwner(Owner.gameObject.transform.root.gameObject);
			bullet.GetComponent<ProjectileBullet>().Launch(bulletSpawnPoint.transform.forward);
			bullet.transform.position = bulletSpawnPoint.position;

			createBullet.Value = false;
		}
	}
}
