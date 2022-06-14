using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
	[SerializeField] private PlayersProvider playersProvider = null;
	[SerializeField] private Slider healthSlider = null;

	private void Start()
	{
		if (playersProvider.IsReady)
			SubscribeToStatsController();
		else
			playersProvider.IsReadyChanged += SubscribeToStatsController;
	}

	private void SubscribeToStatsController()
	{
		var clientPlayerData = playersProvider.ClientPlayer;
		clientPlayerData.StatsController.HealthValueChanged += UpdateHealthBarView;
	}

	private void UpdateHealthBarView(float currentHealth, float maxHealth)
	{
		healthSlider.value = currentHealth / maxHealth;
	}
}
