using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeathScreen : MonoBehaviour
{
	[SerializeField] private CanvasGroup canvasGroup = null;
	[SerializeField] private TextMeshProUGUI deathTimerText = null;

	private void Start()
	{
		var clientPlayerProvider = PlayersProvider.Instance;

		if (clientPlayerProvider.IsReady)
			SubscribeToDeathController();
		else
			clientPlayerProvider.IsReadyChanged += SubscribeToDeathController;
	}

	private void SubscribeToDeathController()
	{
		var clientPlayerData = PlayersProvider.Instance.ClientPlayer;
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
