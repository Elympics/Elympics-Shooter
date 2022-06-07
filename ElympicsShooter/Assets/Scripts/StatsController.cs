using Elympics;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsController : ElympicsMonoBehaviour, IInitializable
{
	[SerializeField] private float maxHealth = 100.0f;

	private ElympicsFloat health = new ElympicsFloat(0);
	public event Action<float, float> HealthValueChanged = null;

	public void Initialize()
	{
		health.Value = maxHealth;
		health.ValueChanged += OnHealthValueChanged;
	}

	public void ChangeHealth(float value)
	{
		if (!Elympics.IsServer)
			return;

		health.Value += value;
	}

	private void OnHealthValueChanged(float lastValue, float newValue)
	{
		HealthValueChanged?.Invoke(newValue, maxHealth);
	}
}
