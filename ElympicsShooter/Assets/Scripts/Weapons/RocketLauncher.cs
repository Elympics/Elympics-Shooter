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
		//TODO: Use object pooling or execute it in Elympcis Update, until then:
		Debug.Log("Pew pew!");
		//var createdBullet = ElympicsInstantiate(bulletPrefab.gameObject.name, ElympicsPlayer.World);
		//if (createdBullet.TryGetComponent(out TestRocketBullet testRocketBullet))
		//{
		//	testRocketBullet.Initialize();
		//}
	}
}
