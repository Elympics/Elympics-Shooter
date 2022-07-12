using UnityEngine;
using Elympics;

public class RocketLauncher : Weapon
{
	[SerializeField] private Transform bulletSpawnPoint = null;
	[SerializeField] private ProjectileBullet bulletPrefab = null;

	[Header("Rocket launcher visual references:")]
	[SerializeField] private GameObject bulletLoadedPreview = null;
	[SerializeField] private ParticleSystem showExplosionPS = null;

	public ProjectileBullet BulletPrefab => bulletPrefab;

	protected override void ProcessWeaponAction()
	{
		var bullet = CreateBullet();

		bullet.transform.position = bulletSpawnPoint.position;
		bullet.transform.rotation = bulletSpawnPoint.transform.rotation;
		bullet.GetComponent<ProjectileBullet>().Launch(bulletSpawnPoint.transform.forward);

		showExplosionPS?.Play();

		WeaponShot?.Invoke();
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
