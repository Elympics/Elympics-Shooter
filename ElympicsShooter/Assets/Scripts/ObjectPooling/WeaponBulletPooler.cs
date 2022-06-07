using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;
using UnityEditor;

public class WeaponBulletPooler : ElympicsMonoBehaviour, IInitializable, IUpdatable
{
	[SerializeField] private Vector3 defaultSpawnPosition = Vector3.zero;
	[SerializeField] private int numberOfNecessaryBullets = 0;

	private ElympicsArray<ElympicsGameObject> poolerBullets = null;
	private ElympicsBool createBulletsRPC = new ElympicsBool(false);
	private ElympicsInt currentAvailableBullet = new ElympicsInt(0);

	private ProjectileBullet bulletPrefab = null;
	private Weapon weapon = null;

	public void Initialize()
	{
		poolerBullets = new ElympicsArray<ElympicsGameObject>(numberOfNecessaryBullets, () => new ElympicsGameObject());

		createBulletsRPC.Value = true;
	}

	public void ElympicsUpdate()
	{
		if (createBulletsRPC == true)
		{
			CreateBullets();
		}
	}

	private void CreateBullets()
	{
		for (int i = 0; i < poolerBullets.Values.Count; i++)
		{
			var createdBullet = ElympicsInstantiate(bulletPrefab.gameObject.name, ElympicsPlayer.World);

			var projectileBullet = createdBullet.GetComponent<ProjectileBullet>();
			projectileBullet.SetPooler(this);
			projectileBullet.SetOwner(weapon.Owner);

			createdBullet.transform.position = defaultSpawnPosition;

			projectileBullet.Initialize();

			poolerBullets.Values[i].Value = createdBullet.gameObject.GetComponent<ElympicsBehaviour>();
		}

		createBulletsRPC.Value = false;
	}

	public ProjectileBullet GetBullet()
	{
		var bulletToTake = poolerBullets.Values[currentAvailableBullet].Value.GetComponent<ProjectileBullet>();
		bulletToTake.OnTaken();

		int newAvailableBulletIndex = currentAvailableBullet.Value + 1;

		if (newAvailableBulletIndex >= poolerBullets.Values.Count)
			newAvailableBulletIndex = 0;

		currentAvailableBullet.Value = newAvailableBulletIndex;

		return bulletToTake;
	}

	public void ReturnBullet(ProjectileBullet returnedBullet)
	{
		returnedBullet.OnReturned();
		returnedBullet.transform.position = defaultSpawnPosition;
	}

	#region Pooler Auto Management

	[ContextMenu("Get recommendation for safe number of bullets")]
	private void AutoFillNumberNecessaryBullets()
	{
		var rocketLauncher = GetComponent<RocketLauncher>();

		if (rocketLauncher.TimeBetweenShoots == 0)
			rocketLauncher.CalculateTimeBetweenShoots();

		numberOfNecessaryBullets = GetNumberOfNecessaryBullets(rocketLauncher.BulletPrefab.TimeToSelfDestroy, rocketLauncher.TimeBetweenShoots);
	}

	private int GetNumberOfNecessaryBullets(float timeToSelfDestroy, float timeBetweenShoots)
	{
		int spawnsInOneBulletLifetime = Mathf.CeilToInt(timeToSelfDestroy / timeBetweenShoots) + 1;

		return spawnsInOneBulletLifetime;
	}

	private void OnValidate()
	{
		var rocketLauncher = GetComponent<RocketLauncher>();

		this.weapon = rocketLauncher;
		this.bulletPrefab = rocketLauncher.BulletPrefab;
	}

	#endregion
}
