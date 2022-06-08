using Elympics;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientPlayerProvider : ElympicsMonoBehaviour, IInitializable
{
	public static ClientPlayerProvider Instance = null;

	public PlayerData ClientPlayer { get; private set; } = null;

	public bool IsReady { get; private set; } = false;
	public event Action IsReadyChanged = null;

	private void Awake()
	{
		if (ClientPlayerProvider.Instance == null)
			ClientPlayerProvider.Instance = this;
		else
			Destroy(this);
	}

	public void Initialize()
	{ 
		FindClientPlayerInScene();
		IsReady = true;
		IsReadyChanged?.Invoke();
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
