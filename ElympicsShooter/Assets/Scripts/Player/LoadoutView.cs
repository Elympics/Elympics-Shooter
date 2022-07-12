using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadoutView : MonoBehaviour
{
	[Header("References:")]
	[SerializeField] private Animator handsRootAnimator = null;
	[SerializeField] private Animator playerHandsAnimator = null;
	[SerializeField] private LoadoutController loadoutController = null;

	//Hands Root Animator Parameters
	private readonly int SwapWeaponTrigger = Animator.StringToHash("triggerWeaponInitialize");

	//Player Hands Animator Parameters
	private readonly int ActiveWeaponIndex = Animator.StringToHash("ActiveWeaponIndex");

	private void Awake()
	{
		loadoutController.CurrentEquipedWeaponIndex.ValueChanged += OnWeaponSwap;
	}

	private void OnWeaponSwap(int lastValue, int newValue)
	{
		handsRootAnimator.SetTrigger(SwapWeaponTrigger);
		playerHandsAnimator.SetInteger(ActiveWeaponIndex, newValue);
	}
}
