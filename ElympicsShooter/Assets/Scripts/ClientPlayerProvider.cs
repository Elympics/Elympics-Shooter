using Elympics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientPlayerProvider : ElympicsMonoBehaviour
{
	public static ClientPlayerProvider Instance = null;

	public PlayerData ClientPlayer { get; private set; } = null;

	private void Awake()
	{
		if (ClientPlayerProvider.Instance == null)
			ClientPlayerProvider.Instance = this;
		else
			Destroy(this);

		FindClientPlayerInScene();
	}

	private void FindClientPlayerInScene()
	{
		var playersInScene = FindObjectsOfType<PlayerData>();

		foreach (PlayerData player in playersInScene)
		{
			if ((int)Elympics.Player == player.PlayerId)
			{
				ClientPlayer = player;
				return;
			}
		}

		//Fix for server side.
		//TODO: Disable HUD on server
		ClientPlayer = playersInScene[0];
	}
}
