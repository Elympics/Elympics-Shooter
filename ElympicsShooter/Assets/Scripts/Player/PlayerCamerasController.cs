using Elympics;
using UnityEngine;
using Cinemachine;
using System;

[RequireComponent(typeof(PlayerData))]
public class PlayerCamerasController : ElympicsMonoBehaviour, IInitializable
{
	[SerializeField] private CinemachineVirtualCamera defaultCamera = null;

	private CinemachineVirtualCamera[] allCamerasInPlayer = null;

	public void Initialize()
	{
		var playerData = GetComponent<PlayerData>();
		allCamerasInPlayer = GetComponentsInChildren<CinemachineVirtualCamera>();

		DisableAllCamerasInPlayer();

		InitializeCamerasAtGameStart(playerData);
	}

	private void InitializeCamerasAtGameStart(PlayerData playerData)
	{
		if (Elympics.IsClient && (int)Elympics.Player == playerData.PlayerId)
			defaultCamera.Priority = (int)VirtualCamPriority.Active;
	}

	private void SetDefaultCameraAsActive()
	{
		DisableAllCamerasInPlayer();

		defaultCamera.Priority = (int)VirtualCamPriority.Active;
	}

	private void DisableAllCamerasInPlayer()
	{
		foreach (CinemachineVirtualCamera camera in allCamerasInPlayer)
			camera.Priority = (int)VirtualCamPriority.Disabled;
	}
}
