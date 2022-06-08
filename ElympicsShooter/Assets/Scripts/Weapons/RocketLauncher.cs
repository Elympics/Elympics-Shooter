using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;

public class RocketLauncher : Weapon
{
	[SerializeField] private Transform bulletSpawnPoint = null;
	[SerializeField] private ProjectileBullet bulletPrefab = null;
	[SerializeField] private Vector3 offscreenPrespawnPosition = Vector3.one * 9999.9f;

	public ProjectileBullet BulletPrefab => bulletPrefab;

	private ElympicsBool launchBullet = new ElympicsBool(false);

	private ElympicsGameObject prespawnedBullet = new ElympicsGameObject();

	protected override void ProcessBulletSpawn()
	{
		launchBullet.Value = true;
	}

	public override void ElympicsUpdate()
	{
		base.ElympicsUpdate();

		//Prespawning bullet for avoiding bullet teleportation (missing tick when it's spawned and launched in the same frame)
		if (prespawnedBullet.Value == null && Elympics.IsServer)
		{
			PrespawnBullet();
		}

		if (launchBullet)
		{
			prespawnedBullet.Value.transform.position = bulletSpawnPoint.position;
			prespawnedBullet.Value.GetComponent<ProjectileBullet>().Launch(bulletSpawnPoint.transform.forward);

			launchBullet.Value = false;
			prespawnedBullet.Value = null;

			if (Elympics.IsServer)
				PrespawnBullet();
		}
	}

	private void PrespawnBullet()
	{
		var bullet = ElympicsInstantiate(bulletPrefab.gameObject.name, ElympicsPlayer.FromIndex(Owner.GetComponent<PlayerData>().PlayerId));
		bullet.GetComponent<ProjectileBullet>().SetOwner(Owner.gameObject.transform.root.gameObject.GetComponent<ElympicsBehaviour>());
		bullet.transform.position = offscreenPrespawnPosition;
		prespawnedBullet.Value = bullet.GetComponent<ElympicsBehaviour>();
	}
}
