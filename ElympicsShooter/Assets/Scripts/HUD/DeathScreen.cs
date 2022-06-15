using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeathScreen : MonoBehaviour
{
	[SerializeField] private PlayersProvider playersProvider = null;
	[SerializeField] private CanvasGroup canvasGroup = null;
	[SerializeField] private TextMeshProUGUI deathTimerText = null;

	private void Start()
	{
		if (playersProvider.IsReady)
			SubscribeToDeathController();
		else
			playersProvider.IsReadyChanged += SubscribeToDeathController;
	}

	private void SubscribeToDeathController()
	{
		var clientPlayerData = playersProvider.ClientPlayer;
		clientPlayerData.DeathController.CurrentDeathTime.ValueChanged += UpdateDeathTimerView;
		clientPlayerData.DeathController.IsDead.ValueChanged += UpdateDeathScreenView;
	}

	private void UpdateDeathScreenView(bool lastValue, bool newValue)
	{
		canvasGroup.alpha = newValue ? 1.0f : 0.0f;
	}

	private void UpdateDeathTimerView(float lastValue, float newValue)
	{
		deathTimerText.text = Mathf.Ceil(newValue).ToString();
	}
}
