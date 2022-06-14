using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;
using System;

public class RailGun : Weapon
{
	[Header("Parameters:")]
	[SerializeField] private float loadingTime = 1.0f;

	[Header("References:")]
	[SerializeField] private new Camera camera = null;

	private ElympicsFloat currentLoadingTime = new ElympicsFloat(0.0f);
	private ElympicsBool isLoadingToShot = new ElympicsBool(false);

	public event Action<float, float> LoadingTimeChanged;
	public event Action<RaycastHit> WeaponFired;

	protected override void ProcessWeaponAction()
	{
		if (isLoadingToShot)
			return;

		currentLoadingTime.Value = 0.0f;
		isLoadingToShot.Value = true;
	}

	public override void ElympicsUpdate()
	{
		base.ElympicsUpdate();

		if (isLoadingToShot)
		{
			ChangeCurrentLoadingTime(currentLoadingTime.Value += Elympics.TickDuration);

			if (currentLoadingTime >= loadingTime)
				ProcessRayShot();
		}
	}

	private void ProcessRayShot()
	{
		RaycastHit hit;

		if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, Mathf.Infinity))
		{
			if (hit.transform.TryGetComponent<StatsController>(out StatsController statsController))
			{
				statsController.ChangeHealth(-damage);
			}
		}

		WeaponFired?.Invoke(hit);

		ChangeCurrentLoadingTime(0.0f);
		isLoadingToShot.Value = false;
	}

	public override void SetIsActive(bool isActive)
	{
		base.SetIsActive(isActive);

		if (!isActive)
			isLoadingToShot.Value = false;

		ChangeCurrentLoadingTime(0.0f);
	}

	private void ChangeCurrentLoadingTime(float newCurrentLoadingTime)
	{
		currentLoadingTime.Value = newCurrentLoadingTime;
		LoadingTimeChanged?.Invoke(currentLoadingTime, loadingTime);
	}
}
