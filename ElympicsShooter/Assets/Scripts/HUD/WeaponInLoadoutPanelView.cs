using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponInLoadoutPanelView : MonoBehaviour
{
	[SerializeField] private Sprite disabledWeapon = null;
	[SerializeField] private Sprite enabledWeapon = null;
	[SerializeField] private Image weaponInLoadoutPanelIcon = null;

	public void UpdateWeaponView(bool isEnabled)
	{
		var spriteToAssign = isEnabled ? enabledWeapon : disabledWeapon;

		weaponInLoadoutPanelIcon.sprite = spriteToAssign;
	}
}
