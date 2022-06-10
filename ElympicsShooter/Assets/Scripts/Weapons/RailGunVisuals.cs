using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RailGun))]
public class RailGunVisuals : MonoBehaviour
{
	[Header("References:")]
	[SerializeField] private Image[] loadingBars = null;
	[SerializeField] private Transform bulletSpawnPoint = null;

	private void Awake()
	{
		var railGun = GetComponent<RailGun>();

		railGun.LoadingTimeChanged += ProcessLoadingTimeChanged;
		railGun.WeaponFired += ProcessWeaponFired;
	}

	private void ProcessWeaponFired(RaycastHit hit)
	{
		
	}

	private void ProcessLoadingTimeChanged(float currentLoadingValue, float maxLoadingValue)
	{
		var fillAmountvalue = currentLoadingValue / maxLoadingValue;

		foreach (Image loadingBar in loadingBars)
			loadingBar.fillAmount = fillAmountvalue;
	}
}
