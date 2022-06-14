using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;

public class RocketLauncher : Weapon
{
	[SerializeField] private Transform bulletSpawnPoint = null;
	[SerializeField] private ProjectileBullet bulletPrefab = null;

	public ProjectileBullet BulletPrefab => bulletPrefab;

	private bool launchBullet = false;

	protected override void ProcessWeaponAction()
	{
		launchBullet = true;
	}

	public override void ElympicsUpdate()
	{
		base.ElympicsUpdate();

		if (launchBullet)
		{
			var bullet = CreateBullet();

			bullet.transform.position = bulletSpawnPoint.position;
			bullet.GetComponent<ProjectileBullet>().Launch(bulletSpawnPoint.transform.forward);

			launchBullet = false;
		}
	}

	private GameObject CreateBullet()
	{
		var bullet = ElympicsInstantiate(bulletPrefab.gameObject.name, ElympicsPlayer.FromIndex(Owner.GetComponent<PlayerData>().PlayerId));
		bullet.GetComponent<ProjectileBullet>().SetOwner(Owner.gameObject.transform.root.gameObject.GetComponent<ElympicsBehaviour>());

		return bullet;
	}
}
