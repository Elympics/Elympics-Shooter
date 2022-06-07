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
		var clientPlayerData = ClientPlayerProvider.Instance.ClientPlayer;

		clientPlayerData.StatsController.HealthValueChanged += UpdateHealthBarView;
	}

	private void UpdateHealthBarView(float currentHealth, float maxHealth)
	{
		healthSlider.value = currentHealth / maxHealth;
	}
}
