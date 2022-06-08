using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
	[SerializeField] private Slider healthSlider = null;

	private void Start()
	{
		var clientPlayerProvider = ClientPlayerProvider.Instance;

		if (clientPlayerProvider.IsReady)
			SubscribeToStatsController();
		else
			clientPlayerProvider.IsReadyChanged += SubscribeToStatsController;
	}

	private void SubscribeToStatsController()
	{
		var clientPlayerData = ClientPlayerProvider.Instance.ClientPlayer;
		clientPlayerData.StatsController.HealthValueChanged += UpdateHealthBarView;
	}

	private void UpdateHealthBarView(float currentHealth, float maxHealth)
	{
		healthSlider.value = currentHealth / maxHealth;
	}
}
