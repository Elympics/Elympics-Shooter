using Elympics;
using UnityEngine;
using Cinemachine;
using System;

[RequireComponent(typeof(PlayerData))]
public class PlayerCamerasController : ElympicsMonoBehaviour, IInitializable
{
	[System.Serializable]
	public struct CMVirtualCameraWithASsignedLayerMask
	{
		public CinemachineVirtualCamera VirtualCamera;
		public LayerMask AssignedLayerMask;
	}

	[Header("References:")]
	[SerializeField] private Camera brainCamera = null;

	[Header("Parameters:")]
	[SerializeField] private CMVirtualCameraWithASsignedLayerMask firstPersonCamera;
	[SerializeField] private CMVirtualCameraWithASsignedLayerMask thirdPersonCamera;

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
			firstPersonCamera.VirtualCamera.Priority = (int)VirtualCamPriority.Active;
	}

	public void SetDefaultCameraAsActive()
	{
		DisableAllCamerasInPlayer();
		SetCameraAsActive(firstPersonCamera);
	}

	public void SetThirdPersonCameraAsActive()
	{
		DisableAllCamerasInPlayer();
		SetCameraAsActive(thirdPersonCamera);
	}

	private void SetCameraAsActive(CMVirtualCameraWithASsignedLayerMask camera)
	{
		camera.VirtualCamera.Priority = (int)VirtualCamPriority.Active;

		brainCamera.cullingMask = camera.AssignedLayerMask;
	}

	private void DisableAllCamerasInPlayer()
	{
		foreach (CinemachineVirtualCamera camera in allCamerasInPlayer)
			camera.Priority = (int)VirtualCamPriority.Disabled;
	}
}
