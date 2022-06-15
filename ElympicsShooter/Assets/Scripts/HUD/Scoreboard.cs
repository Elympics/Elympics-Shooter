using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scoreboard : MonoBehaviour
{
	[Header("References:")]
	[SerializeField] private ScoreboardPlayerCard scoreboardPlayerCardPrefab = null;
	[SerializeField] private Transform cardsContainer = null;
	[SerializeField] private CanvasGroup canvasGroup = null;
	[SerializeField] private PlayersProvider playersProvider = null;
	[SerializeField] private PlayerScoresManager playerScoresManager = null;

	private void Awake()
	{
		if (playersProvider.IsReady)
			SetupScoreboard();
		else
			playersProvider.IsReadyChanged += SetupScoreboard;
	}

	private void SetupScoreboard()
	{
		if (playersProvider.ClientPlayer.TryGetComponent(out HUDController hudController))
		{
			hudController.ShowScoreboardValueChanged += SetScoreboardDisplayStatus;
		}

		if (playerScoresManager.IsReady)
			CreatePlayerCards();
		else
			playerScoresManager.IsReadyChanged += CreatePlayerCards;
	}

	private void SetScoreboardDisplayStatus(bool showScoreboard)
	{
		canvasGroup.alpha = showScoreboard ? 1.0f: 0.0f;
	}

	private void CreatePlayerCards()
	{
		foreach (PlayerData playerData in playersProvider.AllPlayersInScene)
		{
			var createdCard = Instantiate(scoreboardPlayerCardPrefab, cardsContainer);
			createdCard.Initialize(playerData, playerScoresManager.GetScoreForPlayer(playerData.PlayerId));
		}
	}
}
