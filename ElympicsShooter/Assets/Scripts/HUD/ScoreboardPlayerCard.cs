using Elympics;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreboardPlayerCard : MonoBehaviour
{
	[Header("References:")]
	[SerializeField] private Image playerIcon = null;
	[SerializeField] private TextMeshProUGUI playerNickname = null;
	[SerializeField] private TextMeshProUGUI playerScore = null;

	public void Initialize(PlayerData assignedPlayerData, ElympicsInt playerScore)
	{
		SetupView(assignedPlayerData);

		playerScore.ValueChanged += UpdateScoreView;
	}

	private void UpdateScoreView(int lastValue, int newValue)
	{
		playerScore.text = newValue.ToString();
	}

	private void SetupView(PlayerData assignedPlayerData)
	{
		playerIcon.color = assignedPlayerData.ThemeColor;

		playerNickname.text = assignedPlayerData.Nickname;
		playerNickname.color = assignedPlayerData.ThemeColor;
	}
}
