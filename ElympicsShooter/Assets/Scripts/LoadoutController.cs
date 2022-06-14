using Elympics;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadoutController : ElympicsMonoBehaviour, IInitializable, IUpdatable
{
	[Header("References:")]
	[SerializeField] private DeathController deathController = null;
	[SerializeField] private Weapon[] availableWeapons = null;

	[Header("Parameters:")]
	[SerializeField] private float weaponSwapTime = 0.3f;

	public event Action WeaponSwapped = null;

	private ElympicsInt currentEquipedWeaponIndex = new ElympicsInt(0);
	private ElympicsFloat currentWeaponSwapTime = null;

	private Weapon currentEquipedWeapon = null;

	public void Initialize()
	{
		currentWeaponSwapTime = new ElympicsFloat(weaponSwapTime);

		DisableAllWeapons();

		currentEquipedWeaponIndex.ValueChanged += UpdateCurrentEquipedWeaponByIndex;
		UpdateCurrentEquipedWeaponByIndex(currentEquipedWeaponIndex, 0);
	}

	private void DisableAllWeapons()
	{
		foreach (Weapon weapon in availableWeapons)
			weapon.SetIsActive(false);
	}

	public void ProcessLoadoutActions(bool weaponPrimaryAction, int weaponIndex)
	{
		if (deathController.IsDead)
			return;

		if (weaponIndex != -1 && weaponIndex != currentEquipedWeaponIndex)
		{
			SwitchWeapon(weaponIndex);
		}
		else
		{
			if (currentWeaponSwapTime.Value >= weaponSwapTime)
				ProcessWeaponActions(weaponPrimaryAction);
		}
	}

	private void ProcessWeaponActions(bool weaponPrimaryAction)
	{
		if (weaponPrimaryAction)
			ProcessWeaponPrimaryAction();
	}

	private void ProcessWeaponPrimaryAction()
	{
		currentEquipedWeapon.ExecutePrimaryAction();
	}

	public void SwitchWeapon(int weaponIndex)
	{
		currentEquipedWeaponIndex.Value = weaponIndex;
	}

	private void UpdateCurrentEquipedWeaponByIndex(int lastValue, int newValue)
	{
		if (currentEquipedWeapon != null)
			currentEquipedWeapon.SetIsActive(false);

		currentEquipedWeapon = availableWeapons[newValue];
		currentEquipedWeapon.SetIsActive(true);

		currentWeaponSwapTime.Value = 0.0f;

		WeaponSwapped?.Invoke();
	}

	public void ElympicsUpdate()
	{
		if (currentWeaponSwapTime < weaponSwapTime)
			currentWeaponSwapTime.Value += Elympics.TickDuration;
	}
}
