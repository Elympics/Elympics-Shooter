using Elympics;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathController : ElympicsMonoBehaviour, IUpdatable
{
	[Header("Parameters:")]
	[SerializeField] private float deathTime = 2.0f;

	public ElympicsBool IsDead { get; } = new ElympicsBool(false);
	public ElympicsFloat CurrentDeathTime { get; } = new ElympicsFloat(0.0f);

	public event Action PlayerRespawned = null;
	public event Action<int, int> HasBeenKilled = null;


	private PlayerData playerData = null;

	private void Awake()
	{
		playerData = GetComponent<PlayerData>();
	}

	public void ProcessPlayersDeath(int damageOwner)
	{
		CurrentDeathTime.Value = deathTime;
		IsDead.Value = true;

		HasBeenKilled?.Invoke((int)PredictableFor, damageOwner);
	}

	public void ElympicsUpdate()
	{
		if (!IsDead || !Elympics.IsServer)
			return;

		CurrentDeathTime.Value -= Elympics.TickDuration;

		if (CurrentDeathTime.Value <= 0)
		{
			RespawnPlayer();
		}
	}

	private void RespawnPlayer()
	{
		PlayersSpawner.Instance.SpawnPlayer(playerData);
		PlayerRespawned?.Invoke();
		IsDead.Value = false;
	}
}
