using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillCamController : MonoBehaviour
{
	[Header("References:")]
	[SerializeField] private DeathController deathController = null;
	[SerializeField] private PlayersProvider playersProvider = null;

	private void Awake()
	{
		deathController.IsDead.ValueChanged += SetKillCamIsActive;
		deathController.HasBeenKilled += SetupInfoAboutKiller;
	}

	private void SetupInfoAboutKiller(int victimId, int killerId)
	{
		
	}

	private void SetKillCamIsActive(bool lastValue, bool newValue)
	{
		
	}
}
