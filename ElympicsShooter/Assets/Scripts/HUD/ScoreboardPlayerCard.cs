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

	public void Initialize(PlayerData assignedPlayerData)
	{
		SetupView(assignedPlayerData);
	}

	private void SetupView(PlayerData assignedPlayerData)
	{
		playerIcon.color = assignedPlayerData.ThemeColor;

		playerNickname.text = assignedPlayerData.Nickname;
		playerNickname.color = assignedPlayerData.ThemeColor;
	}
}
