using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadoutView : MonoBehaviour
{
	[Header("References:")]
	[SerializeField] private Animator handsAnimator = null;
	[SerializeField] private LoadoutController loadoutController = null;

	[Header("Animator Parameters:")]
	[SerializeField] private const string SwapWeaponTrigger = "triggerWeaponInitialize";

	private void Awake()
	{
		loadoutController.WeaponSwapped += OnWeaponSwap;
	}

	public void OnWeaponSwap()
	{
		handsAnimator.SetTrigger(SwapWeaponTrigger);
	}
}
