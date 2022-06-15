using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;

public class RocketLauncher : Weapon
{
	[SerializeField] private Transform bulletSpawnPoint = null;
	[SerializeField] private ProjectileBullet bulletPrefab = null;

	[Header("Rocke launcher visual references:")]
	[SerializeField] private GameObject bulletLoadedPreview = null;
	[SerializeField] private ParticleSystem showExplosionPS = null;

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
			bullet.transform.rotation = bulletSpawnPoint.transform.rotation;
			bullet.GetComponent<ProjectileBullet>().Launch(bulletSpawnPoint.transform.forward);

			launchBullet = false;

			showExplosionPS?.Play();
		}
	}

	private void Update()
	{
		bulletLoadedPreview.SetActive(IsReady);
	}

	private GameObject CreateBullet()
	{
		var bullet = ElympicsInstantiate(bulletPrefab.gameObject.name, ElympicsPlayer.FromIndex(Owner.GetComponent<PlayerData>().PlayerId));
		bullet.GetComponent<ProjectileBullet>().SetOwner(Owner.gameObject.transform.root.gameObject.GetComponent<ElympicsBehaviour>());

		return bullet;
	}
}
