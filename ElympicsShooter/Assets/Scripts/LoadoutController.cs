using Elympics;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadoutController : ElympicsMonoBehaviour, IInitializable
{
	[Header("References:")]
	[SerializeField] private DeathController deathController = null;
	[SerializeField] private Weapon[] availableWeapons = null;

	private ElympicsInt currentEquipedWeaponIndex = new ElympicsInt(0);
	private Weapon currentEquipedWeapon = null;

	public void ProcessWeaponActions(bool weaponPrimaryAction)
	{
		if (deathController.IsDead)
			return;

		if (weaponPrimaryAction)
			ProcessWeaponPrimaryAction();
	}

	private void ProcessWeaponPrimaryAction()
	{
		currentEquipedWeapon.ExecutePrimaryAction();
	}

	public void SwitchWeapon(float currentWeaponToSwitchIndexModifier)
	{

	}

	public void Initialize()
	{
		currentEquipedWeaponIndex.ValueChanged += UpdateCurrentEquipedWeaponByIndex;

		UpdateCurrentEquipedWeaponByIndex(currentEquipedWeaponIndex, 0);
	}

	private void UpdateCurrentEquipedWeaponByIndex(int lastValue, int newValue)
	{
		currentEquipedWeapon = availableWeapons[newValue];
	}
}
