using Elympics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayersSpawner : ElympicsMonoBehaviour
{
	[SerializeField] private Transform[] spawnPoints = null;

	private System.Random random = null;

	public static PlayersSpawner Instance = null;

	private void Awake()
	{
		if (PlayersSpawner.Instance == null)
			PlayersSpawner.Instance = this;
		else
			Destroy(this);
	}

	public void Start()
	{
		if (!Elympics.IsServer)
			return;

		random = new System.Random();

		var playersProvider = PlayersProvider.Instance;

		if (playersProvider.IsReady)
			InitialSpawnPlayers();
		else
			playersProvider.IsReadyChanged += InitialSpawnPlayers;
	}

	private void InitialSpawnPlayers()
	{
		foreach (PlayerData player in PlayersProvider.Instance.AllPlayersInScene)
		{
			SpawnPlayer(player);
		}
	}

	public void SpawnPlayer(PlayerData player)
	{
		Vector3 spawnPoint = GetSpawnPointWithoutPlayersInRange().position;

		player.transform.position = spawnPoint;
	}

	private Transform GetSpawnPointWithoutPlayersInRange()
	{
		var randomizedSpawnPoints = GetRandomizedSpawnPoints();
		Transform chosenSpawnPoint = null;

		foreach (Transform spawnPoint in randomizedSpawnPoints)
		{
			chosenSpawnPoint = spawnPoint;

			Collider[] objectsInRange = Physics.OverlapSphere(chosenSpawnPoint.position, 3.0f);

			if (!objectsInRange.Any(x => x.transform.root.gameObject.TryGetComponent<PlayerData>(out _)))
				break;
		}

		return chosenSpawnPoint;
	}

	private IOrderedEnumerable<Transform> GetRandomizedSpawnPoints()
	{
		return spawnPoints.OrderBy(x => random.Next());
	}
}
